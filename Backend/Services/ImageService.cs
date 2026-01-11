using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public interface IImageService
{
    Task<string?> UploadImageAsync(IFormFile file, string folder);
}

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string?> UploadImageAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            return null;

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folder
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }
}
