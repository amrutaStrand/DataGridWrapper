using com.strandgenomics.cube.dataset;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace TestApplication.ViewModels
{
    class LucidDataGridViewModel : INotifyPropertyChanged
    {
        IDataset dataset;

        public IDataset Dataset 
        { 
            get { return dataset; }
            set
            {
                dataset = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dataset"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadData()
        {
            string filepath = Utils.ReadFile();
            if (filepath == null)
                return;
            Dataset = Utils.CreateDataset(filepath);
        }

    }
}
