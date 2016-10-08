# EventHubServie class to manually connect to an Azure Event Hub
Azure Event Hubs provide a neat way to collect loads of data that is pushed against them and holds it until its processing. Microsoft offers an SDK to work with Event Hubs for most platforms including .NET, of course. Unfortunately, the provided NuGet package does not work on all .NET platforms, so when targeting UWP for example, you have to implement the Event Hub connection by yourself, which can be a little bit tricky. Here is how to manually connect and send data to an Azure Event Hub.


