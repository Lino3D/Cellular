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
using System.Windows.Shapes;

namespace Individual_Project
{
    /// <summary>
    /// Interaction logic for ChangeGridWindow.xaml
    /// </summary>
    public partial class ChangeGridWindow : Window
    {
        int A;
        int B;
       
        public ChangeGridWindow(int x, int y)
        {
            InitializeComponent();
            XValue.Text = x.ToString();
            YValue.Text = y.ToString();
            A = x;
            B = y;
    
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            int valueX;
            bool isNumeric = int.TryParse(XValue.Text, out valueX);
            if (isNumeric == true)
            {
                A = valueX;
            }
            else
            {
                MessageBox.Show("X must be an integer");

            }
            int valueY;
            bool isYNumeric = int.TryParse(XValue.Text, out valueY);
            if (isYNumeric == true)
            {
                B = valueY;
            }
            else
            {
                MessageBox.Show("X must be an integer");
            }
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
       
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
