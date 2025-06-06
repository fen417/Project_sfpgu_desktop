using System;

namespace Project_sfpgu_desktop.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserModel User { get; set; }
    }
}
