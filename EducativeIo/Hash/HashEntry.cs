namespace EducativeIo.Hash;

public class HashEntry
{
    public string Key { get; set; }
    public int Value { get; set; }
    public HashEntry? Next { get; set; }

    public HashEntry()
    {
        Key = string.Empty;
        Value = -1;
        Next = null;
    }

    public HashEntry(string key, int value)
    {
        Key = key;
        Value = value;
        Next = null;
    }
}
