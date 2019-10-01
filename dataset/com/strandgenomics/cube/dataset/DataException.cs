using System;

namespace com.strandgenomics.cube.dataset
{
    public class DataException : Exception
    {
        public DataException(string message) : base(string.Format("Invalid Student Name: {0}", message))
        {

        }
    }
}
