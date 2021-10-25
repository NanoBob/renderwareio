using RenderWareIo.Structs.Dff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RenderWareBuilders
{
    [Obsolete("DffBuilder is deprecated, use RenderWareBuilder instead.")]
    public class DffBuilder
    {
        private readonly List<Vertex> vertices;
        private readonly List<Triangle> triangles;
        private readonly List<Material> materials;

        public DffBuilder()
        {
            this.vertices = new List<Vertex>();
            this.triangles = new List<Triangle>();
            this.materials = new List<Material>();
        }

        public Vertex AddVertex(Vertex vertex)
        {
            this.vertices.Add(vertex);
            return vertex;
        }

        public Vertex AddVertex(Vector3 position, Vector3 normal, Vector2 uv) => AddVertex(new Vertex() { Position = position, Normal = normal, Uv = uv });

        public Triangle AddTriangle(Triangle triangle)
        {
            this.triangles.Add(triangle);
            return triangle;
        }

        public Triangle AddTriangle(Vertex vertex1, Vertex vertex2, Vertex vertex3, Material material) => AddTriangle(new Triangle()
        {
            Vertex1 = vertex1,
            Vertex2 = vertex2,
            Vertex3 = vertex3,
            Material = material
        });

        public Material AddMaterial(Material material)
        {
            this.materials.Add(material);
            return material;
        }

        public Material AddMaterial(string name, string maskName, System.Drawing.Color color) => AddMaterial(new Material() { Name = name, MaskName = maskName, Color = color });

        public Dff Build()
        {
            var dff = new Dff();
            var geometry = new Geometry();
            var morphTarget = new MorphTarget();

            foreach (var material in this.materials) 
            {
                material.Index = (ushort)geometry.MaterialList.Materials.Count;
                geometry.MaterialList.Materials.Add(new RenderWareIo.Structs.Dff.Material()
                {
                   Texture = new Texture()
                   {
                       Name = new StringBlock()
                       {
                           Value = material.Name + "\0"
                       },
                       MaskName = new StringBlock()
                       {
                           Value = material.MaskName + "\0"
                       },
                   },
                   Textured = 1,
                   Color = new Color()
                   {
                       R = material.Color.R,
                       G = material.Color.G,
                       B = material.Color.B,
                       A = material.Color.A
                   }                   
                });;
            }

            foreach (var vertex in this.vertices)
            {
                vertex.Index = (ushort)morphTarget.Vertices.Count;

                morphTarget.Vertices.Add(vertex.Position);
                morphTarget.Normals.Add(vertex.Normal);
                geometry.TexCoords.Add(new Uv() { X = vertex.Uv.X, Y = vertex.Uv.Y });
            }

            var min = new Vector3(morphTarget.Vertices.Min(v => v.X), morphTarget.Vertices.Min(v => v.Y), morphTarget.Vertices.Min(v => v.Z));
            var max = new Vector3(morphTarget.Vertices.Max(v => v.X), morphTarget.Vertices.Max(v => v.Y), morphTarget.Vertices.Max(v => v.Z));
            var center = min + (max - min) * 0.5f;
            var radius = morphTarget.Vertices.Max(vertex => (new Vector3(vertex.X, vertex.Y, vertex.Z) - center).Length());

            geometry.Flags = 0x76;
            geometry.Sphere = new Sphere()
            {
                Position = center,
                Radius = radius
            };

            foreach(var triangle in this.triangles)
            {
                triangle.Index = (ushort)geometry.Triangles.Count;
                geometry.Triangles.Add(new RenderWareIo.Structs.Dff.Triangle()
                {
                    VertexIndexOne = triangle.Vertex1.Index,
                    VertexIndexTwo = triangle.Vertex2.Index,
                    VertexIndexThree = triangle.Vertex3.Index,
                    MaterialIndex = triangle.Material.Index
                });
            }

            geometry.MorphTargets.Add(morphTarget);

            var frameName = "Frame";
            byte[] buffer = new byte[] { 0xfe, 0xf2, 0x53, 0x02, }
                .Concat(BitConverter.GetBytes(frameName.Length % 4 == 0 ? (uint)frameName.Length : (uint)((frameName.Length / 4) + 1) * 4))
                .Concat(new byte[] { 0xff, 0xff, 0x03, 0x18 })
                .Concat(frameName.Select(c => (byte)c))
                .Concat(new byte[4 - (frameName.Length % 4)])
                .ToArray();

            dff.Clump.FrameList.Frames.Add(new Frame());
            dff.Clump.FrameList.Extensions.Add(new Extension()
            {
                Data = buffer
            });
            dff.Clump.Atomics.Add(new Atomic()
            {
                FrameIndex = 0,
                GeometryIndex = 0,
            });
            dff.Clump.GeometryList.Geometries.Add(geometry);

            return dff;
        }
    }
}
