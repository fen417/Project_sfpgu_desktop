using System.Text.Json.Serialization;

namespace Project_sfpgu_desktop.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string email { get; set; } 
        public string password { get; set; }
        public string Role { get; set; } // student, teacher, head, director, admin
        [JsonPropertyName("GroupName")]
        public string Group { get; set; }
    }
}
