using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// Basic column operations like column + column or column + value etc.
    /// <br>
    /// NOTE: THIS CODE IS GENERATED FROM BasicColumnOperations.py. *** DO NOT EDIT ***
    /// </summary>
    public class BasicColumnOperations: ColumnOperationUtil
    {
        public static IColumn Add(IColumn a, IColumn b, string name)
        {
            if (name == null)
                name = a.GetName() + " + " + b.GetName();

            if (!IsNumericColumn(a) || !IsNumericColumn(b))
                throw new Exception("operation can be performed only on int and float columns.");

            if (DatasetUtil.IsFloatColumn(a) || DatasetUtil.IsFloatColumn(b))
            {
                float[] data = Add_float(a, b, name);
                return CreateFloatColumn(name, data, a, b);
            }

            if (DatasetUtil.IsIntegerColumn(a) && DatasetUtil.IsIntegerColumn(b))
            {
                int[] data = Add_int(a, b, name);
                return CreateIntColumn(name, data, a, b);
            }
            else
                throw new Exception("huh?");
        }

        public static IColumn Add(IColumn a, object b, string name)
        {
            if (name == null)
                name = a.GetName() + " + " + b.ToString();

            if (!IsNumericColumn(a))
                throw new Exception(
                "+ operation can be performed only on int and float columns.");

            if (!((b is int)|| (b is float) || (b is double)))
                throw new Exception(
                "+ operation can be performed only with numbers.");
            
            if (DatasetUtil.IsFloatColumn(a) && (b is float || b is double))
            {
                float[] data = Add_float(a, ToFloat(b), name);
                return CreateFloatColumn(name, data, a);
            }

            if (DatasetUtil.IsIntegerColumn(a) && b is int)
            {
                int[] data = Add_int(a, ToInt(b), name);
                return CreateIntColumn(name, data, a);
            }

            else
                throw new Exception("huh?");
        }

        private static float[] Add_float(IColumn a, IColumn b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b.GetFloat(i);

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x + y;
            }

            return data;
        }

        private static float[] Add_float(IColumn a, float b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b;

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x + y;
            }

            return data;
        }

        private static int[] Add_int(IColumn a, IColumn b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b.GetInt(i);

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x + y;
            }

            return data;
        }

        private static int[] Add_int(IColumn a, int b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b;

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x + y;
            }

            return data;
        }

        public static IColumn Sub(IColumn a, IColumn b, string name)
        {
            if (name == null)
                name = a.GetName() + " - " + b.GetName();

            if (!IsNumericColumn(a) || !IsNumericColumn(b))
                throw new Exception(
                "- operation can be performed only on int and float columns.");

            if (DatasetUtil.IsFloatColumn(a) || DatasetUtil.IsFloatColumn(b))
            {
                float[] data = Sub_float(a, b, name);
                return CreateFloatColumn(name, data, a, b);
            }

            if (DatasetUtil.IsIntegerColumn(a) && DatasetUtil.IsIntegerColumn(b))
            {
                int[] data = Sub_int(a, b, name);
                return CreateIntColumn(name, data, a, b);
            }
            else
                throw new Exception("huh?");
        }

        public static IColumn Sub(IColumn a, object b, string name)
        {
            if (name == null)
                name = a.GetName() + " - " + b.ToString();

            if (!IsNumericColumn(a))
                throw new Exception(
                "- operation can be performed only on int and float columns.");

            if (!((b is int) || (b is float) || (b is double)))
                throw new Exception(
                "- operation can be performed only with numbers.");

            if (DatasetUtil.IsFloatColumn(a) && (b is float || b is double))
            {
                float[] data = Sub_float(a, ToFloat(b), name);
                return CreateFloatColumn(name, data, a);
            }

            if (DatasetUtil.IsIntegerColumn(a) && b is int)
            {
                int[] data = Sub_int(a, ToInt(b), name);
                return CreateIntColumn(name, data, a);
            }

            else
                throw new Exception("huh?");
        }

        private static float[] Sub_float(IColumn a, IColumn b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b.GetFloat(i);

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x - y;
            }

            return data;
        }
        private static float[] Sub_float(IColumn a, float b, String name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b;

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x - y;
            }

            return data;
        }


        private static int[] Sub_int(IColumn a, IColumn b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b.GetInt(i);

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x - y;
            }

            return data;
        }

        private static int[] Sub_int(IColumn a, int b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b;

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x - y;
            }

            return data;
        }

        public static IColumn Mul(IColumn a, IColumn b, string name)
        {
            if (name == null)
                name = a.GetName() + " * " + b.GetName();

            if (!IsNumericColumn(a) || !IsNumericColumn(b))
                throw new Exception(
                "* operation can be performed only on int and float columns.");

            if (DatasetUtil.IsFloatColumn(a) || DatasetUtil.IsFloatColumn(b))
            {
                float[] data = Mul_float(a, b, name);
                return CreateFloatColumn(name, data, a, b);
            }

            if (DatasetUtil.IsIntegerColumn(a) && DatasetUtil.IsIntegerColumn(b))
            {
                int[] data = Mul_int(a, b, name);
                return CreateIntColumn(name, data, a, b);
            }
            else
                throw new Exception("huh?");
        }


        public static IColumn Mul(IColumn a, object b, string name)
        {
            if (name == null)
                name = a.GetName() + " * " + b.ToString();

            if (!IsNumericColumn(a))
                throw new Exception(
                "* operation can be performed only on int and float columns.");

            if (!((b is int) || (b is float) || (b is double)))
                throw new Exception(
                "* operation can be performed only with numbers.");

            if (DatasetUtil.IsFloatColumn(a) && (b is float || b is double))
            {
                float[] data = Mul_float(a, ToFloat(b), name);
                return CreateFloatColumn(name, data, a);
            }

            if (DatasetUtil.IsIntegerColumn(a) && b is int)
            {
                int[] data = Mul_int(a, ToInt(b), name);
                return CreateIntColumn(name, data, a);
            }

            else
                throw new Exception("huh?");
        }

        private static float[] Mul_float(IColumn a, IColumn b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b.GetFloat(i);

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x * y;
            }

            return data;
        }

        private static float[] Mul_float(IColumn a, float b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b;

                if (x == FLOAT_MV || y == FLOAT_MV)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x * y;
            }

            return data;
        }


        private static int[] Mul_int(IColumn a, IColumn b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b.GetInt(i);

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x * y;
            }

            return data;
        }

        private static int[] Mul_int(IColumn a, int b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b;

                if (x == INT_MV || y == INT_MV)
                    data[i] = INT_MV;
                else
                    data[i] = x * y;
            }

            return data;
        }


        public static IColumn Div(IColumn a, IColumn b, string name)
        {
            if (name == null)
                name = a.GetName() + " / " + b.GetName();

            if (!IsNumericColumn(a) || !IsNumericColumn(b))
                throw new Exception(
                "/ operation can be performed only on int and float columns.");

            if (DatasetUtil.IsFloatColumn(a) || DatasetUtil.IsFloatColumn(b))
            {
                float[] data = Div_float(a, b, name);
                return CreateFloatColumn(name, data, a, b);
            }

            if (DatasetUtil.IsIntegerColumn(a) && DatasetUtil.IsIntegerColumn(b))
            {
                int[] data = Div_int(a, b, name);
                return CreateIntColumn(name, data, a, b);
            }
            else
                throw new Exception("huh?");
        }

        public static IColumn Div(IColumn a, object b, string name)
        {
            if (name == null)
                name = a.GetName() + " / " + b.ToString();

            if (!IsNumericColumn(a))
                throw new Exception(
                "/ operation can be performed only on int and float columns.");

            if (!((b is int) || (b is float) || (b is double)))
                throw new Exception(
                "/ operation can be performed only with numbers.");

            if (DatasetUtil.IsFloatColumn(a) && (b is float || b is double))
            {
                float[] data = Div_float(a, ToFloat(b), name);
                return CreateFloatColumn(name, data, a);
            }

            if (DatasetUtil.IsIntegerColumn(a) && b is int)
            {
                int[] data = Div_int(a, ToInt(b), name);
                return CreateIntColumn(name, data, a);
            }

            else
                throw new Exception("huh?");
        }

        private static float[] Div_float(IColumn a, IColumn b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b.GetFloat(i);

                if (x == FLOAT_MV || y == FLOAT_MV || y == 0)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x / y;
            }

            return data;
        }


        private static float[] Div_float(IColumn a, float b, string name)
        {

            float[] data = new float[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                float x = a.GetFloat(i);
                float y = b;

                if (x == FLOAT_MV || y == FLOAT_MV || y == 0)
                    data[i] = FLOAT_MV;
                else
                    data[i] = x / y;
            }

            return data;
        }

        private static int[] Div_int(IColumn a, IColumn b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b.GetInt(i);

                if (x == INT_MV || y == INT_MV || y == 0)
                    data[i] = INT_MV;
                else
                    data[i] = x / y;
            }

            return data;
        }

        private static int[] Div_int(IColumn a, int b, string name)
        {

            int[] data = new int[a.GetSize()];

            for (int i = 0; i < data.Length; i++)
            {
                int x = a.GetInt(i);
                int y = b;

                if (x == INT_MV || y == INT_MV || y == 0)
                    data[i] = INT_MV;
                else
                    data[i] = x / y;
            }

            return data;
        }



    }
}
