using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace DONN.LS.DataDispatch
{
    public class Dispatcher
    {
        private static DBHelperSingle.Base dBHelper;
        private static IManagedMqttClient mqttClient;

        private static MqttApplicationMessageBuilder messageBuilder = new MqttApplicationMessageBuilder()
                .WithAtLeastOnceQoS()
                .WithRetainFlag();
        static Dispatcher()
        {


        }
        public static void Init(string mqttHostName, int mqttPort, string userName, string password = null, string dbConnectStr = null)
        {
            try
            {
                dBHelper = DBHelperSingle.Provider.Instance("db", dbConnectStr, DBHelperSingle.Providers.Pgsql);
                var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId("LSService")
                .WithTcpServer(mqttHostName, mqttPort)
                .WithCredentials(userName, password)
                .Build())
                .Build();

                mqttClient = new MqttFactory().CreateManagedMqttClient();
                mqttClient.StartAsync(options);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        public static async void SendToMQTT(DONN.LS.Entities.TempLocations item, string topic = "location")
        {
            var message = messageBuilder.WithTopic(topic).WithPayload(Newtonsoft.Json.JsonConvert.SerializeObject(item)).Build();
            await mqttClient.PublishAsync(message);
        }

        public static async void SendToDB(IEnumerable<DONN.LS.Entities.TempLocations> items)
        {
            await Task.Run(() => dBHelper.UpdateItems(items));
        }
    }
}