using Core.DDD.Abstract;
using Core.DDD.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Core.DDD.Generic;
using Core._3D.Ef;

namespace Apps
{
    public record SomeDomainCreatedEvent(Guid Correlation) : Event;
    public record SomeDomainSpecialNumberChangedEvent(int NewSpecialNumber) : Event;
    public record SomeDomainTitleChangedEvent(string NewTitle) : Event;
    public record SomeDomainSpecialDateChangedEvent(DateTimeOffset NewSpecialDate) : Event;
    public class SomeDomain : RootDomain
    {
        public int SpecialNumber { get; private set; }
        public string? Title { get; private set; }
        public DateTimeOffset? SpecialDate { get; private set; }
        public SomeDomain() : base()
        {
            ApplyNewEvent(new SomeDomainCreatedEvent(Correlation));
        }

        public SomeDomain(IEnumerable<IEvent> events, Guid correlation) : base(events)
        {
            Correlation = correlation;
        }

        public void ChangeSpecialNumber(int num)
        {
            ApplyNewEvent(new SomeDomainSpecialNumberChangedEvent(num));
        }

        public void ChangeTitle(string title)
        {
            ApplyNewEvent(new SomeDomainTitleChangedEvent(title));
        }

        public void On(SomeDomainTitleChangedEvent @event)
        {
            Title = @event.NewTitle;
        }

        public void On(SomeDomainSpecialNumberChangedEvent @event)
        {
            SpecialNumber = @event.NewSpecialNumber;
        }

        public void On(SomeDomainCreatedEvent @event)
        {
            Correlation = @event.Correlation;
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();
            ConfigureServices(serviceDescriptors);

            ServiceProvider services = serviceDescriptors.BuildServiceProvider();
            var store = services.GetRequiredService<IEventStore>();

            await using (DomainSession<SomeDomain> session = new DomainSession<SomeDomain>(new SomeDomain(), store))
            {
                session.Domain.ChangeTitle("opa session");
                Console.WriteLine(session.Domain.Correlation);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Server=localhost;Database=gmesh;User=sa;Password=Budh@000;TrustServerCertificate=True;";
            services.WithEventStoreMsSql(connectionString);
        }
    }
}