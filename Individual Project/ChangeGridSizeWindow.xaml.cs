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
    /// Interaction logic for ChangeGridSizeWindow.xaml
    /// </summary>
    public partial class ChangeGridSizeWindow : Window
    {
        int X;
        int Y;

        public ChangeGridSizeWindow(int a, int b)
        {
            InitializeComponent();
            NewXBox.Text = a.ToString();
            NewYBox.Text = b.ToString();
            X = a;
            Y = b;
        //    a = X;
       //     b = Y;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            int value;

            bool isNumeric = int.TryParse(NewXBox.Text, out value);
            if (isNumeric == true)
            {
                if(value<=60)
                {
                    X = value;
                    isNumeric = int.TryParse(NewYBox.Text, out value);
                    if (isNumeric == true)
                    {
                        if (value <= 60)
                        {
                            Y = value;
                            this.Close();
                            Check.Text = "Ok";

                        }

                        else
                        {
                            MessageBox.Show("Y must be an integer lesser than 60");
                        }

                    }
                    else
                        MessageBox.Show("Y must be an integer");
                }
                       
                else
                {
                    MessageBox.Show("X must be an integer lesser than 60");
                }
        
            }
            else
                MessageBox.Show("X must be an integer");

    


      
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NewXBox.Text = X.ToString();
            NewYBox.Text = Y.ToString();
            Check.Text = "False";
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            NewXBox.Text = X.ToString();
            NewYBox.Text = Y.ToString();
           Check.Text = "False";
        }


    }
}
