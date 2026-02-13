using eDoctor.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace eDoctor.Services;

public class ImageService : IImageService
{
    public async Task<byte[]> ResizeAsync(byte[] file)
    {
        using MemoryStream inputStream = new MemoryStream(file);
        using Image image = await Image.LoadAsync(inputStream);

        image.Mutate(x => x.Resize(576, 768));

        using MemoryStream outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, new PngEncoder
        {
            CompressionLevel = PngCompressionLevel.BestCompression,
            FilterMethod = PngFilterMethod.Adaptive
        });

        return outputStream.ToArray();
    }
}
