using Newtonsoft.Json;
using RenderWareIo;
using RenderWareIo.Mta;
using RenderWareIo.Structs.BinaryIpl;
using RenderWareIo.Structs.Col;
using RenderWareIo.Structs.Dff;
using RenderWareIo.Structs.Ide;
using RenderWareIo.Structs.Ipl;
using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Sphere = RenderWareIo.Structs.Col.Sphere;
using Texture = RenderWareIo.Structs.Dff.Texture;
using RenderWareIo.Converters.WavefrontConverter;

namespace ConsoleApplication
{
    class Program
    {
        private static void ColTest()
        {
            new ColFile("./files/elevator.col").Write("./files/newelevator.col");

            ColFile newFile = new ColFile("./files/newelevator.col");


            Console.WriteLine($"Version: {string.Join("", newFile.Col.ColCombos.First().Header.FourCC)}");
            Console.WriteLine($"Size: {newFile.Col.ColCombos.First().Header.Size}");
            Console.WriteLine($"Name: {string.Join("", newFile.Col.ColCombos.First().Header.Name)}");
            Console.WriteLine($"Model ID: {newFile.Col.ColCombos.First().Header.ModelId}");
            Console.WriteLine($"Bounds:");
            Console.WriteLine($"\tMin: {newFile.Col.ColCombos.First().Header.Bounds.Min}\n\tMax: {newFile.Col.ColCombos.First().Header.Bounds.Max}\n\tCenter: {newFile.Col.ColCombos.First().Header.Bounds.Center}\n\tRadius:  {newFile.Col.ColCombos.First().Header.Bounds.Radius}");
            Console.WriteLine("");

            Console.WriteLine($"Sphere count: {newFile.Col.ColCombos.First().Header.SphereCount}");
            Console.WriteLine($"Box count: {newFile.Col.ColCombos.First().Header.BoxCount}");
            Console.WriteLine($"Face count: {newFile.Col.ColCombos.First().Header.FaceCount}");
            Console.WriteLine($"Line count: {newFile.Col.ColCombos.First().Header.LineCount}");
            Console.WriteLine("");

            Console.WriteLine($"Flags: {newFile.Col.ColCombos.First().Header.Flags}");
            Console.WriteLine("");

            Console.WriteLine($"Sphere offset: {newFile.Col.ColCombos.First().Header.SphereOffset}");
            Console.WriteLine($"Box offset: {newFile.Col.ColCombos.First().Header.BoxOffset}");
            Console.WriteLine($"Line offset: {newFile.Col.ColCombos.First().Header.LineOffset}");
            Console.WriteLine($"Vertex offset: {newFile.Col.ColCombos.First().Header.VertexOffset}");
            Console.WriteLine($"Face offset: {newFile.Col.ColCombos.First().Header.FaceOffset}");
            Console.WriteLine($"Triangle plane offset: {newFile.Col.ColCombos.First().Header.TrianglePlaneOffset}");
            Console.WriteLine("");


            Console.WriteLine($"Shadow mesh face count: {newFile.Col.ColCombos.First().Header.ShadowMeshFaceCount}");
            Console.WriteLine($"Shadow mesh vertex offset: {newFile.Col.ColCombos.First().Header.ShadowMeshVertexOffset}");
            Console.WriteLine($"Shadow mesh face offset: {newFile.Col.ColCombos.First().Header.ShadowMeshFaceOffset}");
            Console.WriteLine("");

            Console.WriteLine($"Vertices: ");
            foreach (Vertex vertex in newFile.Col.ColCombos.First().Body.Vertices)
            {
                Console.WriteLine($"\tX: {vertex.FirstFloat}, Y: {vertex.SecondFloat}, Z: {vertex.ThirdFloat}");
            }

            Console.WriteLine($"Faces: ");
            foreach (Face face in newFile.Col.ColCombos.First().Body.Faces)
            {
                Console.WriteLine($"\tX: {face.A}, Y: {face.B}, Z: {face.C}");
            }
        }

        private static void TxdTest()
        {
            string file = @"dust2x2.txd";
            //file = "482.txd";

            //new TxdFile($"./files/{file}").Write($"./files/new{file}");

            TxdFile newFile = new TxdFile($"./files/{file}");

            Console.WriteLine($"Type: {newFile.Txd.Header.Type}");
            Console.WriteLine($"Size: {newFile.Txd.Header.Size}");
            Console.WriteLine($"Marker: {newFile.Txd.Header.Marker} ({newFile.Txd.Header.Marker.ToString("X2")})");
            Console.WriteLine($"");

            Console.WriteLine($"Type: {newFile.Txd.TextureContainer.Header.Type}");
            Console.WriteLine($"Size: {newFile.Txd.TextureContainer.Header.Size}");
            Console.WriteLine($"Marker: {newFile.Txd.TextureContainer.Header.Marker} ({newFile.Txd.TextureContainer.Header.Marker.ToString("X2")})");
            Console.WriteLine($"Count: {newFile.Txd.TextureContainer.Count}");
            Console.WriteLine($"Unknown: {newFile.Txd.TextureContainer.Unknown}");
            Console.WriteLine($"");

            Console.WriteLine("Textures: ");
            foreach (RenderWareIo.Structs.Txd.Texture texture in newFile.Txd.TextureContainer.Textures)
            {
                Console.WriteLine($"\tType: {texture.Header.Type}");
                Console.WriteLine($"\tSize: {texture.Header.Size}");
                Console.WriteLine($"\tMarker: {texture.Header.Marker}");

                Console.WriteLine($"\tVersion {texture.Data.Version}");
                Console.WriteLine($"\tFilter flags: {texture.Data.FilterFlags}");
                Console.WriteLine($"\tTexture name: {texture.Data.TextureName}");
                Console.WriteLine($"\tAlpha name: {texture.Data.AlphaName}");

                Console.WriteLine($"\tAlpha flags: {texture.Data.AlphaFlags}");
                Console.WriteLine($"\tTexture format: {texture.Data.TextureFormat}");
                Console.WriteLine($"\tWidth: {texture.Data.Width}");
                Console.WriteLine($"\tHeight: {texture.Data.Height}");
                Console.WriteLine($"\tDepth: {texture.Data.Depth}");
                Console.WriteLine($"\tMipmap count: {texture.Data.MipMapCount}");
                Console.WriteLine($"\tTex code type: {texture.Data.TexCodeType}");
                Console.WriteLine($"\tFlags: {texture.Data.Flags}");

                Console.WriteLine($"\tPallette: ");
                Console.Write($"\t\t");
                foreach (byte palletteByte in texture.Data.Pallette)
                {
                    Console.Write($"{palletteByte.ToString("X2")}");
                }
                Console.WriteLine($"");

                Console.WriteLine($"\tData length: {texture.Data.Data.Length}");

                Console.WriteLine($"\tMip maps:");
                foreach (MipMap mipMap in texture.Data.MipMaps)
                {
                    Console.WriteLine($"\t\tSize: {mipMap.Size}");
                }

                Console.WriteLine($"");
                Directory.CreateDirectory("files/textures");

                string sanitizedName = texture.Data.TextureName.Replace("_", "").Replace("\0", "");
                string format = texture.Data.TextureFormatString;
                string path = $"files/textures/{sanitizedName}-{format}.dds";
                //File.WriteAllBytes(path, DdsHelper.GetDdsBytes(texture.Data.Data, texture.Data.TextureFormat, texture.Data.Width, texture.Data.Height));
                File.WriteAllBytes(path, texture.GetDds(true));
            }

        }

        private static void DffTest(string file = "./files/des_ufoinn.dff")
        {
            new DffFile(file).Write("./files/recreated.dff");

            //DffFile newFile = new DffFile("./files/newdes_ufoinn.dff");
            DffFile newFile = new DffFile(file);

            Console.WriteLine($"Type: {newFile.Dff.Header.Type}");
            Console.WriteLine($"Size: {newFile.Dff.Header.Size}");
            Console.WriteLine($"Marker: {newFile.Dff.Header.Marker} ({newFile.Dff.Header.Marker.ToString("X2")})");
            Console.WriteLine($"");

            Console.WriteLine($"Clump type: {newFile.Dff.Clump.Header.Type}");
            Console.WriteLine($"Clump size: {newFile.Dff.Clump.Header.Size}");
            Console.WriteLine($"Clump marker: {newFile.Dff.Clump.Header.Marker} ({newFile.Dff.Clump.Header.Marker.ToString("X2")})");

            Console.WriteLine($"Clump atomic count: {newFile.Dff.Clump.AtomicCount}");
            Console.WriteLine($"Clump light count: {newFile.Dff.Clump.LightCount}");
            Console.WriteLine($"Clump camera count: {newFile.Dff.Clump.CameraCount}");
            Console.WriteLine($"");

            //Console.WriteLine($"Frame list type: {newFile.Dff.Clump.FrameList.Header.Type}");
            //Console.WriteLine($"Frame list size: {newFile.Dff.Clump.FrameList.Header.Size}");
            //Console.WriteLine($"Frame list marker: {newFile.Dff.Clump.FrameList.Header.Marker} ({newFile.Dff.Clump.FrameList.Header.Marker.ToString("X2")})");
            Console.WriteLine($"Frame count: {newFile.Dff.Clump.FrameList.FrameCount}");

            foreach (Frame frame in newFile.Dff.Clump.FrameList.Frames)
            {
                Console.WriteLine($"\tRot 1: {frame.Rot1.X}, {frame.Rot1.Y}, {frame.Rot1.Z}");
                Console.WriteLine($"\tRot 2: {frame.Rot2.X}, {frame.Rot2.Y}, {frame.Rot2.Z}");
                Console.WriteLine($"\tRot 3: {frame.Rot3.X}, {frame.Rot3.Y}, {frame.Rot3.Z}");
                Console.WriteLine($"\tPosition: {frame.Position.X}, {frame.Position.Y}, {frame.Position.Z}");
                Console.WriteLine($"\tParent: {frame.Parent}");
                Console.WriteLine($"\tFlags: {frame.Flags}");

                Console.WriteLine($"");
            }

            foreach (Geometry geometry in newFile.Dff.Clump.GeometryList.Geometries)
            {
                Console.WriteLine($"\tFlags: {geometry.Flags}");
                Console.WriteLine($"\tColor count: {geometry.Colors.Count}");
                Console.WriteLine($"\tUV count: {geometry.TexCoords.Count}");
                Console.WriteLine($"\tTriangle count: {geometry.TriangleCount}");
                Console.WriteLine($"\tVertex count: {geometry.VertexCount}");
                Console.WriteLine($"\tMorph target count: {geometry.MorphTargetCount}");
                Console.WriteLine($"\tColors: ");
                foreach (Color color in geometry.Colors)
                {
                    //Console.WriteLine($"\t\tR: {color.R}, G: {color.G}, B: {color.B}, A: {color.A}");
                }

                Console.WriteLine($"\tUVs: ");
                foreach (Uv uv in geometry.TexCoords)
                {
                    //Console.WriteLine($"\t\tX: {uv.X}, Y: {uv.Y}");
                }

                Console.WriteLine($"\tTriangles: ");
                foreach (Triangle triangle in geometry.Triangles)
                {
                    //Console.WriteLine($"\t\tVertex 1: {triangle.VertexIndexOne}, Vertex 2: {triangle.VertexIndexTwo}, Vertex 3: {triangle.VertexIndexThree}, Material: {triangle.MaterialIndex}");
                }
                Console.WriteLine($"\tSphere: Position: ({geometry.Sphere.Position.X}, {geometry.Sphere.Position.Y}, {geometry.Sphere.Position.Z}), Radius: {geometry.Sphere.Radius}");

                Console.WriteLine($"\tMorph targets: ");
                foreach (var morphTarget in geometry.MorphTargets)
                {
                    Console.WriteLine($"\t\tMorph target vertex count: {morphTarget.Vertices.Count}");
                    Console.WriteLine($"\t\tMorph target normal count: {morphTarget.Normals.Count}");
                }

                Console.WriteLine($"\tMaterial count: {geometry.MaterialList.MaterialCount}");
                Console.WriteLine($"\tMaterials: ");
                foreach (Material material in geometry.MaterialList.Materials)
                {
                    Console.WriteLine($"\t\tColor: R: {material.Color.R}, G: {material.Color.G}, B: {material.Color.B}, A: {material.Color.A}");
                    Console.WriteLine($"\t\tTextured: {material.Textured}");
                    Console.WriteLine($"\t\tAmbient: {material.Ambient}");
                    Console.WriteLine($"\t\tSpecular: {material.Specular}");
                    Console.WriteLine($"\t\tDiffuse: {material.Diffuse}");

                    if (material.Textured > 0)
                    {
                        Console.WriteLine($"\t\tTexture filter mode: {material.Texture.FilterMode}");
                        Console.WriteLine($"\t\tTexture UV: {material.Texture.UvByte}");
                        Console.WriteLine($"\t\tTexture name: {material.Texture.Name.Value}");
                        Console.WriteLine($"\t\tTexture mask name: {material.Texture.MaskName.Value}");
                    }
                    Console.WriteLine($"");
                }
                Console.WriteLine($"");
            }
            Console.WriteLine($"Atomics:");
            foreach (Atomic atomic in newFile.Dff.Clump.Atomics)
            {
                Console.WriteLine($"\tFrame index: {atomic.FrameIndex}");
                Console.WriteLine($"\tGeometry index: {atomic.GeometryIndex}");
            }
            Console.WriteLine($"");
        }

        private static void ImgTest()
        {
            ImgFile imgFile = new ImgFile("./files/gta3.img");

            Console.WriteLine($"Version: {string.Join("", imgFile.Img.Version)}");
            Console.WriteLine($"Directory entry count: {imgFile.Img.ItemCount}");

            File.WriteAllBytes("files/output.ipl", imgFile.Img.DataEntries["vegasw_stream8.ipl"].Data);

            Console.WriteLine($"IPLs: ");
            foreach (string ipl in imgFile.Img.IplFiles)
            {
                Console.WriteLine($"\t{ipl}");
            }
        }

        private static void IdeTest()
        {
            IdeFile ideFile = new IdeFile("./files/LAe.ide");

            Console.WriteLine($"Objs: ");
            foreach (Obj obj in ideFile.Ide.Objs)
            {
                Console.WriteLine($"\tId: {obj.Id}");
                Console.WriteLine($"\tModel: {obj.ModelName}");
                Console.WriteLine($"\tTexture: {obj.TxdName}");
                Console.WriteLine($"\tMesh count: {obj.MeshCount}");
                Console.WriteLine($"");
            }
        }

        private static void IplTest()
        {
            IplFile iplFile = new IplFile("./files/LAe.ipl");

            Console.WriteLine($"Insts: ");
            foreach (Inst inst in iplFile.Ipl.Insts)
            {
                Console.WriteLine($"\tId: {inst.Id}");
                Console.WriteLine($"\tModel: {inst.ModelName}");
                Console.WriteLine($"\tInterior: {inst.Interior}");

                Console.WriteLine($"\tPosition: {inst.Position.X}, {inst.Position.Y}, {inst.Position.Z}");
                Console.WriteLine($"\tRotation: {inst.Rotation.X}, {inst.Rotation.Y}, {inst.Rotation.Z}, {inst.Rotation.W}");

                Console.WriteLine($"");
            }
        }

        private static void DatTest()
        {
            DatFile datFile = new DatFile("./files/gta.dat");

            Console.WriteLine($"IPL: ");
            foreach (string ipl in datFile.Dat.Ipls)
            {
                Console.WriteLine($"\t{ipl}");
            }
            Console.WriteLine($"");

            Console.WriteLine($"IDE: ");
            foreach (string ide in datFile.Dat.Ides)
            {
                Console.WriteLine($"\t{ide}");
            }
        }

        private static void BinaryIplTest()
        {
            BinaryIplFile iplFile = new BinaryIplFile("./files/output.ipl");

            Console.WriteLine($"Type: {string.Join("", iplFile.BinaryIpl.Header.Type)}");
            Console.WriteLine($"Instance count: {iplFile.BinaryIpl.Header.InstanceCount}");
            Console.WriteLine($"Car count: {iplFile.BinaryIpl.Header.CarCount}");
            Console.WriteLine($"Instance offset: {iplFile.BinaryIpl.Header.ItemOffset}");

            Console.WriteLine($"Insts: ");
            foreach (BinaryInst inst in iplFile.BinaryIpl.Insts)
            {
                Console.WriteLine($"\tId: {inst.Id}");
                Console.WriteLine($"\tModel: {inst.ModelName}");
                Console.WriteLine($"\tInterior: {inst.Interior}");

                Console.WriteLine($"\tPosition: {inst.Position.X}, {inst.Position.Y}, {inst.Position.Z}");
                Console.WriteLine($"\tRotation: {inst.Rotation.X}, {inst.Rotation.Y}, {inst.Rotation.Z}, {inst.Rotation.W}");
                Console.WriteLine($"\tLod: {inst.Lod}");

                Console.WriteLine($"");
            }
        }

        private static void ImgDffTest()
        {
            ImgFile imgFile = new ImgFile("./files/gta3.img");
            DffFile dffFile = new DffFile(imgFile.Img.DataEntries["SFNvilla001_CM.dff".ToLower()].Data);
            Console.WriteLine(JsonConvert.SerializeObject(dffFile));

            Console.WriteLine($"Type: {dffFile.Dff.Header.Type}");
            Console.WriteLine($"Size: {dffFile.Dff.Header.Size}");
            Console.WriteLine($"Marker: {dffFile.Dff.Header.Marker} ({dffFile.Dff.Header.Marker.ToString("X2")})");
            Console.WriteLine($"");

            Console.WriteLine($"Clump type: {dffFile.Dff.Clump.Header.Type}");
            Console.WriteLine($"Clump size: {dffFile.Dff.Clump.Header.Size}");
            Console.WriteLine($"Clump marker: {dffFile.Dff.Clump.Header.Marker} ({dffFile.Dff.Clump.Header.Marker.ToString("X2")})");

            Console.WriteLine($"Clump atomic count: {dffFile.Dff.Clump.AtomicCount}");
            Console.WriteLine($"Clump light count: {dffFile.Dff.Clump.LightCount}");
            Console.WriteLine($"Clump camera count: {dffFile.Dff.Clump.CameraCount}");
            Console.WriteLine($"");

            //Console.WriteLine($"Frame list type: {newFile.Dff.Clump.FrameList.Header.Type}");
            //Console.WriteLine($"Frame list size: {newFile.Dff.Clump.FrameList.Header.Size}");
            //Console.WriteLine($"Frame list marker: {newFile.Dff.Clump.FrameList.Header.Marker} ({newFile.Dff.Clump.FrameList.Header.Marker.ToString("X2")})");
            Console.WriteLine($"Frame count: {dffFile.Dff.Clump.FrameList.FrameCount}");

            foreach (Frame frame in dffFile.Dff.Clump.FrameList.Frames)
            {
                Console.WriteLine($"\tRot 1: {frame.Rot1.X}, {frame.Rot1.Y}, {frame.Rot1.Z}");
                Console.WriteLine($"\tRot 2: {frame.Rot2.X}, {frame.Rot2.Y}, {frame.Rot2.Z}");
                Console.WriteLine($"\tRot 3: {frame.Rot3.X}, {frame.Rot3.Y}, {frame.Rot3.Z}");
                Console.WriteLine($"\tPosition: {frame.Position.X}, {frame.Position.Y}, {frame.Position.Z}");
                Console.WriteLine($"\tParent: {frame.Parent}");
                Console.WriteLine($"\tFlags: {frame.Flags}");

                Console.WriteLine($"");
            }

            Console.WriteLine($"Geometry count: {dffFile.Dff.Clump.GeometryList.GeometryCount}");
            foreach (Geometry geometry in dffFile.Dff.Clump.GeometryList.Geometries)
            {
                Console.WriteLine($"\tFlags: {geometry.Flags}");
                Console.WriteLine($"\tTriangle count: {geometry.TriangleCount}");
                Console.WriteLine($"\tVertex count: {geometry.VertexCount}");
                Console.WriteLine($"\tMorph target count: {geometry.MorphTargetCount}");
                Console.WriteLine($"\tColors: ");
                foreach (Color color in geometry.Colors)
                {
                    Console.WriteLine($"\t\tR: {color.R}, G: {color.G}, B: {color.B}, A: {color.A}");
                }

                Console.WriteLine($"\tUVs: ");
                foreach (Uv uv in geometry.TexCoords)
                {
                    Console.WriteLine($"\t\tX: {uv.X}, Y: {uv.Y}");
                }

                Console.WriteLine($"\tTriangles: ");
                foreach (Triangle triangle in geometry.Triangles)
                {
                    Console.WriteLine($"\t\tVertex 1: {triangle.VertexIndexOne}, Vertex 2: {triangle.VertexIndexTwo}, Vertex 3: {triangle.VertexIndexThree}, Material: {triangle.MaterialIndex}");
                }
                Console.WriteLine($"\tSphere: Position: ({geometry.Sphere.Position.X}, {geometry.Sphere.Position.Y}, {geometry.Sphere.Position.Z}), Radius: {geometry.Sphere.Radius}");

                Console.WriteLine($"\tMaterials: ");
                foreach (Material material in geometry.MaterialList.Materials)
                {
                    Console.WriteLine($"\t\tColor: R: {material.Color.R}, G: {material.Color.G}, B: {material.Color.B}, A: {material.Color.A}");
                    Console.WriteLine($"\t\tTextured: {material.Textured}");
                    Console.WriteLine($"\t\tAmbient: {material.Ambient}");
                    Console.WriteLine($"\t\tSpecular: {material.Specular}");
                    Console.WriteLine($"\t\tDiffuse: {material.Diffuse}");

                    if (material.Textured > 0)
                    {
                        Console.WriteLine($"\t\tTexture filter mode: {material.Texture.FilterMode}");
                        Console.WriteLine($"\t\tTexture UV: {material.Texture.UvByte}");
                        Console.WriteLine($"\t\tTexture name: {material.Texture.Name.Value}");
                        Console.WriteLine($"\t\tTexture mask name: {material.Texture.MaskName.Value}");
                    }
                    Console.WriteLine($"");
                }
                Console.WriteLine($"");
            }
            Console.WriteLine($"Atomics:");
            foreach (Atomic atomic in dffFile.Dff.Clump.Atomics)
            {
                Console.WriteLine($"\tFrame index: {atomic.FrameIndex}");
                Console.WriteLine($"\tGeometry index: {atomic.GeometryIndex}");
            }
            Console.WriteLine($"");
        }

        private static void ImgBinaryIplTest()
        {
            ImgFile imgFile = new ImgFile(@"G:\Users\Bob Desktop\Desktop\MTA\files\stargate\horizon of the universe\stargate\models/gta3.img");
            foreach (var kvPair in imgFile.Img.DataEntries)
            {
                if (kvPair.Key.EndsWith(".ipl"))
                {
                    BinaryIplFile iplFile = new BinaryIplFile(kvPair.Value.Data);

                    Console.WriteLine($"Type: {string.Join("", iplFile.BinaryIpl.Header.Type)}");
                    Console.WriteLine($"Instance count: {iplFile.BinaryIpl.Header.InstanceCount}");
                    Console.WriteLine($"Car count: {iplFile.BinaryIpl.Header.CarCount}");
                    Console.WriteLine($"Instance offset: {iplFile.BinaryIpl.Header.ItemOffset}");

                    Console.WriteLine($"Insts: ");
                    foreach (BinaryInst inst in iplFile.BinaryIpl.Insts.Where(i => i.Id > 18630))
                    {
                        Console.WriteLine($"\tId: {inst.Id}");
                        Console.WriteLine($"\tModel: {inst.ModelName}");
                        Console.WriteLine($"\tInterior: {inst.Interior}");

                        Console.WriteLine($"\tPosition: {inst.Position.X}, {inst.Position.Y}, {inst.Position.Z}");
                        Console.WriteLine($"\tRotation: {inst.Rotation.X}, {inst.Rotation.Y}, {inst.Rotation.Z}, {inst.Rotation.W}");
                        Console.WriteLine($"\tLod: {inst.Lod}");

                        Console.WriteLine($"");
                    }
                }
            }
        }

        private static void CreateColForDff(string input)
        {
            var dffFile = new DffFile(input);
            var dff = dffFile.Dff;

            var colFile = new ColFile();
            var col = colFile.Col;

            col.ColCombos = new List<ColCombo>(){
                new ColCombo()
                {
                    Header = new Header(),
                    Body = new Body()
                    {
                        Spheres = new List<Sphere>(),
                        Boxes = new List<Box>(),
                        Vertices = dff.Clump.GeometryList.Geometries.First().MorphTargets.First().Vertices
                            .Select(vertex => new Vertex()
                            {
                                FirstFloat = vertex.X,
                                SecondFloat = vertex.Y,
                                ThirdFloat = vertex.Z,
                            })
                            .ToList(),
                        FaceGroups = new List<FaceGroup>(),
                        FaceGroupCount = 0,
                        Faces = dff.Clump.GeometryList.Geometries.First().Triangles
                            .Select(triangle => new Face()
                            {
                                A = triangle.VertexIndexOne,
                                B = triangle.VertexIndexTwo,
                                C = triangle.VertexIndexThree,
                                Light = 0,
                                Material = 0
                            })
                            .ToList(),
                        ShadowMeshVertices = new List<Vertex>(),
                        ShadowMeshFaces = new List<Face>()
                    }
                }
            };

            //var x = new ColFile("./files/3x3.col");
            colFile.Write(input.Replace(".dff", ".col"));
        }

        private static void GenerateDff()
        {
            DffFile file = new DffFile();
            var dff = file.Dff;

            var geometry = new Geometry();
            var morphTarget = new MorphTarget();

            geometry.MaterialList.Materials.Add(new Material()
            {
                Texture = new Texture()
                {
                    Name = new StringBlock()
                    {
                        Value = "generatedtexture\0"
                    },
                    MaskName = new StringBlock()
                    {
                        Value = "generatedmask\0"
                    },
                },
                Textured = 1,
                Color = new Color() { R = 200, G = 0, B = 200, A = 150 }
            });

            var xSize = 3;
            var ySize = 3;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    morphTarget.Vertices.Add(new Vector3(x, y, x * y));
                    morphTarget.Normals.Add(new Vector3(0, 0, 1));
                    geometry.TexCoords.Add(new Uv() { X = x, Y = y });
                }
            }


            var min = new Vector3(morphTarget.Vertices.Min(v => v.X), morphTarget.Vertices.Min(v => v.Y), morphTarget.Vertices.Min(v => v.Z));
            var max = new Vector3(morphTarget.Vertices.Max(v => v.X), morphTarget.Vertices.Max(v => v.Y), morphTarget.Vertices.Max(v => v.Z));
            var center = min + (max - min) * 0.5f;
            var radius = morphTarget.Vertices.Max(vertex => (new Vector3(vertex.X, vertex.Y, vertex.Z) - center).Length());
            geometry.Flags = 0x76;// 0x14;
            geometry.Sphere = new RenderWareIo.Structs.Dff.Sphere()
            {
                Position = center,
                Radius = radius
            };

            for (int x = 0; x < xSize - 1; x++)
            {
                for (int y = 0; y < ySize - 1; y++)
                {
                    geometry.Triangles.Add(new Triangle()
                    {
                        VertexIndexOne = (ushort)(y * xSize + x),
                        VertexIndexTwo = (ushort)(y * xSize + x + 1),
                        VertexIndexThree = (ushort)(y * xSize + x + 1 + xSize),
                        MaterialIndex = 0
                    });
                    geometry.Triangles.Add(new Triangle()
                    {
                        VertexIndexOne = (ushort)(y * xSize + x + 1 + xSize),
                        VertexIndexTwo = (ushort)(y * xSize + x + xSize),
                        VertexIndexThree = (ushort)(y * xSize + x),
                        MaterialIndex = 0
                    });
                }
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

            file.Write("./files/output.dff");
            //DffTest("./files/output.dff");
            //CreateColForDff("./files/output.dff");
        }

        private static void GenerateTxd()
        {
            TxdFile sourceFile = new TxdFile($"./files/moneystack.txd");

            TxdFile newTxd = new TxdFile();

            var textureData = new TextureData()
            {
                TextureName = "textureName",
                TextureFormatString = "DXT1"
            };
            textureData.SetDds(sourceFile.Txd.TextureContainer.Textures.First().GetDds(true), true);

            newTxd.Txd.TextureContainer.Textures.Add(new RenderWareIo.Structs.Txd.Texture()
            {
                Data = textureData,
            });
            newTxd.Write("./files/generated.txd");
        }

        private static void FindHighLodDistances(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.ide", SearchOption.AllDirectories);

            var objs = new List<Obj>();

            foreach (var idePath in files)
            {
                var ide = new IdeFile(idePath);
                objs = objs.Concat(ide.Ide.Objs).ToList();
            }

            objs.Sort((a, b) => (int)(b.DrawDistances.Max() - a.DrawDistances.Max()));

            for (int i = 0; i < 2500; i++)
            {
                Console.WriteLine($"{i}: {objs[i].Id} -> {objs[i].DrawDistances.Max()}");
            }
        }

        static void ExportImg(string input, string output)
        {
            var imgFile = new ImgFile(input);
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            foreach (var entry in imgFile.Img.DirectoryEntries)
            {
                var data = imgFile.Img.DataEntries[entry.Name.ToLower()];
                File.WriteAllBytes($"{output}/{entry.Name}", data.Data);
            }
        }

        static void BinMeshTest(string input)
        {
            var dffFile = new DffFile(input);
            var dff = dffFile.Dff;
        }

        static void ScaleDff(string input, string output, float scaleFactor)
        {

        }

        static void ScaleCol(string input, string output, float scaleFactor)
        {

        }

        static void GroupColTest(string imgPath, string colName)
        {
            using var imgFile = new ImgFile(imgPath);
            var img = imgFile.Img;
            var col = new ColFile(img.DataEntries[colName].Data).Col;
        }

        static void ScanColsFor(string imgPath, string colName)
        {

            using var imgFile = new ImgFile(imgPath);
            var img = imgFile.Img;
            foreach (var entry in img.DataEntries)
            {
                if (entry.Key.EndsWith(".col"))
                {
                    try
                    {
                        var col = new ColFile(entry.Value.Data).Col;
                        if (col.ColCombos.Any(x => string.Concat(x.Header.Name).StartsWith(colName, StringComparison.OrdinalIgnoreCase)))
                        {
                            Console.WriteLine(entry.Key);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        private static void CarIdeFileTest()
        {
            IdeFile ideFile = new(@"D:\SteamLibrary\steamapps\common\Grand Theft Auto San Andreas\data\vehicles.ide");

            Console.WriteLine($"vehicles: ");
            foreach (var car in ideFile.Ide.Cars)
            {
                Console.WriteLine($"\tId: {car.Id}");
                Console.WriteLine($"\tModel: {car.ModelName}");
                Console.WriteLine($"\tTexture: {car.TxdName}");
                Console.WriteLine($"");
            }
        }

        private static void WeaponIdeFileTest()
        {
            IdeFile ideFile = new(@"E:\Game Library\steamapps\common\Grand Theft Auto San Andreas\data\default.ide");

            Console.WriteLine($"guns: ");
            foreach (var gun in ideFile.Ide.Weapons)
            {
                Console.WriteLine($"\tId: {gun.Id}");
                Console.WriteLine($"\tModel: {gun.ModelName}");
                Console.WriteLine($"\tTexture: {gun.TxdName}");
                Console.WriteLine($"");
            }
        }

        private static void WeaponDatFileTest()
        {
            DatFile datFile = new(@"E:\Game Library\steamapps\common\Grand Theft Auto San Andreas\data\weapon.dat");

            Console.WriteLine($"guns: ");
            foreach (var gun in datFile.Dat.Guns)
            {
                Console.WriteLine($"\tWeapon: {gun.WeaponType}");
                Console.WriteLine($"\tModel: {gun.Model1Id}");
                Console.WriteLine($"");
            }
            Console.WriteLine($"melee: ");
            foreach (var weapon in datFile.Dat.MeleeWeapons)
            {
                Console.WriteLine($"\tWeapon: {weapon.WeaponType}");
                Console.WriteLine($"\tModel: {weapon.Model1Id}");
                Console.WriteLine($"");
            }
        }

        public static byte[] StreamToByteArray(Stream input)
        {
            using (var memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static void PrelitModel()
        {
            var renderWareBuilder = new RenderWareBuilders.RenderWareBuilder();
            var material = new RenderWareBuilders.Material
            {
                Name = "Metal1_128",
                Color = System.Drawing.Color.White,
                MaskName = "",
            };
            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(0, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 0),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Vertex2 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(0, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 5),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Vertex3 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(5, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 0),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Material = material,
            });
            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(5, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 0),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Vertex2 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(0, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 5),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Vertex3 = renderWareBuilder.AddPrelitVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(5, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 5),
                    Day = System.Drawing.Color.Red,
                    Night = System.Drawing.Color.Blue,
                }),
                Material = material,
            });
            renderWareBuilder.AddMaterial(material);
            var dff = renderWareBuilder.BuildDff();
            using var stream = new MemoryStream();
            dff.Write(stream);
            stream.Position = 0;
            var byteArray = StreamToByteArray(stream);
            File.WriteAllBytes("testPrelitModel.dff", byteArray);
        }

        private static void CollisionMaterial()
        {
            var renderWareBuilder = new RenderWareBuilders.RenderWareBuilder();
            var material1 = new RenderWareBuilders.Material
            {
                Name = "Metal1_128",
                Color = System.Drawing.Color.White,
                MaskName = "",
            };
            var material2 = new RenderWareBuilders.Material
            {
                Name = "redmetal",
                Color = System.Drawing.Color.White,
                MaskName = "",
            };
            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = renderWareBuilder.AddVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(0, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 0),
                }),
                Vertex2 = renderWareBuilder.AddVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(0, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 5),
                }),
                Vertex3 = renderWareBuilder.AddVertex(new RenderWareBuilders.PrelitVertex
                {
                    Position = new Vector3(5, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 0),
                }),
                Material = material1,
            });
            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
                {
                    Position = new Vector3(5, 0, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 0),
                }),
                Vertex2 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
                {
                    Position = new Vector3(0, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(0, 5),
                }),
                Vertex3 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
                {
                    Position = new Vector3(5, 5, 0),
                    Normal = Vector3.UnitZ,
                    Uv = new Vector2(5, 5),
                }),
                Material = material2,
            });
            renderWareBuilder.AddMaterial(material1);
            renderWareBuilder.AddMaterial(material2);
            renderWareBuilder.SetMaterialCollisionMaterialId(material1.Name, MaterialId.Glass);
            renderWareBuilder.SetMaterialCollisionMaterialId(material2.Name, MaterialId.WoodThin);
            {
                var dff = renderWareBuilder.BuildDff();
                using var stream = new MemoryStream();
                dff.Write(stream);
                stream.Position = 0;
                var byteArray = StreamToByteArray(stream);
                File.WriteAllBytes("collisionMaterial.dff", byteArray);
            }
            {
                var col = renderWareBuilder.BuildCol();
                using var stream = new MemoryStream();
                col.Write(stream);
                stream.Position = 0;
                var byteArray = StreamToByteArray(stream);
                File.WriteAllBytes("collisionMaterial.col", byteArray);
            }
        }

        private static void DebugViewTest()
        {
            var renderWareBuilder = new RenderWareBuilders.RenderWareBuilder();
            var material1 = new RenderWareBuilders.Material
            {
                Name = "Metal1_128",
                Color = System.Drawing.Color.White,
                MaskName = "",
            };
            var material2 = new RenderWareBuilders.Material
            {
                Name = "redmetal",
                Color = System.Drawing.Color.White,
                MaskName = "",
            };

            var Vertex1 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
            {
                Position = new Vector3(0, 0, 0),
                Normal = Vector3.UnitZ,
                Uv = new Vector2(0, 0),
            });
            var Vertex2 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
            {
                Position = new Vector3(0, 5, 0),
                Normal = Vector3.UnitZ,
                Uv = new Vector2(0, 5),
            });
            var Vertex3 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
            {
                Position = new Vector3(5, 0, 0),
                Normal = Vector3.UnitZ,
                Uv = new Vector2(5, 0),
            });
            var Vertex4 = renderWareBuilder.AddVertex(new RenderWareBuilders.Vertex
            {
                Position = new Vector3(5, 5, 0),
                Normal = Vector3.UnitZ,
                Uv = new Vector2(5, 5),
            });

            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = Vertex1,
                Vertex2 = Vertex2,
                Vertex3 = Vertex3,
                Material = material1,
            });
            renderWareBuilder.AddTriangle(new RenderWareBuilders.Triangle
            {
                Vertex1 = Vertex2,
                Vertex2 = Vertex3,
                Vertex3 = Vertex4,
                Material = material2,
            });

            var debugView = new MtaDebugView(renderWareBuilder);
            File.WriteAllText("debugWireframe.lua", debugView.Wireframe);
            File.WriteAllText("debugVertices.lua", debugView.Vertices);
        }

        private static void ConvertObj()
        {
            void convertModel(string fileName)
            {
                using var inputFileStream = File.OpenRead(fileName);
                var dff = WavefrontConverter.Convert(inputFileStream);
                using var stream = new MemoryStream();
                dff.Write(stream);
                stream.Position = 0;
                var byteArray = StreamToByteArray(stream);
                File.WriteAllBytes($"{fileName}.dff", byteArray);
            }
            convertModel("cube.oobj");
            convertModel("monkey.oobj");
        }

        static void Main(string[] args)
        {
            //ImgTest();
            //TxdTest();
            //BinaryIplTest();
            //ImgDffTest();
            //var files = Directory.GetFiles(@"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\Vessels", "*.dff");
            //foreach (var file in files)
            //{ 
            //    CreateColForDff(file);
            //}
            //CreateColForDff(@"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\Gates\Milkyway\Innerring.dff");
            //CreateColForDff(@"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\Dhd\dhdstripped.dff");
            //DffTest("./files/3x3.dff");
            //GenerateDff();
            //ImgBinaryIplTest();
            //DffTest();
            //GenerateTxd();
            //DffTest(@"C:\Users\Bobva\Downloads\bob.dff");
            //DffTest(@"C:\Users\Bobva\Downloads\generated.dff");
            //FindHighLodDistances(@"E:\Game Library\steamapps\common\Grand Theft Auto San Andreas");
            //CreateColForDff("");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\models\stargate.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\stargate");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level21.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level21");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level22.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level22");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level23.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level23");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level24.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level24");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level25.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level25");
            //ExportImg(
            //    @"C:\Users\Bobva\Downloads\GTA_Amazing_Stargate\GTA Amazing Stargate\stargate\sgc\Level27-28.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\sgc\Level27-28");
            //ExportImg(
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\planets\abydos.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\planets");
            //ExportImg(
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\planets\Planets.img",
            //    @"C:\Program Files (x86)\MTA San Andreas 1.5\server\mods\deathmatch\resources\[slipe]\stargate\Assets\Mods\AmazingStargate\planets");
            //ExportImg(
            //    @"C:\Program Files (x86)\Rockstar Games\GTA San AndreasAtlantis\models\Atlantis.img",
            //    @"C:\Program Files (x86)\Rockstar Games\GTA San AndreasAtlantis\models\Atlantis");
            //ExportImg(
            //    @"C:\Program Files (x86)\Rockstar Games\GTA San AndreasAtlantis\models\Mainland.img",
            //    @"C:\Program Files (x86)\Rockstar Games\GTA San AndreasAtlantis\models\Mainland");

            //BinMeshTest(@"D:\code\Unity\Project Abydos\Assets\Game\Models\SGC\Level27-28\lab_telephone3.dff");
            //GroupColTest(@"D:\SteamLibrary\steamapps\common\Grand Theft Auto San Andreas Server\models\gta3.img", "countn2_20.col");
            //ScanColsFor(@"D:\SteamLibrary\steamapps\common\Grand Theft Auto San Andreas Server\models\gta3.img", "des_ufoinn");
            //ScanColsFor(@"D:\SteamLibrary\steamapps\common\Grand Theft Auto San Andreas Server\models\gta_int.img", "pinetree05");
            //Console.WriteLine("Press any key to quit...");

            //WeaponIdeFileTest();
            //WeaponDatFileTest();
            //CarIdeFileTest();
            //PrelitModel();
            //CollisionMaterial();
            //DebugViewTest();
            ConvertObj();
        }
    }
}
