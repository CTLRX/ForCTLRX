using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.Helper
{
   public  class ExcelHelper
    {
        /// <summary>
        /// 读取Excel文件到DataSet中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static DataSet ToDataTable(string filePath)
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;

            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [1489578022$]";//Sheet1$

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dtSheetName = null;

            DataSet ds = new DataSet();
            try
            {
                // 初始化连接，并打开
                conn = new OleDbConnection(connStr);
                conn.Open();

                // 获取数据源的表定义元数据                        
                string SheetName = "";
                dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                // 初始化适配器
                da = new OleDbDataAdapter();
                for (int i = 0; i < dtSheetName.Rows.Count; i++)
                {
                    SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

                    if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                    {
                        continue;
                    }

                    da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                    DataSet dsItem = new DataSet();
                    da.Fill(dsItem, SheetName);

                    ds.Tables.Add(dsItem.Tables[0].Copy());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 关闭连接
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
            return ds;
        }
        /// <summary>
        /// 读取数据库数据导出excel文件
        /// </summary>
        /// <param name="tabel"></param>
        public static void Export(DataTable tabel)
        {
            var data = tabel;

            Workbook outExcel = new Workbook(FileFormatType.Xlsx);
            Worksheet sheet = outExcel.Worksheets[0];
            sheet.Name = "Sheet1";
            Cells cells = sheet.Cells;
            cells.ImportDataTable(data, true, 0, 0, data.Rows.Count, data.Columns.Count, true, "yyyy-MM-dd");
            sheet.AutoFitColumns();

            //for (int i = 1; i < cells.Rows.Count; i++)
            //{
            //    var style = cells[i, 1].GetStyle();
            //    var send_id = Convert.ToInt64(cells[i, 0].Value).ToString();
            //    style.Number = 2;
            //    cells[i, 0].Value = (object)send_id;

            //}
            var folder = Path.Combine(AppContext.BaseDirectory, "temp");
            if (!System.IO.Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var path = Path.Combine(folder, "merchant.xlsx");
            outExcel.Save(path, SaveFormat.Xlsx);
        }
    }
}
