using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Functions;
using System.Text.RegularExpressions;

namespace com.strandgenomics.cube.dataset.util
{
    public class XLSXParser
    {
        private FileStream file;
        private ISheet sheet;
        //#framework
        //private IProgressModel parserMonitor;
        private IDataset dataset; /* dataset to be returned */
        private string datasetName;
        private int lineNumber;

        /// <summary>
        /// In excel sheet if cell has formula in it evaluate it else return cell content as such
        /// </summary>
        private IFormulaEvaluator formulaEvaluator;

        /// <summary>
        /// File input stream declared as global variable as in POI workbook cannot be closed so to prevent memory leak need to close inputStream 
        /// </summary>
        private StreamReader fin;

        /// <summary>
        /// Workbook instance will be created from the input excel file
        /// </summary>
        private IWorkbook wb;
        private int currRowNum = 0;

        /// <summary>
        /// variables to denote comments in file and field separator to be used for different cols in a row
        /// </summary>
        private string fieldSeparator, commentIndicator;

        /// <summary>
        /// numRows and numCols denote the numrow and numcols containing data and doesn't include blank row or cols
        /// </summary>
        private int numRows, numCols;

        /// <summary>
        /// totalRows and totalCols denotes the total number of rows and number of cols respectively in the excel sheet
        /// </summary>
        private int totalRows, totalCols;

        private static string EMPTY = "";
        private static string COLUMN = "Column-";
        private static string DOT = ".";

        /// <summary>
        /// default constructor
        /// </summary>
        public XLSXParser()
        {
            this.file = null;
            //this.parserMonitor = null;  //#framework
            this.fieldSeparator = null;
            this.commentIndicator = null;
        }

        /// <summary>
        /// Constructor given a FileStream as input 
        /// </summary>
        /// <param name="file"></param>
        public XLSXParser(FileStream file)
        {
            this.file = file;
            this.sheet = null;
            this.formulaEvaluator = null;
            this.fin = null;
            //this.parserMonitor = null;
            this.fieldSeparator = "\t";
            this.commentIndicator = "#";
        }

        /// <summary>
        /// Take file as input and will return the parsed dataset.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IDataset Parse(FileStream file)
        {
            this.file = file;
            fin = new StreamReader(file);
            return Parse(fin, file.Name);
        }

        /// <summary>
        /// Gets the workbook from the given InputStream, and gets the first
        /// sheet from it.
        /// By default we are using the first sheet. However, we can extend 
        /// functionality to either open ALL sheets in separate datatabs, or
        /// ask the user for which sheet to open.
        /// 
        /// Use formulaEvaluator to evaluate the content of the cell.
        /// Calls parseSheet to create the dataset and populate it.
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
                wb = new XSSFWorkbook(file);
                formulaEvaluator = wb.GetCreationHelper().CreateFormulaEvaluator();
                sheet = wb.GetSheetAt(0);
                ParseSheet();
                input.Close();
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
        /// Assumption taken is that number of columns in the sheet will be the number of cols in the firstNonEmptyRow
        /// Use the first non-empty row to create unique column names.
        /// Calls createDataset to parse the rest of the sheet and create 
        /// the dataset.
        /// </summary>
        private void ParseSheet()
        {


            totalRows = sheet.LastRowNum;
            if (totalRows == 0)
            {
                throw new DataException("The sheet is empty, and does not contain any data");
            }

            int firstRowNum = GetFirstNonEmptyRow(totalRows);
            int firstColNum = sheet.GetRow(firstRowNum).FirstCellNum;

            IRow firstRow = sheet.GetRow(firstRowNum);
            totalCols = firstRow.LastCellNum;

            numCols = firstRow.LastCellNum - firstRow.FirstCellNum;
            numRows = sheet.LastRowNum - sheet.FirstRowNum;

            string[] columnNames = GetColumnNames(firstRowNum, firstRow, numCols);

            int firstDataRowNum = firstRowNum + 1;
            int firstDataColNum = firstColNum;

            try
            {
                CreateDataset(firstDataRowNum, firstDataColNum, columnNames);
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine( e.StackTrace);
            }
        }


        /// <summary>
        /// We can directly access first Non Empty row in POI. But first non empty row may be row with comments.
        /// So to get first valid row with data we'll call this function.
        /// For column as we have got first row we can directly get the first non empty column.
        /// </summary>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        private int GetFirstNonEmptyRow(int totalRows)
        {
            int firstRow = sheet.FirstRowNum;
            while (firstRow <= totalRows)
            {
                IRow row = sheet.GetRow(firstRow);
                if (row == null)
                {
                    firstRow++;
                    continue;
                }
                else
                {
                    int firstCellInRow = row.FirstCellNum;
                    ICell firstCell = row.GetCell(firstCellInRow);
                    if (firstCell == null || (commentIndicator != null && firstCell.ToString().IndexOf(commentIndicator) > -1))
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
        /// Use the first row to populate the column Names array.
        /// Check for duplicate column names. Also check for missing 
        /// column names.
        /// </summary>
        /// <param name="firstRowNum"></param>
        /// <param name="row"></param>
        /// <param name="numCols"></param>
        /// <returns></returns>
        private string[] GetColumnNames(int firstRowNum, IRow row, int numCols)
        {

            string[] columnNames = new string[totalCols];
            Dictionary<string, int> columnNameCountMap = new Dictionary<string, int>(numCols);
            this.lineNumber = firstRowNum;

            int rowLength = row.LastCellNum - row.FirstCellNum;

            for (int j = 0; j < totalCols; j++)
            {
                string strVal = EMPTY;
                strVal = GetContents(row.GetCell(j));
                object o = columnNameCountMap[strVal];
                if (o == null)
                {
                    if ((strVal == null) || (strVal.Equals(EMPTY)))
                    {
                        Error("Missing header for column-number " + j + ". Will import as Column-" + j);
                        strVal = COLUMN + j;
                        columnNameCountMap.Add(COLUMN, int.Parse(j.ToString()));
                    }
                    else
                    {
                        columnNameCountMap.Add(strVal, int.Parse(0.ToString()));
                    }
                }
                else
                {
                    string msg = "Duplicate column name " + strVal + ".";
                    int count = ((int)o);
                    string oldStrVal = strVal;
                    bool colNameExists = true;
                    while (colNameExists)
                    {
                        ++count;
                        strVal = oldStrVal + DOT + count;
                        msg += " Changed to " + strVal;
                        if (columnNameCountMap.ContainsKey(strVal))
                            continue;
                        else
                        {
                            columnNameCountMap.Add(oldStrVal,  (int)count);
                            columnNameCountMap.Add(strVal,  (int)0);
                            colNameExists = false;
                        }
                    }
                    Error(msg);
                }
                columnNames[j] = strVal;
            }
            return columnNames;
        }


        /// <summary>
        /// This function will create the dataset taking firstDataRow ,FirstDataCil and column Names as input. 
        /// It will ignore the columns with all their cells empty.
        /// </summary>
        /// <param name="firstDataRow"></param>
        /// <param name="firstDataCol"></param>
        /// <param name="columnNames"></param>
        private void CreateDataset(int firstDataRow, int firstDataCol, string[] columnNames)
        {

            UpgradeableColumn[]
            columns = new UpgradeableColumn[columnNames.Length];
            for (int i = 0; i < columnNames.Length; i++)
            {
                columns[i] = new UpgradeableColumn(numRows, columnNames[i]);
            }

            // for checking if the column has all empty cells,
            // including the header cell.

            IRow headerRow = sheet.GetRow(firstDataRow - 1);
            int headerRowLength = headerRow.LastCellNum;

            bool[] emptyColumn = new bool[totalCols];
            for (int i = 0; i < totalCols; i++)
            {
                if (i < headerRowLength)
                {
                    string contents = GetContents(headerRow.GetCell(i));
                    if ((contents == null || contents == ""))
                        emptyColumn[i] = true;
                    else
                    {
                        emptyColumn[i] = false;
                    }
                }
                else
                    emptyColumn[i] = true;
            }

            int maxRows = 0;

            for (int i = firstDataRow; i <= totalRows; i++)
            {
                lineNumber = i;

                IRow dataRow = sheet.GetRow(i);
                int dataRowLength = dataRow.LastCellNum;

                // An empty row in the data can have the following condition
                // true.

                if (dataRowLength <= 0)
                    continue;

                int index = 0;
                for (int j = firstDataCol; j < totalCols; j++)
                {
                    if (j < dataRowLength)
                    {
                        string contents = GetContents(dataRow.GetCell(j));
                        columns[j].Add(contents);
                    }
                    else
                    {
                        columns[j].Add("");
                    }
                    // check if all cells in this column are not empty.

                    if (emptyColumn[j])
                    {
                        if ((j) < dataRowLength)
                        {
                            ICell cell = dataRow.GetCell(j);
                            if (cell != null && cell.CellType != CellType.Blank)
                                emptyColumn[j] = false;
                        }
                    }
                }

                if ((i - firstDataRow) % 100 == 0)
                {
                    int currentRow = i - firstDataRow;
                    float percentRowComplete = currentRow * 1.0f / totalRows;
                    //UpdateProgress(percentRowComplete * 100); //#framework
                }

                index++;
                if (index > maxRows)
                    maxRows = index;
            }

            int maxCols = 0;
            for (int i = 0; i < totalCols; i++)
            {
                if (!emptyColumn[i])
                    maxCols++;
            }
            IColumn[] colsArray = new IColumn[maxCols];
            maxCols = 0;
            for (int i = 0; i < totalCols; i++)
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
            //UpdateProgress(100.0f); //#framework

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

        //#framework
        /// <summary>
        /// Update the status how many rows have been read.
        /// </summary>
        /// <param name="p"></param>
        //private void updateProgress(float p)
        //{
        //    if (parserMonitor != null)
        //        parserMonitor.setProgressValue(p);
        //}


        public void SetProperty(string key, object value)
        {
            if (key.Equals("separator"))
                fieldSeparator = (string)value;
            //else if (key.Equals("progressMonitor"))
            //    parserMonitor = (IProgressModel)value;
            else if (key.Equals("commentIndicator"))
                commentIndicator = (string)value;
        }

        /// <summary>
        /// returns the total number of lines in the sheet
        /// </summary>
        /// <returns></returns>
        public int GetLineCount()
        {
            if (sheet == null)
                return 0;
            return totalRows;
        }

        /// <summary>
        /// get-set methods for filedSeparator and commentIndicator
        /// </summary>
        /// <param name="fs"></param>
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
        /// This function will return the contents of the cell if cell is not empty else it will return null
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected string GetContents(ICell c)
        {
            if (c != null && (c.CellType != CellType.Blank))
            {
                string s = null;
                if (c.CellType == CellType.String)
                    s = c.StringCellValue;

                else if (formulaEvaluator != null)
                    s = formulaEvaluator.Evaluate(c).FormatAsString();

                if (s == null)
                    s = c.ToString();
                if (s.Trim().Equals(EMPTY))
                    return null;
                if ( Regex.IsMatch(s,"\\s"))
                    return null;
                return s;
            }
            else
                return "";
        }


        /// <summary>
        /// Open the workbook and sheet and set initial variables like
        /// the number of rows, columns.
        /// </summary>
        public void OpenReader()
        {

            if (file == null)
                throw new IOException("No files associated with the parser");
            try
            {
                fin = new StreamReader(file);
                wb = WorkbookFactory.Create(file);
                formulaEvaluator = wb.GetCreationHelper().CreateFormulaEvaluator();
                sheet = wb.GetSheetAt(0);
            }
            catch (Exception e)
            {
                throw new IOException(e.Message);
            }

            if (sheet == null)
                throw new IOException("Invalid file format");

            totalRows = sheet.LastRowNum;

            if (totalRows == 0)
                throw new IOException("Invalid file format");

            totalCols = sheet.GetRow(sheet.FirstRowNum).LastCellNum;
        }

        /// <summary>
        /// Read a row of the spreadsheet and return it as a line (type String)
        /// separated by the fieldSeparator.
        /// This is a hack for conforming to IImportReader of the file import 
        /// wizard, since the rest of the importers use this framework to parse.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            if (sheet == null)
                return null;
            if (currRowNum > GetLineCount())
                return null;
            StringBuilder line = new StringBuilder();
            IRow row = sheet.GetRow(currRowNum);

            if (row == null)
            {
                currRowNum++;
                return line.ToString();
            }

            for (int i = 0; i < row.LastCellNum; i++)
            {
                line.Append(GetContents(row.GetCell(i)));
                line.Append(fieldSeparator);
            }
            for (int i = row.LastCellNum; i < totalCols; i++)
            {
                line.Append(fieldSeparator);
            }
            currRowNum++;
            return line.ToString();
        }


        public void CloseReader()
        {
            if (fin != null)
                fin.Close();
        }


        public static void Main(string[] args)
        {
            try
            {
                string filename = args[0];
                FileStream file1 = new FileStream(filename,FileMode.Open);
                XLSXParser p = new XLSXParser(file1);
                p.Parse(file1);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


    }
}
