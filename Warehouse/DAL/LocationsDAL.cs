using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class LocationsDAL
    {
        public static async Task<string> AddLocation(CreateLocationDTO location, Store store)
        {
            string url = "Location/";
            return await GenericDAL.SendPostRequestAsync(url, location, store.Token);
        }
        public static async Task<string> GetLocations(Store store)
        {
            string url = "Location/";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);

            if (result.IsSuccessStatusCode)
            {
                if (store.Locations != null)
                {
                    store.Locations.Clear();
                }
                else
                {
                    store.Locations = new ObservableCollection<LocationDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<LocationDTO>>(json);
                foreach (var location in data)
                {
                    store.Locations.Add(location);
                }
                return "Done";
            }
            else
            {
                return await result.Content.ReadAsStringAsync();
            }
        }
        public static async Task<string> EditLocation(UpdateLocationDTO location, int id, Store store)
        {
            string url = $"Location/{id}";
            return await GenericDAL.SendPutRequestAsync(url, location, store.Token);
        }
        public static async Task<string> DeleteLocation(int id, Store store)
        {
            string url = $"Location/{id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
    }
}
