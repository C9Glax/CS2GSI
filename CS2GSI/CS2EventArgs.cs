namespace CS2GSI;

public class CS2EventArgs : EventArgs
{
    public object? Value;

    public CS2EventArgs(object? value = null)
    {
        this.Value = value;
    }

    public T? ValueAsOrDefault<T>()
    {
        return Value is T val ? val : default; 
    }

    public override string ToString()
    {
        return Value?.ToString() ?? "NoArgs";
    }
}