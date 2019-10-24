using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset.util
{
    /// <summary>
    /// The TypeParser class provides utility methods for parsing values 
    /// of different datatypes from the string representation.
    /// <para>
    /// The datatypes supported include integer, float, date and String.
    /// All parsing routines should make use of these methods while 
    /// determining datatypes of the parsed String.
    /// </para>
    /// Caution - The code casts the return value from parse to Long and 
    /// Double in order to determine if the parsed Number is really a Long
    /// in the case of parsing integers. Otherwise, we might get a Double,
    /// and continue working with its intValue().
    /// </summary>
    public sealed class TypeParser
    {
        private static CultureInfo[] culture = { CultureInfo.DefaultThreadCurrentCulture, CultureInfo.CreateSpecificCulture("en-US")};

        /// <summary>
        /// This method returns an integer value parsed from the string value passed
        /// as argument, or throws DataException if it is unable to parse a
        /// valid integer.
        /// TODO
        /// The first try made is to try normal parsing using Integer class. If that
        /// works, the integer value is returned.This however doesnt handle numbers
        /// represented as 1,234 for example.To parse numbers of these kinds, a set
        /// of locales are used to parse the string value.Apart from the default
        /// locale, the following are supported (The format on the right shows how
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ParseInt(string value)
        {
            
            foreach (CultureInfo ci in culture)
            {
                try
                {
                    int num = int.Parse(value, ci);
                    
                    return num;
                }
                catch(ArgumentNullException)
                {
                    
                    return DatasetConstants.INTEGER_MV;
                }
                catch(FormatException)
                {
                    continue;
                }
            }
            throw new DataException("Unable to parse value \"" + value + "\" as an integer.");

        }

        //removed since now parse int does not require this supporting method.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="culture"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private static int ParseIntegerInCultureInfo( CultureInfo culture, string value)
        //{
        //    try
        //    {
        //        long num = long.Parse(value, culture);
        //        if (num > int.MaxValue || num < int.MinValue)
        //            throw new DataException("Integer Overflow");
        //        return (int)num;
        //    }
        //    catch(ArgumentNullException)
        //    {
        //        return DatasetConstants.INTEGER_MV;
        //    }
        //    catch (FormatException)
        //    {

        //        throw;
        //    }

        //}

        public static float ParseFloat(string value)
        {

            foreach (CultureInfo ci in culture)
            {
                try
                {
                    float num = float.Parse(value, ci);

                    return num;
                }
                catch (ArgumentNullException)
                {

                    return DatasetConstants.FLOAT_MV;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            throw new DataException("Unable to parse value \"" + value + "\" as an float.");

        }


        public static double ParseDouble(string value)
        {

            foreach (CultureInfo ci in culture)
            {
                try
                {
                    double num = double.Parse(value, ci);

                    return num;
                }
                catch (ArgumentNullException)
                {

                    return double.MaxValue;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            throw new DataException("Unable to parse value \"" + value + "\" as an double.");

        }

        public static decimal ParseDecimal(string value)
        {

            foreach (CultureInfo ci in culture)
            {
                try
                {
                    decimal num = decimal.Parse(value, ci);

                    return num;
                }
                catch (ArgumentNullException)
                {

                    return decimal.MaxValue;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            throw new DataException("Unable to parse value \"" + value + "\" as an decimal.");

        }

        public static long ParseDate(string value)
        {

            foreach (CultureInfo ci in culture)
            {
                try
                {
                    long num = DateTime.Parse(value, ci).Ticks;

                    return num;
                }
                catch (ArgumentNullException)
                {

                    return DatasetConstants.LONG_MV;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            throw new DataException("Unable to parse value \"" + value + "\" as an Date.");

        }


    }
}
