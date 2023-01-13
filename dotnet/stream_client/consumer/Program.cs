// See https://aka.ms/new-console-template for more information

using System.Buffers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.AMQP;
using RabbitMQ.Stream.Client.Reliable;

Console.WriteLine("Starting DotNet Stream Consumer");
const string stream = "mixing";
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole();
    builder.AddFilter("RabbitMQ.Stream", LogLevel.Information);
});

var consumerLogger = loggerFactory.CreateLogger<Consumer>();
var streamLogger = loggerFactory.CreateLogger<StreamSystem>();


var config = new StreamSystemConfig
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/"
};
// Connect to the broker and create the system object
// the entry point for the client.
var system = await StreamSystem.Create(config, streamLogger);
var consumer = await Consumer.Create(new ConsumerConfig(system, stream)
{
    OffsetSpec = new OffsetTypeFirst(),
    MessageHandler = async (source, rawConsumer, messageContext, message) =>
    {
        Console.WriteLine($"*********************************************");
        Console.WriteLine(
            $"body {Encoding.Default.GetString(message.Data.Contents.ToArray())}");

        if (message.Properties != null)
        {
            Console.WriteLine($"Message id: {message.Properties.MessageId}");
            Console.WriteLine($"CorrelationId: {message.Properties.CorrelationId}");
            Console.WriteLine($"ReplyTo: {message.Properties.ReplyTo}");
            Console.WriteLine($"ContentType: {message.Properties.ContentType}");
            Console.WriteLine($"ContentEncoding: {message.Properties.ContentEncoding}");
            Console.WriteLine($"GroupId: {message.Properties.GroupId}");
        }

        if (message.ApplicationProperties != null)
        {
            foreach (var (key, value) in message.ApplicationProperties)
            {
                Console.WriteLine($"ApplicationProperties: {key} - {value}");
            }
        }

        Console.WriteLine($"=============================================");

        await Task.CompletedTask;
    }
}, consumerLogger);

await Task.Delay(2000);