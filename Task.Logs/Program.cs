using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

class Program
{
    private const string QueueName = "order_logs";
    private const string HostName = "localhost";
    private const int Port = 5672;
    private const string UserName = "guest";
    private const string Password = "guest";

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            HostName = HostName,
            Port = Port,
            UserName = UserName,
            Password = Password
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                LogMessageToFile(message);
            };

            channel.BasicConsume(queue: QueueName,
                                 autoAck: true,
                                 consumer: consumer);


            while (true)
            {
                Console.WriteLine("Введите IP-адрес для фильтрации логов (или 'exit' для выхода):");
                var input = Console.ReadLine();

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.WriteLine("Введите начальную дату и время (yyyy-MM-dd HH:mm:ss):");
                var startTimeInput = Console.ReadLine();
                Console.WriteLine("Введите конечную дату и время (yyyy-MM-dd HH:mm:ss):");
                var endTimeInput = Console.ReadLine();

                if (DateTime.TryParse(startTimeInput, out var startTime) &&
                    DateTime.TryParse(endTimeInput, out var endTime))
                {
                    var filteredLogs = FilterLogs(input, startTime, endTime);
                    Console.WriteLine("Отфильтрованные логи:");
                    foreach (var log in filteredLogs)
                    {
                        Console.WriteLine(log);
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат даты и времени.");
                }
            }
        }
    }

    private static void LogMessageToFile(string message)
    {
        string filePath = "logs.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(message);
            }
            // Console.WriteLine($" [x] Лог записан: {message}");
        }
        catch (Exception ex)
        {
           Console.WriteLine($"При добавлении нового поля в файл произошла ошибка: {ex.Message}");
        }
    }

    public static List<string> FilterLogs(string ipAddress, DateTime startTime, DateTime endTime)
    {
        var filteredLogs = new List<string>();
        var logLines = File.ReadAllLines("logs.txt");

        foreach (var line in logLines)
        {
            var logParts = line.Split(' ');
            var logTime = DateTime.Parse(logParts[0] + " " + logParts[1]);
            var logIp = logParts[3]; 

            if (logIp == ipAddress && logTime >= startTime && logTime <= endTime)
            {
                filteredLogs.Add(line);
            }
        }

        return filteredLogs;
    }
}
