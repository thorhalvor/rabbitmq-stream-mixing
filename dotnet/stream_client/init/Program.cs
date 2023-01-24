using System.Net;
using init;
using RabbitMQ.Stream.Client;

var hostname = "localhost";
var port = 5552;

Console.WriteLine("Initializing..");
var config = new StreamSystemConfig
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    Endpoints = new List<EndPoint> {new DnsEndPoint(hostname,port)},
    AddressResolver = new AddressResolver(new DnsEndPoint(hostname, port)),
};
// Connect to the broker and create the system object
// the entry point for the client.
var system = await StreamSystem.Create(config);
Console.WriteLine($"Delete stream: {Constants.Stream}");
// Delete the stream

try
{
    await system.DeleteStream(Constants.Stream);
}
catch (Exception e)
{
    // ignore
}

Console.WriteLine($"Create Stream: {Constants.Stream}");
await system.CreateStream(new StreamSpec(Constants.Stream));