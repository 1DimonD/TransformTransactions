using TransformTransactions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ETLService>();
    })
    .Build();

await host.RunAsync();
