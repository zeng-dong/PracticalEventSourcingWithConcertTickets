using Core.Serialization.Newtonsoft;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace Core.Api.Testing;

public abstract class ApiFixture<TStartup> : ApiFixture where TStartup : class
{
    public override TestContext CreateTestContext() =>
        new TestContext<TStartup>(GetConfiguration, SetupServices, SetupWebHostBuilder);
}

public abstract class ApiFixture : IAsyncLifetime
{
    protected readonly TestContext Sut;

    protected HttpClient Client => Sut.Client;

    protected TestServer Server => Sut.Server;

    protected abstract string ApiUrl { get; }

    protected virtual Dictionary<string, string> GetConfiguration(string fixtureName) => new();

    protected virtual Action<IServiceCollection>? SetupServices => null;

    protected virtual Func<IWebHostBuilder, IWebHostBuilder>? SetupWebHostBuilder => null;

    protected ApiFixture()
    {
        Environment.SetEnvironmentVariable("SchemaName", GetType().Name.ToLower());

        Sut = CreateTestContext();
    }

    public virtual TestContext CreateTestContext() => new(GetConfiguration, SetupServices, SetupWebHostBuilder);

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;

    public async Task<HttpResponseMessage> Get(string path = "", int maxNumberOfRetries = 0, int retryIntervalInMs = 1000, Func<HttpResponseMessage, ValueTask<bool>>? check = null)
    {
        HttpResponseMessage queryResponse;
        var retryCount = maxNumberOfRetries;

        var doCheck = check ?? (response => new(response.StatusCode == HttpStatusCode.OK));
        do
        {
            queryResponse = await Client.GetAsync(
                $"{ApiUrl}/{path}"
            );

            if (retryCount == 0 || (await doCheck(queryResponse)))
                break;

            await Task.Delay(retryIntervalInMs);
            retryCount--;
        } while (true);
        return queryResponse;
    }

    public Task<HttpResponseMessage> Post(string path, object request)
    {
        return Client.PostAsync(
            $"{ApiUrl}/{path}",
            request.ToJsonStringContent()
        );
    }

    public Task<HttpResponseMessage> Post(object request)
    {
        return Post(string.Empty, request);
    }

    public Task<HttpResponseMessage> Put(string path, object? request = null)
    {
        return Client.PutAsync(
            $"{ApiUrl}/{path}",
            request != null ?
                request.ToJsonStringContent()
                : new StringContent(string.Empty)
        );
    }

    public Task<HttpResponseMessage> Put(object request)
    {
        return Put(string.Empty, request);
    }
}