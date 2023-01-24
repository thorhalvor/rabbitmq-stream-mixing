using System.Text;
using RabbitMQ.Client;
using Constants = init.Constants;


Console.WriteLine("Starting DotNet Rabbitmq 0.9.1 Client");

const int numberOfMessages = 5;

var hostname = "localhost";
var port = 5672;

var factory = new RabbitMQ.Client.ConnectionFactory() {HostName = hostname};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

Console.WriteLine($"Sending {numberOfMessages} standard messages - only body");

for (var i = 0; i < numberOfMessages; i++)
{
    channel.BasicPublish(Constants.Exchange, Constants.Stream,false, null,Encoding.UTF8.GetBytes($"dotnet rabbitmq amqp0.9.1 message {i}"));
}

Console.WriteLine($"Sending {numberOfMessages}  messages with properties");

for (var i = 0; i < numberOfMessages; i++)
{
    var props = channel.CreateBasicProperties();
    SetProperties(props, i);
    channel.BasicPublish(Constants.Exchange, Constants.Stream,false, props ,Encoding.UTF8.GetBytes($"dotnet rabbitmq amqp0.9.1 message with props {i}"));
}


Console.WriteLine($"Sending {numberOfMessages}  messages with properties and headers");

for (var i = 0; i < numberOfMessages; i++)
{
    var props = channel.CreateBasicProperties();
    SetProperties(props, i);
    SetHeaders(props);

    channel.BasicPublish(Constants.Exchange, Constants.Stream,false, props ,Encoding.UTF8.GetBytes($"dotnet rabbitmq amqp0.9.1 message with props {i}"));
}

await Task.Delay(2000);

void SetHeaders(IBasicProperties basicProperties)
{
    basicProperties.Headers = new Dictionary<string, object>()
    {
        {"key_string", "Aa Áá Bb Cc Čč Dd Đđ Ee Ff Gg Hh Ii Jj Kk Ll Mm Nn Ŋŋ Oo Pp Rr Ss Šš Tt Ŧŧ Uu Vv Zz Žž"},
        {"key2_int", 1111},
        {"key2_decimal", 10_000_000_000},
        {"key2_datetimestring", DateTime.UtcNow.ToString("o")}
    };
}

void SetProperties(IBasicProperties basicProperties, int i)
{
    basicProperties.MessageId = $"Aa Áá Bb Cc Čč Dd Đđ Ee Ff Gg Hh Ii Jj Kk Ll Mm Nn Ŋŋ Oo Pp Rr Ss Šš Tt Ŧŧ Uu Vv Zz Žž{i}";
    basicProperties.CorrelationId = $"MyCorrelationId{i}";
    basicProperties.ContentType = "text/plan";
    basicProperties.ContentEncoding = "utf8";
    basicProperties.UserId = "guest";
    basicProperties.ReplyTo = "MyReplyTo";
}