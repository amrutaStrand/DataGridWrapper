using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public class MutableColumn : AbstractRegularColumn, IMutableColumn
    {
        //still needs reviewing.
        protected ArrayList data;
        protected string categorytype;
        protected Dictionary<object,object> userData;
        protected string _datatype = ColumnFactory.DATATYPE_INT;
        public static string FORMAT_ID = "cube.dataset.MutableColumn";

        public MutableColumn(IColumn column): this(column.GetName(), column)
        {
        }

        public MutableColumn(string name, IColumn column): this(name, column.GetDatatype(), DatasetUtil.GetColumnDataAsList(column))
        {
           

            if (_datatype.Equals(ColumnFactory.DATATYPE_INT) && column.IsCategorical())
                SetCategorical(true);
        }

        public MutableColumn(string name): this(name, ColumnFactory.DATATYPE_INT)
        {
            
        }

        public MutableColumn(string name, string datatype):this(name, datatype,  new ArrayList())
        {
            
        }

        public MutableColumn(string name, ArrayList data): this(name, GetDatatypeForList(ColumnFactory.DATATYPE_INT, data), data)
        {
            
        }

        public MutableColumn(string name, string datatype, ArrayList list):base(name, list.Count)
        {
            
            _datatype = datatype;
            data = list;
            CheckAndSetAttributeType();
            if (size != 0)
                stateNeedsUpdate = true;
        }

        

        /** Invoked by ReflectionSerializer. */
        protected MutableColumn()
        {
        }


        protected void Initialize()
        {
            base.Init();
            if (_datatype == null)
                _datatype = ColumnFactory.DATATYPE_OBJECT;
            //checkAndSetAttributeType();
            stateNeedsUpdate = true;
            size = data.Count;
        }


        // TO BE MOVED TO DATASETUTIL
        // chks the datatype of each element in the list against - ColumnFactory.DATATYPE_(OBJECT, STRING, INT, FLOAT, DATE)
        private static string GetDatatypeForList(string datatype, ArrayList list)
        {

            String type = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(null))
                {
                    type = GetNewDatatypeForColumn(datatype, list[i]);
                    if (type.Equals(ColumnFactory.DATATYPE_OBJECT))
                        break;
                }
            }

            if (type == null)
                type = ColumnFactory.DATATYPE_INT;
            return type;
        }

        private static string GetNewDatatypeForColumn(string datatype, object v)
        {
            if (datatype.Equals(ColumnFactory.DATATYPE_OBJECT))
                return ColumnFactory.DATATYPE_OBJECT;
            else
            {
                if (v is int)
                    return datatype;


                else if (v is float)
                {
                    if (datatype.Equals(ColumnFactory.DATATYPE_FLOAT) || datatype.Equals(ColumnFactory.DATATYPE_INT))
                        return ColumnFactory.DATATYPE_FLOAT;

                    return ColumnFactory.DATATYPE_STRING;
                }

                //
                //else if (v is DateTime) {
                //            if (datatype.Equals(ColumnFactory.DATATYPE_DATE))
                //                return ColumnFactory.DATATYPE_DATE;
                //            return ColumnFactory.DATATYPE_STRING;
                //        }


                else if (v is string)
                    return ColumnFactory.DATATYPE_STRING;

                return ColumnFactory.DATATYPE_OBJECT;
            }
        }

        private void CheckAndSetAttributeType()
        {
            if (_datatype.Equals(ColumnFactory.DATATYPE_OBJECT) ||
                _datatype.Equals(ColumnFactory.DATATYPE_STRING) ||
          //_datatype.equalsIgnoreCase(ColumnFactory.DATATYPE_DATE) ||
          (_datatype.Equals(ColumnFactory.DATATYPE_INT) && IsCategorical()))
                SetCategorical(true);
            else
                SetCategorical(false);
        }

        //private int ObjectToInt(object input)
        //{
        //    return (int)input;
        //}


        /// <summary>
        /// Throws <code>DataException</code> when invoked with value false and datatype is float.
        /// </summary>
        /// <param name="b">true to set the column as categorical.</param>
        override public void SetCategorical(bool b)
        {

            if (!b && (_datatype.Equals(ColumnFactory.DATATYPE_STRING)))
            {
                InvalidMethodException();
            }
            if (b && (_datatype.Equals(ColumnFactory.DATATYPE_FLOAT)))
            {
                InvalidMethodException();
            }
            base.SetCategorical(b);
        }


        /// <summary>
        /// Append a value in the column at the end.
        /// </summary>
        /// <param name="o"></param>
        public void AddValue(object o)
        {
            data.Add(o);
            size++;
            categoryIndexArray = null;// needed since size of the categoryIndexArray will need change.
            stateNeedsUpdate = true;

            if (o != null)
            {
                _datatype = GetNewDatatypeForColumn(_datatype, o);
                CheckAndSetAttributeType();
            }
        }

        /// <summary>
        /// Returns the string value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override object Get(int index)
        {
            return data[index];
        }

        /// <summary>
        ///  Returns the comparable object for the specified index. It returns
        ///  the same as <code>get(int index)</code>, when the state of the 
        ///  column has been updated.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        public object GetDataObject(float val)
        {
            float floatObj = float.Parse(val.ToString());
            if (IsCategorical())
                return floatObj.ToString();
            else
                return floatObj;
        }

        /// <summary>
        /// Returns the columntype of the column. For a <code>ObjectColumn</code>.
        /// it returns the String "string".
        /// </summary>
        /// <returns></returns>
        public override string GetDatatype()
        {
            return _datatype;
        }

        public override float GetFloat(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);
            else if (data[index] == null)
                return DatasetConstants.FLOAT_MV;
            else
                return float.Parse(data[index].ToString());
        }

        public override int GetInt(int index)
        {
            if (IsCategorical())
                return  GetCategoryIndex(index);
            else if (data[index] == null)
                return DatasetConstants.INTEGER_MV;
            else
                return int.Parse(data[index].ToString());
        }

        /// <summary>
        /// Returns the category index for the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetNumericValue(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);
            else
                return GetFloat(index);
        }

        /// <summary>
        /// To store a dict of user data
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, object> GetUserData()
        {
            return userData;
        }

        public void SetUserData(Dictionary<object, object> userData)
        {
            this.userData = userData;
        }

        /// <summary>
        /// Set a value in the column at index
        /// NYI - not yet implmented
        /// Needs a careful look on how the datatype will change during a set call.
        /// This change is not same as in add call.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="o"></param>
        public void SetValue(int index, object o)
        {
            throw new NotImplementedException();
        }

        //#framework

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static List<object> GetSubsetList(/*IntArray*/int[] rowIndices, IColumn col)
        {
            List<object> subList = new List<object>();
            for (int i = 0; i < rowIndices.Length; i++)
                subList.Add(col.Get(rowIndices[i]));
            return subList;
        }
    }
    
}
