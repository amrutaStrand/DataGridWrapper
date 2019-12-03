using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// Summarize a dataset by grouping (adding) together row that have the same value for
    /// the column by which the dataset needs to be summarized.
    /// 
    /// All the rows that have the same value for summarizing column are put together by adding the values in respective
    /// columns, except for the summarizing column, whose value is kept the same.
    /// 
    /// Relative summarization can be done by setting the isRelative to true. In such case, each value will be
    /// divided by the sum of the column it is present in, normalizing the values in each column. 
    /// After relative summarization, each column will add up to 1.0
    /// 
    /// The original dataset provided to the class is not modified, rather a new copy is created, modified and
    /// returned when required.
    /// 
    /// Structure of the input dataset:
    ///     [SummarizationCOlumn]	ValueColumn1	ValueColumn2	...		...
    /// 		Category1				...				...			...		...
    /// 		Category2				...				...			...		...
    /// 	    Category3				...				...			...		...		
    /// 	    	...					...				...			...		...
    /// 	    	
    /// @author priyanshu
    /// 
    /// </summary>
    public class SummarizeDataset
    {
        /// <summary>
        ///  initial unsummarized dataset
        /// </summary>
        private IDataset orgDataset;
        
        /// <summary>
        /// final summarized dataset
        /// </summary>
        private IDataset summarizedDataset;

        /// <summary>
        /// summarize the dataset by grouping the values in this column
        /// </summary>
        private IColumn targetColumn;

        /// <summary>
        /// Final target column after grouping together same values
        /// </summary>
        private List<string> summarizedColumn;

        /// <summary>
        /// dataset values grouped together by adding values in same group
        /// </summary>
        private List<List<object>> summarizedList;


        public static string SUMMARIZED_COLUMN_NAME = "Category";

        public SummarizeDataset(IDataset dataset, string columnName, bool isRelative)
        {
            orgDataset = dataset;
            targetColumn = orgDataset.GetColumn(columnName);
            summarizedList = InitializeClassificationListFloat(dataset.GetColumnCount() - 1);
            summarizedColumn = new List<string>();
            MakeSummarizeDataset(isRelative);
        }

        private void MakeSummarizeDataset(bool isRelative)
        {
            int numRows = orgDataset.GetRowCount();
            for (int i = 0; i < numRows; i++)
            {
                string category = (string)targetColumn.Get(i);
                int index = summarizedColumn.IndexOf(category);
                if (index == -1)
                {
                    // add entry to list
                    summarizedColumn.Add(category);
                    AppendFloatToList(summarizedList, orgDataset.GetRow(i));
                }
                else
                {
                    // add values to existing entries
                    AddFloatToList(summarizedList, orgDataset.GetRow(i), index);
                }
            }

            string[] datasetColumnNameList = DatasetUtil.GetColumnNames(orgDataset);
            CreateDatasetFromList(summarizedColumn, summarizedList, datasetColumnNameList, isRelative);
        }


        private float GetColumnSum(List<float> list)
        {
            float sum = 0.0f;
            foreach (float num in list)
            {
                sum += num;
            }
            return sum;
        }

        private void CreateDatasetFromList(List<string> summarizedColumn, List<List<object>> list, string[] columnNameList, bool isRelative)
        {

            IColumn[] columnList = new IColumn[list.Count + 1]; // 1 additional column for the category

            // First column of dataset as the category
            columnList[0] = ColumnFactory.CreateStringColumn(SUMMARIZED_COLUMN_NAME, summarizedColumn.ToArray());

            for (int i = 0; i < list.Count; i++)
            {
                float sum = isRelative == false ? 1.0f : GetColumnSum(list[i].ConvertAll(c=>  (float)c));
                float[] values = new float[list[i].Count];
                int index = 0;
                foreach (object value in list[i])
                {
                    values[index++] = (float)value / sum;
                }
                columnList[i + 1] = ColumnFactory.CreateFloatColumn(columnNameList[i], values);
            }

            summarizedDataset = DatasetFactory.CreateDataset(orgDataset.GetName(), columnList);
        }


        /// <summary>
        /// Returns the dataset in summarized form
        /// </summary>
        /// <returns></returns>
        public IDataset GetSummarizedDataset()
        {
            return summarizedDataset;
        }


        /// <summary>
        /// If the new category is not present in the summarized dataset yet,
        /// add a new row consisting the values obtained from the row of original dataset
        /// </summary>
        /// <param name="list"></param>
        /// <param name="row"></param>
        private void AppendFloatToList(List<List<object>> list, object[] row)
        {
            int i = 0;
            while (i < list.Count)
            {
                list[i].Add((float)((int)row[i]));
                i++;
            }
        }


        /// <summary>
        /// If a category is already present in the summarized dataset,
        /// add the values of new row to the already present row in the dataset,
        /// adding them together as a same category
        /// </summary>
        /// <param name="list"></param>
        /// <param name="row"></param>
        /// <param name="index"></param>
        private void AddFloatToList(List<List<object>> list, object[] row, int index)
        {
            int i = 0;
            while (i < list.Count)
            {
                float newValue = (float)list[i][index] + (float)((int)row[i]);
                list[i][index]= newValue;
                i++;
            }
        }

        /// <summary>
        /// Returns a list of empty lists of type float
        /// </summary>
        /// <param name="len">number of individual empty Lists to be initialized inside the list to be returned</param>
        /// <returns></returns>
        private List<List<object>> InitializeClassificationListFloat(int len)
        {
            List<List<object>> list = new List<List<object>>();

            for (int i = 0; i < len; i++)
            {
                list.Add(new List<object>());
            }
            return list;
        }

    }
}
