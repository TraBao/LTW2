using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.CustomActionFilters;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;
        public PublishersController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet("get-all-publisher")]
        public IActionResult GetAllPublishers()
        {
            var allPublishers = _publisherRepository.GetAllPublishers();
            return Ok(allPublishers);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            return Ok(publisher);
        }

        [HttpPost("add-publisher")]
        [ValidateModel]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var newPublisher = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            if (newPublisher == null)
            {
                return BadRequest($"Tên nhà xuất bản '{addPublisherRequestDTO.Name}' đã tồn tại.");
            }
            return CreatedAtAction(nameof(GetPublisherById), new { id = newPublisher.Id }, newPublisher);
        }

        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherNoIdDTO)
        {
            var updatedPublisher = _publisherRepository.UpdatePublisherById(id, publisherNoIdDTO);
            return Ok(updatedPublisher);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var publisherExists = _publisherRepository.GetPublisherById(id);
            if (publisherExists == null)
            {
                return NotFound();
            }
            var deletedPublisher = _publisherRepository.DeletePublisherById(id);
            if (deletedPublisher == null)
            {
                return BadRequest($"Không thể xóa Nhà xuất bản này vì vẫn còn sách tham chiếu.");
            }
            return NoContent();
        }
    }
}