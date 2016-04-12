using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Q42.HueApi;
using Q42.HueApi.Models;
using Q42.HueApi.Converters;
using Q42.HueApi.WinRT;
using Q42.HueApi.Interfaces;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace NuHue
{
    public partial class App : Application
    {
        //apiKey is used to pass the registered key inside the app.
        public static string apiKey { get; set; }
        public static IBridgeLocator locator = new HttpBridgeLocator();
        public string appKey;
        public static ILocalHueClient client;
        public static Bridge bridge;
        public static List<Light> lights;
        public const bool onState = true;
        public const bool offState = false;
        public const string apiFileName = "apiKey.json";
        public static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public static StorageFile apiKeyFile;

        private async Task SettingsData()
        {
            try
            {
                apiKey = await ReadData();
            }
            catch
            {
                WriteData();
            }
            finally
            {
                BridgeLocation();
            }

        }

        private async Task<string> ReadData()
        {
            var fileText = "";
            apiKeyFile = await storageFolder.GetFileAsync(apiFileName);
            fileText = await FileIO.ReadTextAsync(apiKeyFile);
            if(string.IsNullOrEmpty(fileText))
            {
                throw new NullReferenceException("File text is null or file does not exist");
            }
            return fileText;
        }
        private async Task WriteData()
        {
            await storageFolder.CreateFileAsync(apiFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(apiKeyFile, apiKey);
        }

        /// <summary>
        /// Try to connect to the bridge and register the application client
        /// </summary>
        private async void BridgeLocation()
        {
            //Locate the bridge, pull the current device info and get the bridge IP
            var device = DeviceInfo();
            IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
            //Register the client and pull the application key
            client = ClientCheck(bridgeIPs.First()).Result;
            var a = GetBridgeInfo();
        }
        /// <summary>
        /// Uses Eas Client Device Information to Get the Device Model
        /// </summary>
        /// <returns>Device Model as String</returns>
        public string DeviceInfo()
        {
            // get the device manufacturer and model name
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            var DeviceModel = eas.SystemProductName;
            return DeviceModel;
        }
        /// <summary>
        /// Trys to initialize the Local Hue client, will ask user to press button/register when failure occurs
        /// </summary>
        /// <param name="bridgeIP">IP address sent from Bridge Location</param>
        /// <returns>Returns Task of LocalHueClient asynchronously</returns>
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
        
        public Bridge GetBridgeInfo()
        {
          try
            {
                bridge = client.GetBridgeAsync().Result;
                lights = bridge.Lights.ToList<Light>();
            }
            catch(Exception e)
            {
                var a = e.Message;
            }


            return bridge;
        }


    }
}
