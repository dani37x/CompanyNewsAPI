namespace CompanyNewsAPI.Models
{
    public class NewPassword
    {
        public string? Key { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.AddMinutes(15);

    }
}
