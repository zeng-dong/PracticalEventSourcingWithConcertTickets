using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Aggregates;
using Tickets.Reservations.CreatingTentativeReservation;
using Tickets.Reservations.NumberGeneration;

namespace Tickets.Reservations;

public class Reservation : Aggregate
{
    private IReservationNumberGenerator numberGenerator;

    public Guid SeatId { get; private set; }

    public string Number { get; private set; } = default!;

    public ReservationStatus Status { get; private set; }

    // for serialization
    public Reservation() { }

    public Reservation(Guid id, IReservationNumberGenerator numberGenerator, Guid seatId)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (seatId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(seatId));

        var reservationNumber = numberGenerator.Next();

        var @event = TentativeReservationCreated.Create(
            id,
            seatId,
            reservationNumber
        );

        Enqueue(@event);
        Apply(@event);
    }

    public static Reservation CreateTentative(
        Guid id,
        IReservationNumberGenerator numberGenerator,
        Guid seatId)
    {
        return new Reservation(id, numberGenerator, seatId);
    }

    public void Apply(TentativeReservationCreated @event)
    {
        Id = @event.ReservationId;
        SeatId = @event.SeatId;
        Number = @event.Number;
        Status = ReservationStatus.Tentative;
        Version++;
    }
}