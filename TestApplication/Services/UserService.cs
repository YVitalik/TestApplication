using Newtonsoft.Json;
using TestApplication.Enums;
using TestApplication.Mappers;
using TestApplication.Models;

namespace TestApplication.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var randomUsersTask = GetUsersFromDataSourceAsync("https://randomuser.me/api/?results=500", DataSource.RandomUser);
            var jsonPlaceholderUsersTask = GetUsersFromDataSourceAsync("https://jsonplaceholder.typicode.com/users", DataSource.JsonPlaceholder);
            var dummyJsonUsersTask = GetUsersFromDataSourceAsync("https://dummyjson.com/users", DataSource.DummyJson);
            var reqresUsersTask = GetUsersFromDataSourceAsync("https://reqres.in/api/users", DataSource.Reqres);

            var usersTasks = Task.WhenAll(randomUsersTask, jsonPlaceholderUsersTask, dummyJsonUsersTask, reqresUsersTask);
            await usersTasks;

            List<User> randomUsers = randomUsersTask.Result;
            List<User> jsonPlaceholderUsers = jsonPlaceholderUsersTask.Result;
            List<User> dummyJsonUsers = dummyJsonUsersTask.Result;
            List<User> reqresUsers = reqresUsersTask.Result;

            var allUsers = randomUsers.Concat(jsonPlaceholderUsers).Concat(dummyJsonUsers).Concat(reqresUsers).ToList();
            return allUsers;
        }

        private async Task<List<User>> GetUsersFromDataSourceAsync(string url, DataSource dataSource)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(stringContent);

            if (jsonResponse == null) 
            {
                return new List<User>();
            }

            var result = new List<User>();

            switch (dataSource)
            {
                case DataSource.RandomUser:
                    foreach (var user in jsonResponse.results)
                    {
                        var userToAdd = UserMapper.MapFromRandomUser(user, url);
                        result.Add(userToAdd);
                    }
                    break;

                case DataSource.JsonPlaceholder:
                    foreach (var user in jsonResponse)
                    {
                        var userToAdd = UserMapper.MapFromJsonPlaceholder(user, url);
                        result.Add(userToAdd);
                    }
                    break;

                case DataSource.DummyJson:
                    foreach (var user in jsonResponse.users)
                    {
                        var userToAdd = UserMapper.MapFromDummyJson(user, url);
                        result.Add(userToAdd);
                    }
                    break;

                case DataSource.Reqres:
                    foreach (var user in jsonResponse.data)
                    {
                        var userToAdd = UserMapper.MapFromReqres(user, url);
                        result.Add(userToAdd);
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid data source provided");
            }

            return result;
        }
    }
}
