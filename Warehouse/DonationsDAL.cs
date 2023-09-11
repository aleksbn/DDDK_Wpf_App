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
    internal static class DonationsDAL
    {
        public async static Task<string> AddDonation(CreateDonationDTO donation, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donation);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/Donation", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Done-" + JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public async static Task<string> AddDonationRange(List<CreateDonationDTO> donationsToSend, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donationsToSend);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/Donation/addmultiple", content);

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
        public async static Task<string> GetDonations(Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/Donation/");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DonationDTO>>(json);
                store.Donations = new ObservableCollection<DonationDTO>(data);

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
        public async static Task<string> EditDonation(UpdateDonationDTO donation, int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(donation);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7056/api/Donation/" + id, content);

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
        public async static Task<string> DeleteDonation(int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/Donation/" + id);

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
        public async static Task<string> DeleteDonationRange(int id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/Donation/deletemultiple/" + id);

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
