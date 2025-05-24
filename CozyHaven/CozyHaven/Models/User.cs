
namespace CozyHaven.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string PreferredTheme { get; set; } = "Light";     
        public string PreferredLanguage { get; set; } = "en";
        public string? ProfilePicturePath { get; set; } = "Images/defaultUser.png";
        public DateTime EmploymentDate { get; set; } = DateTime.Now;
        public decimal Salary { get; set; } = 0m;
        public int? DepartmentId { get; set; }  
        public Department? Department { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
