using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NatureOptimizationAlgorithms.Tools
{
    public class Print
    {       
       
        public Print()
        {

        }

        public static void WriteToExcel(string[] columns, List<object[]> rows)
        {
            XLWorkbook workbook = new XLWorkbook();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            ////input data
            //var columns = new[] { "PSO", "MGWO", "GWO", "WOA", "HAGWO", "SOLUTION" };
            //var rows = new object[][]
            //{
            //     new object[] {"1", 2, false },
            //     new object[] { "test", 10000, 19.9 }
            //};

            //Add columns
            dt.Columns.AddRange(columns.Select(c => new DataColumn(c)).ToArray());

            //Add rows
            foreach (var row in rows)
            {
                dt.Rows.Add(row);
            }

            //Convert datatable to dataset and add it to the workbook as worksheet
            ds.Tables.Add(dt);
            workbook.Worksheets.Add(ds);

            //save
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savePath = Path.Combine(desktopPath, "test2.xlsx");
            workbook.SaveAs(savePath, false);
        }
    }
}
