using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Data;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BookWithAuthorAndPublisherDTO> GetAllBooks()
        {
            var allBooksDTO = _dbContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Select(book => new BookWithAuthorAndPublisherDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.Book_Authors.Select(ba => ba.Author.FullName).ToList()
                }).ToList();

            return allBooksDTO;
        }

        public BookWithAuthorAndPublisherDTO? GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books
                .Where(b => b.Id == id)
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Select(book => new BookWithAuthorAndPublisherDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).FirstOrDefault();

            return bookWithDomain;
        }

        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherID
            };

            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            foreach (var authorId in addBookRequestDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author() { BookId = bookDomainModel.Id, AuthorId = authorId };
                _dbContext.Books_Authors.Add(bookAuthor);
            }
            _dbContext.SaveChanges();

            return addBookRequestDTO;
        }

        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description;
                bookDomain.IsRead = bookDTO.IsRead;
                bookDomain.DateRead = bookDTO.DateRead;
                bookDomain.Rate = bookDTO.Rate;
                bookDomain.Genre = bookDTO.Genre;
                bookDomain.CoverUrl = bookDTO.CoverUrl;
                bookDomain.DateAdded = bookDTO.DateAdded;
                bookDomain.PublisherID = bookDTO.PublisherID;
                _dbContext.SaveChanges();

                var existingAuthors = _dbContext.Books_Authors.Where(a => a.BookId == id).ToList();
                if (existingAuthors.Any())
                {
                    _dbContext.Books_Authors.RemoveRange(existingAuthors);
                    _dbContext.SaveChanges();
                }

                foreach (var authorId in bookDTO.AuthorIds)
                {
                    var bookAuthor = new Book_Author() { BookId = id, AuthorId = authorId };
                    _dbContext.Books_Authors.Add(bookAuthor);
                }
                _dbContext.SaveChanges();
                return bookDTO;
            }
            return null;
        }

        public Books? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;
        }
    }
}