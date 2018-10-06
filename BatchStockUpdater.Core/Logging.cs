using System;
using System.IO;
using System.Windows.Forms;

namespace BatchStockUpdater.Core
{
    public class Logging
    {
        private static Logging _me = null;

        private Logging() { }

        // Return new instance of class if not instantiated.
        //  or return instantiated instance of class
        public static Logging GetInstance()
        {
            _me = _me ?? new Logging();
            
            if (!_me._isBooted)
            {
                _me.LogFolder = (Application.UserAppDataPath) + @"\LogFolder";
                SetupFilePathAndStreamWriter(_me.LogFolder);

                _me._isBooted = true;
            }
            
            
            return _me;
        }

        public string LogFolder;
        private bool _isBooted;
        private string _sessionLogFilePath;

        static private void SetupFilePathAndStreamWriter(string logFolder)
        {

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            var dateTime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
            Console.WriteLine(dateTime);
            _me._sessionLogFilePath = logFolder + @"\logfile_"+ dateTime +".txt";
        }

        // Log Open Application
        public void OpenApplication()
        {
            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", Application started");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);

        }

        // Log Close Application
        public void CloseApplication()
        {
            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", Application closed");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Login 
        public void LogLogin(string userName, LogStatus loginStatus)
        {
            var status = ReturnLogStatus(loginStatus);

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + userName + " " + status + " log in");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Logout 
        public void LogLogOut(string userName)
        {

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + userName + " logged out");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Add User 
        public void LogAddUser(string userName)
        {

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + userName + " user added");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Delete User
        public void LogDeleteUser(string userName)
        {

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + userName + " added");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Update User
        public void LogUpdateUser(string userName)
        {

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + userName + " added");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Import CSV
        public void LogImportCSV(LogStatus loginStatus)
        {
            var status = ReturnLogStatus(loginStatus);

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + status + " imported stocklist.csv");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Export XML
        public void LogExportXML(LogStatus loginStatus, int styleSheet)
        {
            var status = ReturnLogStatus(loginStatus);

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", " + status + " exported stocklist.xml style "+styleSheet.ToString());

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }

        // Log Export CSV
        public void LogExportCSV()
        {

            var currentDateTime = DateTime.Now;

            var entry = (currentDateTime.ToString() + ", Successfully exported stocklist.csv");

            WriteEntryToLogFIle(_me._sessionLogFilePath, entry);
        }


        private static string ReturnLogStatus(LogStatus logStatus)
        {
            var status = String.Empty;
            switch (logStatus)
            {
                case LogStatus.Success:
                    status = "successfully";
                    break;
                case LogStatus.Failure:
                    status = "failed to";
                    break;
                default:
                    break;
            }

            return status;
        }

        // Write entry to current log file
        private static void WriteEntryToLogFIle(string path, string logEntry)
        {
            var streamWriter = new StreamWriter(path, append: true);
            streamWriter.WriteLine(logEntry);
            streamWriter.Close();
        }
    }
}
