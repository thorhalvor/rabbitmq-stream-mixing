// See https://aka.ms/new-console-template for more information
const string exchange = "exchangemixing";
const string stream = "mixing";

var hostname = "localhost";


Console.WriteLine("Initializing..");
var factory = new RabbitMQ.Client.ConnectionFactory() {HostName = hostname};
using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();
Console.WriteLine($"Create Exchange: {exchange}");
channel.ExchangeDeclare(exchange, "topic",true, false, null);
Console.WriteLine($"Create stream: {stream}");
channel.QueueDeclare(stream, true, false, false, new Dictionary<string, object>
{
    { "x-queue-type", "stream" },
    {"x-queue-leader-locator", "least-leaders"}
});
Console.WriteLine($"Binding stream to exchange: {exchange}->{stream}->{stream}");
channel.QueueBind(stream,exchange,stream, null);