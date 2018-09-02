using System.Linq;
using System.Windows.Forms;
using BatchStockUpdater.Core;

namespace BatchStockUpdater.Users
{

    public static class UserLogin
    {

        public static IUsersRepository UsersRepository;

        // Check the user credentials match and fire LogInApproved in the MainWinow
        public static AppUser CheckUserCredentials(string userNameCheck, string passwordCheck)
        {
            var userList = UsersRepository.UserList;
            var userListQuery = userList.Where(u => u.UserName.Equals(userNameCheck)).ToArray();

            if (userListQuery.Length > 0)
            {
                // Inactive User
                if (userListQuery[0].Inactive)
                {
                    FailedLoginMessage();

                    Logging.GetInstance().LogLogin(userNameCheck, LogStatus.Failure);

                    return null;
                }

                // Successful credentials or not
                if (userListQuery[0].Password == passwordCheck)
                {
                    Logging.GetInstance().LogLogin(userNameCheck, LogStatus.Success);

                    return userListQuery[0];
                }
                else
                {
                    FailedLoginMessage();
                    Logging.GetInstance().LogLogin(userNameCheck, LogStatus.Failure);

                    return null;
                }
            }
            else
            {
                FailedLoginMessage();
                Logging.GetInstance().LogLogin(userNameCheck, LogStatus.Failure);

                return null;
            }
        }


        // User credentials messagebox
        private static void FailedLoginMessage()
        {
            MessageBox.Show("Sorry, user credentials do not match.");
        }
    }
}
