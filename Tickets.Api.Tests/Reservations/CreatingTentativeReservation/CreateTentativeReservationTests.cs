using Core.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tickets.Api.Tests.Config;

using Tickets.Api.Requests;
using Xunit;
using FluentAssertions;
using System.Net;
using Core.Api.Testing;

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

public class CreateTentativeReservationTests : IClassFixture<CreateTentativeReservationFixture>
{
    private readonly CreateTentativeReservationFixture fixture;

    public CreateTentativeReservationTests(CreateTentativeReservationFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    [Trait("Category", "Acceptance")]
    public async Task CreateCommand_ShouldReturn_CreatedStatus_With_ReservationId()
    {
        var commandResponse = fixture.CommandResponse;
        commandResponse.EnsureSuccessStatusCode();
        commandResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // get created record id
        var createdId = await commandResponse.GetResultFromJson<Guid>();
        createdId.Should().NotBeEmpty();
    }
}