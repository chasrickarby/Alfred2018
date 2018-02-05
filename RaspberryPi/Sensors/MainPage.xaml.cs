using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Restup.Webserver.Rest;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sensors
    {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
        {
        private Rest.Server restServer;
        private IPAddress[] addresses;

        public MainPage()
            {
            this.InitializeComponent();

            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<Rest.SensorController>("bar");

            restServer = new Rest.Server(restRouteHandler);
            restServer.Initialize().Wait();

            IPs_Initialized(this, new System.EventArgs());
            }

        private async void IPs_Initialized(object sender, System.EventArgs e)
            {
            addresses = (await Dns.GetHostAddressesAsync(Dns.GetHostName()))
                .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
            foreach (object address in addresses)
                {
                IPs.Items.Add(address);
                }
            }
        }
    }