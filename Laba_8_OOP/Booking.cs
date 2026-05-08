using RailwayStation.Domain.Interfaces;
 
namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Статус бронювання.
/// </summary>
public enum BookingStatus
{
    /// <summary>Активне бронювання.</summary>
    Active,
    /// <summary>Скасоване бронювання.</summary>
    Cancelled
}
 
/// <summary>
/// Представляє бронювання квитка. Пов'язане з <see cref="Passenger"/> (агрегація),
/// <see cref="Train"/> та <see cref="Wagon"/> (асоціація).
/// Реалізує <see cref="IDisplayable"/>.
/// </summary>
public class Booking : IDisplayable
{
    /// <summary>Унікальний ідентифікатор бронювання.</summary>
    public Guid Id { get; }
 
    /// <summary>Пасажир, що здійснив бронювання.</summary>
    public Passenger Passenger { get; }
 
    /// <summary>Номер потяга.</summary>
    public string TrainNumber { get; }
 
    /// <summary>Номер вагона.</summary>
    public int WagonNumber { get; set; }
 
    /// <summary>Номер місця.</summary>
    public int SeatNumber { get; set; }
 
    /// <summary>Дата та час бронювання.</summary>
    public DateTime BookingDate { get; }
 
    /// <summary>Поточний статус бронювання.</summary>
    public BookingStatus Status { get; private set; }
 
    public Booking(Passenger passenger, string trainNumber, int wagonNumber, int seatNumber)
    {
        ArgumentNullException.ThrowIfNull(passenger);
        ArgumentException.ThrowIfNullOrWhiteSpace(trainNumber);
 
        Id = Guid.NewGuid();
        Passenger = passenger;
        TrainNumber = trainNumber;
        WagonNumber = wagonNumber;
        SeatNumber = seatNumber;
        BookingDate = DateTime.Now;
        Status = BookingStatus.Active;
    }
 
    /// <summary>Скасовує бронювання.</summary>
    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Бронювання вже скасоване.");
        Status = BookingStatus.Cancelled;
    }
 
    /// <summary>Перевіряє, чи є бронювання активним.</summary>
    public bool IsActive => Status == BookingStatus.Active;
 
    /// <inheritdoc/>
    public string GetDisplayInfo() =>
        $"Бронювання {Id.ToString()[..8]} | Потяг №{TrainNumber} | " +
        $"Вагон №{WagonNumber} | Місце №{SeatNumber} | " +
        $"Пасажир: {Passenger.FullName} | " +
        $"Дата: {BookingDate:dd.MM.yyyy HH:mm} | Статус: {Status}";
}
