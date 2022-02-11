// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GrpcClient;

var channel = GrpcChannel.ForAddress("https://localhost:7263");
var client = new Greeter.GreeterClient(channel);

var request = new HelloRequest { Name = "Kostya" };
var reply = await client.SayHelloAsync(request);
Console.WriteLine(reply.Message);
Console.ReadLine();