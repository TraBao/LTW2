// File: Models/DTO/ImageUploadRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace WebAPI_simple.Models.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}