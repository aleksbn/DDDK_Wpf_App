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
    internal static class DonationEventsDAL
    {
        public async static Task<string> AddDonationEvent(CreateDonationEventDTO donationEventDTO, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donationEventDTO);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/DonationEvent", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static async Task<string> DeleteDonationEvent(int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/DonationEvent/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static async Task<string> EditDonationEvent(UpdateDonationEventDTO donationEventDTO, int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donationEventDTO);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7056/api/DonationEvent/" + id, content);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static async Task<string> GetDonationEvents(Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/DonationEvent/");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonationEventDTO>>(json);
                store.DonationEvents = new ObservableCollection<DonationEventDTO>(data);
                if (result.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return "Fail";
                }
            }
        }
    }
}
