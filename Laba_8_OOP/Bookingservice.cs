using RailwayStation.Domain.Exceptions;
using RailwayStation.Domain.Models;
 
namespace RailwayStation.Domain.Services;
 
/// <summary>
/// Сервіс управління бронюваннями. Залежить від <see cref="TrainService"/>.
/// </summary>
public class BookingService
{
    private readonly TrainService _trainService;
    private readonly Dictionary<Guid, Booking> _bookings = new();
 
    public BookingService(TrainService trainService)
    {
        ArgumentNullException.ThrowIfNull(trainService);
        _trainService = trainService;
    }
 
    /// <summary>Створює нове бронювання.</summary>
    public Booking CreateBooking(Passenger passenger, string trainNumber, int wagonNumber, int seatNumber)
    {
        ArgumentNullException.ThrowIfNull(passenger);
 
        var train = _trainService.GetTrain(trainNumber);
        var wagon = train.GetWagon(wagonNumber);
 
        wagon.BookSeat(seatNumber);
 
        var booking = new Booking(passenger, trainNumber, wagonNumber, seatNumber);
        _bookings[booking.Id] = booking;
        return booking;
    }
 
    /// <summary>Скасовує бронювання за ідентифікатором.</summary>
    public void CancelBooking(Guid bookingId)
    {
        var booking = GetBooking(bookingId);
        booking.Cancel();
 
        var train = _trainService.GetTrain(booking.TrainNumber);
        var wagon = train.GetWagon(booking.WagonNumber);
        wagon.ReleaseSeat(booking.SeatNumber);
    }
 
    /// <summary>Змінює місце або вагон у бронюванні.</summary>
    public void ModifyBooking(Guid bookingId, int newWagonNumber, int newSeatNumber)
    {
        var booking = GetBooking(bookingId);
        if (!booking.IsActive)
            throw new InvalidOperationException("Неможливо змінити скасоване бронювання.");
 
        var train = _trainService.GetTrain(booking.TrainNumber);
 
        var oldWagon = train.GetWagon(booking.WagonNumber);
        var newWagon = train.GetWagon(newWagonNumber);
 
        newWagon.BookSeat(newSeatNumber);
        oldWagon.ReleaseSeat(booking.SeatNumber);
 
        booking.WagonNumber = newWagonNumber;
        booking.SeatNumber = newSeatNumber;
        
    }
 
    /// <summary>Повертає бронювання за ідентифікатором.</summary>
    public Booking GetBooking(Guid bookingId) =>
        _bookings.TryGetValue(bookingId, out var booking)
            ? booking
            : throw new EntityNotFoundException("Бронювання", bookingId.ToString());
 
    /// <summary>Повертає всі бронювання.</summary>
    public IReadOnlyCollection<Booking> GetAllBookings() => _bookings.Values;
 
    /// <summary>Шукає бронювання за датою.</summary>
    public IEnumerable<Booking> SearchByDate(DateTime date) =>
        _bookings.Values.Where(b => b.BookingDate.Date == date.Date);
 
    /// <summary>Повертає активні бронювання для вказаного вагона.</summary>
    public IEnumerable<Booking> GetActiveBookingsForWagon(string trainNumber, int wagonNumber) =>
        _bookings.Values.Where(b =>
            b.TrainNumber == trainNumber &&
            b.WagonNumber == wagonNumber &&
            b.IsActive);
}
