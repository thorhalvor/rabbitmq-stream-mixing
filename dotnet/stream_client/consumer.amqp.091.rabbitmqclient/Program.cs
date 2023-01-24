// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Starting DotNet Client Consumer");


var hostname = "localhost";
var port = 5672;

var factory = new ConnectionFactory() {HostName = hostname};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (ch, message) =>
{
    Console.WriteLine($"*********************************************");
    Console.WriteLine($"body {Encoding.Default.GetString(message.Body.ToArray())}");

    if (message.BasicProperties != null)
    {
        Console.WriteLine($"Message id: {message.BasicProperties.MessageId}");
        Console.WriteLine($"CorrelationId: {message.BasicProperties.CorrelationId}");
        Console.WriteLine($"UserId: {message.BasicProperties.UserId}");
        Console.WriteLine($"ReplyTo: {message.BasicProperties.ReplyTo}");
        Console.WriteLine($"ContentType: {message.BasicProperties.ContentType}");
        Console.WriteLine($"ContentEncoding: {message.BasicProperties.ContentEncoding}");
        Console.WriteLine($"GroupId: n/a");
        Console.WriteLine($"Subject: n/a");
        Console.WriteLine($"To: n/a");
    }

    if (message.BasicProperties?.Headers != null)
    {
        foreach (var (key, value) in message.BasicProperties?.Headers)
        {
            Console.WriteLine($"Headers: {key} - {value}");
        }
    }
    channel.BasicAck(message.DeliveryTag, false);
    Console.WriteLine($"=============================================");
};
IDictionary<string,object> props = new Dictionary<string, object>();
props.Add("x-stream-offset", "first");
var consumerTag = channel.BasicConsume(init.Constants.Stream, false, "", false, false, props, consumer);

await Task.Delay(5000);