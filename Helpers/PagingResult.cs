namespace Transferciniz.API.Helpers;

public class PagingResult<T>
{
    public int TotalCount { get; set; }
    public T Data { get; set; }
}