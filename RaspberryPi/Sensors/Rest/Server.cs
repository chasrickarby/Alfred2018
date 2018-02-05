using System.Diagnostics;
using System.Threading.Tasks;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;

namespace Sensors.Rest
    {
    public class Server
        {
        RestRouteHandler restRouteHandler = new RestRouteHandler();
        HttpServer httpServer;

        public Server(RestRouteHandler restRouteHandler)
            {
            this.restRouteHandler = restRouteHandler;
            }

        ~Server()
            {
            httpServer.StopServer();
            }

        public async Task Initialize()
            {
            var configuration = new HttpServerConfiguration()
              .ListenOnPort(8800)
              .RegisterRoute("api", restRouteHandler)
              .EnableCors();

            httpServer = new HttpServer(configuration);
            await httpServer.StartServerAsync();
            // now make sure the app won't stop after this (eg use a BackgroundTaskDeferral)
            
            Debug.WriteLine("REST server initialization complete!");
            }
        }
    }