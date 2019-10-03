using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;
namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// ObjectColumn class holds data of a string column. A string column
    /// will always be categorical, and cannot be made continuous. The column
    /// uses an optimised internal representation of the values in the column.
    /// </summary>
    sealed public class ObjectColumn : AbstractRegularColumn
    {
        public static string FORMAT_ID = "cube.dataset.ObjectColumn";
    
        public static  string DATATYPE = "object";


        private object[] data;
        /// <summary>
        /// Hack .. to be removed after meta-data is enhanced .. 
        /// </summary>
        private Dictionary<object,object> moreMetaData;

        /// <summary>
        /// calls ObjectColumn with size = data.length
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public ObjectColumn(string name, object[] data) : this(name, data, data.Length)
        {
            
        }

        /// <summary>
        /// Constructs a new <code>ObjectColumn</code> with the specified name
        /// holding the specified data. The data is an Object array.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public ObjectColumn(string name, object[] data, int size): base(name, size)
        {
            

            if (size > data.Length)
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");

            this.data = data;
            SetCategorical(true);
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private ObjectColumn()
        {
            SetCategorical(true);
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private void Initialize()
        {
            base.Init(name, data.Length);

            // XXX: categoryIndexArray is computed unnecesarily.
            base.UpdateState();
        }


        /// <summary>
        /// Throws <code>DataException</code> when invoked with value false.
        /// A <code>ObjectColumn</code> cannot be made continuous, and is by
        /// default always categorical.
        /// </summary>
        /// <param name="b">true to set the column as categorical.</param>
        override public void SetCategorical(bool b)
        {
            // Object column does not support continuous type.
            if (!b)
            {
                InvalidMethodException();
            }
            base.SetCategorical(b);
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
        /// Returns the comparable object for the specified index. It returns
        /// the same as <code>get(int index)</code>, when the state of the 
        /// column has been updated.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        /// <summary>
        /// Returns the datatype of the column. For a <code>ObjectColumn</code>
        /// it returns the String "string".
        /// </summary>
        /// <returns></returns>
        public override string GetDatatype()
        {
            return DATATYPE;
        }

        /// <summary>
        /// Throws <code>DataException</code> when invoked. A string column
        /// cannot return a float value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetFloat(int index)
        {
            return DatasetConstants.FLOAT_MV;
        }

        /// <summary>
        /// Throws <code>DataException</code> when invoked. A ObjectColumn 
        /// cannot return an integer value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetInt(int index)
        {
            return DatasetConstants.INTEGER_MV;
        }

        /// <summary>
        /// Returns the category index for the value at the specified index.    
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetNumericValue(int index)
        {
            return GetCategoryIndex(index);
        }

        /// <summary>
        /// This is a hack . will be removed after meta data is enhanced to keep an arbitrary map
        /// </summary>
        /// <returns></returns>
        public Dictionary<object,object> GetMoreMetaData()
        {
            return moreMetaData;
        }

        
        public void SetMoreMetaData(Dictionary<object,object> data)
        {
            moreMetaData = data;
        }
        //#framework
        int[] objectSortOrder = null;

        new private int[] GetSortOrder()
        {
            if (stateNeedsUpdate || objectSortOrder == null)
            {
                UpdateState();
                objectSortOrder = (int[])sortOrder;
            }
            return objectSortOrder;
        }
    }
}
