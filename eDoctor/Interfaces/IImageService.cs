namespace eDoctor.Interfaces;

public interface IImageService
{
    Task<byte[]> ResizeAsync(byte[] file);
}
