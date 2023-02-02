//producer

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare("message", false, false, false, null);

channel.BasicPublish("", "message", null, Encoding.UTF8.GetBytes("message1"));
channel.BasicPublish("", "message", null, Encoding.UTF8.GetBytes("message2"));
channel.BasicPublish("", "message", null, Encoding.UTF8.GetBytes("message3"));

Console.ReadKey();