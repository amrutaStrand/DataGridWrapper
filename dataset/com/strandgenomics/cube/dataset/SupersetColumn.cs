using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public sealed class SupersetColumn: AbstractColumn
    {
        public static string FORMAT_ID = "cube.dataset.SupersetColumn";
        private int size;
        public IColumn orgColumn;
        //#framework
        private /*IndexedIntArray*/ int[] dataIndices;
        private bool stateNeedsUpdate;

        private float missingValue;
        private int remainingSize;

        private string namePrefix = "super";

        public SupersetColumn(IColumn c, int[] d, int size)
        {
            orgColumn = c;
            dataIndices = d;
            this.size = size;

            Init();
        }

        /// <summary>
        ///  invoked by hexff.
        /// </summary>
        private SupersetColumn()
        {
        }

        /// <summary>
        /// invoked by hexff. 
        /// </summary>
        private void Initialize()
        {
            Init();
        }


        private void Init()
        {
            this.missingValue = GetMissingValue();
            this.remainingSize = size - orgColumn.GetSize();

            stateNeedsUpdate = true;
        }
        internal string GetNamePrefix()
        {
            return namePrefix;
        }

        public void SetNamePrefix(string prefix)
        {
            this.namePrefix = prefix;
        }

        public override object Get(int index)
        {
            int dataIndex = GetDataIndex(index);
            if (dataIndex != -1)
                return orgColumn.Get(dataIndex);
            else
                return null;
        }

        public override int GetCategoryCount()
        {
            if (OrgHasMissingValues() || orgColumn.GetSize() == GetSize())
                return orgColumn.GetCategoryCount();
            else
                return orgColumn.GetCategoryCount() + 1;
        }


        private bool OrgHasMissingValues()
        {
            return orgColumn.GetMissingValueCount() > 0;
        }


        public override int GetCategoryIndex(int rowIndex)
        {
            if (!IsCategorical())
                InvalidMethodException();

            int dataIndex = GetDataIndex(rowIndex);
            if (dataIndex != -1)
                return orgColumn.GetCategoryIndex(dataIndex);
            else
                return GetCategoryCount() - 1;
        }

        public override int GetCategorySize(int categoryIndex)
        {
            if (!IsCategorical())
                InvalidMethodException();

            if ((GetMissingValueCount() > 0) && (categoryIndex == GetCategoryCount() - 1))
                return orgColumn.GetMissingValueCount() + remainingSize;
            else
                return orgColumn.GetCategorySize(categoryIndex);
        }

        public override object GetCategoryValue(int categoryIndex)
        {
            if (!IsCategorical())
                InvalidMethodException();

            if ((GetMissingValueCount() > 0) && (categoryIndex == GetCategoryCount() - 1))
                return null;
            else
                return orgColumn.GetCategoryValue(categoryIndex);
        }

        public override IComparable GetComparable(int index)
        {
            int dataIndex = GetDataIndex(index);
            if (dataIndex != -1)
                return orgColumn.GetComparable(dataIndex);
            else
                return null;
        }

        public override string GetDatatype()
        {
            return orgColumn.GetDatatype();
        }

        public override float GetFloat(int index)
        {
            string datatype = GetDatatype();
            if (datatype.Equals("integer") || !IsCategorical())
            {
                int dataIndex = GetDataIndex(index);
                if (dataIndex != -1)
                    return orgColumn.GetFloat(dataIndex);
                else
                    return missingValue;
            }
            else
                return GetCategoryIndex(index);
        }

        public override int GetInt(int index)
        {
            string datatype = GetDatatype();
            if (datatype.Equals("integer") || !IsCategorical())
            {
                int dataIndex = GetDataIndex(index);
                if (dataIndex != -1)
                    return orgColumn.GetInt(dataIndex);
                else
                    return (int)missingValue;
            }
            else
                return GetCategoryIndex(index);
        }



        public override void SetCategorical(bool b)
        {
            if (orgColumn.IsCategorical() == b)
                return;
            orgColumn.SetCategorical(b);
            stateNeedsUpdate = true;
            base.SetCategorical(b);
        }

        public override int GetMaxIndex()
        {
            return dataIndices[orgColumn.GetMaxIndex()];
        }

        public override ColumnMetaData GetMetaData()
        {
            return orgColumn.GetMetaData();
        }

        public override int GetMinIndex()
        {
            return dataIndices[orgColumn.GetMinIndex()];
        }

        public override int GetMissingValueCount()
        {
            return orgColumn.GetMissingValueCount() + remainingSize;
        }

        //#framework
        public override IntSet GetMissingValueIndices()
        {
            if (GetMissingValueCount() == 0)
                //return ArrayUtil.EMPTY_INT_SET;
                return null;
            else
                return null;
                //return ArrayUtil.append(
                //    redirect(orgColumn.getMissingValueIndices()),
                //    ArrayUtil.difference(ArrayUtil.range(0, size), dataIndices));
        }

        public override string GetName()
        {
            return namePrefix + "." + orgColumn.GetName();
        }

        public IColumn GetOriginalColumn()
        {
            return orgColumn;
        }
        private int GetDataIndex(int index)
        {
            return Array.IndexOf(dataIndices,index);
        }

        public override float GetNumericValue(int index)
        {
            string datatype = GetDatatype();
            if (datatype.Equals("integer") || !IsCategorical())
            {
                int dataIndex = GetDataIndex(index);
                if (dataIndex != -1)
                    return orgColumn.GetNumericValue(dataIndex);
                else
                    return missingValue;
            }
            else
            {
                return GetCategoryIndex(index);
            }
        }

        public override IntSet GetRowIndicesInRange(float min, float max)
        {
            return GetRowIndicesInRange(min, false, max, false);
        }


        //#framework
        public override IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            return null;
            //if (IsCategorical())
            //{
            //    int maxCategoryIndex = GetCategoryCount() - 1;
            //    if ((min > maxCategoryIndex) && (max > maxCategoryIndex))
            //        return ArrayUtil.EMPTY_INT_SET;

            //    if ((min >= maxCategoryIndex)
            //        && (GetMissingValueCount() > 0))
            //        return GetMissingValueIndices();

            //    if ((max >= maxCategoryIndex)
            //        && (GetMissingValueCount() > 0))
            //        return ArrayUtil.combine(
            //            redirect(orgColumn.GetRowIndicesInRange(min, minOpen, max, maxOpen)),
            //            ArrayUtil.range(0, size));
            //}

            //return redirect(orgColumn.GetRowIndicesInRange(min, minOpen, max, maxOpen));
        }

        //#framework
        public override IntSet GetRowIndicesInSortedOrder(bool ascending)
        {

            return null;
            //if (ascending)
            //    return ArrayUtil.combine(
            //    redirect(orgColumn.GetRowIndicesInSortedOrder(ascending)),
            //    ArrayUtil.range(0, size));
            //else
            //    return ArrayUtil.append(
            //    ArrayUtil.difference(
            //        ArrayUtil.reverse(ArrayUtil.range(0, size)), dataIndices),
            //    redirect(orgColumn.GetRowIndicesInSortedOrder(ascending)));
        }

        public override IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            if (!IsCategorical())
                InvalidMethodException();

            if ((GetMissingValueCount() > 0) && (categoryIndex == GetCategoryCount() - 1))
            {
                return GetMissingValueIndices();
            }
            else
            {
                return redirect(orgColumn.GetRowIndicesOfCategory(categoryIndex));
            }
        }

        private IntSet redirect(IntSet intSet)
        {
            //#framework
            //return ArrayUtil.subset(dataIndices, intSet);
            return null;
        }

        public override int GetSize()
        {
            return size;
        }

        public override float GetSum()
        {
            return orgColumn.GetSum();
        }

        public override bool IsCategorical()
        {
            return orgColumn.IsCategorical();
        }

        public override bool IsMissingValue(int index)
        {
            int dataIndex = GetDataIndex(index);
            if (dataIndex != -1)
                return orgColumn.IsMissingValue(dataIndex);
            else
                return true;
        }

        public override void SetMetaData(ColumnMetaData metaData)
        {
            orgColumn.SetMetaData(metaData);
        }

        public override void SetName0(string newName)
        {
            if (!newName.StartsWith(namePrefix))
                throw new DataException("Column name should start with " + namePrefix);

            String orgName = newName.Substring(namePrefix.Length, newName.Length);

            //TODO-Anand: handle namePrefix issues
            orgColumn.SetName(newName);
        }

       


        // method doesnt make sense for string column. but dont crib.
        private float GetMissingValue()
        {
            if (orgColumn.GetDatatype().Equals(ColumnFactory.DATATYPE_INT))
                return DatasetConstants.INTEGER_MV;
            else if (orgColumn.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT))
                return DatasetConstants.FLOAT_MV;
            else
                return (float)DatasetConstants.LONG_MV;
        }

        private void InvalidMethodException()
        {
            throw new DataException("Method invoked on the column is not compatible with its state. Categorical column = " + IsCategorical());
        }
    }
}
