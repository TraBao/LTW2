using WebAPI_simple.Models.Domain;

namespace WebAPI_simple.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}