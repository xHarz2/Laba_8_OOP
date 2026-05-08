using RailwayStation.Domain.Interfaces;
 
namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Тип вагона.
/// </summary>
public enum WagonType
{
    /// <summary>Плацкарт.</summary>
    Platzkart,
    /// <summary>Купе.</summary>
    Coupe,
    /// <summary>СВ (м'який).</summary>
    Sv,
    /// <summary>Сидячий.</summary>
    Seat
}
 
/// <summary>
/// Представляє вагон потяга. Є частиною (композиція) об'єкта <see cref="Train"/>.
/// Реалізує <see cref="IBookable"/> та <see cref="IDisplayable"/>.
/// </summary>
public class Wagon : IBookable, IDisplayable
{
    /// <summary>Номер вагона в складі потяга.</summary>
    public int Number { get; }
 
    /// <summary>Тип вагона.</summary>
    public WagonType Type { get; }
 
    /// <summary>Загальна кількість місць у вагоні.</summary>
    public int TotalSeats { get; }
 
    private readonly HashSet<int> _bookedSeats = new();
 
    public Wagon(int number, WagonType type, int totalSeats)
    {
        if (number <= 0) throw new ArgumentException("Номер вагона має бути більше нуля.", nameof(number));
        if (totalSeats <= 0) throw new ArgumentException("Кількість місць має бути більше нуля.", nameof(totalSeats));
 
        Number = number;
        Type = type;
        TotalSeats = totalSeats;
    }
 
    /// <summary>Повертає кількість заброньованих місць.</summary>
    public int BookedSeatsCount => _bookedSeats.Count;
 
    /// <summary>Повертає відсоток заброньованих місць.</summary>
    public double BookingPercentage => TotalSeats == 0 ? 0 : (double)BookedSeatsCount / TotalSeats * 100;
 
    /// <summary>Перевіряє наявність будь-яких броней у вагоні.</summary>
    public bool HasBookings => _bookedSeats.Count > 0;
 
    /// <inheritdoc/>
    public int GetAvailableSeatsCount() => TotalSeats - _bookedSeats.Count;
 
    /// <inheritdoc/>
    public bool IsSeatAvailable(int seatNumber) =>
        seatNumber >= 1 && seatNumber <= TotalSeats && !_bookedSeats.Contains(seatNumber);
 
    /// <summary>Бронює місце з вказаним номером.</summary>
    public void BookSeat(int seatNumber)
    {
        if (!IsSeatAvailable(seatNumber))
            throw new Exceptions.SeatNotAvailableException(seatNumber);
        _bookedSeats.Add(seatNumber);
    }
 
    /// <summary>Скасовує бронювання місця з вказаним номером.</summary>
    public void ReleaseSeat(int seatNumber)
    {
        _bookedSeats.Remove(seatNumber);
    }
 
    /// <summary>Повертає список всіх заброньованих місць.</summary>
    public IReadOnlyCollection<int> GetBookedSeats() => _bookedSeats;
 
    /// <summary>Повертає список всіх вільних місць.</summary>
    public IEnumerable<int> GetAvailableSeats() =>
        Enumerable.Range(1, TotalSeats).Where(s => !_bookedSeats.Contains(s));
 
    /// <inheritdoc/>
    public string GetDisplayInfo() =>
        $"Вагон №{Number} [{Type}] | Місць: {TotalSeats} | " +
        $"Вільно: {GetAvailableSeatsCount()} | Зайнято: {BookedSeatsCount} ({BookingPercentage:F1}%)";
}
