using DDDK_Wpf.DTOs;
using DDDK_Wpf.Helpers;
using DDDK_Wpf.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for DonatorEventsPage.xaml
    /// </summary>
    public partial class DonationEventsPage : Page
    {
        private Store _store;
        private string mode = "new";

        public DonationEventsPage(Store store)
        {
            _store = store;
            InitializeComponent();
            ToggleLock(false);
        }

        private void ToggleLock(bool value)
        {
            cbLocations.IsEnabled = value;
            tbDate.IsEnabled = value;
            tbDescription.IsEnabled = value;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(_store.Locations == null)
            {
                await LocationsDAL.GetLocations(_store);
            }
            cbLocations.ItemsSource = _store.Locations.OrderBy(l => l.name);

            if(_store.DonationEvents == null)
            {
                await ForceReload();
            }

            lbDonationEvents.ItemsSource = _store.DonationEvents;
            lbDonationEvents.SelectedIndex = -1;

            if (_store.DonationEvents == null)
            {
                tbResStatus.Text = "There are no donation events. Add some!";
                tbDate.Focus();
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
                ToggleLock(true);
            }
        }

        private async Task ForceReload()
        {
            await DonationEventsDAL.GetDonationEvents(_store);
            lbDonationEvents.SelectedIndex = -1;
            lbDonationEvents.ItemsSource = null;
            lbDonationEvents.ItemsSource = _store.DonationEvents;

        }

        private void lbDonationEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbDonationEvents.SelectedIndex > -1) 
            {
                var donationEvent = (DonationEventDTO)lbDonationEvents.SelectedItem;
                tbId.Text = donationEvent.id.ToString();
                tbDescription.Text = donationEvent.description;
                tbDate.Text = donationEvent.eventDate.ToString("dd.MM.yyyy");
                cbLocations.SelectedItem = cbLocations.Items[cbLocations.Items.IndexOf(cbLocations.Items.Cast<LocationDTO>().First(bt => bt.id == donationEvent.locationId))];
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                tbResStatus.Text = "";
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            lbDonationEvents.SelectedIndex = -1;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            ToggleLock(true);
            ClearForm();
            tbDate.Focus();
            mode = "new";
            tbResStatus.Text = "";
            cbLocations.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            tbDate.Clear();
            tbDescription.Clear();
            tbId.Clear();
            cbLocations.SelectedIndex = -1;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await ChangeData();
        }

        private async Task ChangeData()
        {
            if (Validate())
            {
                if (mode == "new")
                {
                    CreateDonationEventDTO donationEventDTO = new CreateDonationEventDTO()
                    {
                        eventDate = DateTime.ParseExact(tbDate.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        description = tbDescription.Text,
                        locationId = ((LocationDTO)cbLocations.SelectedItem).id
                    };

                    var result = await DonationEventsDAL.AddDonationEvent(donationEventDTO, _store);

                    if(result == "Done")
                    {
                        ToggleLock(false);
                        await ForceReload();
                        lbDonationEvents.SelectedIndex = lbDonationEvents.Items.Count - 1;
                        tbResStatus.Text = "Donation event has been added!";
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
                    UpdateDonationEventDTO donationEventDTO = new UpdateDonationEventDTO()
                    {
                        eventDate = DateTime.ParseExact(tbDate.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        description = tbDescription.Text,
                        locationId = ((LocationDTO)cbLocations.SelectedItem).id,
                        id = int.Parse(tbId.Text)
                    };

                    var result = await DonationEventsDAL.EditDonationEvent(donationEventDTO, int.Parse(tbId.Text), _store);

                    if (result == "Done")
                    {
                        ToggleLock(false);
                        await ForceReload();
                        lbDonationEvents.SelectedIndex = lbDonationEvents.Items.Count - 1;
                        tbResStatus.Text = "Donation event has been edited!";
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

        private bool Validate()
        {
            if (!DataCheckers.IsDate(tbDate.Text))
            {
                MessageBox.Show("Date of the event must be in DD.MM.YYYY format!", "Error in validation");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                MessageBox.Show("You must enter the event's description!", "Error in validation");
                return false;
            }
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
                var result = await DonationEventsDAL.DeleteDonationEvent(((DonationEventDTO)lbDonationEvents.SelectedItem).id, _store);
                lbDonationEvents.SelectedIndex = -1;

                if (result == "Done")
                {
                    ClearForm();
                    await ForceReload();
                    btnEdit.IsEnabled = false;
                    tbResStatus.Text = "Donation event has been deleted!";
                }
                else
                {
                    tbResStatus.Text = result;
                }
            }
        }
    }
}
