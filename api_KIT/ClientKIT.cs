using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace api_KIT;

public class ClientKIT
{
    private string _apiKey;

    public ClientKIT(string apiKey)
    {
        _apiKey = apiKey;
    }
    
    public async Task<string?> Request(string urlRequest, string jsonRequestData)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
                HttpResponseMessage response = await client.PostAsync(urlRequest, content);
                    
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }

                Console.WriteLine("Error: " + response.StatusCode);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
            
        return null;
    }

    public async Task<string?> CalculatePrice(string cityArrival, double mass, double volume)
    {
        string urlRequest = "https://capi.tk-kit.com/1.1/order/calculate";
        string cityDerivalCode = "590001200000"; //code Чайковский
        if (cityArrival.Contains(','))
        {
            cityArrival = cityArrival.Split(',')[0];
            Console.WriteLine(cityArrival);
        }
        string? cityArrivalCode = await GetCodeCity(cityArrival);
        string massStr = mass.ToString(CultureInfo.InvariantCulture);
        string volumeStr = volume.ToString(CultureInfo.InvariantCulture);
        
        string jsonData = $"{{\n    \"city_pickup_code\": \"{cityDerivalCode}\",\n    " +
                          $"\"city_delivery_code\": \"{cityArrivalCode}\",\n    \"declared_price\": \"1000\",\n    \"places\": [\n        {{\n            " +
                          $"\"count_place\": \"1\",\n            \"volume\": \"{volumeStr}\",\n            \"weight\": \"{massStr}\"\n        }}\n    ]\n}}";

        string? responce = await Request(urlRequest, jsonData);
   
        string pattern = @"""standart""\s*:\s*\{[^}]*""cost""\s*:\s*(\d+)";
        if (responce != null)
        {
            Match match = Regex.Match(responce, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }
        
        return null;
    }
    public async Task<string?> GetCodeCity(string cityDerival)
    {
        string urlRequest = "https://capi.tk-kit.com/1.1/tdd/search/by-name";
        string jsonRequestData = $"{{\"title\": \"{cityDerival}\"}}";

        string? responce = await Request(urlRequest, jsonRequestData);
        
        string pattern = @"""code""\s*:\s*""([^""]+)""";
        if (responce != null)
        {
            Match match = Regex.Match(responce, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }
        
        return null;
    }
}