using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public class MultisetColumn : ProxyColumn
    {
        public static string FORMAT_ID = "cube.dataset.MultisetColumn";
        public int[] /*IndexedBintArray*/ indices;

        /// <summary>
        /// for hesxff
        /// </summary>
        public MultisetColumn()
        {

        }

        //#framework
        public MultisetColumn(IColumn master, int[] indices) : base(master)
        {

            this.indices = indices;
        }

        //#framework

        override public int GetSize()
        {
            //return indices.GetSize();
            return 0;
        }

        //override public int GetMasterIndex(int index)
        //{
        //    return indices.getBinIndex(index);
        //}

        //override public int GetReverseIndex(int masterIndex)
        //{
        //    return indices.getBin(masterIndex).get(0);
        //}

        ///** converts masterRowIndices to rowIndices. */
        //override public IntSet Map(IntSet set)
        //{
        //    return BintArrayUtil.subset(indices, set);
        //}

        override public int GetCategorySize(int categoryIndex)
        {
            return master.GetCategorySize(categoryIndex);
            //throw new RuntimeException("NYI");
        }

        override public int GetCategoryCount()
        {
            return master.GetCategoryCount();
            //throw new RuntimeException("NYI");
        }
    }
}
