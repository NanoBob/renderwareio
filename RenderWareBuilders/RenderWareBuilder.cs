using RenderWareIo.Structs.Col;
using RenderWareIo.Structs.Dff;
using RenderWareIo.Structs.Dff.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RenderWareBuilders
{
    public struct DayNightColors
    {
        public System.Drawing.Color day;
        public System.Drawing.Color night;
    }

    public class RenderWareBuilder
    {
        private readonly List<Vertex> vertices;
        private readonly List<PrelitVertex> prelitVertices;
        private readonly List<Triangle> triangles;
        private readonly List<Material> materials;
        private readonly Dictionary<string, MaterialId> materialCollisionMaterialIds;

        public IReadOnlyCollection<Vertex> Vertices => vertices.AsReadOnly();
        public IReadOnlyCollection<PrelitVertex> PrelitVertices => prelitVertices.AsReadOnly();
        public IReadOnlyCollection<Triangle> Triangles => triangles.AsReadOnly();

        public RenderWareBuilder()
        {
            this.vertices = new List<Vertex>();
            this.prelitVertices = new List<PrelitVertex>();
            this.triangles = new List<Triangle>();
            this.materials = new List<Material>();
            this.materialCollisionMaterialIds = new Dictionary<string, MaterialId>();
        }

        public void SetMaterialCollisionMaterialId(string material, MaterialId materialId)
        {
            materialCollisionMaterialIds[material] = materialId;
        }

        public Vertex AddVertex(Vertex vertex)
        {
            vertex.Index = (ushort)this.vertices.Count;
            this.vertices.Add(vertex);
            return vertex;
        }
        
        public Vertex AddPrelitVertex(PrelitVertex vertex)
        {
            vertex.Index = (ushort)this.prelitVertices.Count;
            this.prelitVertices.Add(vertex);
            return vertex;
        }

        public Vertex AddVertex(Vector3 position, Vector3 normal, Vector2 uv) => AddVertex(new Vertex() { Position = position, Normal = normal, Uv = uv });

        public Triangle AddTriangle(Triangle triangle)
        {
            triangle.Index = (ushort)this.triangles.Count;
            this.triangles.Add(triangle);
            return triangle;
        }

        public Triangle AddTriangle(Vertex vertex1, Vertex vertex2, Vertex vertex3, Material material = null) => AddTriangle(new Triangle()
        {
            Vertex1 = vertex1,
            Vertex2 = vertex2,
            Vertex3 = vertex3,
            Material = material
        });

        public Material AddMaterial(Material material)
        {
            material.Index = (ushort)this.materials.Count;
            this.materials.Add(material);
            return material;
        }

        public Material AddMaterial(string name, string maskName, System.Drawing.Color color) => AddMaterial(new Material() { Name = name, MaskName = maskName, Color = color });

        public Dff BuildDff()
        {
            var dff = new Dff();
            var geometry = new Geometry();
            var morphTarget = new MorphTarget();

            geometry.Flags = 0x76;

            foreach (var material in this.materials) 
            {
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

            if(this.prelitVertices.Any())
            {
                geometry.Flags |= (int)GeometryFlags.PRELIT;

                foreach (var vertex in this.prelitVertices)
                {
                    morphTarget.Vertices.Add(vertex.Position);
                    morphTarget.Normals.Add(vertex.Normal);
                    geometry.TexCoords.Add(new Uv() { X = vertex.Uv.X, Y = vertex.Uv.Y });
                    geometry.Colors.Add(new Color(vertex.Day));
                }

                geometry.Extension.Extensions.Add(new NightVertex(prelitVertices.Select(x => x.Night).ToArray()));
            }
            else
            {
                foreach (var vertex in this.vertices)
                {
                    morphTarget.Vertices.Add(vertex.Position);
                    morphTarget.Normals.Add(vertex.Normal);
                    geometry.TexCoords.Add(new Uv() { X = vertex.Uv.X, Y = vertex.Uv.Y });
                }
            }

            var min = new Vector3(morphTarget.Vertices.Min(v => v.X), morphTarget.Vertices.Min(v => v.Y), morphTarget.Vertices.Min(v => v.Z));
            var max = new Vector3(morphTarget.Vertices.Max(v => v.X), morphTarget.Vertices.Max(v => v.Y), morphTarget.Vertices.Max(v => v.Z));
            var center = min + (max - min) * 0.5f;
            var radius = morphTarget.Vertices.Max(vertex => (new Vector3(vertex.X, vertex.Y, vertex.Z) - center).Length());

            geometry.Sphere = new RenderWareIo.Structs.Dff.Sphere()
            {
                Position = center,
                Radius = radius
            };

            foreach(var triangle in this.triangles)
            {
                geometry.Triangles.Add(new RenderWareIo.Structs.Dff.Triangle()
                {
                    VertexIndexOne = triangle.Vertex1.Index,
                    VertexIndexTwo = triangle.Vertex2.Index,
                    VertexIndexThree = triangle.Vertex3.Index,
                    MaterialIndex = triangle.Material.Index
                });
            }

            geometry.MorphTargets.Add(morphTarget);

            var frameName = "Model";
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

        public Col BuildCol()
        {
            var vertices = this.vertices.Count > 0 ? this.vertices : (IEnumerable<Vertex>)this.prelitVertices;
            return new Col
            {
                ColCombos = new List<ColCombo>()
                {
                    new ColCombo()
                    {
                        Header = new Header(),
                        Body = new Body()
                        {
                            Spheres = new List<RenderWareIo.Structs.Col.Sphere>(),
                            Boxes = new List<Box>(),
                            Vertices = vertices
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
                                .Select(triangle => {
                                    MaterialId materialId = MaterialId.Default;
                                    this.materialCollisionMaterialIds.TryGetValue(triangle.Material.Name, out materialId);
                                    return new Face()
                                    {
                                        A = triangle.Vertex1.Index,
                                        B = triangle.Vertex2.Index,
                                        C = triangle.Vertex3.Index,
                                        Light = 15,
                                        Material = materialId
                                    };
                                })
                                .ToList(),
                            ShadowMeshVertices = new List<RenderWareIo.Structs.Col.Vertex>(),
                            ShadowMeshFaces = new List<Face>()
                        }
                    }
                }
            };
        }
    }
}
