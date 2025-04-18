using Hub.Api;
using Hub.Database;
using Hub.Entities;
using Hub.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Backgrounds;

public class ClientsBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly IHubContext<ClientsHub> _hub;
    private HashSet<int> _clientIds;

    public ClientsBackgroundWorker(IServiceScopeFactory factory, IHubContext<ClientsHub> hub)
    {
        _factory = factory;
        _hub = hub;
        _clientIds = new HashSet<int>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _factory.CreateScope();
            ApplicationContext db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            List<Client> clients = await db.Clients.ToListAsync(stoppingToken);

            HashSet<int> current = clients.Select(c => c.Id).ToHashSet();
            IEnumerable<int> added = current.Except(_clientIds);
            IEnumerable<int> removed = _clientIds.Except(current);

            foreach (int id in added)
            {
                _clientIds.Add(id);
                await _hub.Clients.All.SendAsync("ClientAdded", id, stoppingToken);


                await ServerEventsController.Publish($"Client {id} added");
            }

            foreach (int id in removed)
            {
                await _hub.Clients.All.SendAsync("ClientRemoved", id, stoppingToken);
                await ServerEventsController.Publish($"Client {id} removed");
            }

            _clientIds = current;
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
