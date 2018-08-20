using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BatchStockUpdater.Users
{
    public class ReturnADataTable
    {
        string[] _csvHeader = new string[]
        {
            "Item Code",
            "Item Description",
            "Current Count",
            "On Order"
        };


        public DataTable ReturnTestDataTable()
        {
            var dataTable = new DataTable();

            var csvDataTable = SetupDataTable(dataTable, _csvHeader);

            // Process rows

            var rowArray1 = new string[] { "A0001", "Horse on Wheels", "4", "No" };
            var rowArray2 = new string[] { "A0002", "Elephant on Wheels", "20", "No" };
            var rowArray3 = new string[] { "A0003", "Dog on Wheels", "10", "Yes" };
            var rowArray4 = new string[] { "A0004", "Seal on Wheels", "3", "No" };

            csvDataTable.Rows.Add(ReturnDataRow(dataTable.NewRow(), rowArray1, _csvHeader));
            csvDataTable.Rows.Add(ReturnDataRow(dataTable.NewRow(), rowArray2, _csvHeader));
            csvDataTable.Rows.Add(ReturnDataRow(dataTable.NewRow(), rowArray3, _csvHeader));
            csvDataTable.Rows.Add(ReturnDataRow(dataTable.NewRow(), rowArray4, _csvHeader));
            Console.WriteLine("ReturnADataTable "+ dataTable.Rows);

            return dataTable;
        }

        private DataRow ReturnDataRow(DataRow row, string[] rowArray, string[] csvHeader)
        {

            row[csvHeader[0]] = rowArray[0];
            row[csvHeader[1]] = rowArray[1];
            row[csvHeader[2]] = int.Parse(rowArray[2]);
            row[csvHeader[3]] = rowArray[3];

            return row;

        }

        // Setup columns for DataTable object
        private DataTable SetupDataTable(DataTable csvDataTable, string[] csvHeader)
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
