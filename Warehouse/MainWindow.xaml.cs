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
using System.Collections.ObjectModel;

namespace Warehouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _connection = "Data Source=base.sqlite;Mode=ReadWrite;";
        private ObservableCollection<InfoClass> _InfoRows;
        private List<Good> _goodsList;
        private Dictionary<int, string> _goodsTypes;
        private Dictionary<int, string> _providers;
        private int _modeView;

        public MainWindow()
        {
            InitializeComponent();
            _InfoRows = new();
            _goodsList = new();
            _providers = new Dictionary<int, string>();
            _goodsTypes = new();
            InformationView.ItemsSource = _InfoRows;
            _goodsList = DbWork.getGood(_connection);
            _goodsTypes = DbWork.getGoodTypes(_connection);
            _providers = DbWork.getProviders(_connection);
            _modeView = 0;
            TypesAdd.IsEnabled = false;
            TypesEdit.IsEnabled = false;
            TypesRemove.IsEnabled = false;
            GoodsAdd.IsEnabled = true;
            GoodsEdit.IsEnabled = true;
            GoodsRemove.IsEnabled = true;
            ProviderAdd.IsEnabled = false;
            ProviderEdit.IsEnabled = false;
            ProviderRemove.IsEnabled = false;
            refreshView();
        }

        private void GoodsShow_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "GoodsShow")
            {
                _modeView = 0;
                TypesAdd.IsEnabled = false;
                TypesEdit.IsEnabled = false;
                TypesRemove.IsEnabled = false;
                GoodsAdd.IsEnabled = true;
                GoodsEdit.IsEnabled = true;
                GoodsRemove.IsEnabled = true;
                ProviderAdd.IsEnabled = false;
                ProviderEdit.IsEnabled = false;
                ProviderRemove.IsEnabled = false;
                refreshView();
            }
            if (((Button)sender).Name == "GoodsAdd")
            {
                GoodsEdit dialog = new GoodsEdit(_goodsTypes, _providers);
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                bool succes01 = Double.TryParse(dialog.GoodsPrice.Text, out double tempPrice);
                bool succes02 = Double.TryParse(dialog.GoodsQuantity.Text, out double tempQuantity);
                if(dialog.GoodsName.Text == "" || !succes01 || !succes02 || tempPrice == 0 || tempQuantity == 0)
                {
                    MessageBox.Show("Не заполнены все обязательные поля!", "Ошибка");
                    return;
                }
                int result = DbWork.AddGood(dialog.GoodsName.Text,  ((KeyValuePair<int, string>)(dialog.ListOfTypes.SelectedItem)).Key, ((KeyValuePair<int, string>)(dialog.ListOfProviders.SelectedItem)).Key, tempPrice, tempQuantity, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить тип {dialog.GoodsName.Text}", "Ошибка");
                    return;
                }
                Good newGood = new Good(result, ((KeyValuePair<int, string>)(dialog.ListOfProviders.SelectedItem)).Key, ((KeyValuePair<int, string>)(dialog.ListOfTypes.SelectedItem)).Key, dialog.GoodsName.Text, tempPrice, tempQuantity);
                _goodsList.Add(newGood);
                refreshView();
            }
            if (((Button)sender).Name == "GoodsEdit")
            {
                int goodId = ((InfoClass)InformationView.SelectedItem).Id;
                GoodsEdit dialog = new GoodsEdit(_goodsList.Find(x => x.ID == goodId), _goodsTypes, _providers);
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                bool succes01 = Double.TryParse(dialog.GoodsPrice.Text, out double tempPrice);
                bool succes02 = Double.TryParse(dialog.GoodsQuantity.Text, out double tempQuantity);
                if (dialog.GoodsName.Text == "" || !succes01 || !succes02 || tempPrice == 0 || tempQuantity == 0)
                {
                    MessageBox.Show("Не заполнены все обязательные поля!", "Ошибка");
                    return;
                }
                DbWork.updateGoods(goodId, dialog.GoodsName.Text, ((KeyValuePair<int, string>)(dialog.ListOfTypes.SelectedItem)).Key, ((KeyValuePair<int, string>)(dialog.ListOfProviders.SelectedItem)).Key, tempPrice, tempQuantity, _connection);
                _goodsList.Find(x => x.ID == goodId).UpdateGood(((KeyValuePair<int, string>)(dialog.ListOfProviders.SelectedItem)).Key, ((KeyValuePair<int, string>)(dialog.ListOfTypes.SelectedItem)).Key, dialog.GoodsName.Text, tempPrice, tempQuantity);
                refreshView();
                
            }
            if (((Button)sender).Name == "GoodsRemove")
            {
                DbWork.deleteGood((((InfoClass)InformationView.SelectedItem).Id), _connection);
                int removeId = (((InfoClass)InformationView.SelectedItem).Id);
                _goodsList.Remove(_goodsList.Where(x => x.ID == removeId).Single());
                refreshView();
            }
        }

        private void ProviderShow_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "ProviderShow")
            {
                _modeView = 2;
                TypesAdd.IsEnabled = false;
                TypesEdit.IsEnabled = false;
                TypesRemove.IsEnabled = false;
                GoodsAdd.IsEnabled = false;
                GoodsEdit.IsEnabled = false;
                GoodsRemove.IsEnabled = false;
                ProviderAdd.IsEnabled = true;
                ProviderEdit.IsEnabled = true;
                ProviderRemove.IsEnabled = true;
                refreshView();
            }
            if (((Button)sender).Name == "ProviderAdd")
            {
                ProviderEdit dialog = new ProviderEdit();
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                int result = DbWork.AddProvider(dialog.ProviderName.Text, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить тип {dialog.ProviderName.Text}", "Ошибка");
                    return;
                }
                InfoClass newType = new InfoClass(result, dialog.ProviderName.Text);
                _providers.Add(result, dialog.ProviderName.Text);
                refreshView();
            }
            if (((Button)sender).Name == "ProviderEdit")
            {
                ProviderEdit dialog = new ProviderEdit(_providers[(((InfoClass)InformationView.SelectedItem).Id)]);
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                DbWork.updateProviders((((InfoClass)InformationView.SelectedItem).Id), dialog.ProviderName.Text, _connection);
                _providers[(((InfoClass)InformationView.SelectedItem).Id)] = dialog.ProviderName.Text;
                refreshView();
            }
            if (((Button)sender).Name == "ProviderRemove")
            {
                DbWork.deleteProvider((((InfoClass)InformationView.SelectedItem).Id), _connection);
                _providers.Remove((((InfoClass)InformationView.SelectedItem).Id));
                refreshView();
            }
        }

        private void TypesShow_Click(object sender, RoutedEventArgs e)
        {
            if(((Button)sender).Name == "TypesShow")
            {
                _modeView = 1;
                TypesAdd.IsEnabled = true;
                TypesEdit.IsEnabled = true;
                TypesRemove.IsEnabled = true;
                GoodsAdd.IsEnabled = false;
                GoodsEdit.IsEnabled = false;
                GoodsRemove.IsEnabled = false;
                ProviderAdd.IsEnabled = false;
                ProviderEdit.IsEnabled = false;
                ProviderRemove.IsEnabled = false;
                refreshView();
            }
            if (((Button)sender).Name == "TypesAdd")
            {
                GoodsTypesEdit dialog = new GoodsTypesEdit();
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                int result = DbWork.AddType(dialog.GoodsTypeName.Text, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить тип {dialog.GoodsTypeName.Text}", "Ошибка");
                    return;
                }
                InfoClass newType = new InfoClass(result, dialog.GoodsTypeName.Text);
                _goodsTypes.Add(result, dialog.GoodsTypeName.Text);
                refreshView();
            }
            if (((Button)sender).Name == "TypesEdit")
            {
                GoodsTypesEdit dialog = new GoodsTypesEdit(_goodsTypes[(((InfoClass)InformationView.SelectedItem).Id)]);
                dialog.ShowDialog();
                if (dialog.resultDialog == false)
                    return;
                DbWork.updateTypes((((InfoClass)InformationView.SelectedItem).Id), dialog.GoodsTypeName.Text, _connection);
                _goodsTypes[(((InfoClass)InformationView.SelectedItem).Id)] = dialog.GoodsTypeName.Text;
                refreshView();
            }
            if (((Button)sender).Name == "TypesRemove")
            {
                DbWork.deleteTypes((((InfoClass)InformationView.SelectedItem).Id), _connection);
                _goodsTypes.Remove((((InfoClass)InformationView.SelectedItem).Id));
                refreshView();
            }
        }

        private void refreshView()
        {
            _InfoRows.Clear();
            if (_modeView == 0)
            {
                foreach (var tempGood in _goodsList)
                {
                    string tempGoodStr = "ID: " + tempGood.ID + ". " + tempGood.Name + " Тип: " + _goodsTypes[tempGood.TypeId] + " Поставщик: " + _providers[tempGood.ProviderId] + " Цена: " + tempGood.Price + " Количество " + tempGood.Quantity;
                    InfoClass goodsItem = new InfoClass(tempGood.ID, tempGoodStr);
                    _InfoRows.Add(goodsItem);
                }
                if (_InfoRows.Count == 0)
                {
                    GoodsEdit.IsEnabled = false;
                    GoodsRemove.IsEnabled = false;
                    if (_providers.Count == 0 || _goodsTypes.Count == 0)
                    {
                        GoodsAdd.IsEnabled = false;
                    }
                }
                else
                {
                    GoodsEdit.IsEnabled = true;
                    GoodsRemove.IsEnabled = true;
                    InformationView.SelectedIndex = 0;
                }
            }
            if (_modeView == 1)
            {
                foreach (var tempType in _goodsTypes)
                {
                    string tempTypeStr = "ID: " + tempType.Key + ". " + tempType.Value;
                    InfoClass goodsItem = new InfoClass(tempType.Key, tempTypeStr);
                    _InfoRows.Add(goodsItem);
                }
                if (_InfoRows.Count == 0)
                {
                    TypesEdit.IsEnabled = false;
                    TypesRemove.IsEnabled = false;
                }
                else
                {
                    TypesEdit.IsEnabled = true;
                    TypesRemove.IsEnabled = true;
                    InformationView.SelectedIndex = 0;
                }
            }
            if (_modeView == 2)
            {
                foreach (var tempProv in _providers)
                {
                    string tempProvStr = "ID: " + tempProv.Key + ". " + tempProv.Value;
                    InfoClass provItem = new InfoClass(tempProv.Key, tempProvStr);
                    _InfoRows.Add(provItem);
                }
                if (_InfoRows.Count == 0)
                {
                    ProviderEdit.IsEnabled = false;
                    ProviderRemove.IsEnabled = false;
                }
                else
                {
                    ProviderEdit.IsEnabled = true;
                    ProviderRemove.IsEnabled = true;
                    InformationView.SelectedIndex = 0;
                }
            }
        }
        
        private string findTypeName(int id)
        {
            int found = 0;
            foreach(var i in _goodsList)
            {
                if (i.ID == id)
                {
                    found = i.TypeId;
                    break;
                }
            }
            return _goodsTypes[found];
        }
        private string findProviderName(int id)
        {
            int found = 0;
            foreach (var i in _goodsList)
            {
                if (i.ID == id)
                {
                    found = i.ProviderId;
                    break;
                }
            }
            return _providers[found];
        }

        private void ShowProvReport_Click(object sender, RoutedEventArgs e)
        {
            if (_goodsList.Count == 0)
                return;
            Dictionary<int, double> tempData = new();
            foreach(var i in _providers)
            {
                tempData.Add(i.Key, 0);
            }
            foreach(var i in _goodsList)
            {
                tempData[i.ProviderId] += i.Quantity;
            }
            if (((Button)sender).Name == "ProviderMaxShow")
            {
                double max = 0;
                int found = 0;
                foreach(var i in tempData)
                {
                    if (i.Value > max)
                    {
                        found = i.Key;
                        max = i.Value;
                    }                   
                }
                MessageBox.Show($"Поставщик с максимальным количеством товара {_providers[found]}");
            }
            if (((Button)sender).Name == "ProviderMinShow")
            {
                double min = tempData.ElementAt(0).Value;
                int found = 0;
                foreach (var i in tempData)
                {
                    if (i.Value < min)
                    {
                        found = i.Key;
                        min = i.Value;
                    }
                }
                MessageBox.Show($"Поставщик с минимальным количеством товара {_providers[found]}");
            }
        }

        private void ShowTypesReport_Click(object sender, RoutedEventArgs e)
        {
            if (_goodsList.Count == 0)
                return;
            Dictionary<int, double> tempData = new();
            foreach (var i in _goodsTypes)
            {
                tempData.Add(i.Key, 0);
            }
            foreach (var i in _goodsList)
            {
                tempData[i.TypeId] += i.Quantity;
            }
            if (((Button)sender).Name == "TypesMaxShow")
            {
                double max = 0;
                int found = 0;
                foreach (var i in tempData)
                {
                    if (i.Value > max)
                    {
                        found = i.Key;
                        max = i.Value;
                    }
                }
                MessageBox.Show($"Поставщик с максимальным количеством товара {_goodsTypes[found]}");
            }
            if (((Button)sender).Name == "TypesMinShow")
            {
                double min = tempData.ElementAt(0).Value;
                int found = 0;
                foreach (var i in tempData)
                {
                    if (i.Value < min)
                    {
                        found = i.Key;
                        min = i.Value;
                    }
                }
                MessageBox.Show($"Поставщик с минимальным количеством товара {_goodsTypes[found]}");
            }
        }
    }
}
