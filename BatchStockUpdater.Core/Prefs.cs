﻿using System;
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


        private bool _checkFileDateAndTime = true;
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

        private string _folederName = @"C:\CertIV";
        public string FolderName
        {
            get { return _me._folederName; }
            set { _me._folederName = value; }
        }

        private string _fileName = "stocklist.CSV";
        public string FileName
        {
            get { return _me._fileName; }
            set { _me._fileName = value; }
        }

        public string FilePath
        {
            get { return _me._folederName + @"\" + _me.FileName; }
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
                _me.FileName = loadedPrefs.FileName;
                _me.FolderName = loadedPrefs.FolderName;


                file.Close();

                successfulLoad = true;
            }
            else
            {
                MessageBox.Show("Preferences file missing.\n Will be using default settings");
                _me.SavePrefs();

                successfulLoad = false;
            }

            return successfulLoad;
        }

    }
}