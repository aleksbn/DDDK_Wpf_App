using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class BloodTypesDAL
    {
        public static async Task<string> GetBloodTypes(Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/bloodtype");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<BloodTypeDTO>>(json);
                store.BloodTypes = new ObservableCollection<BloodTypeDTO>(data);
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
