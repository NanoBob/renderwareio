using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Body
    {
        public List<Sphere> Spheres { get; set; }
        public List<Box> Boxes { get; set; }
        public List<Vertex> Vertices { get; set; }
        // padding

        public List<FaceGroup> FaceGroups { get; set; }
        public uint FaceGroupCount { get; set; }
        public List<Face> Faces { get; set; }

        public List<Vertex> ShadowMeshVertices { get; set; }
        // padding
        public List<Face> ShadowMeshFaces { get; set; }

        public uint Size => (uint)(
            this.Spheres.Sum(item => item.Size) + 
            this.Boxes.Sum(item => item.Size) + 
            this.Vertices.Sum(item => item.Size) + 
            this.FaceGroups.Sum(item => item.Size) + 
            (FaceGroupCount > 0 ? 4 : 0) +
            this.Faces.Sum(item => item.Size) + 
            this.ShadowMeshVertices.Sum(item => item.Size) + 
            this.ShadowMeshFaces.Sum(item => item.Size));
        
        private static List<T> ReadBinaryStructure<T>(Stream stream, int count) where T: IBinaryStructure<T>, new()
        {
            List<T> list = new List<T>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new T().Read(stream));
            }

            return list;
        }

        private static void WriteBinaryStructure<T>(Stream stream, List<T> binaryStructures) where T: IBinaryStructure<T>
        {
            foreach(T structure in binaryStructures)
            {
                structure.Write(stream);
            }
        }

        public Body Read(Stream stream, Header header, long start)
        {
            int shadowVertexCount = 0;

            var spheres = ReadBinaryStructure<Sphere>(stream, header.SphereCount);
            var boxes = ReadBinaryStructure<Box>(stream, header.BoxCount);

            var vertexStart = stream.Position;
            stream.Position = start + header.FaceOffset + 4;
            var faces = ReadBinaryStructure<Face>(stream, header.FaceCount);
            stream.Position = start + header.VertexOffset + 4;

            int vertexCount = faces.Any() ? faces.Max(x => Math.Max(Math.Max(x.A, x.B), x.C)) + 1 : 0;
            var vertices = ReadBinaryStructure<Vertex>(stream, vertexCount);

            if ((vertexCount * 6) % 4 != 0 && stream.Position != stream.Length)
            {
                _ = RenderWareFileHelper.ReadChars(stream, 2);
            }

            uint faceGroupCount = 0;
            var faceGroups = new List<FaceGroup>();

            if ((header.Flags & 8) != 0)
            {
                faceGroupCount = RenderWareFileHelper.ReadUint32(stream); // switch ???
                faceGroups = ReadBinaryStructure<FaceGroup>(stream, 0);// (int)faceGroupCount); // switch ???
            }


            this.Spheres = spheres;
            this.Boxes = boxes;
            this.Vertices = vertices;
            this.FaceGroupCount = faceGroupCount;
            this.FaceGroups = faceGroups;
            this.Faces = faces;

            if (header.ColVersion >= 3 )
            {
                if (header.ShadowMeshFaceCount > 0)
                {
                    stream.Position = start + header.ShadowMeshVertexOffset + 4;
                }
                var shadowVertices = ReadBinaryStructure<Vertex>(stream, shadowVertexCount);
                if ((shadowVertexCount * 6) % 4 != 0 && stream.Position != stream.Length)
                {
                    _ = RenderWareFileHelper.ReadChars(stream, 2);
                }
                var shadowFaces = ReadBinaryStructure<Face>(stream, (int)header.ShadowMeshFaceCount);

                this.ShadowMeshVertices = shadowVertices;
                this.ShadowMeshFaces = shadowFaces;
            }

            return this;
        }

        public void Write(Stream stream, Header header)
        {
            WriteBinaryStructure(stream, this.Spheres);
            WriteBinaryStructure(stream, this.Boxes);
            WriteBinaryStructure(stream, this.Vertices);

            if ((this.Vertices.Count * 6) % 4 != 0 && stream.Position != stream.Length)
            {
                RenderWareFileHelper.WriteChars(stream, new char[] { '\0', '\0' });
            }

            if (this.FaceGroupCount > 0)
            {
                RenderWareFileHelper.WriteUint32(stream, this.FaceGroupCount);
                WriteBinaryStructure(stream, this.FaceGroups);
            }

            WriteBinaryStructure(stream, this.Faces);

            if (header.ColVersion >= 3)
            {
                WriteBinaryStructure(stream, this.ShadowMeshVertices);

                if ((this.ShadowMeshVertices.Count * 6) % 4 != 0 && stream.Position != stream.Length)
                {
                    RenderWareFileHelper.WriteChars(stream, new char[] { '\0', '\0' });
                }

                WriteBinaryStructure(stream, this.ShadowMeshFaces);
            }
        }
    }
}
