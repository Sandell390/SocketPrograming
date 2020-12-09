using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Client
{
    public class MyMessage 
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }


    class Program
    {
       
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press Enter to Connect");
            Console.ReadKey();

            var endpoint = new IPEndPoint(IPAddress.Loopback, 9000);

            //var channel = new ClientChannel<JsonMessageProtocol, JObject>();
            var channel = new ClientChannel<XmlMessageProtocol, XDocument>();
            channel.OnMessage(OnMessage);

            await channel.ConnectAsync(endpoint).ConfigureAwait(false);

            var MyMessage = new MyMessage
            {
                IntProperty = 404,
                StringProperty = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(MyMessage);


            await channel.SendAsync(MyMessage).ConfigureAwait(false);



            Console.ReadKey();

        }

        static Task OnMessage(JObject jObject) 
        {
            Console.WriteLine("Received Jobjet Message");
            Print(Convert(jObject));
            return Task.CompletedTask;
        }
        
        static Task OnMessage(XDocument xDocument) 
        {
            Console.WriteLine("Received Xdoc Message");
            Print(Convert(xDocument));
            return Task.CompletedTask;
        }

        static MyMessage Convert(JObject jObject) 
            => jObject.ToObject(typeof(MyMessage)) as MyMessage;
        static MyMessage Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(MyMessage)).Deserialize(new StringReader(xmlDocument.ToString())) as MyMessage;
        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProperty = {m.IntProperty}, MyMessage.StringProperty = {m.StringProperty}");
    }
}
