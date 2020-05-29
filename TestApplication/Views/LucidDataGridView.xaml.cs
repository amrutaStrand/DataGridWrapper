using com.strandgenomics.cube.dataset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestApplication.ViewModels;
using UIControls;

namespace TestApplication.Views
{
    /// <summary>
    /// Interaction logic for TestLucidDataGrid.xaml
    /// </summary>
    public partial class LucidDataGridView : UserControl
    {
        LucidDataGrid dataGrid;
        LucidDataGridViewModel Model;
        public LucidDataGridView()
        {
            InitializeComponent();
            Model = new LucidDataGridViewModel();
            AddLucidDataGrid();          
        }

        private void AddLucidDataGrid()
        {
            dataGrid = new LucidDataGrid();
            dataGrid.Dataset = Model.Dataset;
            dataGrid.UpdateLayout();
            DataGridHost.Children.Clear();
            this.DataGridHost.Children.Add(dataGrid);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.LoadData();
            AddLucidDataGrid();
        }
    }
}
