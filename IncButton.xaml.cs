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
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace IncButton
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class IncButton : UserControl, INotifyPropertyChanged
    {
        private long _value = 100;
        private long _max = 10000;
        private long _min = 10;
        private long _inc = 10;
        private long _value_saved = 100;
        private int _way = 0;           // 1: Up    2: Down
        private TimeSpan _startTime;
        private DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Normal);

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public int Way 
        {
            get { return this._way; }
            set
            {
                this._way = value;
            }
        }

        public long Value
        {
            get { return this._value; }
            set
            {
                if (value > this._max) {
                    this._value = this._max;
                }
                else if (value < this._min)
                {
                    this._value = this._min;
                }
                else {
                    this._value = value;
                }
                NotifyPropertyChanged("Value");
            }
        }

        public IncButton()
        {
            InitializeComponent();

            this._timer.Interval = TimeSpan.FromMilliseconds(100);
            this._timer.Tick += new EventHandler(this.Timer);
            this._timer.Start();
        }

        public void config(/*MainWindow mainWindow,*/long min, long max, long inc, long current, int MainColumn_Width)
        {
            //this.mainWindow = mainWindow;
            this._min = min;
            this._max = max;
            this._inc = inc;
            this._value = current;

            this.IncButton_Grid.DataContext = this;

            this.MainColumn.Width = new GridLength((double)MainColumn_Width);
        }

        private void btnDown_PreviewContactDown(object sender, Microsoft.Surface.Presentation.ContactEventArgs e)
        {
            this.Way = 2;
            this._value_saved = this.Value;
            this._startTime = new TimeSpan(DateTime.Now.Ticks);
        }

        private void btnDown_PreviewContactUp(object sender, Microsoft.Surface.Presentation.ContactEventArgs e)
        {
            this.Way = 0;
            // gère le cas ou le "clic" durerait moins de 100 ms et de ce fait ca ne changerait pas la valeur
            if (this._value_saved == this.Value)
            {
                this.Value = this._value_saved - this._inc;
            }
        }

        private void btnUp_PreviewContactDown(object sender, Microsoft.Surface.Presentation.ContactEventArgs e)
        {
            this.Way = 1;
            this._value_saved = this.Value;
            this._startTime = new TimeSpan(DateTime.Now.Ticks);
        }

        private void btnUp_PreviewContactUp(object sender, Microsoft.Surface.Presentation.ContactEventArgs e)
        {
            this.Way = 0;
            // gère le cas ou le "clic" durerait moins de 100 ms et de ce fait ca ne changerait pas la valeur
            if (this._value_saved == this.Value)
            {
                this.Value = this._value_saved + this._inc;
            }
        }

        private void Timer(Object sender, EventArgs args)
        {
            double Ecart = (new TimeSpan(DateTime.Now.Ticks)).TotalMilliseconds - this._startTime.TotalMilliseconds;
            long value_to_add = this._inc;

            if (Ecart >= 10000)
            {
                value_to_add = this._inc * 500;
            }
            else if (Ecart >= 3000)
            {
                value_to_add = this._inc * 100;
            }
            else if (Ecart >= 1000)
            {
                value_to_add = this._inc * 10;
            }

            if (this._way == 1)
            {
                this.Value = this._value + value_to_add;
            }
            else if (this._way == 2)
            {
                this.Value = this._value - value_to_add;
            }

        }

        private void txtValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key[] allowed = new Key[] {
                Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, 
                Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                Key.Enter, Key.Return, Key.Tab
            };

            // bloque toutes les touches non numériques
            e.Handled = true;

            // init tests
            bool KeyOK = allowed.Contains(e.Key);
            bool CapsLock = Keyboard.IsKeyToggled(Key.CapsLock);
            bool Shift = Keyboard.Modifiers == ModifierKeys.Shift;

            // debloque si ok
            if (KeyOK && (CapsLock && !Shift || !CapsLock && Shift ))
            {
                e.Handled = false;
            }
        }


    }
}
