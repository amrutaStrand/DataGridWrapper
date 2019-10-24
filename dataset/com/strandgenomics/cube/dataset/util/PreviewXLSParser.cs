using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace com.strandgenomics.cube.dataset.util
{
    public class PreviewXLSParser : XLSParser
    {
        private DataFormatter formater;

        public PreviewXLSParser(FileStream file) : base(file)
        {
            this.formater = new DataFormatter(new CultureInfo("en-US"));
        }

        protected string GetContents(ICell c)
        {
            if (c != null && (c.CellType != CellType.Blank))
            {
                string s = null;
                if (c.CellType == CellType.String)
                    s = c.StringCellValue;

                else if (formater != null)
                    s = formater.FormatCellValue(c);

                if (s == null)
                    s = c.ToString();
                if (s.Trim().Equals(""))
                    return null;
                if (Regex.IsMatch(s, "\\s"))
                    return null;
                return s;
            }
            else
                return "";

        }

        override public void OpenReader()
        {
            base.OpenReader();
	    }

}
}
