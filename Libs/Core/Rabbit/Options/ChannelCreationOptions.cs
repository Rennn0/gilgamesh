namespace Core.Rabbit.Options
{
    public struct ChannelCreationOptions
    {
        public readonly uint Prefetchsize;
        public readonly ushort Prefetchcount;
        public readonly bool Global;
        public readonly bool Durable;
        public readonly bool Exclusive;
        public readonly bool AutoDelete;
        public readonly bool AutoAck;
    }
}