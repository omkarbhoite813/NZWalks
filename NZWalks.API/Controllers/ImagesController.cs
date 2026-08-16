using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileExtension(request);
            if (ModelState.IsValid)
            {
                //Conver DTO to Model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length
                };

                //User Repository to upload image
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileExtension(ImageUploadRequestDto request)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("File", "Invalid file extension. Only .jpg, .jpeg, .png, and .gif are allowed.");
            }
            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "File size exceeds the limit of 5MB.");
            }
        }
    }
}
