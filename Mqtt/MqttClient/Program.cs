using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                exec();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static async void exec()
        {
            Console.WriteLine("Enter ClientID");
            string ClientID = Console.ReadLine();

            Console.WriteLine("Chat with:");
            string Friend = Console.ReadLine();

            IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();

            try
            {
                MqttClientOptionsBuilderTlsParameters TlsOptions = new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = false
                };

                var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString("N"))
                .WithTcpServer("localhost")
                .WithTls(TlsOptions);

                var moptions = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(options)
                    .Build();

                await mqttClient.StartAsync(moptions);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            mqttClient.ApplicationMessageReceived += (s, e) =>
                        {
                            //Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                            Console.WriteLine($"{e.ApplicationMessage.Topic}: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                           
                            //Console.WriteLine($"+ Payload = ");
                            //Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                            //Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                            Console.WriteLine();
                        };

            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic($"chat/{Friend}").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            };

            while (true)
            {
                string msg = Console.ReadLine();
                var message = new MqttApplicationMessageBuilder()
                .WithTopic($"chat/{ClientID}")
                .WithPayload(msg)
                .Build();

                await mqttClient.PublishAsync(message);
            }
        }
    }

    public class ProductDetection
    {
        public float[] bbox { get; set; }
        public List<int> top_classes { get; set; }
        public List<float> top_probs { get; set; }
        public List<string> top_names { get; set; }
    }
}
