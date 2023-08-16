namespace CompanyNewsAPI.Models
{
    public class Error
    {
        public string Message {get; set;}
        public DateTime Date { get; set;} = DateTime.Now.AddDays(1);
    }
}
