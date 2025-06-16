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

        public SomeDomain(IEnumerable<IEvent> events) : base(events)
        { }

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
            var session = services.GetRequiredService<DomainSession<SomeDomain>>();
            await using (session)
            {
                session.AttachDomain(await RootDomain.ReflectAsync<SomeDomain>(store, "BCC64348-17BF-4FC4-A20D-C1C6EF71AEEE"));

                session.Domain.ChangeSpecialNumber(54);

                session.Domain.ChangeTitle("xaxvi");
            }

            SomeDomain someDomain = await RootDomain.ReflectAsync<SomeDomain>(store, "BCC64348-17BF-4FC4-A20D-C1C6EF71AEEE");
            Console.WriteLine(someDomain.SpecialNumber);
            Console.WriteLine(someDomain.Title);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_LOCAL_CONNECTION") ?? throw new ArgumentNullException();
            services.WithEventStoreMsSql(connectionString);

            services.AddTransient(typeof(DomainSession<>));
        }
    }
}