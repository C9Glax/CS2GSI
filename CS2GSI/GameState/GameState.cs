using System.Reflection;

namespace CS2GSI.GameState;

public abstract record GameState
{
    public override string ToString()
    {
        string ret = "";
        foreach (FieldInfo field in GetType().GetFields())
        {
            string filler = GetType().GetFields().Last() != field ? "\u251c\u2500" : "\u2514\u2500";
            string filler2 = GetType().GetFields().Last() != field ? "\u2502" : " ";
            if (field.FieldType.BaseType == typeof(GameState))
                ret += $"\b{filler}\u2510{field.Name}\n{field.GetValue(this)?.ToString()?.Replace("\b", $"\b{filler2} ")}";
            else
                ret += $"\b{filler} {field.Name}{new string('.', field.Name.Length > 25 ? 0 : 25-field.Name.Length)}{field.GetValue(this)}\n";
        }
        return ret;
    }
}