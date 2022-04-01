namespace DamienVDK.Pizza_Mug.IntentHandler;

public sealed class IntentAttribute : Attribute
{
    public IntentAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}