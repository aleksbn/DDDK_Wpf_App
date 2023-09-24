using DDDK_Wpf.DTOs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Views.Controls
{
    public partial class DonationControl : UserControl
    {
        public DonationDTO Donation 
        { 
            get { return (DonationDTO)GetValue(DonationProperty); } 
            set { SetValue(DonationProperty, value); }
        }

        public static readonly DependencyProperty DonationProperty = 
            DependencyProperty.Register("Donation", typeof(DonationDTO), typeof(DonationControl), new PropertyMetadata(new DonationDTO()
            {
                donatorFullName = "Full Name",
                donatorId = 0
            }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DonationControl control = (DonationControl)d;
            if(control != null)
            {
                control.tbFullName.Text = (e.NewValue as DonationDTO).donatorFullName.Split(" (")[0];
                control.tbDonatorId.Text = "Donator ID: " + (e.NewValue as DonationDTO).donatorId.ToString();
                control.tbDonationDate.Text = (e.NewValue as DonationDTO).donatorFullName.Split(" - ")[1];
            }
        }

        public DonationControl()
        {
            InitializeComponent();
        }
    }
}
