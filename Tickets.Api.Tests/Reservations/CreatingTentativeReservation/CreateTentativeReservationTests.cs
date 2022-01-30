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
using Tickets.Reservations.CreatingTentativeReservation;
using Tickets.Reservations.GettingReservationById;
using Tickets.Reservations;

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

    [Fact]
    [Trait("Category", "Acceptance")]
    public async Task CreateCommand_ShouldPublish_TentativeReservationCreated()
    {
        var createdReservationId = await fixture.CommandResponse.GetResultFromJson<Guid>();

        fixture.PublishedInternalEventsOfType<TentativeReservationCreated>()
            .Should()
            .HaveCount(1)
            .And.Contain(@event =>
                @event.ReservationId == createdReservationId
                && @event.SeatId == fixture.SeatId
                && !string.IsNullOrEmpty(@event.Number)
            );
    }

    [Fact]
    [Trait("Category", "Acceptance")]
    public async Task CreateCommand_ShouldCreate_ReservationDetailsReadModel()
    {
        var createdReservationId = await fixture.CommandResponse.GetResultFromJson<Guid>();

        // prepare query
        var query = $"{createdReservationId}";

        //send query
        var queryResponse = await fixture.Get(query);
        queryResponse.EnsureSuccessStatusCode();

        var reservationDetails = await queryResponse.GetResultFromJson<ReservationDetails>();
        reservationDetails.Id.Should().Be(createdReservationId);
        reservationDetails.Number.Should().NotBeNull().And.NotBeEmpty();
        reservationDetails.Status.Should().Be(ReservationStatus.Tentative);
    }
}