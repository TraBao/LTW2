using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AuthorDTO> GetAllAuthors()
        {
            var allAuthors = _dbContext.Authors.Select(author => new AuthorDTO()
            {
                Id = author.Id,
                FullName = author.FullName
            }).ToList();
            return allAuthors;
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var author = _dbContext.Authors.Where(a => a.Id == id)
                .Select(author => new AuthorNoIdDTO()
                {
                    FullName = author.FullName
                }).FirstOrDefault();
            return author;
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorDomain = new Authors
            {
                FullName = addAuthorRequestDTO.FullName
            };

            _dbContext.Authors.Add(authorDomain);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }

        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (authorDomain != null)
            {
                authorDomain.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();
            }
            return authorNoIdDTO;
        }

        public Authors? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return authorDomain;
        }
    }
}