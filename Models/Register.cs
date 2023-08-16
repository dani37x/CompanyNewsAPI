﻿namespace CompanyNewsAPI.Models
{
    public class Register
    {
        public string Key { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.AddMinutes(15);
        public User User { get; set; }
    }
}
