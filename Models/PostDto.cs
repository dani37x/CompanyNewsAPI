namespace CompanyNewsAPI.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "title";
        public string Description { get; set; } = "description";
        public int AuthorId { get; set; } = 1;
    }
}
