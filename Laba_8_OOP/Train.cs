using RailwayStation.Domain.Exceptions;
using RailwayStation.Domain.Interfaces;
 
namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Представляє потяг. Містить вагони (композиція).
/// Реалізує <see cref="ISearchable"/> та <see cref="IDisplayable"/>.
/// </summary>
public class Train : ISearchable, IDisplayable
{
    /// <summary>Унікальний номер потяга.</summary>
    public string TrainNumber { get; }
 
    /// <summary>Станція відправлення.</summary>
    public string DepartureStation { get; }
 
    /// <summary>Станція призначення.</summary>
    public string ArrivalStation { get; }
 
    /// <summary>Час відправлення.</summary>
    public DateTime DepartureTime { get; }
 
    /// <summary>Час прибуття.</summary>
    public DateTime ArrivalTime { get; }
 
    private readonly List<Wagon> _wagons = new();
 
    public Train(string trainNumber, string departureStation, string arrivalStation,
                 DateTime departureTime, DateTime arrivalTime)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trainNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(departureStation);
        ArgumentException.ThrowIfNullOrWhiteSpace(arrivalStation);
 
        if (arrivalTime <= departureTime)
            throw new ArgumentException("Час прибуття має бути пізніше часу відправлення.");
 
        TrainNumber = trainNumber;
        DepartureStation = departureStation;
        ArrivalStation = arrivalStation;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
    }
 
    /// <summary>Повертає незмінну колекцію вагонів потяга.</summary>
    public IReadOnlyList<Wagon> Wagons => _wagons.AsReadOnly();
 
    /// <summary>Додає вагон до потяга.</summary>
    public void AddWagon(Wagon wagon)
    {
        ArgumentNullException.ThrowIfNull(wagon);
        if (_wagons.Any(w => w.Number == wagon.Number))
            throw new DuplicateEntityException("Вагон", wagon.Number.ToString());
        _wagons.Add(wagon);
    }
 
    /// <summary>Видаляє вагон з потяга. Забороняє видалення за наявності броней.</summary>
    public void RemoveWagon(int wagonNumber)
    {
        var wagon = _wagons.FirstOrDefault(w => w.Number == wagonNumber)
            ?? throw new EntityNotFoundException("Вагон", wagonNumber.ToString());
 
        if (wagon.HasBookings)
            throw new WagonHasBookingsException(wagonNumber);
 
        _wagons.Remove(wagon);
    }
 
    /// <summary>Повертає вагон за номером.</summary>
    public Wagon GetWagon(int wagonNumber) =>
        _wagons.FirstOrDefault(w => w.Number == wagonNumber)
        ?? throw new EntityNotFoundException("Вагон", wagonNumber.ToString());
 
    /// <inheritdoc/>
    public bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return false;
        var kw = keyword.ToLowerInvariant();
        return TrainNumber.ToLowerInvariant().Contains(kw)
            || DepartureStation.ToLowerInvariant().Contains(kw)
            || ArrivalStation.ToLowerInvariant().Contains(kw);
    }
 
    /// <inheritdoc/>
    public string GetDisplayInfo() =>
        $"Потяг №{TrainNumber} | {DepartureStation} → {ArrivalStation} | " +
        $"Відпр: {DepartureTime:dd.MM.yyyy HH:mm} | Приб: {ArrivalTime:dd.MM.yyyy HH:mm} | " +
        $"Вагонів: {_wagons.Count}";
}
