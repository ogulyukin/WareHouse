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
    /// Логика взаимодействия для GoodsTypesEdit.xaml
    /// </summary>
    public partial class GoodsTypesEdit : Window
    {
        public bool resultDialog;
        public GoodsTypesEdit()
        {
            InitializeComponent();
            resultDialog = false;
            this.Title = "Добавление типа";
        }
        public GoodsTypesEdit(String name)
        {
            InitializeComponent();
            resultDialog = false;
            GoodsTypeName.Text = name;

            this.Title = "Изменение типа";
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
