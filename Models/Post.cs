using CompanyNewsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CompanyNewsAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
