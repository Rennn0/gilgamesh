using System.Text;
using Core.Rabbit;

async Task ConsumerStartTask()
{
    try
    {
        RabbitBasicDirectConsumer rbdc = new RabbitBasicDirectConsumer(
            "rbdc",
            "10.45.82.13",
            "guest",
            "guest",
            "consumer",
            8208
        );
        await rbdc.InitializeAsync();

        rbdc.AttachCallback(
            (sender, @event) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(@event.Body.Span));
                return Task.CompletedTask;
            }
        );

        await Task.Delay(Timeout.Infinite);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

var consumerTask = Task.Run(ConsumerStartTask);

await Task.WhenAll([consumerTask]);
