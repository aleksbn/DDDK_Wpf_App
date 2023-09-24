using DDDK_Wpf.DTOs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Views.Controls
{
    public partial class DonationEventControl : UserControl
    {
        public DonationEventDTO DonationEvent 
        {
            get { return (DonationEventDTO)GetValue(DonationEventProperty); } 
            set { SetValue(DonationEventProperty, value); }
        }

        public static readonly DependencyProperty DonationEventProperty = 
            DependencyProperty.Register("DonationEvent", typeof(DonationEventDTO), typeof(DonationEventControl), new PropertyMetadata(new DonationEventDTO()
            {
                locationName = "Donation Event Location",
                eventDate = DateTime.Now
            }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DonationEventControl control = d as DonationEventControl;
            if (control != null)
            {
                control.tbDonationEventLocation.Text = (e.NewValue as DonationEventDTO).locationName;
                control.tbDonationEventDate.Text = (e.NewValue as DonationEventDTO).eventDate.ToString("dd.MM.yyyy");
            }
        }

        public DonationEventControl()
        {
            InitializeComponent();
        }
    }
}
