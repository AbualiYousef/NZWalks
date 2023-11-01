using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NZWalksDbContext _context;

    public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,
        NZWalksDbContext context)
    {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor= httpContextAccessor;
            _context = context;
    }
    public async Task<Image> Upload(Image image)
    {
        //Create a local file path
        var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
            $"{image.FileName}{image.FileExtension}");
        //Create a file stream
        await using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);
        //https://localhost:5001/Images/FileName.FileExtension
        //Create a URL path
        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
        image.FilePath = urlFilePath;
        //Add the image to the database
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
        //Return the image
        return image;
    }
}