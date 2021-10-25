using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderWareIo.ReadWriteHelpers
{
    public static class DdsHelper
    {
        public static byte[] GetDdsBytes(byte[] data, byte[] fourCC, uint width, uint height, byte[][] mipmaps = null)
        {
            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);


            byte[] mipMapCountBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            byte[] unknownSet = new byte[] { 0x00, 0x10, 0x00, 0x00 };
            byte mipMapFlagByte = 0x00;

            if (mipmaps != null && mipmaps.Length > 1)
            {
                int mipMapCount = mipmaps.Length - 1;
                unknownSet = new byte[] { 0x08, 0x10, 0x40, 0x00 };
                mipMapFlagByte = 0x02;

                foreach (byte[] mipmap in mipmaps)
                {
                    if (mipmap.Length == 0)
                    {
                        mipMapCount--;
                    } else
                    {
                        data = data.Concat(mipmap).ToArray();
                        //if (mipmap.Length % 8 != 0)
                        //{
                        //    data = data.Concat(new byte[8 - (mipmap.Length % 8)]).ToArray();
                        //}
                    }
                }
                //data = data.Concat(new byte[width == height ? 16 : 24]).ToArray();
                mipMapCountBytes = BitConverter.GetBytes(mipMapCount);
            }

            byte[] header = new byte[]
            {
                0x44,0x44,0x53,0x20,0x7c,0x00,0x00,0x00,0x07,0x10,mipMapFlagByte,0x00, heightBytes[0], heightBytes[1], heightBytes[2], heightBytes[3],
                widthBytes[0], widthBytes[1], widthBytes[2], widthBytes[3],0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,mipMapCountBytes[0],mipMapCountBytes[1],mipMapCountBytes[2],mipMapCountBytes[3],
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x20,0x00,0x00,0x00,
                0x04,0x00,0x00,0x00,fourCC[0],fourCC[1],fourCC[2],fourCC[3],0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,unknownSet[0],unknownSet[1],unknownSet[2],unknownSet[3],
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            };

            return header.Concat(data).ToArray();
        }

        public static byte[] GetDdsBytes(byte[] data, uint fourCC, uint width, uint height, byte[][] mipmaps = null)
        {
            return GetDdsBytes(data, BitConverter.GetBytes(fourCC), width, height, mipmaps);
        }

        public static byte[] StripDdsHeader(byte[] data)
        {
            return data.Skip(128).ToArray();
        }

        public static byte[] StripDdsHeader(byte[] data, out uint fourCC, out uint width, out uint height)
        {
            fourCC = BitConverter.ToUInt32(data.Skip(84).Take(4).ToArray(), 0);
            width = BitConverter.ToUInt32(data.Skip(16).Take(4).ToArray(), 0);
            height = BitConverter.ToUInt32(data.Skip(12).Take(4).ToArray(), 0);
            return data.Skip(128).ToArray();
        }

        public static byte[] StripDdsHeader(byte[] data, out byte[] fourCC, out uint width, out uint height)
        {
            fourCC = data.Skip(84).Take(4).ToArray();
            width = BitConverter.ToUInt32(data.Skip(16).Take(4).ToArray(), 0);
            height = BitConverter.ToUInt32(data.Skip(12).Take(4).ToArray(), 0);
            return data.Skip(128).ToArray();
        }
    }
}
