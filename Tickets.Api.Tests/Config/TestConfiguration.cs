using System.Collections.Generic;

namespace Tickets.Api.Tests.Config;

public static class TestConfiguration
{
    public static Dictionary<string, string> Get(string fixtureName) =>
        new Dictionary<string, string>
        {
            {
                "EventStore:ConnectionString",
                "PORT = 5432; HOST = localhost; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'ConcertTickets_Test'; PASSWORD = 'tester'; USER ID = 'tester'"
            },
            {"EventStore:WriteModelSchema", $"{fixtureName}Write"},
            {"EventStore:ReadModelSchema", $"{fixtureName}Read"},
            {"EventStore:ShouldRecreateDatabase", "true"}
        };
}