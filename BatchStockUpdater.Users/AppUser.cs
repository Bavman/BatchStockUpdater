using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchStockUpdater.Core;

namespace BatchStockUpdater.Users
{
    public class AppUser
    {
        public int ID { get; set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime StartDate { get; set;}

        public UserTypeEnum UserType { get; set; }

        public bool ProtectedUser { get; set; }

        public bool Inactive { get; set; }

    }
}
