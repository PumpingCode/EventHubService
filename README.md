# EventHubServie class to manually connect to an Azure Event Hub
Azure Event Hubs provide a neat way to collect loads of data that is pushed against them and holds it until its processing. Microsoft offers an SDK to work with Event Hubs for most platforms including .NET, of course. Unfortunately, the provided NuGet package does not work on all .NET platforms, so when targeting UWP for example, you have to implement the Event Hub connection by yourself, which can be a little bit tricky. Here is how to manually connect and send data to an Azure Event Hub.

## Get the connection information
Once the Event Hub has been created, open the Service Bus section in the [Azure Portal](https://manage.windowsazure.com/) and navigate to your Event Hub. Select the *Configure* tab and scroll down to the *Shared access policies* section. Make sure, you created a policy that has the permission to send data to the Event Hub and note its *Name* and *Primary Key*. You need both later.

Besides, we need the base address of the Event Hub, that we can find at the *Dashboard* tab. The base address is everything **before** the first slash. In my case it's `https://dachautemp-ns.servicebus.windows.net`.

![GitHub Logo](http://pumpingco.de/wp-content/uploads/2016/10/Screen-Shot-2016-10-08-at-17.05.55.png)

## Details
Read the details in the [article at the blog](http://pumpingco.de/manually-connect-and-send-data-to-an-azure-event-hub/).
