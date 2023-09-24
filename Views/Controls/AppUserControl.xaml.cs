using DDDK_Wpf.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Views.Controls
{
    public partial class AppUserControl : UserControl
    {
        public UserDTO AppUser
        {
            get { return (UserDTO)GetValue(AppUserProperty); }
            set { SetValue(AppUserProperty, value); }
        }

        public static readonly DependencyProperty AppUserProperty =
            DependencyProperty.Register("AppUser", typeof(UserDTO), typeof(AppUserControl), new PropertyMetadata(new UserDTO()
            {
                id = "",
                email = "test@test.net",
                role = "role"
            }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AppUserControl control = d as AppUserControl;
            if (control != null)
            {
                control.tbUsername.Text = (e.NewValue as UserDTO).email;
                control.tbRole.Text = (e.NewValue as UserDTO).role;
            }
        }

        public AppUserControl()
        {
            InitializeComponent();
        }
    }
}
