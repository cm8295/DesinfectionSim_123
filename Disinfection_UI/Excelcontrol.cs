using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;
using System.Data.Odbc;
using Microsoft.Win32;
using System.Windows;

namespace Disinfection_UI
{
    class ExcelControl
    {
        private Excel.Application _excelApp = null;
        private Excel.Workbooks _books = null;
        private Excel._Workbook _book = null;
        private Excel.Sheets _sheets = null;
        private Excel._Worksheet _sheet = null;
        private object _optionalValue = Missing.Value;
        private Excel.Range _range = null;

        void CreateExcelRef()
        {
            _excelApp = new Excel.Application();
            _books = (Excel.Workbooks)_excelApp.Workbooks;
            _book = (Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Excel.Sheets)_book.Worksheets;
            _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
        }
        void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }
        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Columns.AutoFit();
        }
        object[] CreateHeader(DataTable dt)
        {
            List<object> objHeader = new List<object>();
            for (int n = 0; n < dt.Columns.Count; n++)
            {
                objHeader.Add(dt.Columns[n].ColumnName);
            }
            var HeaderToAdd = objHeader.ToArray();
            AddExcelRows("A1", 1, HeaderToAdd.Length, HeaderToAdd);
            return HeaderToAdd;
        }
        void SaveExcel(string excelName)
        {
            _excelApp.Visible = false;
            _book.SaveAs(excelName,Excel.XlFileFormat.xlAddIn8);
            _excelApp.Quit();
        }
        DataTable LoadExcel(string pPath)
        {
            //Driver={Driver do Microsoft Excel(*.xls)} 这种连接写法不需要创建一个数据源DSN，DRIVERID表示驱动ID，Excel2003后都使用790，FIL表示Excel文件类型，Excel2007用excel 8.0，MaxBufferSize表示缓存大小，DBQ表示读取Excel的文件名（全路径）

            string connString = "Driver={Driver do Microsoft Excel(*.xls)};DriverId=790;SafeTransactions=0;ReadOnly=1;MaxScanRows=16;Threads=3;MaxBufferSize=2024;UserCommitSync=Yes;FIL=excel 8.0;PageTimeout=5;";
            connString += "DBQ=" + pPath;
            OdbcConnection conn = new OdbcConnection(connString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = conn;
            string sheetName = this.GetExcelSheetName(pPath);
            //获取Excel中第一个Sheet名称，作为查询时的表名
            string sql = "select * from [" + sheetName.Replace('.', '#') + "$]";
            cmd.CommandText = sql;
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch
            {
                ds = null;
                throw new Exception("从Excel文件中获取数据时发生错误！");
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                da.Dispose();
                da = null;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn = null;
            }
        }
        private void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("释放资源时发生错误！");
            }
            finally
            {
                pObj = null;
            }
        }
        private string GetExcelSheetName(string pPath)
        {
            //打开一个Excel应用

            _excelApp = new Excel.Application();
            if (_excelApp == null)
            {
                throw new Exception("打开Excel应用时发生错误！");
            }
            _books = _excelApp.Workbooks;
            //打开一个现有的工作薄
            _book = _books.Add(pPath);
            _sheets = _book.Sheets;
            //选择第一个Sheet页
            _sheet = (Excel._Worksheet)_sheets.get_Item(1);
            string sheetName = _sheet.Name;

            ReleaseCOM(_sheet);
            ReleaseCOM(_sheets);
            ReleaseCOM(_book);
            ReleaseCOM(_books);
            _excelApp.Quit();
            ReleaseCOM(_excelApp);
            return sheetName;
        }
        void WriteData(object[] header, DataTable dt)
        {
            object[,] objData = new object[dt.Rows.Count, header.Length];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                var item = dt.Rows[j];
                for (int i = 0; i < header.Length; i++)
                {
                    var y = dt.Rows[j][i];
                    objData[j, i] = (y == null) ? "" : y.ToString();
                }
            }
            AddExcelRows("A2", dt.Rows.Count, header.Length, objData);
            AutoFitColumns("A1", dt.Rows.Count + 1, header.Length);

        }
        void Fillsheet(DataTable dt)
        {
            object[] header = CreateHeader(dt);
            WriteData(header, dt);
        }
        void SaveToExcel(string ExcelName, DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        CreateExcelRef();
                        Fillsheet(dt);
                        SaveExcel(ExcelName);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                ReleaseCOM(_sheet);
                ReleaseCOM(_sheets);
                ReleaseCOM(_book);
                ReleaseCOM(_books);
                ReleaseCOM(_excelApp);
            }

        }
        public bool ExportCanExcute(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count < 1) return false;
            else return true;
        }
        public void ExportExcute(DataTable dt)   //导出数据表
        {
            SaveFileDialog savefdiag = new SaveFileDialog();
            savefdiag.Filter = "Excel(*.xls)|*.xls";
            if ((bool)(savefdiag.ShowDialog()))
            {
                try
                {
                    SaveToExcel(savefdiag.FileName, dt);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("导出失败！" + e1.Message);
                }
                MessageBox.Show("导出成功！");
            }
        }
        public DataTable ImportExcute()   //导入数据表
        {
            OpenFileDialog openfilediag = new OpenFileDialog();
            openfilediag.Filter = "Excel(*.xls)|*.xls";
            if ((bool)(openfilediag.ShowDialog()))
            {
                string ExcelName = openfilediag.FileName;
                DataTable dt = LoadExcel(ExcelName);
                return dt;
            }
            else
                return null;
        }
    }
}
