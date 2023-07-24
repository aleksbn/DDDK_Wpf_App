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
        public async static Task<string> GetDonators(Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/Donator/");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonatorDTO>>(json);
                store.Donators = new ObservableCollection<DonatorDTO>(data);
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

        public static async Task<string> AddDonator(CreateDonatorDTO donator, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donator);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/Donator", content);
                
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

        public static async Task<string> EditDonator(UpdateDonatorDTO donator, int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donator);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7056/api/Donator/" + id, content);

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

        public static async Task<string> DeleteDonator(int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/Donator/" + id);

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
    }
}
