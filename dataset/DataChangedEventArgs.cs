using System;
using framework;
namespace dataset
{
    //changed name since DatachangedEvent in java is actually EventArgs in c#.
    public class DataChangedEventArgs : EventArgs
    {
        public const int COLUMNS_ADDED = 0;
        public const int COLUMNS_REMOVED = 1;
        public const int COLUMNS_REPLACED = 2;
        public const int ROWS_ADDED = 3;


        private int type;

        public IntSet columnIndices;
        public IColumn[] columns;
        public int colCountBeforeChange;

        public IColumn[] oldColumns;
        public IColumn[] newColumns;

        public IntSet rowIndices;

        private IDataset source;

        public DataChangedEventArgs(IDataset source, int type)
        {
            this.source = source;
            this.type = type;
        }

        public IDataset GetSource()
        {
            return source;
        }

        /// <summary>
        /// changed name from getType to GetChangeType Since getType is already a method.
        /// </summary>
        /// <returns></returns>
        public int GetChangeType()
        {
            return type;
        }

        public IntSet GetColumnIndices()
        {
            return columnIndices;
        }

        public IColumn[] GetColumns()
        {
            return columns;
        }

        public int ColumnCountBeforeChange()
        {
            return colCountBeforeChange;
        }

        public IColumn[] GetOldColumns()
        {
            return oldColumns;
        }

        public IColumn[] GetNewColumns()
        {
            return newColumns;
        }

        public IntSet GetRowIndices()
        {
            return rowIndices;
        }


    }
    public delegate void DataChangedEventHandler(object sender, DataChangedEventArgs e);
}
