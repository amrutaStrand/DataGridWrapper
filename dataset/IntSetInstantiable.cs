using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    public class IntSetInstantiable : IntSet
    {
        private List<int> sortOrder;
        AbstractRegularColumn c;
        int categoryIndex=-21;
        int[] categoryRank;
        public IntSetInstantiable()
        {
            sortOrder = new List<int>();
        }

        public IntSetInstantiable(AbstractRegularColumn c )
        {
            this.c = c;
            sortOrder = new List<int>( c.GetSortOrder());
        }

        public IntSetInstantiable(AbstractRegularColumn c, int categoryIndex, int[] categoryRank)
        {
            this.c = c;
            sortOrder = new List<int>(c.GetSortOrder());
            this.categoryIndex = categoryIndex;
            this.categoryRank = categoryRank;
        }

        //public bool Contains(int element)
        //{
        //    return sortOrder.Contains(element);
        //}

        public IEnumerator<int> GetEnumerator()
        {
            return (IEnumerator<int>)sortOrder;
        }

       

        public bool Contains(int value)
        {
            if (value < 0 || value >= sortOrder.Count())
                return false;
            if (categoryIndex!=-21)
            {
                if (c.GetCategoryIndex(value) == categoryIndex)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return c.IsMissingValue(value);
        }

        public IEnumerator<int> Iterator()
        {
            if (categoryIndex!=-21)
            {
                return (IEnumerator<int>)sortOrder.GetRange(categoryRank[categoryIndex],categoryRank[categoryIndex+1]);
            }
            int mvCount = c.GetMissingValueCount();
            return (IEnumerator<int>)sortOrder.GetRange(c.GetSize() - mvCount, c.GetSize());
        }

       
    }
}
