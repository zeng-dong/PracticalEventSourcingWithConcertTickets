namespace Tickets.Tickets
{
    public class Ticket
    {
        public Guid SeatId { get; private set; }
        public string Name { get; private set; }
        public string Number { get; private set; }

        public Ticket(Guid seatId, string number)
        {
            SeatId = seatId;
            Number = number;
        }
    }
}