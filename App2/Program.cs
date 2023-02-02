// consumer

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest",
    DispatchConsumersAsync = true
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare("message", false, false, false, null);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += async (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.Write(message + " saving...");
    
    await SendMessage(args.DeliveryTag);
};

channel.BasicConsume(consumer, "message", false);
Console.ReadKey();

async Task SendMessage(ulong tag)
{
    try
    {
        var randomInt = new Random().Next(1, 3);

        var client = new HttpClient();

        if (randomInt is 1 )
            client.BaseAddress = new Uri("https://www.google.com");
        else
            client.BaseAddress = new Uri("https://localhost");

        await client.GetAsync("/search?client=firefox-b-d&q=c%23");
        Console.WriteLine();

        channel.BasicAck(tag, false);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}