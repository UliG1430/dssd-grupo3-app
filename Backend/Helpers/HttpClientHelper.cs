using System.Net.Http.Headers;

namespace Backend.Helpers;

public static class HttpClientHelper
{
    private static HttpClient _httpClient = new HttpClient();
    
    public static async Task<string> SendRequest(string url, string method, object body, string token)
    {
        try
        {
            using (_httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var result = new HttpResponseMessage();
                switch (method)
                {
                    case "GET":
                        result = await _httpClient.GetAsync(url);
                    break;
                    case "POST":
                        if (token != null)
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        var json = JsonContent.Create(body);
                        result = await _httpClient.PostAsync(url, json);
                    break;
                }
                var response = await result.Content.ReadAsStringAsync();
                return response;
            }   
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}