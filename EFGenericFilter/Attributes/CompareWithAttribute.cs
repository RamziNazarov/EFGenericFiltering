namespace EFGenericFilter.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CompareWithAttribute : Attribute
{
    public string CompareWith { get; }
    
    public CompareWithAttribute(string compareWith)
    {
        CompareWith = compareWith;
    }
}