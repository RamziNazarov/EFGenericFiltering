using EFGenericFilter.Attributes;

namespace EFGenericFilter.DTOs;

public class RequestParameters
{
    public virtual string Query { get; set; }
    [IgnoreFiltering]
    public int PageNumber { get; set; }
    [IgnoreFiltering]
    public int PageSize { get; set; }
}