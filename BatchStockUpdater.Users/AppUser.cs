using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            set
            {
                var isValid = CheckFields(value, "^[a-zA-Z0-9_]{4,20}$", @" must be 4-12 characters long and can only contain letters, numbers and '_'.", "User Name");

                _userName = isValid ? value : null;
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                var isValid = CheckFields(value, @"^(?=.*\d)(?=.*[a-zA-Z]).{4,12}$", @" must be 4-12 characters long and contain at least one letter and one number", "Password");

                _password = isValid ? value : null;

            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                var isValid = CheckFields(value, @"^[a-zA-Z]+$", @"can only contain upper and lower case letters", "First Name");

                _firstName = isValid ? value : null;

            }
        }


        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                var isValid = CheckFields(value, @"^[a-zA-Z]+$", @"can only contain upper and lower case letters", "Last Name");

                _lastName = isValid ? value : null;

            }
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                var isValid = CheckFields(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", @"must be a valid email address", "Email");
                _email = isValid ? value : null;
            }
        }

        public DateTime StartDate { get; set;}

        public UserTypeEnum UserType { get; set; }

        public bool ProtectedUser { get; set; }

        public bool Inactive { get; set; }

        // Checks the UI input fields making sure they match the required length and symbols
        // of the requested field. eg. email address requres the user to enter a valid email address
        // not just any text.
        private bool CheckFields(string textField, string matchChars, string mustContainMessage, string fieldName)
        {

            var isValid = Regex.IsMatch(textField, matchChars);

            Console.WriteLine(isValid);

            if (!isValid)
            {

                var showString = string.Format("The {0}{1}.", fieldName, mustContainMessage);
                MessageBox.Show(showString);

                return false;
            }

            return true;
        }
    }
}
