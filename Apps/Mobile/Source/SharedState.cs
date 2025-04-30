namespace Mobile.Source;

public partial class SharedState
{
    private bool m_isMe = Preferences.Default.Get("Username", string.Empty) == "RENNNO";

    public bool IsMe
    {
        get => m_isMe;
        set
        {
            if (m_isMe == value)
                return;
            m_isMe = value;
            OnAuthStateChanged();
        }
    }

    public event Action? AuthStateChanged;

    protected virtual void OnAuthStateChanged()
    {
        AuthStateChanged?.Invoke();
    }

    public void SetIsMe() => IsMe = Preferences.Default.Get("Username", string.Empty) == "RENNNO";
}
