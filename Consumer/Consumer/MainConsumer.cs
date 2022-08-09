using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    public sealed class MainConsumer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var queue = channel.QueueDeclare
            (
               "fila_teste",
                false,
                false,
                false,
                null
            );

            Console.WriteLine($"Coletando do Queue : { queue.QueueName }");

            Console.WriteLine($"Total de mensagens : { queue.MessageCount }");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensagem recebida : { message }");
            };

            channel.BasicConsume(
                 "fila_teste",
                  true,
                  consumer);
        }
    }
}