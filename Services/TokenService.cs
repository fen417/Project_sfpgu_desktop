using System;

namespace Project_sfpgu_desktop.Services
{
    public static class TokenService
    {
        public static string Token { get; private set; }
        public static DateTime Expiration { get; private set; }

        public static void SaveToken(string token, DateTime expires)
        {
            Token = token;
            Expiration = expires;
        }
        public static void ClearToken()
        {
            Token = null;
            Expiration = DateTime.MinValue;
        }

        public static bool IsTokenValid() => !string.IsNullOrEmpty(Token) && DateTime.Now < Expiration;

        public static string GetToken()
        {
            if (IsTokenValid())
                return Token;
            return null;
        }
    }
}
