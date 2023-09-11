using DDDK_Wpf.DTOs;
using DDDK_Wpf.Warehouse;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for DonationsPage.xaml
    /// </summary>
    public partial class DonationsPage : Page
    {
        private Store _store;
        private string mode = "new";

        public DonationsPage(Store store)
        {
            _store = store;
            InitializeComponent();
            ToggleLock(false);
        }

        private void ToggleLock(bool value)
        {
            cbDonationEvents.IsEnabled = value;
            cbDonators.IsEnabled = value;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(_store.DonationEvents == null)
            {
                await DonationEventsDAL.GetDonationEvents(_store);
            }
            cbDonationEvents.ItemsSource = _store.DonationEvents;

            if (_store.Donators == null)
            {
                await DonatorsDAL.GetDonators(_store);
            }

            if (_store.Donations == null)
            {
                await ForceReload();
            }
            cbDonators.ItemsSource = _store.Donators;

            if(_store.Donations == null)
            {
                await DonationsDAL.GetDonations(_store);
            }
            lbDonations.ItemsSource = _store.Donations;
            lbDonations.SelectedIndex = -1;

            if (_store.Donations == null)
            {
                tbResStatus.Text = "There are no donations. Add some!";
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
                ToggleLock(true);
            }
        }

        private async Task ForceReload()
        {
            await DonationsDAL.GetDonations(_store);
            lbDonations.SelectedIndex = -1;
        }

        private void lbDonations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbDonations.SelectedIndex > -1) 
            { 
                var donation = (DonationDTO)lbDonations.SelectedItem;
                tbId.Text = donation.id.ToString();
                cbDonators.SelectedItem = cbDonators.Items[cbDonators.Items.IndexOf(cbDonators.Items.Cast<DonatorDTO>().FirstOrDefault(d => d.id == donation.donatorId))];
                cbDonationEvents.SelectedItem = cbDonationEvents.Items[cbDonationEvents.Items.IndexOf(cbDonationEvents.Items.Cast<DonationEventDTO>().FirstOrDefault(de => de.id == donation.donationEventId))];
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                tbResStatus.Text = "";
                ToggleLock(false);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            lbDonations.SelectedIndex = -1;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            ToggleLock(true);
            ClearForm();
            mode = "new";
            tbResStatus.Text = "";
            cbDonators.SelectedIndex = 0;
            cbDonationEvents.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            tbId.Clear();
            cbDonationEvents.SelectedIndex = -1;
            cbDonators.SelectedIndex = -1;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await ChangeData();
        }

        private async Task ChangeData()
        {
            if (mode == "new")
            {
                CreateDonationDTO donation = new CreateDonationDTO()
                {
                    donationEventId = ((DonationEventDTO)cbDonationEvents.SelectedItem).id,
                    donatorId = ((DonatorDTO)cbDonators.SelectedItem).id
                };

                var result = await DonationsDAL.AddDonation(donation, _store);
                if (result.Contains("Done"))
                {
                    ToggleLock(false);
                    await ForceReload();
                    lbDonations.SelectedItem = lbDonations.Items[lbDonations.Items.IndexOf(lbDonations.Items.Cast<DonationDTO>().FirstOrDefault(d => d.id == int.Parse(result.Split("-")[1])))];
                    tbResStatus.Text = "Donation has been added!";
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
                UpdateDonationDTO donation = new UpdateDonationDTO()
                {
                    donationEventId = ((DonationEventDTO)cbDonationEvents.SelectedItem).id,
                    donatorId = ((DonatorDTO)cbDonators.SelectedItem).id
                };
                var result = await DonationsDAL.EditDonation(donation, int.Parse(tbId.Text), _store);
                    
                if (result == "Done")
                {
                    ToggleLock(false);
                    await ForceReload();
                    lbDonations.SelectedItem = _store.Donations.First(d => d.id == int.Parse(tbId.Text));
                    tbResStatus.Text = "Donation edited!";
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
                var result = await DonationsDAL.DeleteDonation(((DonationDTO)lbDonations.SelectedItem).id, _store);
                lbDonations.SelectedIndex = -1;

                if (result == "Done")
                {
                    ClearForm();
                    tbResStatus.Text = "Donation deleted!";
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
