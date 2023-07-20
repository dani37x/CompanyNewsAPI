using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Repositories
{
    public class PostRepo : IPostRepo
    {
        private readonly DataContext _dataContext;

        public PostRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<Post> GetSinglePost(int id)
        {
            return await _dataContext.Posts.FindAsync(id);
        }

        public async Task<List<Post>> GetAuthorPosts(int id)
        {
            var author = await _dataContext.Users.FindAsync(id);

            if (author == null)
            {
                return new List<Post>();
            }

            return await _dataContext.Posts.Where(user => user.UserId == id).ToListAsync();
        }

        public async Task<bool> AddThePost(PostDto request)
        {
            var author = await _dataContext.Users.FindAsync(request.UserId);
            if (author != null)
            {
                var postToAdd = new Post
                {
                    Title = request.Title,
                    Description = request.Description,
                    Date = DateTime.Now,
                    User = author
                };
                await _dataContext.Posts.AddAsync(postToAdd);
            }

            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateThePost(PostDto request)
        {
            var post = await _dataContext.Posts.FindAsync(request.Id);

            if (post != null)
            {
                var user = await _dataContext.Users.FindAsync(request.UserId);

                post.Id = request.Id;
                post.Title = request.Title;
                post.Description = request.Description;
                post.UserId = request.UserId;
                post.User = user;
            }
  
            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteThePost(int id)
        {
            var result = await _dataContext.Posts.FindAsync(id);

            if (result != null)
            {
                _dataContext.Posts.Remove(result);
            }

            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

    }
}
