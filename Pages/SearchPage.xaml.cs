using DDDK_Wpf.DTOs;
using DDDK_Wpf.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        private Store _store;
        List<DonatorSearchDTO> donatorSearchDTOs;
        List<DonationEventSearchDTO> donationEventSearchDTOs;
        List<DonatorSearchDTO> donatorsFromEvent;

        public SearchPage(Store store)
        {
            InitializeComponent();
            _store = store;
        }

        public void LoadAllDonators()
        {
            donatorSearchDTOs = new List<DonatorSearchDTO>();
            foreach (var item in _store.Donators)
            {
                var allDonationEventIds = _store.Donations.Where(d => d.donatorId == item.id).Select(d => d.donationEventId);
                var allDonationEvents = _store.DonationEvents.Where(de => allDonationEventIds.Contains(de.id)).OrderByDescending(de => de.eventDate);
                var lastDonationDate = allDonationEvents.ToList()[0].eventDate.ToString("dd.MM.yyyy");
                var totalDonations = _store.Donations.Where(d => d.donatorId == item.id).Count() + item.previousDonations;
                donatorSearchDTOs.Add(new DonatorSearchDTO()
                {
                    id = item.id,
                    firstName = item.firstName,
                    lastName = item.lastName,
                    phoneNumber = item.phoneNumber,
                    email = item.email,
                    bloodType = _store.BloodTypes.Single(bt => bt.id == item.bloodTypeId).ToString(),
                    lastDonationDate = lastDonationDate,
                    totalDonations = totalDonations
                });
            }
        }

        private void LoadAllDonationEvents()
        {
            donationEventSearchDTOs = new List<DonationEventSearchDTO>();
            foreach (var item in _store.DonationEvents)
            {
                var location = _store.Locations.First(l => l.id == item.locationId);
                donationEventSearchDTOs.Add(new DonationEventSearchDTO()
                {
                    id = item.id,
                    location = location.name,
                    description = item.description,
                    donationDate = item.eventDate.ToString("dd.MM.yyyy")
                });
            }
        }

        private async void tcSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.OriginalSource is ComboBox || e.OriginalSource is ListBox || e.OriginalSource is DataGrid)
            {
                return;
            }
            if(tcSearch.SelectedIndex == 0)
            {
                if (_store.BloodTypes == null)
                {
                    await BloodTypesDAL.GetBloodTypes(_store);
                }
                cbBloodTypes.ItemsSource = _store.BloodTypes;

                await DonatorsDAL.GetDonators(_store);
                await DonationsDAL.GetDonations(_store);
                await DonationEventsDAL.GetDonationEvents(_store);
                await LocationsDAL.GetLocations(_store);

                LoadAllDonators();
                dgDonors.ItemsSource = donatorSearchDTOs;
            }
            else
            {
                await BloodTypesDAL.GetBloodTypes(_store);
                dgDonors.ItemsSource = null;
                dgDonors.ItemsSource = donatorSearchDTOs;
            }
        }

        private void ApplyFiltersAvailability()
        {
            LoadAllDonators();
            dgDonors.ItemsSource = null;

            if (cbBloodTypes.SelectedIndex > -1)
            {
                donatorSearchDTOs = donatorSearchDTOs.Where(d => d.bloodType == (((BloodTypeDTO)cbBloodTypes.SelectedItem).name + ((BloodTypeDTO)cbBloodTypes.SelectedItem).phFactor)).Select(d => d).ToList();
            }

            if(cbAvailable.IsChecked == true)
            {
                var toRemove = new List<DonatorSearchDTO>();
                foreach (var item in donatorSearchDTOs)
                {
                    var dateParts = item.lastDonationDate.Split('.');
                    if ((new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0])).AddMonths(4) >= DateTime.Now.Date))
                    {
                        toRemove.Add(item);
                    }
                }
                if (toRemove.Count > 0)
                {
                    foreach (var item in toRemove)
                    {
                        donatorSearchDTOs.Remove(item);
                    }
                }
            }

            dgDonors.ItemsSource = donatorSearchDTOs;
        }

        private void cbBloodTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAvailability();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbBloodTypes.SelectedIndex = -1;
            cbAvailable.IsChecked = false;
        }

        private void cbAvailable_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAvailability();
        }

        private void tbDonatorId_KeyDown(object sender, KeyEventArgs e)
        {
            tbDonatorFirstName.Clear();
            tbDonatorLastName.Clear();
            if (e.Key == Key.Enter)
            {
                SearchDonators();
            }
        }

        private void tbDonatorFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            tbDonatorId.Clear();
            tbDonatorLastName.Clear();
            if(e.Key == Key.Enter)
            {
                SearchDonators();
            }
        }

        private void tbDonatorLastName_KeyDown(object sender, KeyEventArgs e)
        {
            tbDonatorId.Clear();
            tbDonatorFirstName.Clear();
            if (e.Key == Key.Enter)
            {
                SearchDonators();
            }
        }

        private void btnSearchDonators_Click(object sender, RoutedEventArgs e)
        {
            SearchDonators();
        }

        private void lbDonators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbDonators.SelectedIndex > -1)
            {
                var donator = (DonatorSearchDTO)lbDonators.SelectedItem;
                tblBloodType.Text = donator.bloodType;
                tblFirstName.Text = donator.firstName;
                tblLastName.Text = donator.lastName;
                tblPhoneNumber.Text = donator.phoneNumber;
                tblLastDonationDate.Text = donator.lastDonationDate;

                List<DonationSearchDTO> items = new List<DonationSearchDTO>();
                var donations = _store.Donations.Where(d => d.donatorId == donator.id).ToList();
                foreach (var i in donations)
                {
                    var donationEvent = _store.DonationEvents.First(x => x.id == i.donationEventId);
                    items.Add(new DonationSearchDTO()
                    {
                        location = _store.Locations.First(d => d.id == donationEvent.locationId).name,
                        donationDate = donationEvent.eventDate.ToString("dd.MM.yyyy")
                    });
                }
                dgDonations.ItemsSource = items;
            }
            else
            {
                tblBloodType.Text = "";
                tblFirstName.Text = "";
                tblLastDonationDate.Text = "";
                tblLastName.Text = "";
            }
        }

        private void SearchDonators()
        {
            lbDonators.SelectedIndex = -1;
            LoadAllDonators();
            if (tbDonatorId.Text != "")
            {
                donatorSearchDTOs = donatorSearchDTOs.Where(d => d.id.ToString().Equals(tbDonatorId.Text)).ToList();
            }
            else if (tbDonatorFirstName.Text != "" || tbDonatorLastName.Text != "")
            {
                donatorSearchDTOs = donatorSearchDTOs.Where(d => d.firstName.ToLower().Contains(tbDonatorFirstName.Text.ToLower()) && d.lastName.ToLower().Contains(tbDonatorLastName.Text.ToLower())).ToList();
            }
            lbDonators.ItemsSource = null;
            lbDonators.ItemsSource = donatorSearchDTOs;
        }

        private void SearchDonationEvents()
        {
            lbDonationEvents.SelectedIndex = -1;
            LoadAllDonationEvents();
            if(tbLocation.Text != "")
            {
                donationEventSearchDTOs = donationEventSearchDTOs.Where(d => d.location.ToLower().Contains(tbLocation.Text.ToLower())).ToList();
            }
            if (tbDate.Text != "")
            {
                donationEventSearchDTOs = donationEventSearchDTOs.Where(d => d.donationDate.Equals(tbDate.Text)).ToList();
            }
            lbDonationEvents.ItemsSource = null;
            lbDonationEvents.ItemsSource = donationEventSearchDTOs;
        }

        private void tbLocation_KeyDown(object sender, KeyEventArgs e)
        {
            tbDate.Clear();
            if(e.Key == Key.Enter)
            {
                SearchDonationEvents();
            }
        }

        private void tbDate_KeyDown(object sender, KeyEventArgs e)
        {
            tbLocation.Clear();
            if (e.Key == Key.Enter)
            {
                SearchDonationEvents();
            }
        }

        private void btnSearchDonationEvents_Click(object sender, RoutedEventArgs e)
        {
            SearchDonationEvents();
        }

        private void lbDonationEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbDonationEvents.SelectedIndex > -1)
            {
                dgDonationsFromEvent.ItemsSource = null;

                var donationEvent = (DonationEventSearchDTO)lbDonationEvents.SelectedItem;
                tblLocation.Text = donationEvent.location;
                tblDate.Text = donationEvent.donationDate;
                donatorsFromEvent = new List<DonatorSearchDTO>();

                var donations = _store.Donations.Where(d => d.donationEventId == donationEvent.id).ToList();
                tblDonators.Text = donations.Count.ToString();
                foreach (var donation in donations)
                {
                    var donator = _store.Donators.First(d => d.id == donation.donatorId);
                    var bloodType = _store.BloodTypes.First(bt => bt.id == donator.bloodTypeId);
                    donatorsFromEvent.Add(new DonatorSearchDTO()
                    {
                        fullName = donator.firstName + " " + donator.lastName,
                        bloodType = bloodType.name + bloodType.phFactor
                    });
                }
                dgDonationsFromEvent.ItemsSource = donatorsFromEvent;
            }
            else
            {
                tblBloodType.Text = "";
                tblFirstName.Text = "";
                tblLastDonationDate.Text = "";
                tblLastName.Text = "";
            }
        }
    }
}
