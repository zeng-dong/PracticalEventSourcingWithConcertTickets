using Core.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tickets.Api.Tests.Config;

using Tickets.Api.Requests;

namespace Tickets.Api.Tests.Reservations.CreatingTentativeReservation;

public class CreateTentativeReservationFixture : ApiWithEventsFixture<Startup>
{
    protected override string ApiUrl => "/api/Reservations";

    protected override Dictionary<string, string> GetConfiguration(string fixtureName) =>
        TestConfiguration.Get(fixtureName);

    public readonly Guid SeatId = Guid.NewGuid();

    public HttpResponseMessage CommandResponse = default!;

    public override async Task InitializeAsync()
    {
        // send create command
        CommandResponse = await Post(new CreateTentativeReservationRequest { SeatId = SeatId });
    }
}

public class CreateTentativeReservationTests
{
}