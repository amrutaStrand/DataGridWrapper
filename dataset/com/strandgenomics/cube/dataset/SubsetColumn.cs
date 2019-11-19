using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public class SubsetColumn: AbstractColumn
    {
        public const string FORMAT_ID = "cube.dataset.SubsetColumn";

        /// <summary>
        /// The original column associated with this subset column.
        /// </summary>
        private IColumn orgColumn;

        private int[] dataIndices;

        private int[] _categoryIndexArray;
        private bool stateNeedsUpdate;
        private float _sum;
        private int _mvCount;
        private int[] _categoryMap;

        /// <summary>
        /// maskPrefix is used to compute the column name from the name of 
        ///  original column.
        ///  <pre>
        ///  new_name = mask.startswith(name.prefix) ? name.suffix : name
        ///  e.g.
        ///  <i>Original_Name	mask	new_name</i>
        ///  a.b.name 	a.b	name
        ///  a.name		a	name
        ///  x.y.name	a.b	x.y.name
        ///  </pre>
        /// </summary>
        public string maskPrefix;

        public SubsetColumn(IColumn c, int[] i)
        {
            orgColumn = c;
            dataIndices = i;
            stateNeedsUpdate = true;
        }

        /// <summary>
        /// invoked by hexff. 
        /// </summary>
        private SubsetColumn()
        {
        }

        public void SetMaskPrefix(string prefix)
        {
            string oldName = GetName();
            this.maskPrefix = prefix;

            string newName = GetName();

            if (!oldName.Equals(newName))
                FireNameChanged(oldName, GetName());
        }

        private void FireNameChanged(string oldName, string newName)
        {
            ColumnChangedEventArgs args = new ColumnChangedEventArgs(ColumnChangedEventArgs.NAME_CHANGED);

            args.Column = this;

            args.OldName = oldName;

            args.NewName = newName;

            NotifyColumnChanged(args);
        }
        public override object Get(int index)
        {
            return orgColumn.Get(dataIndices[index]);
        }

        public override int GetCategoryCount()
        {
            int[] categoryMap = GetCategoryMap();
            return categoryMap.Length;
        }

        public override int GetCategoryIndex(int rowIndex)
        {
            int[] categoryIndexArray = GetCategoryIndexArray();
            return categoryIndexArray[rowIndex];
        }


        private int[] GetCategoryIndexArray()
        {
            if (!IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return _categoryIndexArray;
        }


        public override int GetCategorySize(int categoryIndex)
        {
            IntSet intSet = GetRowIndicesOfCategory(categoryIndex);
            return (intSet as List<int>).Count;
        }

        public override object GetCategoryValue(int categoryIndex)
        {
            int[] categoryMap = GetCategoryMap();
            return orgColumn.GetCategoryValue(categoryMap[categoryIndex]);
        }

        public override IComparable GetComparable(int index)
        {
            return orgColumn.GetComparable(GetDataIndex(index));
        }

        public override string GetDatatype()
        {
            return orgColumn.GetDatatype();
        }

        public override float GetFloat(int index)
        {
            return orgColumn.GetFloat(GetDataIndex(index));
        }

        public override int GetInt(int index)
        {
            return orgColumn.GetInt(GetDataIndex(index));
        }

        public override int GetMaxIndex()
        {
            if (stateNeedsUpdate)
                UpdateState();

            IEnumerator<int> iter = GetSortOrder(false).GetEnumerator();

            // skip all missing values
            for (int i = 0; i < _mvCount; i++)
                iter.MoveNext();

            return iter.MoveNext() ? iter.Current : 0;
        }

        public override ColumnMetaData GetMetaData()
        {
            return orgColumn.GetMetaData();
        }

        public override int GetMinIndex()
        {
            IEnumerator<int> iter = GetSortOrder().GetEnumerator();

            return iter.MoveNext() ? iter.Current : 0;
        }

        private IntSet GetSortOrder()
        {
            return GetSortOrder(true);
        }

        private IntSet GetSortOrder(bool isAscending)
        {
            IntSet orgSortOrder = orgColumn.GetRowIndicesInSortedOrder(isAscending);
            IntSet sortOrder = Map(orgSortOrder);
            return sortOrder;
        }
        public override int GetMissingValueCount()
        {
            if (stateNeedsUpdate)
                UpdateState();

            return _mvCount;
        }

        public override IntSet GetMissingValueIndices()
        {
            return Map(orgColumn.GetMissingValueIndices());
        }

        public override string GetName()
        {
            string name = orgColumn.GetName();

            //XXX Anand:
            // Too special case. any ideas to generalize??
            if (orgColumn is SupersetColumn) {
                SupersetColumn c = (SupersetColumn)orgColumn;
                string prefix = c.GetNamePrefix();
                    
                // mask prefix should be applied in the following 2 cases.
                //   1.when the maskPrefix is equals the namePrefix.
                //      e.g. mastercolumn name is Log.abc and the maskPrefix is Log
                //      condition -> maskPrefix == prefix
                //   2.when the maskPrefix starts with the prefix.
                //	    e.g. mastercolumn.name is Log.abc and the maskPrefix is Log.xx
                // 	    condition -> maskPrefix.startswith(prefix) 
                // 	    But this will also pass the condition when the node name is Log2, which it should not.
                //      to prevent that condition is changed like this...
                // 	    condition -> maskPrefix.startswith(prefix + '.')
                //      If somebody gives node name containing dot, there will be problems. 
                //	    but this should be prevented at the project level.
                if (maskPrefix != null && (maskPrefix.Equals(prefix) || maskPrefix.StartsWith(prefix + '.')))
                    return c.orgColumn.GetName();
            }

            return name;
        }


        public override float GetNumericValue(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);
            else
                return orgColumn.GetNumericValue(GetDataIndex(index));
        }

        private int GetDataIndex(int index)
        {
            return dataIndices[index];
        }

        new public void SetCategorical(bool b)
        {
            if (orgColumn.IsCategorical() == b)
                return;
            orgColumn.SetCategorical(b);
            stateNeedsUpdate = true;
            base.SetCategorical(b);
        }
        public override IntSet GetRowIndicesInRange(float min, float max)
        {
            return GetRowIndicesInRange(min, false, max, false);
        }
            
        public override bool IsCategorical()
        {
            return orgColumn.IsCategorical();
        }



        public override IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            if (min > max)
                return new RowIndicesIntSet();

            if (IsCategorical())
            {
                int[] categoryMap = GetCategoryMap();

                int maxCategoryIndex = categoryMap.Length - 1;
                if ((min > maxCategoryIndex) && (max > maxCategoryIndex))
                    return new RowIndicesIntSet();

                if ((((int)min) == maxCategoryIndex)
                    && (max > maxCategoryIndex)
                    && (GetMissingValueCount() > 0))
                    return GetMissingValueIndices();

                if (max > maxCategoryIndex)
                    max = maxCategoryIndex;
                float orgMin = (float)categoryMap[(int)(min + 0.5f)];
                float orgMax = (float)categoryMap[(int)max];
                return Map(orgColumn.GetRowIndicesInRange(orgMin, minOpen, orgMax, maxOpen));
            }
            else
            {
                return Map(orgColumn.GetRowIndicesInRange(min, minOpen, max, maxOpen));
            }
        }
        private int[] GetCategoryMap()
        {
            if (!IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return _categoryMap;
        }

        public override IntSet GetRowIndicesInSortedOrder(bool ascending)
        {
            return GetSortOrder(ascending);
        }

        public override IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            int[] categoryMap = GetCategoryMap();
            return Map(orgColumn.GetRowIndicesOfCategory(categoryMap[categoryIndex]));
        }

        public override int GetSize()
        {
            return dataIndices.Length;
        }

        public override float GetSum()
        {
            if (IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return _sum;
        }

        private void UpdateState()
        {
            if (IsCategorical())
                UpdateCategoricalState();
            else
                UpdateContinuousState();
            stateNeedsUpdate = false;
        }

        private void UpdateContinuousState()
        {
            // update sum.
            _sum = 0;
            _mvCount = 0;
            int size = dataIndices.Length;
            for (int i = 0; i < size; i++)
            {
                if (Get(i) == null)
                    _mvCount++;
                else
                    _sum += GetNumericValue(i);
            }
        }

        

        public IColumn GetParentColumn()
        {
            return orgColumn;
        }

        private void UpdateCategoricalState()
        {
            IntSet sortOrder = Map(orgColumn.GetRowIndicesInSortedOrder(true));
            int size = dataIndices.Length;

            _categoryIndexArray = new int[size];
            IEnumerator<int> iter = sortOrder.GetEnumerator();
            List<int> categoryMap = new List<int>();

            int categoryCount = 1;
            _mvCount = 0;
            iter.MoveNext();
            int rowIndex = iter.Current;
            object catValue = Get(rowIndex);
            _categoryIndexArray.SetValue(rowIndex, categoryCount - 1);
            int orgCatIndex = orgColumn.GetCategoryIndex(GetDataIndex(rowIndex));
            categoryMap.Add(orgCatIndex);

            if (catValue == null)
            {
                _mvCount++;
            }

            while (iter.MoveNext())
            {
                
                rowIndex = iter.Current;
                object o = Get(rowIndex);
                if (o == null)
                {
                    _mvCount++;
                }
                if (catValue != null)
                {
                    if (!catValue.Equals(o))
                    {
                        catValue = o;
                        categoryCount++;
                        orgCatIndex = orgColumn.GetCategoryIndex(GetDataIndex(rowIndex));
                        categoryMap.Add(orgCatIndex);
                    }
                }
                _categoryIndexArray.SetValue(rowIndex, categoryCount - 1);
            }

            _categoryMap = new int[categoryCount];
            for (int i = 0; i < categoryCount; i++)
                _categoryMap[i] = categoryMap[i];

        }
        private IntSet Map(IntSet intSet)
        {
            return Isubset(Intersect(intSet,dataIndices));
        }

        private IntSet Isubset( IntSet dataindices )
        {
            var xdataindices = dataindices as List<int>;
            var len = (xdataindices).Count;
            var x = new List<int>(xdataindices as List<int>);
            var res = new int[len];
            x.Sort();
            for(int i=0; i < len; i++)
            {
                var orgIndex = xdataindices.IndexOf(x[i]);
                res[orgIndex] = i;
            }
            return (IntSet)new List<int>(res);
        }

        private IntSet Intersect(IntSet intSet, int[] data)
        {
            List<int> temp = new List<int>();
            var iter = intSet.GetEnumerator();
            do
            {
                if(data.Contains(iter.Current))
                {
                    temp.Add(iter.Current);
                }
            }
            while (iter.MoveNext());
            return (IntSet)temp;
        }
        private void InvalidMethodException()
        {
            throw new DataException("Method invoked on the column is not compatible with its state. Categorical column = " + IsCategorical());
        }


        public override bool IsMissingValue(int index)
        {
            return orgColumn.IsMissingValue(dataIndices[index]);
        }

        public override void SetMetaData(ColumnMetaData metaData)
        {
            orgColumn.SetMetaData(metaData);
        }

        public override void SetName0(string name)
        {
            orgColumn.SetName(name);
        }
    }
}
