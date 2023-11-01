using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        //POST: api/images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDto request)
        {
            // Validate the request
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                //Convert DTO to DomainModel
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.FileName),    
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };
                //Upload the image using the repository
                await _imageRepository.Upload(imageDomainModel);
                //Return the image
                return Ok(imageDomainModel);
            }
            // If the request is invalid, return a 400 Bad Request
            return BadRequest(ModelState); 
        }//end of Upload method
        
        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            // Check if file extension is allowed
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension.");
            }
            // Check if file size is allowed
            if(request.File.Length > 1048576)
            {
                ModelState.AddModelError("file", "File size cannot exceed 1MB.");
            }
        }
    }
}
