namespace Core.Rabbit.Options
{
    public struct ChannelCreationOptions
    {
        public ChannelCreationOptions(
            uint prefetchsize = 0,
            ushort prefetchcount = 5,
            bool global = false,
            bool durable = false,
            bool exclusive = false,
            bool autoDelete = false,
            bool autoAck = false)
        {
            Prefetchsize = prefetchsize;
            Prefetchcount = prefetchcount;
            Global = global;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            AutoAck = autoAck;
        }
        public readonly uint Prefetchsize;
        public readonly ushort Prefetchcount;
        public readonly bool Global;
        public readonly bool Durable;
        public readonly bool Exclusive;
        public readonly bool AutoDelete;
        public readonly bool AutoAck;
    }
}