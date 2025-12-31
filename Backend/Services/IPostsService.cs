using Backend.DTOs;

namespace Backend.Services
{
    public interface IPostsService
    {
        public Task<IEnumerable <PostsDTOs>> Get();
    } 
}
