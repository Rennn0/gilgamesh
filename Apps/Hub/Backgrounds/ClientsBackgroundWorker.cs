using Hub.Api;
using Hub.Database;
using Hub.Entities;
using Hub.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Backgrounds;

public class ClientsBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _mFactory;
    private readonly IHubContext<ClientsHub> _mHub;
    private HashSet<int> _mClientIds;

    public ClientsBackgroundWorker(IServiceScopeFactory factory, IHubContext<ClientsHub> hub)
    {
        _mFactory = factory;
        _mHub = hub;
        _mClientIds = new HashSet<int>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _mFactory.CreateScope();
            ApplicationContext db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            List<Client> clients = await db.Clients.ToListAsync(stoppingToken);

            HashSet<int> current = clients.Select(c => c.Id).ToHashSet();
            IEnumerable<int> added = current.Except(_mClientIds);
            IEnumerable<int> removed = _mClientIds.Except(current);

            foreach (int id in added)
            {
                _mClientIds.Add(id);
                await _mHub.Clients.All.SendAsync("ClientAdded", id, stoppingToken);

                await ServerEventsController.PublishAsync($"Client {id} added");
            }

            foreach (int id in removed)
            {
                await _mHub.Clients.All.SendAsync("ClientRemoved", id, stoppingToken);
                await ServerEventsController.PublishAsync($"Client {id} removed");
            }

            _mClientIds = current;
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
