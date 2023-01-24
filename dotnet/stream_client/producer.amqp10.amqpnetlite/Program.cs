using System.Text;
using Amqp;
using Amqp.Framing;
using init;

Console.WriteLine("Starting DotNet AmqpNetLite Amqp 1.0 Client");

const int numberOfMessages = 100;

var hostname = "localhost";
var port = 5672;

var url = $"amqp://{hostname}:{port}/";
var address = $"/exchange/{Constants.Exchange}/{Constants.Stream}";

var connectionFactory = new ConnectionFactory();
var connection = await connectionFactory.CreateAsync(new Address(url));
var session = new Session(connection);

var senderLink = new SenderLink(session, $"test-sender-{Guid.NewGuid()}", address);
Console.WriteLine($"SenderLink Created for stream {address} ");

Console.WriteLine($"Sending {numberOfMessages} standard messages - only body");

for (var i = 0; i < numberOfMessages; i++)
{
    await senderLink.SendAsync(new Message(Encoding.UTF8.GetBytes($"dotnet amqpnetlite amqp1.0 message {i}")));
}

Console.WriteLine($"Sending {numberOfMessages}  messages with properties");

for (var i = 0; i < numberOfMessages; i++)
{
    await senderLink.SendAsync(new Message(Encoding.ASCII.GetBytes($"dotnet amqpnetlite amqp1.0 with props {i}"))
    {
        Properties =  GetProperties(i),
    });
}


Console.WriteLine($"Sending {numberOfMessages}  messages with properties and application properties");

for (var i = 0; i < numberOfMessages; i++)
{
    await senderLink.SendAsync(new Message(Encoding.ASCII.GetBytes($"dotnet message with props and app-props {i}"))
    {
        Properties = GetProperties(i),
        ApplicationProperties = GetApplicationProperties()
    });
}

await Task.Delay(2000);

ApplicationProperties GetApplicationProperties()
{
    return new ApplicationProperties()
    {
        ["key_string"] = "value",
        ["key2_int"] = 1111,
        ["key2_decimal"] = 10_000_000_000,
        ["key2_datetime"] = DateTime.UtcNow,
    };
}

Properties GetProperties(int i)
{
    return new Properties()
    {
        MessageId = $"MyMessageId{i}",
        CorrelationId = $"MyCorrelationId{i}",
        ContentType = "text/plain",
        ContentEncoding = "utf-8",
        UserId = Encoding.UTF8.GetBytes("guest"),
        GroupSequence = 9999,
        ReplyToGroupId = "MyReplyToGroupId",
        Subject = "MySubject",
        To = "MyToValue"
    };
}