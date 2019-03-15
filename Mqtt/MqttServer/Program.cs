using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttServer
{
    class Program
    {
        static  void Main(string[] args)
        {
            exec();
        }

        static async void exec()
        {
            // Start a MQTT server.
            var mqttServer = new MqttFactory().CreateMqttServer();
            await mqttServer.StartAsync(new MqttServerOptions());
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await mqttServer.StopAsync();
        }
    }
}
