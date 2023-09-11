using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class DonationsDAL
    {
        public static async Task<string> AddDonation(CreateDonationDTO donation, Store store)
        {
            string url = "Donation/";
            return await GenericDAL.SendPostRequestAsync(url, donation, store.Token);
        }
        public static async Task<string> AddDonationRange(List<CreateDonationDTO> donationsToSend, Store store)
        {
            string url = "Donation/addmultiple";
            return await GenericDAL.SendPostRequestAsync(url, donationsToSend, store.Token);
        }
        public static async Task<string> GetDonations(Store store)
        {
            string url = "Donation/";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);

            if (result.IsSuccessStatusCode)
            {
                if(store.Donations != null)
                {
                    store.Donations.Clear();
                }
                else
                {
                    store.Donations = new ObservableCollection<DonationDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonationDTO>>(json);
                foreach (var donationDTO in data)
                {
                    store.Donations.Add(donationDTO);
                }
                return "Done";
            }
            else
            {
                return "Fail";
            }

        }
        public static async Task<string> EditDonation(UpdateDonationDTO donation, int id, Store store)
        {
            string url = $"Donation/{id}";
            return await GenericDAL.SendPutRequestAsync(url, donation, store.Token);
        }
        public static async Task<string> DeleteDonation(int id, Store store)
        {
            string url = $"Donation/{id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
        public static async Task<string> DeleteDonationRange(int id, Store store)
        {
            string url = $"Donation/deletemultiple/{id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
    }
}
