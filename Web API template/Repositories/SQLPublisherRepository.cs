using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            var allPublishers = _dbContext.Publishers.Select(p => new PublisherDTO()
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return allPublishers;
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisher = _dbContext.Publishers.Where(p => p.Id == id)
                .Select(p => new PublisherNoIdDTO()
                {
                    Name = p.Name
                }).FirstOrDefault();
            return publisher;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherDomain = new Publishers()
            {
                Name = addPublisherRequestDTO.Name
            };
            _dbContext.Publishers.Add(publisherDomain);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisherDomain != null)
            {
                publisherDomain.Name = publisherNoIdDTO.Name;
                _dbContext.SaveChanges();
            }
            return publisherNoIdDTO;
        }

        public Publishers? DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publishers.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return publisherDomain;
        }
    }
}