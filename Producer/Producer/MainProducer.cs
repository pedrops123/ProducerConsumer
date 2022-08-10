using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer
{
    public sealed class MainProducer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                channel.ConfirmSelect();

                var queue = channel.QueueDeclare
                 (
                        queue: "fila_teste",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                     );

                Console.WriteLine($"Iniciando no Queue { queue.QueueName }");

                string message = "Teste de mensagem corpo.";

                var parsedBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "fila_teste",
                    basicProperties: null,  
                    body: parsedBody);

                // TODO : Revisar o porque o channel não aguarda a confirmação para validação.

                channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 1, 50));

                if (queue.MessageCount > 0)
                {
                    Console.WriteLine($"* Mensagem Enviada com Sucesso ! Mensagem : { message }");
                }
                else
                {
                    Console.WriteLine("Nenhuma mensagem enviada !");
                }
            }
        }
    }
}