using System;
using System.Collections.Generic;

namespace dataset
{
    public class ColumnMetaData
    {
        public const string FORMAT_ID = "cube.dataset.ColumnMetaData";

        /// <summary>
        /// Source for the column. The column source may occassionaly be different from
        /// the column name.
        /// </summary>
        private String source;

        /// <summary>
        ///  Map state used to store miscellaneous information about the column
        /// </summary>
        Dictionary<object, object> state;

        /// <summary>
        /// The mark value for the column.
        /// </summary>
        private String mark;

        public ColumnMetaData()
        {
        }

        public void SetSource(string source)
        {
            this.source = source;
        }

        public string GetSource()
        {
            return source;
        }

        public void SetMark(string s)
        {
            mark = s;
        }

        public string GetMark()
        {
            return mark;
        }

        public bool IsMarked()
        {
            return mark != null;
        }

        public Dictionary<object, object> GetState()
        {
            if (state == null)
                state = new Dictionary<object, object>();

            return state;
        }

    }
}
