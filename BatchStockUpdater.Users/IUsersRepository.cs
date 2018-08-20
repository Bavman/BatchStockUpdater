using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace BatchStockUpdater.Users
{
    public interface IUsersRepository
    {
        IList<AppUser> UserList { get; set; }

        void InitializeUsers();

        bool LoadUsers();

        AppUser CreateAdminUser();

        void AddUser(AppUser user);

        bool DoesUserExist(string newUserName);

        void DeleteUser(int userID);

        void UpdateUser(AppUser user);

        AppUser ReturnUser(int id);

        int ReturnLastUserID();
    }
}
