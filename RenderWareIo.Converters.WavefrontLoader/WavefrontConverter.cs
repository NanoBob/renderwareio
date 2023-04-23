using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using RenderWareBuilders;
using RenderWareIo.Structs.Dff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Material = RenderWareBuilders.Material;
using Texture = ObjLoader.Loader.Data.VertexData.Texture;
using Vertex = ObjLoader.Loader.Data.VertexData.Vertex;

namespace RenderWareIo.Converters.WavefrontConverter
{
    public static class WavefrontConverter
    {
        private static Vector3 ConvertNormal(Normal normal)
        {
            return new Vector3(normal.X, normal.Y, normal.Z);
        }

        private static Vector3 ConvertVertex(Vertex vertex)
        {
            return new Vector3(vertex.X, vertex.Y, vertex.Z);
        }

        private static Vector2 ConvertTexture(Texture texture)
        {
            return new Vector2
            {
                X = texture.X,
                Y = texture.Y
            };
        }

        public static Dff Convert(Stream stream, bool prelitVertices = false)
        {
            RenderWareBuilder renderWareBuilder = new RenderWareBuilder();
            var objLoaderFactory = new ObjLoaderFactory();
            var result = objLoaderFactory.Create().Load(stream);

            Dictionary<int, RenderWareBuilders.Vertex> vertices = new Dictionary<int, RenderWareBuilders.Vertex>();
            Dictionary<string, Material> materials = new Dictionary<string, Material>();

            foreach (var group in result.Groups)
            {
                var material = renderWareBuilder.AddMaterial(new Material
                {
                    Name = group.Name,
                    Color = System.Drawing.Color.White,
                    MaskName = ""
                });
                materials.Add(group.Name, material);

                foreach (var face in group.Faces)
                {
                    for (int i = 0; i < face.Count; i++)
                    {
                        var faceVertex = face[i];
                        var index = faceVertex.VertexIndex - 1;
                        if (!vertices.ContainsKey(index))
                        {
                            RenderWareBuilders.Vertex v;
                            if (prelitVertices)
                            {
                                v = renderWareBuilder.AddPrelitVertex(new PrelitVertex
                                {
                                    Day = System.Drawing.Color.White,
                                    Night = System.Drawing.Color.White,
                                    Normal = Vector3.UnitZ,
                                    Position = ConvertVertex(result.Vertices[index]),
                                    Uv = ConvertTexture(result.Textures[faceVertex.TextureIndex - 1]),
                                });
                            }
                            else
                            {
                                v = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
                                {
                                    Normal = Vector3.UnitZ,
                                    Position = ConvertVertex(result.Vertices[index]),
                                    Uv = ConvertTexture(result.Textures[faceVertex.TextureIndex - 1]),
                                });
                                vertices[index] = v;
                            }
                            vertices[index] = v;
                        }
                        else
                        {
                            ;
                        }
                    }

                    for (int i = 0; i < face.Count - 2; i++)
                    {
                        renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
                        {
                            Vertex1 = vertices[face[i].VertexIndex - 1],
                            Vertex2 = vertices[face[i + 1].VertexIndex - 1],
                            Vertex3 = vertices[face[i + 2].VertexIndex - 1],
                            Material = materials[group.Name],
                        });
                    }
                    //;
                }
            }

            return renderWareBuilder.BuildDff();
        }
    }
}
