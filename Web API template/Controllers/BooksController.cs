using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;
using System.Net;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        // Inject IBookRepository vào constructor
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: /api/books/get-all-books
        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _bookRepository.GetAllBooks();
            return Ok(allBooks);
        }

        // GET: /api/books/get-book-by-id/{id}
        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST: /api/books/add-book
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var addedBook = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(addedBook);
        }

        // PUT: /api/books/update-book-by-id/{id}
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updatedBook = _bookRepository.UpdateBookById(id, bookDTO);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }

        // DELETE: /api/books/delete-book-by-id/{id}
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deletedBook = _bookRepository.DeleteBookById(id);
            if (deletedBook == null)
            {
                return NotFound();
            }
            return Ok(); // Hoặc trả về Ok(deletedBook) nếu muốn
        }
    }
}