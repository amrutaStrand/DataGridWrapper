using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    /// <summary>
    /// this is helper class for AbstractRegularColumn.GetRowIndicesInRange().
    /// </summary>
    public class FloorIntset : IntSet
    {
        private AbstractRegularColumn c;
        private bool maxOpen;
        private object omax;
        private bool minOpen;
        private object omin;
        private int begin;
        private int end;
        private List<int> sortOrder;

        public FloorIntset(AbstractRegularColumn c, bool maxOpen, object omax,  bool minOpen, object omin, int begin, int end)
        {
            this.c = c;
            this.maxOpen = maxOpen;
            this.minOpen = minOpen;
            this.omin = omin;
            this.omax = omax;
            this.begin = begin;
            this.end = end;
            this.sortOrder = new  List<int>( c.GetSortOrder());
        }

        public bool Contains(int element)
        {
            if (element < 0 || element >= c.GetSortOrder().Length)
                return false;

            IComparable o = c.GetComparable(element);

            if (o == null)
                return !maxOpen && omax == null;

            if (omax == null)
                return minOpen ? o.CompareTo(omin) > 0 : o.CompareTo(omin) >= 0;

            return true
                && (minOpen ? o.CompareTo(omin) > 0 : o.CompareTo(omin) >= 0)
                && (maxOpen ? o.CompareTo(omax) < 0 : o.CompareTo(omax) <= 0)
                ;
        }

        public IEnumerator<int> GetEnumerator()
        {   
            return (IEnumerator<int>)sortOrder.GetRange(begin, (end + 1-begin));
        }
    }
}
