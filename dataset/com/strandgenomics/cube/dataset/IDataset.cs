using framework;
using System;
namespace com.strandgenomics.cube.dataset
{

    public interface IDataset
    {

        /// <summary>
        /// Returns the name of the dataset.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Sets name of the dataset.
        /// </summary>
        /// <param name="name"></param>
        void SetName(string name);

        /// <summary>
        /// Returns the number of rows in the dataset.
        /// </summary>
        /// <returns></returns>
        int GetRowCount();

        /// <summary>
        /// Returns the number of columns in the dataset.
        /// </summary>
        /// <returns></returns>
        int GetColumnCount();

        /// <summary>
        /// Returns the column at the specified index.
        /// </summary>
        /// <param name="index">index of the column required.</param>
        /// <returns></returns>
        IColumn GetColumn(int index);

        /// <summary>
        /// Returns true if this dataset has a column with the specified name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasColumn(string name);

        /// <summary>
        /// Returns the column with specified name.
        /// </summary>
        /// <param name="name">name of the column required</param>
        /// <returns></returns>
        IColumn GetColumn(string name);

        /// <summary>
        /// Returns the columnMetaData of the column with specified name.
        /// </summary>
        /// <param name="name">name of the column required</param>
        /// <returns></returns>
        ColumnMetaData GetMetaData(string name);

        /// <summary>
        /// Sets the column meta data of column with the specified name.
        /// </summary>
        /// <param name="columnName">of the column required.</param>
        /// <param name="columnMetaData">for the column required</param>
        void SetMetaData(string columnName, ColumnMetaData columnMetaData);

        /// <summary>
        /// Returns the contents from all columns at specified index.
        /// </summary>
        /// <param name="index">row number required</param>
        /// <returns></returns>
        object[] GetRow(int index);

        /// <summary>
        /// Returns index of the specified column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        int IndexOf(IColumn column);

        /// <summary>
        /// Return an intset of sorted order for given column name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        IntSet GetRowIndicesInSortedOrder(string columnName, bool isAscending);

        /// <summary>
        /// Returns index of the specified column name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        int IndexOf(string columnName);

        /// <summary>
        /// Returns name of the specified column.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetColumnName(int index);

        /// <summary>
        /// Returns type of the specified column.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetColumnType(int index);

        //Removed Method Map getGroupInfo(); 
        //Reason: deprecated.

        /// <summary>
        ///  Adds the specified columns to the dataset. The set of columns 
        ///  added should have the same rowCount as that of the dataset.
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="column"> 
        /// is <c>null</c></exception>
        /// <param name="column"></param>
        void AddColumns(IColumn[] column);

        /// <summary>
        /// Adds the specified column to the dataset. The column being
        /// added should have the same rowCount as that of the dataset.
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="column"> 
        /// is <c>null</c></exception>
        /// <param name="column"></param>
        void AddColumn(IColumn column);

        /// <summary>
        /// Removes the specified columns from the dataset.
        /// </summary>
        ///  <exception cref="DataException">Thrown when <paramref name="column"> 
        /// is <c>null</c></exception>
        /// <param name="column"></param>
        void RemoveColumns(IColumn[] column);

        /// <summary>
        /// Removes the specified column from the dataset.
        /// </summary>
        ///  <exception cref="DataException">Thrown when <paramref name="column"> 
        /// is <c>null</c></exception>
        /// <param name="column"></param>
        void RemoveColumn(IColumn column);

        /// <summary>
        /// Replaces the column at the specified column index with the sprecified
        /// replaceColumn. Fires a <code>DataChangeEvent</code> notifying 
        /// listeners of the replace.
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="newColumn"> and <paramref name="columnIndex"/>
        /// is <c>null</c></exception>
        /// <param name="columnIndex"></param>
        /// <param name="newColumn"></param>
        void ReplaceColumn(int columnIndex, IColumn newColumn);

        /// <summary>
        /// Sets the lock state of the dataset. When the dataset is locked,
        /// it is guaranteed that the state of the dataset will not change.
        /// That means no columns will be removed/replaced in the dataset.
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="b">
        /// is <c>null</c></exception>
        /// <param name="b"></param>
        void SetLock(bool b);

        /// <summary>
        /// Returns whether the dataset is locked or not.
        /// </summary>
        /// <returns></returns>
        bool IsLocked();

        //Addlistner and remove listner is removed because it is not required in c#. (source of these methods Avadis java class).
        ///// <summary>
        ///// Adds a DataListener to this dataset.
        ///// </summary>
        ///// <param name="l"></param>
        //void AddDataListener(IDataChanged l);

        ///// <summary>
        ///// Remove the DataListener from this dataset.
        ///// </summary>
        ///// <param name="l"></param>
        //void RemoveDataListener(IDataChanged l);

        /// <summary>
        /// Cleanup.
        /// </summary>
        void Cleanup();
    }
}
