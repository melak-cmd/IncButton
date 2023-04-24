using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

namespace IncButton
{
    /// <summary>
    /// Interaction logic for SurfaceUserControl1.xaml
    /// </summary>
    public partial class IncButtonMinMax : SurfaceUserControl
    {

        public long ValueMin
        {
            get { return this.IncButtonMin.Value; }
        }

        public long ValueMax
        {
            get { return this.IncButtonMax.Value; }
        }

        public void config(/*MainWindow mainWindow,*/long min, long max, long inc, long current_min, long current_max, int MainColumn_Width)
        {
            //this.mainWindow = mainWindow;

            this.IncButtonMin.config(min, max, inc, current_min, MainColumn_Width);
            this.IncButtonMax.config(min, max, inc, current_max, MainColumn_Width);

        }

        public IncButtonMinMax()
        {
            InitializeComponent();
        }

        private void IncButtonMax_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IncButtonMax.Value < this.IncButtonMin.Value)
            {
                this.IncButtonMax.Value = this.IncButtonMin.Value;
            }
        }

        private void IncButtonMin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IncButtonMin.Value > this.IncButtonMax.Value)
            {
                this.IncButtonMin.Value = this.IncButtonMax.Value;
            }
        }
    }
}
