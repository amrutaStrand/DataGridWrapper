using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;

namespace com.strandgenomics.cube.dataset.util
{
    public class XLSParser
    {
        /* private variables */
        private FileStream file;      /* file being parsed */
        private ISheet sheet;    /* first sheet in the XL workbook */

        //#framework
        //private IProgressModel parserMonitor;
        private IDataset dataset;
        private string datasetName;
        private int lineNumber;

        // IImportReader related
        private IWorkbook wb;
        private int currRowNum = 0;
        private string fieldSeparator, commentIndicator;
        private int numRows, numCols;

        private static string EMPTY = "";
        private static string COLUMN = "Column-";
        private static string DOT = ".";


        public XLSParser()
        {
            this.file = null;
            //this.parserMonitor = null;  //#framework
            this.fieldSeparator = null;
            this.commentIndicator = null;
        }

        //#framework
        //public XLSParser(IProgressModel parserMonitor)
        //{
        //    this.file = null;
        //    this.parserMonitor = parserMonitor;
        //}

        public XLSParser(FileStream file)
        {
            this.file = file;
            this.sheet = null;
            //this.parserMonitor = null;
            this.fieldSeparator = "\t";
            this.commentIndicator = "#";
        }

        public IDataset Parse(FileStream file)
        {
            this.file = file;
            return Parse(new StreamReader(file), file.Name);
        }

        public void SetProperty(string key, Object value)
        {
            if (key.Equals("separator"))
                fieldSeparator = (string)value;
            //else if (key.Equals("progressMonitor"))  //#framework
            //    parserMonitor = (IProgressModel)value;
            else if (key.Equals("commentIndicator"))
                commentIndicator = (string)value;
        }

        /// <summary>
        /// Gets the workbook from the given InputStream, and gets the first
        /// sheet from it.
        /// By default we are using the first sheet. However, we can extend 
        /// functionality to either open ALL sheets in separate datatabs, or
        /// ask the user for which sheet to open.
        /// 
        /// Calls parseSheet to create the dataset and populate it.
        /// 
        /// Calls close on the workbook to free up all allocated memory used
        /// by the API.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="datasetName"></param>
        /// <returns></returns>
        public IDataset Parse(StreamReader input, string datasetName)
        {

            this.lineNumber = 0;
            this.datasetName = datasetName;

            try
            {
                IWorkbook wb = new HSSFWorkbook(file);
                sheet = wb.GetSheetAt(0);
                ParseSheet();
                wb.Close();
            }
            catch (Exception e)
            {
                throw new DataException(e.Message);
            }

            return dataset;
        }


        /// <summary>
        /// Check the validity of the sheet to see it has some data in it.
        /// Get the first non-empty row and non-empty column to start parsing.
        /// Use the first non-empty row to create unique column names.
        /// Calls createDataset to parse the rest of the sheet and create 
        /// the dataset.
        /// </summary>
        private void ParseSheet()
        {


            int numRows =  sheet.LastRowNum - sheet.FirstRowNum;
            int numCols = sheet.GetRow(sheet.FirstRowNum).LastCellNum - sheet.GetRow(sheet.FirstRowNum).FirstCellNum;
            if (numRows == 0 || numCols == 0)
            {
                throw new DataException(
                    "The sheet is empty, and does not contain any data");
            }

            int firstRow = GetFirstNonEmptyRow(numRows);
            int firstCol = GetFirstNonEmptyCol(numCols);

            ICell[] rowCells = sheet.GetRow(firstRow).Cells.ToArray();
            string[] columnNames = GetColumnNames(firstRow, rowCells, numCols);
            int firstDataRow = firstRow + 1;
            int firstDataCol = firstCol;
            CreateDataset(firstDataRow, firstDataCol, columnNames);
        }

        /// <summary>
        /// There might be empty rows or columns in the beginning of 
        /// this block of numRows x numColumns. Omitting them.
        /// BUG - the condition for checking columns does not seem to 
        /// work. Even if a column has all empty values, sheet.getColumn() 
        /// returns Cell[] with length numRows-1.
        /// </summary>
        /// <param name="numRows"></param>
        /// <returns></returns>
        private int GetFirstNonEmptyRow(int numRows)
        {
            int firstRow = 0;
            while (firstRow < numRows)
            {
                ICell[] row = sheet.GetRow(firstRow).Cells.ToArray();
                if (row == null || row.Length == 0)
                {
                    firstRow++;
                    continue;
                }
                else
                {
                    if (row[0].StringCellValue.IndexOf(commentIndicator) > -1)
                    {
                        firstRow++;
                        continue;
                    }
                    else
                        break;
                }
            }
            return firstRow;
        }

        /// <summary>
        /// The following doesnt work. It does not detect columns with
        /// all empty cells.
        /// </summary>
        /// <param name="numCols"></param>
        /// <returns></returns>
        private int GetFirstNonEmptyCol(int numCols)
        {
            int firstCol = 0;
            while (firstCol < numCols)
            {
                ICell col = sheet.GetRow(sheet.FirstRowNum).GetCell(firstCol);
                if (col == null)
                {
                    firstCol++;
                    continue;
                }
                else
                {
                    break;
                }
            }
            return firstCol;
        }

        /// <summary>
        /// Use the first row to populate the column Names array.
        /// Check for duplicate column names. Also check for missing 
        /// column names.
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="rowCells"></param>
        /// <param name="numCols"></param>
        /// <returns></returns>

        private string[] GetColumnNames(int firstRow, ICell[] rowCells, int numCols)
        {

            string[] columnNames = new string[numCols];
            Dictionary<string,int> columnNameCountMap = new Dictionary<string, int>(numCols);
            this.lineNumber = firstRow;

            for (int j = 0; j < numCols; j++)
            {
                string strVal = EMPTY;
                if (j < rowCells.Length)
                    strVal = rowCells[j].StringCellValue;
                object o = columnNameCountMap[strVal];
                if (o == null)
                {
                    if ((strVal == null) || (strVal.Equals(EMPTY)))
                    {
                        Error("Missing header for column-number " +
                    j + ". Will import as Column-" + j);
                        strVal = COLUMN + j;
                        columnNameCountMap.Add(COLUMN, int.Parse(j.ToString()));
                    }
                    else
                    {
                        columnNameCountMap.Add(strVal,0);
                    }
                }
                else
                {
                    string msg = "Duplicate column name " + strVal + ".";
                    int count = ((int)o);
                    string oldStrVal = strVal;
                    ++count;
                    strVal = oldStrVal + DOT + count;
                    msg += " Changed to " + strVal;
                    Error(msg);
                    columnNameCountMap.Add(oldStrVal, int.Parse(count.ToString()));
                    columnNameCountMap.Add(strVal, 0);
                }
                columnNames[j] = strVal;
            }

            return columnNames;
        }


        private string GetContents(ICell c)
        {
            string s = c.StringCellValue;
            if (s.Trim().Equals(EMPTY))
                return null;
            if (Regex.IsMatch(s,"\\s"))
                return null;
            return s.Trim();
        }

        /// <summary>
        /// Parse the sheet roe by row to populate the dataset.
        /// </summary>
        /// <param name="firstDataRow"></param>
        /// <param name="firstDataCol"></param>
        /// <param name="columnNames"></param>
        private void CreateDataset(int firstDataRow, int firstDataCol, string[] columnNames)
        {


            int numRows = sheet.LastRowNum - sheet.FirstRowNum;
            int numCols = sheet.GetRow(sheet.FirstRowNum).Cells.Count;
            int totalCols = numCols - firstDataCol;
            int totalRows = numRows - firstDataRow;

            UpgradeableColumn[] columns = new UpgradeableColumn[columnNames.Length];
            for (int i = 0; i < columnNames.Length; i++)
            {
                columns[i] = new UpgradeableColumn(totalRows, columnNames[i]);
            }

            // for checking iof the column has all empty cells,
            // including the header cell.
            ICell[] headerCells = sheet.GetRow(firstDataRow - 1).Cells.ToArray();
            bool[] emptyColumn = new bool[numCols];
            for (int i = 0; i < numCols; i++)
            {
                if (i < headerCells.Length)
                {
                    CellType headerCellType = headerCells[i].CellType;
                    string contents = headerCells[i].StringCellValue;
                    if (headerCellType.Equals(CellType.Blank) || contents == null || contents == "")
                        emptyColumn[i] = true;
                    else
                        emptyColumn[i] = false;
                }
                else
                    emptyColumn[i] = true;
            }

            int maxRows = 0;
            for (int i = firstDataRow; i < numRows; i++)
            {
                lineNumber = i;
                ICell[] rowCells = sheet.GetRow(i).Cells.ToArray();

                // An empty row in the data can have the following condition
                // true. (But it doesnt seem to be so always).
                if (rowCells.Length <= firstDataCol)
                    continue;
                if (rowCells == null || rowCells.Length == 0)
                {
                    continue;
                }
                // Ignore the row, if all cells are empty in this row.
                bool emptyRow = true;
                for ( int x = 0; x < rowCells.Length; x++)
                {
                    string contents = GetContents(rowCells[x]);
                    if (contents != null && contents.Trim() != "")
                    {
                        emptyRow = false;
                        break;
                    }
                }
                if (emptyRow)
                    continue;
                int index = 0;
                for (int j = firstDataCol; j < numCols; j++)
                {
                    if (j < rowCells.Length)
                    {
                        string contents = GetContents(rowCells[j]);
                        columns[j].Add(contents);
                    }
                    else
                        columns[j].Add("");

                    // check if all cells in this column are not empty.
                    if (emptyColumn[j])
                    {
                        if (j < rowCells.Length)
                        {
                            CellType cellType = rowCells[j].CellType;
                            string contents = rowCells[j].StringCellValue;
                            if (!cellType.Equals(CellType.Blank))
                                emptyColumn[j] = false;
                        }
                    }
                }
                if ((i - firstDataRow) % 100 == 0)
                {
                    int currentRow = i - firstDataRow;
                    float percentRowComplete = currentRow * 1.0f / totalRows;
                    //		float percent = (currentCol + percentRowComplete)*1.f/totalCols;
                    //UpdateProgress(percentRowComplete * 100);
                }

                index++;
                if (index > maxRows)
                    maxRows = index;
            }

            int maxCols = 0;
            for (int i = 0; i < numCols; i++)
            {
                if (!emptyColumn[i])
                    maxCols++;
            }
            IColumn[] colsArray = new IColumn[maxCols];
            maxCols = 0;
            for (int i = 0; i < numCols; i++)
            {
                if (emptyColumn[i])
                {
                    Warning("Omitting column " + columnNames[i] + ", since all cells in this column are empty");
                    continue;
                }
                else
                {
                    colsArray[maxCols] = columns[i].GetColumn();
                    maxCols++;
                }
            }
            dataset = DatasetFactory.CreateDataset(datasetName, colsArray);
            //UpdateProgress(100.0f);
        }


        private void Warning(string message)
        {
            Console.WriteLine(message);
            /*
                if (parserMonitor != null)
                    parserMonitor.warning(lineNumber, message);
            */
        }

        private void Error(string message)
        {
            Console.WriteLine(message);
            /*
                if (parserMonitor != null)
                    parserMonitor.error(lineNumber, message);
            */
        }

        private void Fatal(string message)
        {
            Console.WriteLine(message);
            /*
                if (parserMonitor != null)
                    parserMonitor.fatal(lineNumber, message);
            */
            throw new DataException(message);
        }

        //#framework
        //private void UpdateProgress(float p)
        //{
        //    if (parserMonitor != null)
        //        parserMonitor.setProgressValue(p);
        //}

        // Start IImportReader implementation
        public int GetLineCount()
        {
            if (sheet == null)
                return 0;

            return numRows;
        }

        public void SetFieldSeparator(string fs)
        {
            this.fieldSeparator = fs;
        }

        public string GetFieldSeparator()
        {
            return this.fieldSeparator;
        }

        public void SetCommentIndicator(string comment)
        {
            this.commentIndicator = comment;
        }

        public string GetCommentIndicator()
        {
            return this.commentIndicator;
        }

        /// <summary>
        /// Open the workbook and sheet and set initial variables like
        /// the number of rows, columns, and first non-empty row, column.
        /// </summary>
        public void OpenReader()
        {

            if (file == null)
                throw new IOException("No files associated with the parser");

            try
            {
                StreamReader input = new StreamReader(file);
                wb = new HSSFWorkbook(file);
                sheet = wb.GetSheetAt(0);
            }
            catch (Exception e)
            {
                throw new IOException(e.Message);
            }

            numRows = sheet.LastRowNum - sheet.FirstRowNum;
            numCols = sheet.GetRow(sheet.FirstRowNum).Cells.Count;
        }

        public string ReadLine()
        {
            if (sheet == null)
                return null;
            if (currRowNum >= GetLineCount())
                return null;

            StringBuilder line = new StringBuilder();
            ICell[] rowCells = sheet.GetRow(currRowNum).Cells.ToArray();
            for (int i = 0; i < rowCells.Length; i++)
            {
                line.Append(rowCells[i].StringCellValue);
                line.Append(fieldSeparator);
            }
            for (int i = rowCells.Length; i < numCols; i++)
            {
                line.Append(fieldSeparator);
            }
            currRowNum++;

            return line.ToString();
        }


        public void CloseReader()
        {
            if (wb != null)
                wb.Close();
        }

        // End IImportReader implementation

        public static void Main(string[] args)
        {
            try
            {
                string filename = args[0];
                FileStream file = new FileStream(filename,FileMode.Open);
                XLSParser p = new XLSParser(file);
                p.Parse(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


    }
}
