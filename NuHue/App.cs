using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace NuHue
{
    public partial class App : Application
    {
        //apiKey is used to pass the registered key inside the app.
        public static string apiKey { get; set; } = "";
        public string appKey;
        public static ILocalHueClient client;

        /// <summary>
        /// Try to connect to the bridge and register the application client
        /// </summary>
        public async void BridgeLocation()
        {
            //Locate the bridge, pull the current device info and get the bridge IP
            IBridgeLocator locator = new HttpBridgeLocator();
            var device = DeviceInfo();
            IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
            //Register the client and pull the application key
            client = ClientCheck(bridgeIPs.First()).Result;
        }

        public async Task<LocalHueClient> ClientCheck(string bridgeIP)
        {
            client = new LocalHueClient(bridgeIP);
            try
            {
                client.Initialize(apiKey);
            }
            catch (Exception e)
            {
                var a = e.Message;
                //TODO: Catch exception and prompt user to press the link button and register app with bridge
                appKey = await client.RegisterAsync("NuHue", apiKey);
            }
            return client as LocalHueClient;
        }

        public string DeviceInfo()
        {
            // get the device manufacturer and model name
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            var DeviceManufacturer = eas.SystemManufacturer;
            var DeviceModel = eas.SystemProductName;
            return DeviceModel;
        }

    }
}
