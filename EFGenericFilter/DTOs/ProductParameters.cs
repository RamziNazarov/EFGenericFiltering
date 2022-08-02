using EFGenericFilter.Attributes;
using EFGenericFilter.Entities;
using EFGenericFilter.Enums;

namespace EFGenericFilter.DTOs;

public class ProductParameters : RequestParameters
{
    [CompareWith(nameof(Product.Name))]
    [OperationType(Operations.Contains)]
    public override string Query { get; set; }
    [OperationType(Operations.Equal)]
    public DateTimeOffset? CreatedAt { get; set; }
}