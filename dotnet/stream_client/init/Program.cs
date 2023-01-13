// See https://aka.ms/new-console-template for more information

using RabbitMQ.Stream.Client;
const string stream = "mixing";


Console.WriteLine("Initializing..");
var config = new StreamSystemConfig
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/"
};
// Connect to the broker and create the system object
// the entry point for the client.
var system = await StreamSystem.Create(config);
Console.WriteLine($"Delete stream: {stream}");
// Delete the stream
await system.DeleteStream(stream);

Console.WriteLine($"Create Stream: {stream}");
await system.CreateStream(new StreamSpec(stream));

