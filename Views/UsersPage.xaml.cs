using DDDK_Wpf.DTOs;
using DDDK_Wpf.Helpers;
using DDDK_Wpf.Warehouse;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DDDK_Wpf.Pages
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private Store _store;
        private string mode = "new";
        private string oldEmail = "";

        public UsersPage(Store store)
        {
            InitializeComponent();
            _store = store;
            ToggleLock(false);
            ToggleVisibility(false);
        }

        private void ToggleLock(bool value)
        {
            tbEmail.IsEnabled = value;
            cbRoles.IsEnabled = value;
        }

        private void ToggleVisibility(bool value)
        {
            tbEmailConfirm.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            tbPassword.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            tbPasswordConfirm.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            tblEmailConfirm.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            tblPassword.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            tblPasswordConfirm.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            cbRoles.Items.Add(new RoleDTO() { RoleName = "Administrator"});
            cbRoles.Items.Add(new RoleDTO() { RoleName = "Moderator" });
            if (_store.Users == null)
            {
                await ForceReload();
            }
            lbUsers.ItemsSource = _store.Users;
            lbUsers.SelectedIndex = -1;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ForceReload()
        {
            await UsersDAL.GetUsers(_store);
            lbUsers.SelectedIndex = -1;
        }

        private void lbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbUsers.SelectedIndex > -1)
            {
                var user = (UserDTO)lbUsers.SelectedItem;
                tbId.Text = user.id;
                cbRoles.SelectedIndex = user.role == "Admin" ? 0 : 1;
                tbEmail.Text = user.email;
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                oldEmail = tbEmail.Text;
                tbResStatus.Text = "";
                ToggleLock(false);
                ToggleVisibility(false);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            lbUsers.SelectedIndex = -1;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            ToggleLock(true);
            ToggleVisibility(true);
            ClearForm();
            tbResStatus.Text = "";
            cbRoles.SelectedIndex = 0;
            tbId.Clear();
        }

        private void ClearForm()
        {
            tbId.Clear();
            tbEmail.Clear();
            tbEmailConfirm.Clear();
            tbPassword.Clear();
            tbPasswordConfirm.Clear();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await ChangeData();
        }

        private async Task ChangeData()
        {
            if (ValidateInputs())
            {
                if(mode == "new")
                {
                    RegisterUserDTO user = new RegisterUserDTO()
                    {
                        Email = tbEmail.Text,
                        Password = tbPassword.Password,
                        EmailConfirmation = tbEmailConfirm.Text,
                        PasswordConfirmation = tbPasswordConfirm.Password,
                        Role = cbRoles.SelectedItem.ToString() == "Admin" ? "ADMINISTRATOR" : "MODERATOR"
                    };

                    var result = await UsersDAL.RegisterUser(user, _store);
                    if (result.Contains("Done"))
                    {
                        ToggleLock(false);
                        await ForceReload();
                        lbUsers.SelectedIndex = lbUsers.Items.Count - 1;
                        tbResStatus.Text = $"User ({user.Role}) has been added!";
                        btnDelete.IsEnabled = true;
                        btnEdit.IsEnabled = true;
                        btnSave.IsEnabled = false;
                        ToggleVisibility(false);
                    }
                    else
                    {
                        tbResStatus.Text = result;
                    }
                }
                else
                {
                    UpdateUserDTO user = new UpdateUserDTO()
                    {
                        OldEmail = oldEmail,
                        Email = tbEmail.Text,
                        Password = tbPassword.Password,
                        EmailConfirmation = tbEmailConfirm.Text,
                        PasswordConfirmation = tbPasswordConfirm.Password,
                        Role = cbRoles.SelectedItem.ToString() == "Admin" ? "ADMINISTRATOR" : "MODERATOR"
                    };
                    var result = await UsersDAL.UpdateUser(user, _store);
                    int selectedIndex = lbUsers.SelectedIndex;
                    if (result == "Done")
                    {
                        ToggleLock(false);
                        await ForceReload();
                        lbUsers.SelectedIndex = selectedIndex;
                        tbResStatus.Text = "User edited!";
                        btnDelete.IsEnabled = true;
                        btnEdit.IsEnabled = true;
                        btnSave.IsEnabled = false;
                        ToggleVisibility(false);
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
            if (ValidationHelper.ValidateElement(tbEmail, "You must enter a propper email address.", ValidationType.Email)) return false;
            if (ValidationHelper.ValidateElement(tbEmailConfirm, "Email and its confirmation must match.", ValidationType.StringEquality, tbEmail.Text)) return false;
            if (ValidationHelper.ValidateElement(tbPassword, "Password must contain 8 characters, uppercase, lowercase, special sign and a number.", ValidationType.Password)) return false;
            if (ValidationHelper.ValidateElement(tbPasswordConfirm, "Password and its confirmation must match.", ValidationType.Password, tbPassword.Password)) return false;
            return true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            mode = "old";
            ToggleLock(true);
            ToggleVisibility(true);
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            tbResStatus.Text = "";
            tbEmailConfirm.Text = " ";
            tbPassword.Password = " ";
            tbPasswordConfirm.Password = " ";
            tbEmailConfirm.Clear();
            tbPassword.Clear();
            tbPasswordConfirm.Clear();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete selected entity?", "Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var result = await UsersDAL.DeleteUser(((UserDTO)lbUsers.SelectedItem).id, _store);
                lbUsers.SelectedIndex = -1;

                if (result == "Done")
                {
                    ClearForm();
                    tbResStatus.Text = "User deleted!";
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
