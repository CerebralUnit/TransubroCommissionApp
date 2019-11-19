using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Transubro.CMS.API;
using Transubro.CMS.Model;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class ClientList : InjectableUserControl, INotifyPropertyChanged
    {
        private ICollectionView _dataGridCollection;
        private string _filterString = "";

        public event PropertyChangedEventHandler PropertyChanged;
 
        private List<Client> allClients { get; set; }
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                NotifyPropertyChanged("FilterString");
                FilterCollection();
            }
        }
        private void FilterCollection()
        {
            if (_dataGridCollection != null)
            {
                _dataGridCollection.Refresh();
            }
        }
        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set { _dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
        }
        public ClientList( )
        { 
            InitializeComponent();

            this.DataContext = this;
            
            this.Loaded += delegate
            {
                allClients = new QuickbooksService().GetClients();
                DataGridCollection = CollectionViewSource.GetDefaultView(allClients);
                DataGridCollection.Filter = new Predicate<object>(Filter);
                ClientsList.ItemsSource = DataGridCollection;
            };
        }
        public bool Filter(object obj)
        {
            var data = obj as Client;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    return data.Name.ToUpper().Contains(_filterString.ToUpper()) || data.Name.ToUpper().Contains(_filterString.ToUpper());
                }
                return true;
            }
            return false;
        }
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
