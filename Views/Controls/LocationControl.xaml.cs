using DDDK_Wpf.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Views.Controls
{
    public partial class LocationControl : UserControl
    {
        public LocationDTO Location
        {
            get { return (LocationDTO)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(LocationDTO), typeof(LocationControl), new PropertyMetadata(new LocationDTO()
            {
                name = "Location name",
                description = "Location Description",
            }, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LocationControl control = d as LocationControl;
            if(control != null)
            {
                control.tbLocationName.Text = (e.NewValue as LocationDTO).name;
                control.tbLocationDescription.Text = (e.NewValue as LocationDTO).description;
            }
        }

        public LocationControl()
        {
            InitializeComponent();
        }
    }
}
