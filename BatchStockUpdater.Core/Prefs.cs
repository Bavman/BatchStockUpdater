using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BatchStockUpdater.Core
{
    public class Prefs
    {
        private static Prefs _me = null;

        private Prefs(){}

        // Return new instance of class if not instantiated.
        //  or return instantiated instance of class
        public static Prefs GetInstance()
        {
            if (_me == null)
            {
                _me = new Prefs();
            }

            //_me.SavePrefs();
            return _me;
        }


        private bool _checkFileDateAndTime = false;
        public bool CheckFileDateAndTime
        {
            get { return _me._checkFileDateAndTime; }
            set { _me._checkFileDateAndTime = value; }
        }

        private int _nDaysBefore = 1;
        public int NDaysBefore
        {
            get { return _me._nDaysBefore; }
            set { _me._nDaysBefore = value; }
        }


        private TimeSpan _timeCheck = new TimeSpan(15, 00, 00);
        public TimeSpan TimeCheck
        {
            get { return _me._timeCheck; }
            set { _me._timeCheck = value; }
        }

        private string _folederName = @"C:\";
        public string FolderName
        {
            get { return _me._folederName; }
            set { _me._folederName = value; }
        }

        private string _csvFileName = "stocklist.CSV";
        public string CSVFileName
        {
            get { return _me._csvFileName; }
            set { _me._csvFileName = value; }
        }

        private string _xmlFileNameStyle1 = "stocklist1.xml";

        public string XMLFileNameStyle1
        {
            get { return _xmlFileNameStyle1; }
        }

        private string _xmlFileNameStyle2 = "stocklist2.xml";

        public string XMLFileNameStyle2
        {
            get { return _xmlFileNameStyle2; }
        }

        public string CSVFilePath
        {
            get { return _me._folederName + @"\" + _me.CSVFileName; }
        }

        public string XMLFilePathStyle1
        {
            get { return _me._folederName + @"\" + _me._xmlFileNameStyle1; }
        }

        public string XMLFilePathStyle2
        {
            get { return _me._folederName + @"\" + _me._xmlFileNameStyle2; }
        }
        // Save class properties to Json file
        public void SavePrefs()
        {
            var prefsJsonFilePath = (Application.UserAppDataPath)+@"\prefs.Json";

            using (var file = File.CreateText(prefsJsonFilePath))
            {
                var jsonSerializer = new JsonSerializer();
                
                jsonSerializer.Formatting = Formatting.Indented;
                jsonSerializer.Serialize(file, _me);
            }
        }

        // Load prefs.json file and populate class properties
        public bool LoadPrefs()
        {
            var successfulLoad = false;

            var serializer = new JsonSerializer();
            var prefsJsonPath = (Application.UserAppDataPath) + @"\prefs.Json";


            if (File.Exists(prefsJsonPath))
            {
                var file = File.OpenText(prefsJsonPath);

                var loadedPrefs = new Prefs();

                try
                {
                    loadedPrefs = (Prefs)serializer.Deserialize(file, typeof(Prefs));
                }
                catch (Exception)
                {

                    throw new FormatException();
                }

                _me.CheckFileDateAndTime = loadedPrefs.CheckFileDateAndTime;
                _me.TimeCheck = loadedPrefs.TimeCheck;
                _me.NDaysBefore = loadedPrefs.NDaysBefore;
                _me.CSVFileName = loadedPrefs.CSVFileName;
                _me.FolderName = loadedPrefs.FolderName;

                file.Close();

                successfulLoad = true;
            }
            else
            {
                // Disabled Messagebox to reduce confusion on first load.
                // MessageBox.Show("Preferences file missing.\n Will be using default settings");
                _me.SavePrefs();

                successfulLoad = false;
            }

            return successfulLoad;
        }

    }
}