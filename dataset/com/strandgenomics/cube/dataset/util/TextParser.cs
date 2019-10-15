using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;

namespace com.strandgenomics.cube.dataset.util
{
    public sealed class TextParser
    {
        // private static final VMLogger logger = VMLogger.LOGGER;
        private static string DOT = ".";
        private static string EMPTY = "";
        private static string DEFAULT_SEPARATOR = ",";
        private static string DEFAULT_COMMENT_INDICATOR = "##";

        private int lineNumber, lineCount;
        private int columnCount;
        private string[] columnNames;
        private UpgradeableColumn[] columns;
        private string separator = DEFAULT_SEPARATOR;
        private string commentIndicator = DEFAULT_COMMENT_INDICATOR;
        //#framework
        //private IProgressModel parserMonitor;
        public TextParser()
        {
        }

        public void SetProperty(string key, Object value)
        {
            if (key.Equals("separator"))
                separator = (string)value;

            if (key.Equals("commentIndicator"))
                commentIndicator = (string)value;
            //#framework
            //if (key.Equals("progressMonitor"))
            //    parserMonitor = (IProgressModel)value;
        }

        public IDataset Parse(string path)
        {
            if (separator.Length != 1)
                Fatal("Separator must be single char");

            lineNumber = 0;
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(path);
                StreamReader s = new StreamReader(fs);
                lineCount = GetLineCount(s);
                s.Close();
                fs.Close();
                fs = File.OpenRead(path);
                s = new StreamReader(fs);
                ParseHeader(s);
                ParseData(s);
            }
            catch (Exception e)
            {
                throw new DataException("io: " + e.Message);
            }
            finally
            {
                try
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                catch (Exception e1)
                {
                    throw e1;
                }
            }

            IColumn[] cols = new IColumn[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                cols[i] = columns[i].GetColumn();
            }
            return (IDataset)DatasetFactory.CreateDataset(fs.Name, cols);


        }

        private void ParseData(StreamReader s)
        {
            string line;
            int rowIndex = 0;
            float progress = 1.0f / lineCount;

            // update progress after reading progressStep lines
            int progressStep = (lineCount + 99) / 100;

            while ((line = NextLine(s)) != null)
            { // per line

                string[] tokens = line.Split(separator.ToCharArray(), StringSplitOptions.None);
                for (int colIndex = 0; colIndex < columnCount && colIndex < tokens.Length; colIndex++)
                {
                    string tok = tokens[colIndex].Trim();
                    if (tok.Equals(EMPTY))
                        columns[colIndex].Add(null);
                    else
                        columns[colIndex].Add(tok);
                }

                // put missing-value for remaining columns if any
                if (tokens.Length < columnCount)
                {
                    Error(
                        "Too few values in row "
                            + rowIndex
                            + ". Marked as missing");
                }
                for (int colIndex = tokens.Length; colIndex < columnCount; colIndex++)
                {
                    columns[colIndex].Add(null);
                }

                rowIndex++;
                //#framework
                //if (rowIndex % progressStep == 0)
                //    UpdateProgress(rowIndex * progress);

            } // end for each line
            //#framework
            /*UpdateProgress(1);*/ // 100%
                                   // logger.info("#rows=" + rowIndex);
        }

        private void ParseHeader(StreamReader s)
        {
            HashSet<object> columnSet = new HashSet<object>();

            string line = NextLine(s);
            while (line != null && line.StartsWith(commentIndicator))
            { // gobble comments
                line = NextLine(s);
            }
            if (line == null)
                Fatal("Missing header");

            // limit is -1; check string API for meaning of negative limit.
            string[] tokens = line.Split(separator.ToCharArray(), StringSplitOptions.None);
            if (tokens.Length == 0)
                Fatal("empty line");

            columnNames = new string[tokens.Length];
            columns = new UpgradeableColumn[tokens.Length];
            for (int colIndex = 0; colIndex < tokens.Length; colIndex++)
            {
                string tok = tokens[colIndex].Trim();

                string colName;
                if (tok.Equals(EMPTY))
                {
                    colName = "Column-" + colIndex;

                    Error(
                        "Missing header for column-number "
                        + colIndex
                        + ". Will import as " + colName);
                }
                else
                {
                    colName = tok;
                }

                columnNames[colIndex] = GetUniqColName(columnSet, colName);
                columns[colIndex] = new UpgradeableColumn(lineCount - 1, columnNames[colIndex]);
            }

            columnCount = columnNames.Length;
            // logger.info("#columns=" + columnCount);
        }

        /// <summary>
        /// adds the specified column-name to 'columnNames'. If the name
        /// already exists, a unique name is generated.
        /// </summary>
        /// <param name="columnSet"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetUniqColName(HashSet<object> columnSet, string name)
        {
            string colName = name;
            int count = 0;
            while (columnSet.Contains(colName)) // name exists
                colName = name + DOT + (++count);

            columnSet.Add(colName);
            if (!name.Equals(colName)) // changed column-name
                Error("Duplicate column name " + name + ". Changed to " + colName);

            return colName;
        }


        private string GetColumnName(int index)
        {
            return columnNames[index];
        }

        private int GetLineCount(StreamReader input)
        {
            int lineCount = 0;
            while (input.ReadLine() != null)
                lineCount++;
            return lineCount;
        }


        private string NextLine(StreamReader s)
        {
            string l;
            while (true)
            {
                ++lineNumber;
                l = s.ReadLine();
                string patternStr = separator + "*";
                if (l == null || (l.Trim().Length > 0 && !Regex.IsMatch(patternStr, l.Trim())))
                    return l;
                Warning("Skipped empty line");
            }
        }

        //#framework
        //private void UpdateProgress(float p)
        //{
        //    if (parserMonitor != null)
        //    {
        //        if (!parserMonitor.canContinue())
        //        {
        //            parserMonitor.setExecutionState(IProgressModel.EXEC_ABORTED);
        //            throw new DataException("Parsing dataset has been aborted.");
        //        }
        //        parserMonitor.setProgressValue(p * 100);
        //    }
        //}

        private void Warning(string message)
        {
            /*
                if (parserMonitor != null)
                    parserMonitor.warning(lineNumber, message);
            */
        }

        private void Error(string message)
        {
            /*
                if (parserMonitor != null)
                    parserMonitor.error(lineNumber, message);
            */
        }

        private void Fatal(string message)
        {
            /*
                if (parserMonitor != null)
                    parserMonitor.fatal(lineNumber, message);
            */
            throw new DataException(message);
        }


    }
}
