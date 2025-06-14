﻿
namespace CozyHaven.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
