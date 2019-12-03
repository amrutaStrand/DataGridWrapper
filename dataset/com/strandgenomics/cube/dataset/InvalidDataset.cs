using framework;
using System;

namespace com.strandgenomics.cube.dataset
{
    public class InvalidDataset : IDataset
    {
        /** Hexff format id. */
        public const string FORMAT_ID = "cube.dataset.InvalidDataset";

        public const string DEFAULT_NAME = "InvalidDataset";
        public const string DEFAULT_REASON = "Don't Know";

        private string datasetName;

        /**
         * Reason for being invalid
         */
        private string reasonForBeingInvalid;

        public InvalidDataset()
        {
            this.datasetName = DEFAULT_NAME;
            this.reasonForBeingInvalid = DEFAULT_REASON;
        }

        public InvalidDataset(string datasetName, string reasonForBeingInvalid)
        {
            this.datasetName = datasetName;
            this.reasonForBeingInvalid = reasonForBeingInvalid;
        }

        public void AddColumn(IColumn column)
        {
            throw new NotImplementedException();
        }

        public void AddColumns(IColumn[] column)
        {
            throw new NotImplementedException();
        }

        public void AddDataListener(IDataChanged l)
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        public IColumn GetColumn(int index)
        {
            throw new NotImplementedException();
        }

        public IColumn GetColumn(string name)
        {
            throw new NotImplementedException();
        }

        public int getColumnCount()
        {
            throw new NotImplementedException();
        }

        public int GetColumnCount()
        {
            throw new NotImplementedException();
        }

        public string GetColumnName(int index)
        {
            throw new NotImplementedException();
        }

        public string GetColumnType(int index)
        {
            throw new NotImplementedException();
        }

        public ColumnMetaData GetMetaData(string name)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public String getReason()
        {
            return reasonForBeingInvalid;
        }

        public object[] GetRow(int index)
        {
            throw new NotImplementedException();
        }

        public int GetRowCount()
        {
            throw new NotImplementedException();
        }

        public IntSet getRowIndicesInSortedOrder(string columnName, bool isAscending)
        {
            throw new NotImplementedException();
        }

        public IntSet GetRowIndicesInSortedOrder(string columnName, bool isAscending)
        {
            throw new NotImplementedException();
        }

        public bool HasColumn(string name)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(IColumn column)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(string columnName)
        {
            throw new NotImplementedException();
        }

        public bool IsLocked()
        {
            throw new NotImplementedException();
        }

        public void RemoveColumn(IColumn column)
        {
            throw new NotImplementedException();
        }

        public void RemoveColumns(IColumn[] column)
        {
            throw new NotImplementedException();
        }

        public void RemoveDataListener(IDataChanged l)
        {
            throw new NotImplementedException();
        }

        public void ReplaceColumn(int columnIndex, IColumn newColumn)
        {
            throw new NotImplementedException();
        }

        public void SetLock(bool b)
        {
            throw new NotImplementedException();
        }

        public void SetMetaData(string columnName, ColumnMetaData columnMetaData)
        {
            throw new NotImplementedException();
        }

        public void SetName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
