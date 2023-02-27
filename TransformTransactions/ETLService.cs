using RabbitMQ.Client;
using System.Text;
using TransformTransactions.Extractors;
using TransformTransactions.Models.Validators;
using TransformTransactions.Models.ConcreteTransactions;
using System.Text.RegularExpressions;

namespace TransformTransactions
{
    public class ETLService : BackgroundService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger<ETLService> _logger;

        public ETLService(IHostApplicationLifetime applicationLifetime, ILogger<ETLService> logger)
        {
            _applicationLifetime = applicationLifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "file-processing-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            int workerCount = 4;
            for (int i = 0; i < workerCount; i++)
            {
                Task.Run(() =>
                {
                    BasicGetResult? message = null;
                    IDataExtractor dataExtractor = new FileDataExtractor();
                    IValidator validator = new CityValidator();
                    IDataTransformer dataTransformer = new JsonDataTransformer();
                    IDataLoader dataLoader = new DataLoader();
                    while (!_applicationLifetime.ApplicationStopping.IsCancellationRequested)
                    {
                        try
                        {
                            message = channel.BasicGet(queue: "file-processing-queue", autoAck: false);
                            if (message != null)
                            {
                                var filePath = Encoding.UTF8.GetString(message.Body.ToArray());
                                if (dataExtractor is FileDataExtractor)
                                {
                                    ((FileDataExtractor)dataExtractor).SetFilePath(filePath);
                                }
                                List<string> data = dataExtractor.ExtractData();

                                var pattern = @"(""[^""]*\""|[^"", ]+)";
                                List<City> transformedData = from line in data.AsParallel()
                                                             let values = Regex.Matches(line, pattern)
                                                                          .Cast<Match>()
                                                                          .Select(m => m.Value.Trim())
                                                                          .ToList()
                                                             where validator.IsValid(values)
                                                             select dataTransformer.Transform(values);

                                dataLoader.LoadData(transformedData);
                                channel.BasicAck(deliveryTag: message.DeliveryTag, multiple: false);
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception occurred: {ex.Message}");

                            if (message != null)
                            {
                                channel.BasicNack(deliveryTag: message.DeliveryTag, multiple: false, requeue: false);
                            }
                        }   
                    }

                    channel.Close();
                });
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}