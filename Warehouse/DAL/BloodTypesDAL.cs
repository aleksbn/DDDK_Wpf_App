using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDDK_Wpf.Warehouse
{
    internal static class BloodTypesDAL
    {
        public static async Task<string> GetBloodTypes(Store store)
        {
            string url = "bloodtype";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);
                
            if (result.IsSuccessStatusCode)
            {
                if(store.BloodTypes != null)
                {
                    store.BloodTypes.Clear();
                }
                else
                {
                    store.BloodTypes = new ObservableCollection<BloodTypeDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<BloodTypeDTO>>(json);
                foreach (var bloodTypeDTO in data)
                {
                    store.BloodTypes.Add(bloodTypeDTO);
                }
                return "Done";
            }
            else
            {
                return "Fail";
            }

        }
    }
}
