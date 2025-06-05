using System.Text;
using Core.Rabbit;

async Task ConsumerStartTask()
{
    try
    {
        RabbitBasicDirectConsumer rbdc = new RabbitBasicDirectConsumer(
            "C1",
            "rbdc",
            "10.45.82.13",
            "guest",
            "guest",
            8208
        );
        await rbdc.StartListeningAsync();

        rbdc.AttachCallback(
            (sender, @event) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(Encoding.UTF8.GetString(@event.Body.Span));
                Console.ResetColor();
                return Task.CompletedTask;
            }
        );

        rbdc.AttachCallback(
            async (sender, @event) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Encoding.UTF8.GetString(@event.Body.Span));
                Console.ResetColor();
                await rbdc.AcknowledgeAsync(@event.DeliveryTag);
            }
        );

        await Task.Delay(Timeout.Infinite);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

async Task PublisherStartTask()
{
    try
    {
        var rbdc = new RabbitBasicDirectPublisher(
            "P1",
            "rbdc",
            "10.45.82.13",
            "guest",
            "guest",
            port: 8208
        );

        Timer timer = new Timer(
            async void (_) =>
            {
                try
                {
                    await rbdc.PublishAsync(DateTimeOffset.Now.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            },
            null,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5)
        );

        await Task.Delay(Timeout.Infinite);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

async Task PublisherStartTask2()
{
    try
    {
        await using RabbitBasicDirectPublisher rbdc = new RabbitBasicDirectPublisher(
            "P2",
            "rbdc",
            "10.45.82.13",
            "guest",
            "guest",
            port: 8208
        );

        Timer timer = new Timer(
            async void (_) =>
            {
                try
                {
                    await rbdc.PublishAsync(DateTimeOffset.Now.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            },
            null,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(1)
        );

        await Task.Delay(Timeout.Infinite);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

Task consumerTask = Task.Run(ConsumerStartTask);
Task publisherStartTask = Task.Run(PublisherStartTask);
Task publisherStartTask2 = Task.Run(PublisherStartTask2);
await Task.WhenAll([consumerTask, publisherStartTask, publisherStartTask2]);
