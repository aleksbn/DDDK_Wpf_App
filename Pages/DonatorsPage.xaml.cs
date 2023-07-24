﻿using DDDK_Wpf.DTOs;
using DDDK_Wpf.Helpers;
using DDDK_Wpf.Warehouse;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for DonatorsPage.xaml
    /// </summary>
    public partial class DonatorsPage : Page
    {
        private Store _store;
        private string mode = "new";

        public DonatorsPage(Store store)
        {
            _store = store;
            InitializeComponent();
            ToggleLock(false);
        }

        private void ToggleLock(bool value)
        {
            cbBloodTypes.IsEnabled = value;
            tbAddress.IsEnabled = value;
            tbBirthDate.IsEnabled = value;
            tbEmail.IsEnabled = value;
            tbFirstName.IsEnabled = value;
            tbLastName.IsEnabled = value;
            tbPhoneNumber.IsEnabled = value;
            tbPreviousDonations.IsEnabled = value;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(_store.BloodTypes == null)
            {
                await BloodTypesDAL.GetBloodTypes(_store);
            }
            cbBloodTypes.ItemsSource = _store.BloodTypes;

            if(_store.Donators == null)
            {
                await ForceReload();
            }

            lbDonators.ItemsSource = _store.Donators;
            lbDonators.SelectedIndex = -1;

            if(_store.Donators == null)
            {
                tbResStatus.Text = "There are no blood donors. Add some!";
                tbFirstName.Focus();
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
                ToggleLock(true);
            }
        }

        private async Task ForceReload()
        {
            await DonatorsDAL.GetDonators(_store);
            lbDonators.SelectedIndex = -1;
            lbDonators.ItemsSource = null;
            lbDonators.ItemsSource = _store.Donators;
        }

        private void lbDonators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbDonators.SelectedIndex > -1)
            {
                var donator = (DonatorDTO)lbDonators.SelectedItem;
                tbAddress.Text = donator.address;
                tbBirthDate.Text = donator.birthDate.ToString("dd.MM.yyyy");
                tbEmail.Text = donator.email;
                tbFirstName.Text = donator.firstName;
                tbLastName.Text = donator.lastName;
                tbPhoneNumber.Text = donator.phoneNumber;
                tbId.Text = donator.id.ToString();
                tbPreviousDonations.Text = donator.previousDonations.ToString();
                cbBloodTypes.SelectedItem = cbBloodTypes.Items[cbBloodTypes.Items.IndexOf(cbBloodTypes.Items.Cast<BloodTypeDTO>().First(bt => bt.id == donator.bloodTypeId))];
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                tbResStatus.Text = "";
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            lbDonators.SelectedIndex = -1;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            ToggleLock(true);
            ClearForm();
            tbFirstName.Focus();
            mode = "new";
            tbResStatus.Text = "";
            cbBloodTypes.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            tbAddress.Clear();
            tbBirthDate.Clear();
            tbFirstName.Clear();
            tbLastName.Clear();
            tbPhoneNumber.Clear();
            tbId.Clear();
            tbPreviousDonations.Clear();
            tbEmail.Clear();
            cbBloodTypes.SelectedIndex = -1;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await ChangeData();
        }

        private async Task ChangeData()
        {
            if (Validate())
            {
                if(mode == "new")
                {
                    CreateDonatorDTO donator = new CreateDonatorDTO()
                    {
                        firstName = tbFirstName.Text,
                        lastName = tbLastName.Text,
                        phoneNumber = tbPhoneNumber.Text,
                        address = tbAddress.Text,
                        birthDate = DateTime.ParseExact(tbBirthDate.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        previousDonations = int.Parse(tbPreviousDonations.Text),
                        email = tbEmail.Text,
                        bloodTypeId = ((BloodTypeDTO)cbBloodTypes.SelectedItem).id
                    };

                    var result = await DonatorsDAL.AddDonator(donator, _store);
                    if (result == "Done")
                        {
                            ToggleLock(false);
                            await ForceReload();
                            lbDonators.SelectedIndex = lbDonators.Items.Count - 1;
                            tbResStatus.Text = "Donator has been added!";
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
                    UpdateDonatorDTO donator = new UpdateDonatorDTO() 
                    {
                        firstName = tbFirstName.Text,
                        lastName = tbLastName.Text,
                        phoneNumber = tbPhoneNumber.Text,
                        address = tbAddress.Text,
                        birthDate = DateTime.ParseExact(tbBirthDate.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        previousDonations = int.Parse(tbPreviousDonations.Text),
                        email = tbEmail.Text,
                        bloodTypeId = ((BloodTypeDTO)cbBloodTypes.SelectedItem).id
                    };
                    var result = await DonatorsDAL.EditDonator(donator, int.Parse(tbId.Text), _store);
                    int selectedIndex = lbDonators.SelectedIndex;
                    if (result == "Done")
                    {
                        ToggleLock(false);
                        await ForceReload();
                        lbDonators.SelectedIndex = selectedIndex;
                        tbResStatus.Text = "Donator edited!";
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
            if (string.IsNullOrWhiteSpace(tbFirstName.Text))
            {
                MessageBox.Show("You must enter the donator's first name!", "Error in validation");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbLastName.Text))
            {
                MessageBox.Show("You must enter the donator's last name!", "Error in validation");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                MessageBox.Show("You must enter the donator's address!", "Error in validation");
                return false;
            }
            if (!DataCheckers.IsPhoneNumber(tbPhoneNumber.Text))
            {
                MessageBox.Show("You must enter the donator's phone number! The number consists only of numbers and possibly signs +, - and /.", "Error in validation");
                return false;
            }
            if (!DataCheckers.IsEmail(tbEmail.Text))
            {
                MessageBox.Show("You must enter the donator's email as the proper email address!", "Error in validation");
                return false;
            }
            int x;
            if (!int.TryParse(tbPreviousDonations.Text, out x) && x <= 0)
            {
                MessageBox.Show("Previous donations must be zero or greather than zero.", "Error in validation");
                return false;
            }

            if (!DataCheckers.IsDate(tbBirthDate.Text))
            {
                MessageBox.Show("Date of birth must be in DD.MM.YYYY format!", "Error in validation");
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
                var result = await DonatorsDAL.DeleteDonator(((DonatorDTO)lbDonators.SelectedItem).id, _store);
                lbDonators.SelectedIndex = -1;

                if (result == "Done")
                {
                    ClearForm();
                    tbResStatus.Text = "Donator deleted!";
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
