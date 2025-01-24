using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Models
{
    class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EventsCount { get; set; }
        public string EventsString { get; set; }
        public string NewPassword { get; set; }
        public UserModel Original { get; set; }

        public bool HasChanged()
        {
            return Login != Original?.Login || FirstName != Original?.FirstName || LastName != Original?.LastName || Role != Original?.Role;
        }
    }
}
