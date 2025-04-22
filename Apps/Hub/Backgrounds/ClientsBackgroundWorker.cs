using Hub.Api;
using Hub.Database;
using Hub.Entities;
using Hub.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Backgrounds;

public class ClientsBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory m_factory;
    private readonly IHubContext<ClientsHub> m_hub;
    private HashSet<int> m_clientIds;

    public ClientsBackgroundWorker(IServiceScopeFactory factory, IHubContext<ClientsHub> hub)
    {
        m_factory = factory;
        m_hub = hub;
        m_clientIds = new HashSet<int>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = m_factory.CreateScope();
            ApplicationContext db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            List<Client> clients = await db.Clients.ToListAsync(stoppingToken);

            HashSet<int> current = clients.Select(c => c.Id).ToHashSet();
            IEnumerable<int> added = current.Except(m_clientIds);
            IEnumerable<int> removed = m_clientIds.Except(current);

            foreach (int id in added)
            {
                m_clientIds.Add(id);
                await m_hub.Clients.All.SendAsync("ClientAdded", id, stoppingToken);

                await ServerEventsController.Publish($"Client {id} added");
            }

            foreach (int id in removed)
            {
                await m_hub.Clients.All.SendAsync("ClientRemoved", id, stoppingToken);
                await ServerEventsController.Publish($"Client {id} removed");
            }

            m_clientIds = current;
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
