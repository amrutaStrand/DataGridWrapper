using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public class DatasetFactory
    {
        internal static IDataset CreateDataset(string v, IColumn[] cols)
        {
            throw new NotImplementedException();
        }

        internal static IDataset CreateDataset(string name, IColumn[] cols, int length)
        {
            throw new NotImplementedException();
        }
    }
}
