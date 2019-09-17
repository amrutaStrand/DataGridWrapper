using System.Collections.Generic;

namespace dataset
{
    public interface IMutableDataset : IDataset
    {

        /// <summary>
        /// Set a value
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="o"></param>
        void SetValue(int row, int column, object o);


        /// <summary>
        /// Add a row
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="data"></param>
        void AddRow(List<object> data);

        /// <summary>
        /// Add a row
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="data"></param>
        void AddRow(Dictionary<object, object> data);

        void IncrementRowCount();
    }
}
