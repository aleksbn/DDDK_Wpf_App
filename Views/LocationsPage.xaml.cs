using DDDK_Wpf.DTOs;
using DDDK_Wpf.Helpers;
using DDDK_Wpf.Warehouse;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for LocationsPage.xaml
    /// </summary>
    public partial class LocationsPage : Page
    {
        private Store _store;
        private string mode = "new";

        public LocationsPage(Store store)
        {
            _store = store;
            InitializeComponent();
            ToggleLock(false);
        }

        private void ToggleLock(bool into)
        {
            tbName.IsEnabled = into;
            tbDescription.IsEnabled = into;
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_store.Locations == null)
            {
                await ForceReload();
            }
            
            lbLocations.ItemsSource = _store.Locations;
            
            if(_store.Locations == null)
            {
                tbResStatus.Text = "There are no locations. Add one!";
                tbName.Focus();
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
                ToggleLock(true);
            }
        }

        private async Task ForceReload()
        {
            await LocationsDAL.GetLocations(_store);
            lbLocations.SelectedIndex = -1;
        }

        private void lbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLocations.SelectedIndex > -1)
            {
                var location = (LocationDTO)lbLocations.SelectedItem;
                tbId.Text = location.id.ToString(); 
                tbName.Text = location.name; 
                tbDescription.Text = location.description;
                ToggleLock(false);
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                tbResStatus.Text = "";
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            lbLocations.SelectedIndex = -1;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            ToggleLock(true);
            ClearForm();
            tbName.Focus();
            mode = "new";
            tbResStatus.Text = "";
        }

        private void ClearForm()
        {
            tbDescription.Clear();
            tbId.Clear();
            tbName.Clear();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await ChangeData();
        }

        private async Task ChangeData()
        {
            if (ValidateInputs())
            {
                if (mode == "new")
                {
                    CreateLocationDTO location = new CreateLocationDTO()
                    {
                        name = tbName.Text,
                        description = tbDescription.Text
                    };

                    var result = await LocationsDAL.AddLocation(location, _store);
                    if (result == "Done")
                    {
                        tbResStatus.Text = "New location added!";
                        ToggleLock(false);
                        await ForceReload();
                        lbLocations.SelectedIndex = lbLocations.Items.Count - 1;
                        btnDelete.IsEnabled = true;
                        btnEdit.IsEnabled = true;
                        btnSave.IsEnabled = false;
                    }
                    else
                    {
                        tbResStatus.Text = result;
                    }
                }
                else
                {
                    UpdateLocationDTO location = new UpdateLocationDTO() { name = tbName.Text, description = tbDescription.Text };
                    var result = await LocationsDAL.EditLocation(location, int.Parse(tbId.Text), _store);
                    int selectedIndex = lbLocations.SelectedIndex;
                    if (result == "Done")
                    {
                        tbResStatus.Text = "Location edited!";
                        ToggleLock(false);
                        await ForceReload();
                        lbLocations.SelectedIndex = selectedIndex;
                        btnDelete.IsEnabled = true;
                        btnEdit.IsEnabled = true;
                        btnSave.IsEnabled = false;
                    }
                    else
                    {
                        tbResStatus.Text = result;
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            if (ValidationHelper.ValidateElement(tbDescription, "You must enter the location's description!", ValidationType.Text)) return false;
            if (ValidationHelper.ValidateElement(tbName, "You must enter the location's name!", ValidationType.Text)) return false;
            return true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            mode = "old";
            ToggleLock(true);
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            tbResStatus.Text = "";
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete selected entity?", "Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var result = await LocationsDAL.DeleteLocation(((LocationDTO)lbLocations.SelectedItem).id, _store);
                lbLocations.SelectedIndex = -1;

                if (result == "Done")
                {
                    ClearForm();
                    tbResStatus.Text = "Location deleted!";
                    await ForceReload();
                    btnEdit.IsEnabled = false;
                }
                else
                {
                    tbResStatus.Text = result;
                }
            }
        }
    }
}
