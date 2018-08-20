using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Windows.Forms;

namespace BatchStockUpdater.Core
{
    public class DataMethods
    {
        private static DataMethods _me = null;

        private DataMethods() { }

        // Return new instance of class if not instantiated.
        //  or return instantiated instance of class
        public static DataMethods GetInstance()
        {
            if (_me == null)
            {
                _me = new DataMethods();
            }

            return _me;
        }

        // Populate Datatable ojbect from with CSV data
        public DataTable PopulateDataTable(TextFieldParser csvData, string[] csvHeader)
        {
            var fields = new string[0];

            var csvDataTable = SetupDataTable(new DataTable(), csvHeader);

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
                    var row = csvDataTable.NewRow();

                    row[csvHeader[0]] = fields[0];
                    row[csvHeader[1]] = fields[1];
                    var intCheck = Int32.TryParse(fields[2], out var field2);
                    if (intCheck)
                    {
                        row[csvHeader[2]] = field2;
                    }
                    else
                    {
                        MessageBox.Show("A 'Current Count' field is not an integer");
                    }

                    row[csvHeader[3]] = fields[3];

                    csvDataTable.Rows.Add(row);

                }

            }
            return csvDataTable;
        }

        // Setup columns for DataTable object
        public DataTable SetupDataTable(DataTable csvDataTable, string[] csvHeader)
        {

            var itemCodeCol = SetupColumn(csvHeader[0], Type.GetType("System.String"), true);
            var itemDescriptionCol = SetupColumn(csvHeader[1], Type.GetType("System.String"), true);
            var currentCountCol = SetupColumn(csvHeader[2], Type.GetType("System.Int32"), false);
            var onOrderCol = SetupColumn(csvHeader[3], Type.GetType("System.String"), true);

            csvDataTable.Columns.Add(itemCodeCol);
            csvDataTable.Columns.Add(itemDescriptionCol);
            csvDataTable.Columns.Add(currentCountCol);
            csvDataTable.Columns.Add(onOrderCol);

            return csvDataTable;
        }

        // Return a list of collections (columns) from a dataTable 
        public IList<T> ReturnColumnCollection<T>(DataTable dataTable, string columnHeader)
        {
            var currentCountCollection = new List<T>();

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var dataRow = dataTable.Rows[i];

                var dataRowCell = (dataRow[columnHeader]);

                var dataRowCellOfType = (T)Convert.ChangeType(dataRowCell, typeof(T));

                currentCountCollection.Add(dataRowCellOfType);
                //currentCountCollection.Add();
            }

            return currentCountCollection;
        }

        // Tests Primitive type lists only or memore references.
        // Does not test variables within class lists
        public bool CompareCurrentCountsTables<T>(IList<T> originalCollection, IList<T> modifiedCollection)
        {
            var sequenceEquals = originalCollection.SequenceEqual(modifiedCollection);

            return sequenceEquals;
        }

        // Setup DataTable Column parameters
        private DataColumn SetupColumn(string colName, Type type, bool isReadOnly)
        {
            var column = new DataColumn();
            column.DataType = type;
            column.ColumnName = colName;
            column.ReadOnly = isReadOnly;

            return column;
        }

    }
}
