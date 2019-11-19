using framework;
using System;
namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// moved constants from java interface in DatasetConstants so use it for any constants you may need. i.e.
    /// INTEGER_MV, FLOAT_MV, LONG_MV
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// returns column-name
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// sets the name of the column.
        /// </summary>
        /// <param name="name"></param>
        void SetName(string name);

        /// <summary>
        /// return datatype
        /// </summary>
        /// <returns></returns>
        string GetDatatype();

        //data access methods
        /// <summary>
        /// eturns the size of the column
        /// </summary>
        /// <returns></returns>
        int GetSize();

        /// <summary>
        /// returns a numeric representation for the value at 
        /// specified index. in case of categorical columns, it
        /// might be the categoryIndex at that index. but all 
        /// columns must support it.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        float GetNumericValue(int index);

        /// <summary>
        /// returns the integer value at specified index
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="index">
        /// is <c>null</c></exception>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetInt(int index);

        /// <summary>
        ///  returns the float value at specified index
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="index">
        /// is <c>null</c></exception>
        /// <param name="index"></param>
        /// <returns></returns>
        float GetFloat(int index);

        /// <summary>
        /// returns an object representing column value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object Get(int index);

        /// <summary>
        /// returns a comparable object representing column value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IComparable GetComparable(int index);

        // meta data for the column.


        /// <summary>
        ///  Gets the meta data for the column.
        /// </summary>
        /// <returns></returns>
        ColumnMetaData GetMetaData();

        /// <summary>
        /// Sets the meta data for the column.
        /// </summary>
        /// <param name="metaData"></param>
        void SetMetaData(ColumnMetaData metaData);

        // if a column cannot be set as categorical/continuous, it throws
        // DataException

        /// <summary>
        /// sets the column type as categorical/continuous.
        /// </summary>
        /// <exception cref="DataException">Thrown when <paramref name="b">
        /// is <c>null</c></exception>
        /// <param name="b"></param>
        void SetCategorical(bool b);

        /// <summary>
        /// returns whether the column type is categorical/continuous.
        /// </summary>
        /// <returns></returns>
        bool IsCategorical();


        // state related methods. some of the following are valid when the column 
        // is categorical, others when it is continuous. column throws exception
        // when method called is not in tandem with state.
        /// <summary>
        /// returns the sum of the values in the column
        /// </summary>
        /// <exception cref="DataException">
        /// <returns></returns>
        float GetSum();

        /// <summary>
        /// returns #categories
        /// </summary>
        /// <exception cref="DataException">
        /// <returns></returns>
        int GetCategoryCount();

        /// <summary>
        /// returns the #items in the category
        /// </summary>
        /// <exception cref="DataException"> Thrown when <paramref name="categoryIndex">
        /// is <c>invalid</c></exception>
        /// <param name="categoryIndex"></param>
        /// <returns></returns>
        int GetCategorySize(int categoryIndex);

        /// <summary>
        /// returns the index of representaive row index for the
        /// specified category index.
        /// </summary>
        /// <exception cref="DataException"> Thrown when <paramref name="categoryIndex">
        /// is <c>invalid</c></exception>
        /// <param name="categoryIndex"></param>
        /// <returns></returns>
        object GetCategoryValue(int categoryIndex);

        /// <summary>
        ///  Returns the indices of values that belongs to the category
        ///  specified by the categoryIndex.
        /// </summary>
        /// <exception cref="DataException"> Thrown when <paramref name="categoryIndex">
        /// is <c>invalid</c></exception>
        /// <param name="categoryIndex"></param>
        /// <returns></returns>
        IntSet GetRowIndicesOfCategory(int categoryIndex);

        /// <summary>
        /// Returns the indices of values that belongs to the category
        ///  specified by the categoryIndex.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        int GetCategoryIndex(int rowIndex);

        // missing values information

        /// <summary>
        /// returns true if value at specified index is missing-value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool IsMissingValue(int index);

        /// <summary>
        /// Returns the number of missing values in the column.
        /// </summary>
        /// <returns></returns>
        int GetMissingValueCount();

        /// <summary>
        /// Returns the indices of the missing values in the column. Returns
        /// an empty int set if the column has no missing values.
        /// </summary>
        /// <returns></returns>
        IntSet GetMissingValueIndices();

        // bounds information

        /// <summary>
        /// returns the index of the element with minimum value
        /// </summary>
        /// <returns></returns>
        int GetMinIndex();

        /// <summary>
        /// returns the index of the element with maximum value
        /// </summary>
        /// <returns></returns>
        int GetMaxIndex();

        /// <summary>
        /// Returns the row indices in the sorted order of their
        /// values.If ascending is set to true,
        /// the first index of the array returned is 
        /// the index of the smallest element in the column, while
        /// the last index is the index of the largest element.If
        /// ascending is set to false, then the reverse order is returned.
        /// </summary>
        /// <param name="ascending"></param>
        /// <returns></returns>
        IntSet GetRowIndicesInSortedOrder(bool ascending);

        /// <summary>
        /// Returs the same as
        /// <code> GetRowIndicesInRange (min, false, max, false)</code>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        IntSet GetRowIndicesInRange(float min, float max);

        /// <summary>
        /// Returns the indices of the rows with values in the range min, max.
        ///    x = {i | min &lt;= column.get(i) &lt;= max}
        /// If minOpen is true, then the following set is returned:
        /// x = { i | min & lt; column.get(i) &lt;= max}
        /// If maxOpen is true, then the following set is returned:
        /// x = { i | min & lt;= column.get(i) &lt; max}
        /// </summary>
        /// <param name="min"></param>
        /// <param name="minOpen"></param>
        /// <param name="max"></param>
        /// <param name="maxOpen"></param>
        /// <returns></returns>
        IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen);

        

        /// <summary>
        /// Adds a column listener to this column. This method is
        /// for internal use only for the dataset to be aware of
        /// column change events.
        /// </summary>
        /// <param name="l"></param>
        void AddColumnListener();

        /// <summary>
        ///  Removes a column listener from this column. This method 
        ///  is for internal use only.
        /// </summary>
        /// <param name="l"></param>
        void RemoveColumnListener();

    }
}
