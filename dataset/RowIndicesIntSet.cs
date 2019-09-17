using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    class RowIndicesIntSet : IntSet
    {
        AbstractRegularColumn abstractRegular;
        List<int> sortOrder;
        int size;

        public RowIndicesIntSet(AbstractRegularColumn abstractRegularColumn)
        {
            abstractRegular = abstractRegularColumn;
            sortOrder = new List<int>( abstractRegular.GetSortOrder());
            this.size = abstractRegular.GetSize();
        }
        /// <summary>
        /// for empty rowindices set.
        /// </summary>
        public RowIndicesIntSet()
        {

        }

        public bool Contains(int element)
        {
            return (0 <= element && element < this.size);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return (IEnumerator<int>)sortOrder;
        }
    }
}
