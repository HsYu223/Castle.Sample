namespace Castle.Sample.Services;

public interface ISampleService
{
    Task<string> GetAsync(string a);
}