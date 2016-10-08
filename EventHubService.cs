using DachauTemp.Windows.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Pumpingcode.Services
{
    public class EventHubService
    {
        private const string sasKeyName = "{SAS Key Name}";
        private const string sasKeyValue = "{SAS Key Value}";
        private const string baseAddress = "{Azure Event Hub base address}";
        private HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Authenticates at the Event Hub and refreshes the authentication token
        /// </summary>
        public void Authenticate()
        {
            TimeSpan fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string expiry = Convert.ToString((int)fromEpochStart.TotalSeconds + 86400); // 86400s = 24h
            string stringToSign = WebUtility.UrlEncode(baseAddress) + "\n" + expiry;

            // Create hash
            MacAlgorithmProvider provider = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var hash = provider.CreateHash(CryptographicBuffer.ConvertStringToBinary(sasKeyValue, BinaryStringEncoding.Utf8));
            hash.Append(CryptographicBuffer.ConvertStringToBinary(stringToSign, BinaryStringEncoding.Utf8));

            // Generate token
            var signature = CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
            string token = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                            WebUtility.UrlEncode(baseAddress), WebUtility.UrlEncode(signature), expiry, sasKeyName);

            // Set HTTP Access token
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
        }
        
        /// <summary>
        /// Sends an object in JSON format to the Event Hub
        /// </summary>
        public async Task SendEventAsync(object datapoint)
        {
            // Convert datapoint to JSON
            var json = JsonConvert.SerializeObject(datapoint);
            var content = new StringContent(json);

            // Send event
            var response = await httpClient.PostAsync(baseAddress + "/dachautemp/messages", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Maybe token expired available. Renew and try again
                RefreshAccessToken();

                // Try again
                content = new StringContent(json);
                response = await httpClient.PostAsync(baseAddress + "/dachautemp/messages", content);
            }

            // Check if everything went fine
            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Could not connect to Azure Event Hub. Access denied.");            
        }
    }
}
