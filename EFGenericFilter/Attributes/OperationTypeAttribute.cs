using EFGenericFilter.Enums;

namespace EFGenericFilter.Attributes;

[AttributeUsage(AttributeTargets.Property)] 
public class OperationTypeAttribute : Attribute
{
    public Operations Operation { get; }

    public OperationTypeAttribute(Operations operation)
    {
        Operation = operation;
    }
}