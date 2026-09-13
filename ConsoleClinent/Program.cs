


using ConsoleClinet.Services;
using ConsoleClinet.UI;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5021/") 
};


IEventApiClient apiClient = new EventApiClient(httpClient);
var menu = new EventConsoleMenu(apiClient);

await menu.RunAsync();