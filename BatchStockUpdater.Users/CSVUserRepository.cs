using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using BatchStockUpdater.Core;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace BatchStockUpdater.Users
{
    public class CSVUserRepository : IUsersRepository
    {
        #region Singleton setup
        private static CSVUserRepository _me = null;

        private CSVUserRepository() { }

        public static CSVUserRepository GetInstance()
        {
            if (_me == null)
            {
                _me = new CSVUserRepository();

            }
            return _me;

        }

        #endregion

        // Variables 
        private string _csvUserFileName = @"\users.csv";
        private string[] _csvHeader = new string[]
        {
            "ID",
            "UserName",
            "Password",
            "FirstName",
            "LastName",
            "EmailText",
            "StartDateDatePicker",
            "UserType",
            "ProtectedUser",
            "InactiveUser"
        };
        private IList<AppUser> _userList = new List<AppUser>();
        private IFormatProvider provider;

        public IList<AppUser> UserList
        {
            get { return _userList; }
            set { _userList = value; }
        }

        // Tries to load users from a CSV file.
        // If unsuccessful creates a new Admin User and saves a new CSV file
        public void InitializeUsers()
        {
            if (LoadUsers() == false)
            {
                // Create and admin user and write it to the user.csv file
                UserList.Add(CreateAdminUser());
                WriteCSVUserFile();
            }
        }

        // Loads CSV from the Application folder then assigns
        // the TextFieldParser object to 'PopulateUserList' function 
        public bool LoadUsers()
        {
            var _fileIO = new FileIO();
            var filePath = (Application.UserAppDataPath + _csvUserFileName);

            var usersCSV = _fileIO.LoadCSV(filePath, false);

            if (usersCSV == null)
            {
                return false;
            }

            PopulateUserList(usersCSV);
            return true;
        }

        // Assigns CSV user data to AppUser Classes then adds the class to
        // The _userList IList
        private void PopulateUserList(TextFieldParser csvData)
        {
            var fields = new string[0];

            // Loop through CSV Data to populate DataTable fields
            while (!csvData.EndOfData)
            {
                try
                {
                    fields = csvData.ReadFields();
                }
                catch (Exception)
                {
                    throw;
                }

                // Avoid processing header row
                if (csvData.LineNumber != 2)
                {
                    // Process rows
                    var user = new AppUser();
                    user.ID = int.Parse(fields[0]);
                    user.UserName = fields[1];
                    user.Password = fields[2];
                    user.FirstName = fields[3];
                    user.LastName = fields[4];
                    user.Email = fields[5];

                    var date = new DateTime();
                    date = DateTime.ParseExact(fields[6], "d", provider);
                    user.StartDate = date;
                    user.UserType = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), fields[7]);
                    user.ProtectedUser = bool.Parse(fields[8]);
                    user.Inactive = bool.Parse(fields[9]);

                    _userList.Add(user);
                }
            }
        }

        // Creates a generic Admin User. Used if CSV file cannot be found.
        // Creates an admin user so the program can be accessed on first boot.
        public AppUser CreateAdminUser()
        {
            var user = new AppUser();
            user.ID = 0;
            user.UserName = "Admin";
            user.Password = "Passw0rd";
            user.FirstName = "";
            user.LastName = "";
            user.Email = "support@skillageit.com.au";
            user.StartDate = new DateTime(2000, 1, 1);
            user.UserType = UserTypeEnum.Administrator;
            user.ProtectedUser = true;
            user.Inactive = false;

            return user;
        }

        // Receives a populated AppUser class and adds to the _userList IList then writes
        // all the users to the user.csv file
        public void AddUser(AppUser user)
        {
            _userList.Add(user);
            WriteCSVUserFile();
        }

        // Parses user data to a CSV file and writes to disk.
        private void WriteCSVUserFile()
        {
            
            var filePath = (Application.UserAppDataPath + _csvUserFileName);

            var streamWriter = new StreamWriter(filePath);

            // Collect column names and add to string builder
            var headRowStringBuilder = new StringBuilder();

            headRowStringBuilder.AppendLine(string.Join(",", _csvHeader));

            streamWriter.WriteLine(headRowStringBuilder);

            

            for (var i = 0; i < _userList.Count; i++)
            {
                var userCSVLine = String.Empty;

                // TODO @ Zac to refine code
                /*
                var userProperties = _userList[i].GetType().GetProperties().ToString();
                var userStringBuilder = new StringBuilder();
                userStringBuilder.AppendLine(string.Join(",", userProperties));

                streamWriter.WriteLine(userStringBuilder);
                */

                userCSVLine += _userList[i].ID.ToString() + ",";
                userCSVLine += _userList[i].UserName + ",";
                userCSVLine += _userList[i].Password + ",";
                userCSVLine += _userList[i].FirstName + ",";
                userCSVLine += _userList[i].LastName + ",";
                userCSVLine += _userList[i].Email + ",";
                userCSVLine += _userList[i].StartDate.ToShortDateString() + ",";
                userCSVLine += _userList[i].UserType.ToString() + ",";
                userCSVLine += _userList[i].ProtectedUser.ToString() + ",";
                userCSVLine += _userList[i].Inactive.ToString();

                streamWriter.WriteLine(userCSVLine);
                
            }
            streamWriter.Close();


        }

        // Checks if user exists. Used when entering new user in the ManageUser UI.
        // Method could be moved to the ManageUser.xaml.cs class.
        public bool DoesUserExist(string newUserName)
        {
            var userListQuery = _userList.Where(u => u.UserName.ToLower().Equals(newUserName.ToLower())).ToArray();

            if (userListQuery.Length > 0)
            {
                return true;
            }

            return false;
        }

        // Deletes user except for the admin user at ID 0
        public void DeleteUser(int userID)
        {
            if (userID != 0)
            {
                _userList.RemoveAt(userID);
                WriteCSVUserFile();
            }
            else
            {
                MessageBox.Show("Sorry, cannot delete this user");
            }
        }

        // Receives an updated AppUser class and overwrites the same user in _userList IList
        public void UpdateUser(AppUser user)
        {
            var userID = user.ID;

            _userList[userID] = (user);
            WriteCSVUserFile();
        }

        // Return user based on user ID
        public AppUser ReturnUser(int id)
        {
            return UserList[id];
        }

        // Returns the _userList IList count
        public int ReturnLastUserID()
        {
            return _userList.Count;
        }

        // Loops through all users and finds the user with the largest user ID then adds 1.
        private int GenerateNewUserID(IEnumerable<AppUser> userCollection)
        {
            var newID = 1;

            var userList = userCollection.ToList();

            for (var i = 0; i < userList.Count; i++)
            {
                var userID = userList[i].ID;
                if (userID > newID)
                {
                    newID = userID + 1;
                }
            }

            return newID;
        }

    }
    
}
