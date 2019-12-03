using framework;
using System;

namespace com.strandgenomics.cube.dataset
{
    public class NullColumn : IColumn
    {
        private string name;

        public NullColumn(string name)
        {
            this.name = name;
        }

        public void AddColumnListener()
        {
            throw new NotImplementedException();
        }

        public void AddColumnListener(IColumnChanged o)
        {
            throw new NotImplementedException();
        }

        public object Get(int index)
        {
            return "";
        }

        public int GetCategoryCount()
        {
            return DatasetConstants.INTEGER_MV;
        }

        public int GetCategoryIndex(int rowIndex)
        {
            return DatasetConstants.INTEGER_MV;
        }

        public int GetCategorySize(int categoryIndex)
        {
            return DatasetConstants.INTEGER_MV;

        }

        public object GetCategoryValue(int categoryIndex)
        {
            return null;
        }

        public IComparable GetComparable(int index)
        {
            return null;
        }

        public string GetDatatype()
        {
            return "string";
        }

        public decimal GetDecimal(int index)
        {
            return DatasetConstants.DECIMAL_MV;
        }

        public double GetDouble(int index)
        {
            return DatasetConstants.DOUBLE_MV;
        }

        public float GetFloat(int index)
        {
            return DatasetConstants.FLOAT_MV;
        }

        public int GetInt(int index)
        {
            return DatasetConstants.INTEGER_MV;
        }

        public int GetMaxIndex()
        {
            return DatasetConstants.INTEGER_MV;
        }

        public ColumnMetaData GetMetaData()
        {
            return null;
        }

        public int GetMinIndex()
        {
            return -1;
        }

        public int GetMissingValueCount()
        {
            return DatasetConstants.INTEGER_MV;
        }

        public IntSet GetMissingValueIndices()
        {
            return null;
        }

        public string GetName()
        {
            return name;
        }

        public float GetNumericValue(int index)
        {
            return DatasetConstants.FLOAT_MV;
        }

        public IntSet GetRowIndicesInRange(float min, float max)
        {
            return null;
        }

        public IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            return null;
        }

        public IntSet GetRowIndicesInSortedOrder(bool ascending)
        {
            return null;
        }

        public IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            return null;
        }

        public int GetSize()
        {
            return -1;
        }

        public float GetSum()
        {
            return DatasetConstants.FLOAT_MV;
        }

        public bool IsCategorical()
        {
            return false;
        }

        public bool IsMissingValue(int index)
        {
            return true;
        }

        public void RemoveColumnListener()
        {
            throw new NotImplementedException();
        }

        public void RemoveColumnListener(IColumnChanged o)
        {
            throw new NotImplementedException();
        }

        public void SetCategorical(bool b)
        {

        }

        public void SetMetaData(ColumnMetaData metaData)
        {

        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}
