using SqlViewer.DataTransfer.Worker.Consumers;
using SqlViewer.DataTransfer.Worker.Services;
using SqlViewer.Etl.Core.Services;

namespace SqlViewer.DataTransfer.Worker;

public sealed class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddScoped<IInboxService, DataTransferInboxService>();
        builder.Services.AddHostedService<DataTransferCommandConsumer>();

        //builder.Services.AddHostedService<Worker>();

        IHost host = builder.Build();
        host.Run();
    }
}