using Core.DDD.Abstract;
using Core.DDD.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Core.DDD.Generic;
using Core._3D.Ef;
using System.Text;

namespace Apps
{
    public record SomeDomainCreatedEvent(Guid Correlation) : Event;
    public record SomeDomainSpecialNumberChangedEvent(int NewSpecialNumber) : Event;
    public record SomeDomainTitleChangedEvent(string NewTitle) : Event;
    public record SomeDomainSpecialDateChangedEvent(DateTimeOffset NewSpecialDate) : Event;
    public class SomeDomain : RootDomain
    {
        public int SpecialNumber
        {
            get; private set;
        }
        public string? Title
        {
            get; private set;
        }
        public DateTimeOffset? SpecialDate
        {
            get; private set;
        }
        public SomeDomain() : base()
        {
            ApplyNewEvent(new SomeDomainCreatedEvent(Correlation));
        }

        public SomeDomain(IEnumerable<IEvent> events) : base(events)
        {
        }

        public void ChangeSpecialNumber(int num) => ApplyNewEvent(new SomeDomainSpecialNumberChangedEvent(num));

        public void ChangeTitle(string title) => ApplyNewEvent(new SomeDomainTitleChangedEvent(title));

        public void On(SomeDomainTitleChangedEvent @event) => Title = @event.NewTitle;

        public void On(SomeDomainSpecialNumberChangedEvent @event) => SpecialNumber = @event.NewSpecialNumber;

        public void On(SomeDomainCreatedEvent @event) => Correlation = @event.Correlation;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();
            ConfigureServices(serviceDescriptors);

            // Console.WriteLine(BitConverter.ToString([0x6c, 0x75, 0x6b, 0x61]));
            // Console.WriteLine(Encoding.ASCII.GetString([0x6c, 0x75, 0x6b, 0x61]));
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // string connectionString = Environment.GetEnvironmentVariable("SQL_LOCAL_CONNECTION") ?? throw new ArgumentNullException();
            // services.WithEventStoreMsSql(connectionString);

            // services.AddTransient(typeof(DomainSession<>));
        }
    }
}