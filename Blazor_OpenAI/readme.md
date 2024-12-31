Sure! Here's an example of how you can use the Semantic Kernel to fetch and display real-time train timings for Indian Railways using the eRail API.

**Steps:**

1. **Set Up the Environment**:
   - Ensure you have .NET installed and set up a new project.

2. **Install Necessary Packages**:
   - Install the Semantic Kernel and any other required packages using NuGet.

3. **Define the API**:
   - Use the eRail API to fetch train timings. You'll need to sign up for an API key from [eRail](http://api.erail.in/auth/register).

4. **Create Functions**:
   - Define functions in C# to call the eRail API and fetch train timings.

5. **Integrate with Semantic Kernel**:
   - Add these functions to the Semantic Kernel pipeline.

**Example Code**:

```csharp
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class TrainInfo
{
    public string TrainNumber { get; set; }
    public string DepartureTime { get; set; }
    public string ArrivalTime { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
}

public class TrainService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "YourAPIKeyHere";

    public TrainService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [SemanticKernelFunction]
    public async Task<List<TrainInfo>> GetTrainTimingsAsync(string sourceStation, string destinationStation, string date)
    {
        var response = await _httpClient.GetStringAsync($"https://api.erail.in/live-train-status/train/{sourceStation}/{destinationStation}/date/{date}/apikey/{_apiKey}/");
        var trainTimings = JsonConvert.DeserializeObject<List<TrainInfo>>(response);
        return trainTimings;
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var trainService = new TrainService(httpClient);

        var sourceStation = "PUNE";
        var destinationStation = "MUMBAI";
        var date = "30-12-2024";

        var trainTimings = await trainService.GetTrainTimingsAsync(sourceStation, destinationStation, date);

        foreach (var train in trainTimings)
        {
            Console.WriteLine($"Train Number: {train.TrainNumber}, Departure: {train.DepartureTime}, Arrival: {train.ArrivalTime}, Source: {train.Source}, Destination: {train.Destination}");
        }
    }
}
```

In this example:
- The `TrainService` class contains a method `GetTrainTimingsAsync` that fetches train timings from the eRail API.
- The `Program` class demonstrates how to use this service to get and display train timings for a specific route and date.

Replace `"YourAPIKeyHere"` with your actual API key from eRail.

Would you like more details on any specific part of this example?