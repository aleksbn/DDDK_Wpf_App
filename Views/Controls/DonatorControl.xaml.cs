using DDDK_Wpf.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Views.Controls
{
    public partial class DonatorControl : UserControl
    {
        public DonatorDTO Donator
        {
            get { return (DonatorDTO)GetValue(DonatorProperty); }
            set { SetValue(DonatorProperty, value); }
        }

        public static readonly DependencyProperty DonatorProperty =
            DependencyProperty.Register("Donator", typeof(DonatorDTO), typeof(DonatorControl), new PropertyMetadata(new DonatorDTO()
            {
                firstName = "First name",
                lastName = "Last name",
                address = "Unknown",
                phoneNumber = "Unknown"
            }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DonatorControl control = d as DonatorControl;
            if(control != null)
            {
                control.tbFullName.Text = (e.NewValue as DonatorDTO).firstName + " " + (e.NewValue as DonatorDTO).lastName;
                control.tbAddress.Text = (e.NewValue as DonatorDTO).address;
                control.tbPhone.Text = (e.NewValue as DonatorDTO).phoneNumber;
            }
        }

        public DonatorControl()
        {
            InitializeComponent();
        }
    }
}
