using com.strandgenomics.cube.dataset;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIControls
{
    public class LucidDataGrid : XamDataGrid
    {
        IDataset dataset;
        public IDataset Dataset 
        { 
            get { return dataset; }
            set
            {
                dataset = value;
                UpdateDataSource(dataset);
            }
        }

        private void UpdateDataSource(IDataset dataset)
        {
            if (dataset == null)
                return;

            int rowCount = dataset.GetRowCount();
            for (int i = 0; i < rowCount; i++)
                this.DataItems.Add(dataset.GetRow(i));
        }
    }
}
