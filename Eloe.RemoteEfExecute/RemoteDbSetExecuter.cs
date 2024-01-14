using System.Text;
using System.Text.Json;

namespace Eloe.RemoteEfExecute
{
    public class RemoteDbSetExecuter : IRemoteDbSetExecuter
    {
        private HttpClient _httpClient;
        private readonly string _controllerPath;

        public RemoteDbSetExecuter(string url, string controllerPath = "api/Ef")
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _controllerPath = controllerPath;
        }

        public async Task<string?> FirstOrDefault(RemoteDbSetExecuteParam execParams)
        {
            var jsonParam = JsonSerializer.Serialize(execParams);
            var stringContent = new StringContent(jsonParam, Encoding.UTF8, "application/json");
            var httpRes = await _httpClient.PostAsync($"{_controllerPath}/ExecuteFirstOrDefault", stringContent);
            var res = await httpRes.Content.ReadAsStringAsync();
            if (httpRes.IsSuccessStatusCode)
            {
                return res;
            }

            throw new Exception(res);
        }

        public async Task<string?> ToList(RemoteDbSetExecuteParam execParams)
        {
            var jsonParam = JsonSerializer.Serialize(execParams);
            var stringContent = new StringContent(jsonParam, Encoding.UTF8, "application/json");
            var httpRes = await _httpClient.PostAsync($"{_controllerPath}/ExecuteToList", stringContent);
            var res = await httpRes.Content.ReadAsStringAsync();
            if (httpRes.IsSuccessStatusCode)
            {
                return res;
            }

            throw new Exception(res);
        }

        public async Task<int> SaveChanges(RemoteDbSetSaveChangesParams saveChangesParams)
        {
            var jsonParam = JsonSerializer.Serialize(saveChangesParams);
            var stringContent = new StringContent(jsonParam, Encoding.UTF8, "application/json");
            var httpRes = await _httpClient.PostAsync($"{_controllerPath}/SaveChanges", stringContent);
            var res = await httpRes.Content.ReadAsStringAsync();
            if (httpRes.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<int>(res);
            }

            throw new Exception(res);
        }
    }
}
