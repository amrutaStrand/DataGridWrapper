using System.Collections.Generic;

namespace com.strandgenomics.cube.dataset
{
    public interface IMutableColumn : IColumn
    {

        /// <summary>
        /// Set a value in the column at index
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="index"></param>
        /// <param name="o"></param>
        void SetValue(int index, object o);

        /// <summary>
        /// Append a value in the column at the end.
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="o"></param>
        void AddValue(object o);


        /// <summary>
        /// Get the user data
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> GetUserData();


        /// <summary>
        ///  Set the user data
        /// </summary>
        /// <param name="userData"></param>
        void SetUserData(Dictionary<object, object> userData);

        // 
        /// <summary>
        /// Get the DataObject
        /// </summary>
        /// <exception cref="DataException"></exception>
        /// <param name="val"></param>
        /// <returns></returns>
        object GetDataObject(float val);

    }
}
