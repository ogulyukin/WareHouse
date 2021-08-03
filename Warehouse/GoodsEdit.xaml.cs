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

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для GoodsEdit.xaml
    /// </summary>
    public partial class GoodsEdit : Window
    {
        public bool resultDialog;

        public GoodsEdit(Dictionary<int, string> goodsTypes, Dictionary<int, string> providers)
        {
            InitializeComponent();
            resultDialog = false;
            ListOfTypes.ItemsSource = goodsTypes;
            ListOfTypes.SelectedIndex = 0;
            ListOfProviders.ItemsSource = providers;
            ListOfProviders.SelectedIndex = 0;
        }

        public GoodsEdit(Good good, Dictionary<int, string> goodsTypes, Dictionary<int, string> providers)
        {
            InitializeComponent();
            resultDialog = false;
            GoodsName.Text = good.Name;
            GoodsPrice.Text = good.Price.ToString();
            GoodsQuantity.Text = good.Quantity.ToString();
            ListOfTypes.ItemsSource = goodsTypes;
            ListOfTypes.SelectedIndex = 0;
            ListOfProviders.ItemsSource = providers;
            ListOfProviders.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "Ok")
            {
                resultDialog = true;
                this.Close();
            }
            else
            {
                resultDialog = false;
                this.Close();
            }
        }
    }
}
