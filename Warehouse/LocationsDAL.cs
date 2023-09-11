using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class LocationsDAL
    {
        public static async Task<string> AddLocation(CreateLocationDTO location, Store store)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(location);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/Location", content);
                
                if(response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<string> GetLocations(Store store)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/Location/");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<LocationDTO>>(json);
                store.Locations = new ObservableCollection<LocationDTO>(data);
                if(result.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<string> EditLocation(UpdateLocationDTO location, int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(location);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7056/api/Location/" + id, content);

                if(response.IsSuccessStatusCode )
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<string> DeleteLocation(int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/Location/" + id);

                if(response.IsSuccessStatusCode)
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
