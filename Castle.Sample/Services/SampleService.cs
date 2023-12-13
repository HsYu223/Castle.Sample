using Castle.Sample.Attributes;

namespace Castle.Sample.Services;

public class SampleService : ISampleService
{
    [ServicesLogging("SampleService GetAsync")]
    public async Task<string> GetAsync(string a)
    {
        return a;
    }
}