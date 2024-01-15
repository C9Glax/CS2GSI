namespace CS2GSI;

public class CS2EventArgs : EventArgs
{
    private readonly object? _value;
    public readonly bool HasValue;

    public CS2EventArgs(object? value = null)
    {
        this._value = value;
        this.HasValue = value is not null;
    }

    public T? ValueAsOrDefault<T>()
    {
        return _value is T val ? val : default; 
    }

    public override string ToString()
    {
        return _value?.ToString() ?? "NoArgs";
    }
}