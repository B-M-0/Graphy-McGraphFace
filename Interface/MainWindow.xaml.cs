using System.Windows;
using System.Windows.Controls;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rect.Visibility == System.Windows.Visibility.Collapsed)
            {
                rect.Visibility = System.Windows.Visibility.Visible;
                Input0.Visibility = System.Windows.Visibility.Visible;
                
                


                (sender as Button).Content = "<";

            }
            else
            {
                rect.Visibility = System.Windows.Visibility.Collapsed;
                Input0.Visibility = System.Windows.Visibility.Collapsed;
                Input1.Visibility = System.Windows.Visibility.Collapsed;
                Input2.Visibility = System.Windows.Visibility.Collapsed;
                Input3.Visibility = System.Windows.Visibility.Collapsed;
                Input4.Visibility = System.Windows.Visibility.Collapsed;
                (sender as Button).Content = ">";
            }
        }

        private void InputOrdering(object sender, TextChangedEventArgs e)
        {
            //foreach
        }
        private void TextBox0_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Input0.Text))
            {
                Input1.Visibility = System.Windows.Visibility.Visible;
            }
            else 
            {
                Input1.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Input1.Text))
            {
                Input2.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Input2.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Input2.Text))
            {
                Input3.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Input3.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
       
        
    }
}
