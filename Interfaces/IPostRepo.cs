using CompanyNewsAPI.Models;

namespace CompanyNewsAPI.Interfaces
{
    public interface IPostRepo
    {
        public Task<List<Post>> GetAllPosts();
        public Task<Post> GetSinglePost(int id);
        public Task<List<Post>> GetAuthorPosts(int id);
        public Task<bool> AddThePost(PostDto request);
        public Task<bool> UpdateThePost(PostDto request);
        public Task<bool> DeleteThePost(int id);
    }
}
