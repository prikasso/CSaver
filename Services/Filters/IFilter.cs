namespace CSaver.Services.Filters;

public interface IFilter<T>
{
    List<T> filter(List<T> objectList);
}