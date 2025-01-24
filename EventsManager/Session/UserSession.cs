using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Session
{
    internal class UserSession
    {
        public static string? Login { get; set; }
        public static string? FirstName { get; set; }
        public static string? LastName { get; set; }
        public static string? Role { get; set; }
        public static bool IsAuth
        {
            get
            {
                return Login != null && FirstName != null && LastName != null;
            }
        }
        public static string? JWT_TOKEN { get; set; }
    }
}
