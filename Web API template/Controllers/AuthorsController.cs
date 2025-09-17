using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthors()
        {
            var allAuthors = _authorRepository.GetAllAuthors();
            return Ok(allAuthors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            return Ok(author);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var author = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(author);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorNoIdDTO)
        {
            var updatedAuthor = _authorRepository.UpdateAuthorById(id, authorNoIdDTO);
            return Ok(updatedAuthor);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var deletedAuthor = _authorRepository.DeleteAuthorById(id);
            return Ok(deletedAuthor);
        }
    }
}