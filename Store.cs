using DDDK_Wpf.DTOs;
using System.Collections.ObjectModel;

namespace DDDK_Wpf
{
    public class Store
    {
        public ObservableCollection<User> Users { get; set; } = null;
        public ObservableCollection<LocationDTO> Locations { get; set; } = null;
        public ObservableCollection<DonatorDTO> Donators { get; set; } = null;
        public ObservableCollection<DonationEventDTO> DonationEvents { get; set; } = null;
        public ObservableCollection<DonationDTO> Donations { get; set; } = null;
        public ObservableCollection<BloodTypeDTO> BloodTypes { get; set; } = null;

        public string Token { get; set; }
        public string Role { get; set; }
    }
}
