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
    /// Логика взаимодействия для ProviderEdit.xaml
    /// </summary>
    public partial class ProviderEdit : Window
    {
        public bool resultDialog;
        public ProviderEdit()
        {
            InitializeComponent();
            this.Title = "Добавление поставщика";
            resultDialog = false;
        }
        public ProviderEdit(string name)
        {
            InitializeComponent();
            this.Title = "Изменение поставщика";
            resultDialog = false;
            ProviderName.Text = name;
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
