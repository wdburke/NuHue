using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Q42.HueApi;
using Q42.HueApi.Interfaces;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NuHue
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LightCommand command = new LightCommand();
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void onButton_Click(object sender, RoutedEventArgs e)
        {
            command.TurnOn();
            App.client.SendCommandAsync(command);
        }

        private void offButton_Click(object sender, RoutedEventArgs e)
        {
            command.TurnOff();
            App.client.SendCommandAsync(command);
            
        }
    }
}
