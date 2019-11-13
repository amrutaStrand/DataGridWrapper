using com.strandgenomics.cube.dataset;

namespace dataset.Test.com.strandgenomics.cube.dataset
{
    public class TestUtils
    {
        public static bool IsDeepEqual(IColumn actual, IColumn expected)
        {
            if (actual.GetSize() != expected.GetSize())
                return false;

            for (int i = 0; i < actual.GetSize(); i++)
            {
                var at = actual.Get(i);
                var et = expected.Get(i);
                if (!at.ToString().Equals(et.ToString()))
                    return false;
            }
            return true;
        }
    }
}
