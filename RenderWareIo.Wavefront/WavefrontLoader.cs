using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;
using RenderWareBuilders;
using RenderWareIo.Structs.Col;
using RenderWareIo.Structs.Dff;
using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace RenderWareIo.Wavefront
{
    public class WavefrontLoaderOptions
    {
        public DayNightColors? DayNightColor { get; set; }
        public Dictionary<string, MaterialId> CollisionMaterials { get; set; }
    }

    public class WavefrontLoader : IDisposable
    {
        private readonly FileStream fileStream;
        private readonly IObjLoader objLoader;
        private readonly LoadResult loadResult;

        public WavefrontLoader(string fileName)
        {
            this.fileStream = File.OpenRead(fileName);
            this.objLoader = new ObjLoaderFactory().Create();
            this.loadResult = objLoader.Load(fileStream);
        }

        public void Dispose()
        {
            fileStream.Dispose();
        }

        public string[] GetAllGroups()
        {
            return this.loadResult.Groups.Select(x => x.Name).ToArray();
        }
        
        public string GetGroupTextureName(string groupName)
        {
            var textureMap = this.loadResult.Groups.Where(x => x.Name == groupName).First().Material.DiffuseTextureMap;
            return textureMap != null ? Path.GetFileNameWithoutExtension(textureMap) : "";
        }

        public RenderWareBuilder CreateBuilder(string[] materialGroups, WavefrontLoaderOptions options = null)
        {
            var renderWareBuilder = new RenderWareBuilder();
            foreach (var groupName in materialGroups)
            {
                var group = this.loadResult.Groups.Where(x => x.Name == groupName).Single();

                RenderWareBuilders.Material material = null;

                if(group.Material != null)
                {
                    var textureMap = group.Material.DiffuseTextureMap != null ? Path.GetFileNameWithoutExtension(group.Material.DiffuseTextureMap) : "";
                    var ambientColor = group.Material.AmbientColor;
                    var color = System.Drawing.Color.FromArgb(255, (int)(ambientColor.X * 255), (int)(ambientColor.Y * 255), (int)(ambientColor.Z * 255));
                    material = new RenderWareBuilders.Material
                    {
                        Name = textureMap,
                        Color = color,
                    };
                    renderWareBuilder.AddMaterial(material);
                }

                foreach (var face in group.Faces)
                {
                    PrelitVertex CreatePrelitVertex(FaceVertex faceVertex, DayNightColors verticesColors)
                    {
                        var vertex = this.loadResult.Vertices[faceVertex.VertexIndex - 1];
                        var texture = this.loadResult.Textures[faceVertex.TextureIndex - 1];
                        return new PrelitVertex
                        {
                            Position = new Vector3(vertex.X, vertex.Y, vertex.Z),
                            Normal = Vector3.Zero,
                            Uv = new Vector2(texture.X, texture.Y),
                            Day = verticesColors.day,
                            Night = verticesColors.night,
                        };
                    }
                    
                    RenderWareBuilders.Vertex CreateVertex(FaceVertex faceVertex)
                    {
                        var vertex = this.loadResult.Vertices[faceVertex.VertexIndex - 1];
                        var texture = this.loadResult.Textures[faceVertex.TextureIndex - 1];
                        return new RenderWareBuilders.Vertex
                        {
                            Position = new Vector3(vertex.X, vertex.Y, vertex.Z),
                            Normal = Vector3.Zero,
                            Uv = new Vector2(texture.X, texture.Y),
                        };
                    }

                    if(options != null && options.DayNightColor != null)
                    {
                        renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
                        {
                            Vertex3 = renderWareBuilder.AddPrelitVertex(CreatePrelitVertex(face[0], options.DayNightColor.Value)),
                            Vertex2 = renderWareBuilder.AddPrelitVertex(CreatePrelitVertex(face[1], options.DayNightColor.Value)),
                            Vertex1 = renderWareBuilder.AddPrelitVertex(CreatePrelitVertex(face[2], options.DayNightColor.Value)),
                            Material = material,
                        });
                    }
                    else
                    {
                        renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
                        {
                            Vertex3 = renderWareBuilder.AddVertex(CreateVertex(face[0])),
                            Vertex2 = renderWareBuilder.AddVertex(CreateVertex(face[1])),
                            Vertex1 = renderWareBuilder.AddVertex(CreateVertex(face[2])),
                            Material = material,
                        });
                    }
                }
            }

            return renderWareBuilder;
        }

        public Dff CreateDff(string[] materialGroups, WavefrontLoaderOptions options = null)
        {
            var renderWareBuilder = CreateBuilder(materialGroups, options);
            var dff = renderWareBuilder.BuildDff();
            return dff;
        }
        
        public Col CreateCol(string[] materialGroups, WavefrontLoaderOptions options = null)
        {
            var renderWareBuilder = CreateBuilder(materialGroups, options);
            if(options != null && options.CollisionMaterials != null)
            {
                foreach (var pair in options.CollisionMaterials)
                {
                    renderWareBuilder.SetMaterialCollisionMaterialId(pair.Key, pair.Value);
                }
            }
            var col = renderWareBuilder.BuildCol();
            return col;
        }
    }
}