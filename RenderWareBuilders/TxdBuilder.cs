using BCnEncoder.Encoder;
using RenderWareIo.Structs.Txd;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using BCnEncoder.ImageSharp;
using System.Collections.Generic;
using System.IO;

namespace RenderWareBuilders
{
    public class TxdBuilder
    {
        private readonly Dictionary<string, Image<Rgba32>> images = [];

        public TxdBuilder AddImage(string name, string fileName)
        {
            images[name] = Image<Rgba32>.Load<Rgba32>(fileName);

            return this;
        }
        
        public TxdBuilder AddImage(string name, Image<Rgba32> image)
        {
            images[name] = image;

            return this;
        }

        public Txd Build()
        {
            var txd = new Txd();

            foreach (var (name, image) in images)
            {
                var isTransparent = false;
                for (int x = 0; x < image.Width; x++)
                    for (int y = 0; y < image.Height; y++)
                    {
                        if (image[x, y].A == 0)
                        {
                            image[x, y] = new Rgba32(0, 0, 0, 0);
                            isTransparent = true;
                            break;
                        }
                    }

                var encoder = new BcEncoder();
                encoder.OutputOptions.GenerateMipMaps = false;
                encoder.OutputOptions.Quality = CompressionQuality.BestQuality;
                encoder.OutputOptions.Format = isTransparent ? BCnEncoder.Shared.CompressionFormat.Bc1WithAlpha : BCnEncoder.Shared.CompressionFormat.Bc1;
                encoder.OutputOptions.FileFormat = BCnEncoder.Shared.OutputFileFormat.Dds;

                var stream = new MemoryStream();
                encoder.EncodeToStream(image, stream);
                stream.Position = 0;
                var reader = new BinaryReader(stream);

                var textureData = new TextureData()
                {
                    TextureName = name,
                    TextureFormatString = "DXT1"
                };
                textureData.SetDds(reader.ReadBytes((int)stream.Length), false);

                if (isTransparent)
                {
                    textureData.AlphaFlags = 0x0100;
                    textureData.Flags |= 0x01;
                }

                txd.TextureContainer.Textures.Add(new Texture()
                {
                    Data = textureData,
                });
            }

            return txd;
        }
    }
}