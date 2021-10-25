using RenderWareIo.Structs.Col;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RenderWareBuilders
{
    [Obsolete("ColBuilder is deprecated, use RenderWareBuilder instead.")]
    public class ColBuilder
    {
        private readonly List<Vertex> vertices;
        private readonly List<Triangle> triangles;

        public ColBuilder()
        {
            this.vertices = new List<Vertex>();
            this.triangles = new List<Triangle>();
        }

        public Vertex AddVertex(Vertex vertex)
        {
            vertex.Index = (ushort)this.vertices.Count();
            this.vertices.Add(vertex);
            return vertex;
        }

        public Vertex AddVertex(Vector3 position, Vector3 normal, Vector2 uv) => AddVertex(new Vertex() { Position = position, Normal = normal, Uv = uv });

        public Triangle AddTriangle(Triangle triangle)
        {
            this.triangles.Add(triangle);
            return triangle;
        }

        public Triangle AddTriangle(Vertex vertex1, Vertex vertex2, Vertex vertex3) => AddTriangle(new Triangle()
        {
            Vertex1 = vertex1,
            Vertex2 = vertex2,
            Vertex3 = vertex3
        });

        public Col Build()
        {
            var col = new Col();

            col.Header = new Header();
            col.Body = new Body()
            {
                Spheres = new List<Sphere>(),
                Boxes = new List<Box>(),
                Vertices = this.vertices
                    .Select(vertex => new RenderWareIo.Structs.Col.Vertex()
                    {
                        FirstFloat = vertex.Position.X,
                        SecondFloat = vertex.Position.Y,
                        ThirdFloat = vertex.Position.Z,
                    })
                    .ToList(),
                FaceGroups = new List<FaceGroup>(),
                FaceGroupCount = 0,
                Faces = this.triangles
                    .Select(triangle => new Face()
                    {
                        A = triangle.Vertex1.Index,
                        B = triangle.Vertex2.Index,
                        C = triangle.Vertex3.Index,
                        Light = 0,
                        Material = 0
                    })
                    .ToList(),
                ShadowMeshVertices = new List<RenderWareIo.Structs.Col.Vertex>(),
                ShadowMeshFaces = new List<Face>()
            };

            return col;
        }
    }
}
