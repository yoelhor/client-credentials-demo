// Get the Token acquirer factory instance. By default it reads an appsettings.json
// file if it exists in the same folder as the app (make sure that the 
// "Copy to Output Directory" property of the appsettings.json file is "Copy if newer").
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

var tokenAcquirerFactory = TokenAcquirerFactory.GetDefaultInstance();

// Create a downstream API service named 'MyApi' which comes loaded with several
// utility methods to make HTTP calls to the DownstreamApi configurations found
// in the "MyWebApi" section of your appsettings.json file.
tokenAcquirerFactory.Services.AddDownstreamApi("MyWebApi",
    tokenAcquirerFactory.Configuration.GetSection("MyWebApi"));
var sp = tokenAcquirerFactory.Build();

// Extract the downstream API service from the 'tokenAcquirerFactory' service provider.
var _authorizationHeaderProvider = sp.GetRequiredService<IAuthorizationHeaderProvider>();

//var result = await _authorizationHeaderProvider.GetForAppAsync<IEnumerable<string>>("MyWebApi");

// Get an access token to call the MiddleTier Web API (the first API in line)
string accessToken = await _authorizationHeaderProvider.CreateAuthorizationHeaderForAppAsync("api://0f72f472-7c52-4991-94cf-d2a55852b64c/.default");
Console.WriteLine(accessToken);

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", accessToken);
string response = await client.GetStringAsync("https://localhost:44351/api/TodoList");

Console.WriteLine(response);
Console.ReadLine();