// See https://aka.ms/new-console-template for more information
using Amqp;
using init;

Console.WriteLine("Starting DotNet AMQP 1.0 Client Consumer");


var hostname = "localhost";
var port = 5672;

var url = $"amqp://{hostname}:{port}/";
var address = $"/amq/queue/{Constants.Stream}";

var connectionFactory = new ConnectionFactory();
var connection = await connectionFactory.CreateAsync(new Address(url));
var session = new Session(connection);

var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromMilliseconds(5000));
var receiverLink = new ReceiverLink(session, $"test-receiver-{Guid.NewGuid()}", address);
while (!cts.IsCancellationRequested)
{
    var message = await receiverLink.ReceiveAsync(TimeSpan.FromMilliseconds(500));
    if (message == null) continue;
    Console.WriteLine($"*********************************************");
    Console.WriteLine($"body {message.Body}");

    if (message.Properties != null)
    {
        Console.WriteLine($"Message id: {message.Properties.MessageId}");
        Console.WriteLine($"CorrelationId: {message.Properties.CorrelationId}");
        Console.WriteLine($"UserId: {message.Properties.UserId}");
        Console.WriteLine($"ReplyTo: {message.Properties.ReplyTo}");
        Console.WriteLine($"ContentType: {message.Properties.ContentType}");
        Console.WriteLine($"ContentEncoding: {message.Properties.ContentEncoding}");
        Console.WriteLine($"GroupId: n/a");
        Console.WriteLine($"Subject: n/a");
        Console.WriteLine($"To: n/a");
    }

    if (message.ApplicationProperties != null)
    {
        foreach (var (key, value) in message.ApplicationProperties.Map)
        {
            Console.WriteLine($"ApplicationProperties: {key} - {value}");
        }
    }

    receiverLink.Release(message);
    Console.WriteLine($"=============================================");
};
