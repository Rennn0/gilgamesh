using MessagePack;

namespace Hub.Entities;

[MessagePackObject]
public class Client
{
    [Key(0)]
    public virtual int Id
    {
        get; set;
    }
    [Key(1)]
    public virtual string Name
    {
        get; set;
    }
}
