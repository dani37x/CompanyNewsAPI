using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyNewsAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Team { get; set; }
        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
