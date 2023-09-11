using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class DonatorsDAL
    {
        public static async Task<string> AddDonator(CreateDonatorDTO donator, Store store)
        {
            string url = "Donator/";
            return await GenericDAL.SendPostRequestAsync(url, donator, store.Token);
        }
        public static async Task<string> GetDonators(Store store)
        {
            string url = "Donator/";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);

            if (result.IsSuccessStatusCode)
            {
                if(store.Donators != null)
                {
                    store.Donators.Clear();
                }
                else
                {
                    store.Donators = new ObservableCollection<DonatorDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonatorDTO>>(json);
                foreach (var donatorDTO in data)
                {
                    store.Donators.Add(donatorDTO);
                }
                return "Done";
            }
            else
            {
                return "Fail";
            }
        }
        public static async Task<string> EditDonator(UpdateDonatorDTO donator, int id, Store store)
        {
            string url = $"Donator/{id}";
            return await GenericDAL.SendPutRequestAsync(url, donator, store.Token);
        }
        public static async Task<string> DeleteDonator(int id, Store store)
        {
            string url = $"Donator/{id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
    }
}
