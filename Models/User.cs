using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyNewsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Team { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
