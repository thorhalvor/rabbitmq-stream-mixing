// See https://aka.ms/new-console-template for more information

using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.AMQP;
using RabbitMQ.Stream.Client.Reliable;

Console.WriteLine("Starting DotNet Stream Producer");
const string stream = "mixing";
const int numberOfMessages = 100;
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole();
    builder.AddFilter("RabbitMQ.Stream", LogLevel.Information);
});

var producerLogger = loggerFactory.CreateLogger<Producer>();
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
var count = 0;
var producer = await Producer.Create(new ProducerConfig(system, stream)
{
    ConfirmationHandler = async confirmation =>
    {
        if (Interlocked.Increment(ref count) % 300 == 0)
        {
            Console.WriteLine($"Confirmed {confirmation.PublishingId}");
        }

        await Task.CompletedTask;
    },
}, producerLogger);

Console.WriteLine($"Producer Created for stream {stream} ");

Console.WriteLine($"Sending {numberOfMessages} standard messages - only body");

for (var i = 0; i < numberOfMessages; i++)
{
    await producer.Send(new Message(Encoding.ASCII.GetBytes($"dotnet message {i}")));
}

Console.WriteLine($"Sending {numberOfMessages}  messages with properties");

for (var i = 0; i < numberOfMessages; i++)
{
    await producer.Send(new Message(Encoding.ASCII.GetBytes($"dotnet message {i}"))
    {
        Properties = new Properties()
        {
            MessageId = $"MyMessageId{i}",
            CorrelationId = $"MyCorrelationId{i}",
            ContentType = "text/plain",
            ContentEncoding = "utf-8",
            UserId = Encoding.UTF8.GetBytes("guest"),
            GroupSequence = 9999,
            ReplyToGroupId = "MyReplyToGroupId",
        }
    });
}


Console.WriteLine($"Sending {numberOfMessages}  messages with properties and application properties");

for (var i = 0; i < numberOfMessages; i++)
{
    await producer.Send(new Message(Encoding.ASCII.GetBytes($"dotnet message {i}"))
    {
        Properties = new Properties()
        {
            MessageId = $"MyMessageId{i}",
            CorrelationId = $"MyCorrelationId{i}",
            ContentType = "text/plain",
            ContentEncoding = "utf-8",
            UserId = Encoding.UTF8.GetBytes("guest"),
            GroupSequence = 9999,
            ReplyToGroupId = "MyReplyToGroupId",
        },
        ApplicationProperties = new ApplicationProperties()
        {
            ["key_string"] = "value",
            ["key2_int"] = 1111,
            ["key2_decimal"] = 10_000_000_000,
        }
    });
}

await Task.Delay(2000);