using com.strandgenomics.cube.dataset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApplication.ViewModels
{
    class Utils
    {
        public static string ReadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filePath = null;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                filePath = openFileDialog.FileName;

            return filePath;
        }

        private static char GetDelimeter(string line)
        {
            char delimeter = ',';
            string[] vals = line.Split('\t');
            if (vals.Length > 1)
                delimeter = '\t';
            return delimeter;
        }

        private static Type[] GetTypes(string[] vals)
        {
            List<Type> types = new List<Type>();
            foreach (string val in vals)
            {
                if (val.Length == 0)
                {
                    types.Add(typeof(string));
                    continue;
                }

                Type type = typeof(bool);
                try
                {
                    Convert.ToBoolean(val);
                }
                catch (FormatException)
                {
                    type = typeof(double);
                    try
                    {
                        Convert.ToDouble(val);
                    }
                    catch (FormatException)
                    {
                        type = typeof(string);
                    }
                }
                types.Add(type);
            }
            return types.ToArray();
        }

        public static IDataset CreateDataset(string filepath)
        {           
            
            Dataset dataset = new Dataset();
            string[] lines = File.ReadAllLines(filepath);
            char delimeter = GetDelimeter(lines[0]);
            string[] headers = lines[0].Split(delimeter);
            float[][] cols = new float[headers.Length][];

            if (lines.Length <= 1)
                return dataset;

            char[][] ids = new char[lines.Length-1][];
            for (int i = 0; i < headers.Length; i++)
                cols[i] = new float[lines.Length];

            for(int i=1; i<lines.Length; i++)
            {
                string line = lines[i];
                string[] vals = line.Split(delimeter);
                ids[i-1] = vals[0].ToCharArray();
                for (int j = 1; j < vals.Length; j++)
                    cols[j][i] = float.Parse(vals[j]);
            }

            StringColumn idColumn = new StringColumn(headers[0], ids);
            dataset.AddColumn(idColumn);
            for(int i=0; i<cols.Length; i++)
            {
                float[] col = cols[i];
                FloatColumn column = new FloatColumn(headers[i], col);
                dataset.AddColumn(column);
            }

            return dataset;
        }

        public static DataTable ParseData(string filepath)
        {
            DataTable data = new DataTable();
            string[] lines = File.ReadAllLines(filepath);

            char delimeter = GetDelimeter(lines[0]);
            string[] headers = lines[0].Split(delimeter);

            if (lines.Length <= 1)
                return data;

            string[] valsFirst = lines[1].Split(delimeter);
            Type[] types = GetTypes(valsFirst);

            int columnIndex = 0;
            foreach (string header in headers)
                data.Columns.Add(header, types[columnIndex++]);

            foreach (string line in lines.Skip(1))
            {
                DataRow row = data.NewRow();
                string[] vals = line.Split(delimeter);
                for (int i = 0; i < vals.Length; i++)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(data.Columns[i].DataType);
                    row[i] = converter.ConvertFromString(vals[i]);
                }
                data.Rows.Add(row);
            }

            return data;
        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }
}
