using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class DonationEventsDAL
    {
        public static async Task<string> AddDonationEvent(CreateDonationEventDTO donationEventDTO, Store store)
        {
            string url = "DonationEvent/";
            return await GenericDAL.SendPostRequestAsync(url, donationEventDTO, store.Token);
        }
        public static async Task<string> GetDonationEvents(Store store)
        {
            string url = "DonationEvent/";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);

            if (result.IsSuccessStatusCode)
            {
                if(store.DonationEvents != null)
                {
                    store.DonationEvents.Clear();
                } 
                else
                {
                    store.DonationEvents = new ObservableCollection<DonationEventDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonationEventDTO>>(json);
                foreach (var donationEventDTO in data)
                {
                    store.DonationEvents.Add(donationEventDTO);
                }
                return "Done";
            }
            else
            {
                return "Fail";
            }

        }
        public static async Task<string> EditDonationEvent(UpdateDonationEventDTO donationEventDTO, int id, Store store)
        {
            string url = $"DonationEvent/{id}";
            return await GenericDAL.SendPutRequestAsync(url, donationEventDTO, store.Token);
        }
        public static async Task<string> DeleteDonationEvent(int id, Store store)
        {
            string url = $"DonationEvent/{id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
    }
}
