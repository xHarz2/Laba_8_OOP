using RailwayStation.Domain.Exceptions;
using RailwayStation.Domain.Models;
 
namespace RailwayStation.Domain.Services;
 
/// <summary>
/// Сервіс управління потягами. Залежить від <see cref="Train"/>.
/// </summary>
public class TrainService
{
    private readonly Dictionary<string, Train> _trains = new();
 
    /// <summary>Додає новий потяг.</summary>
    public void AddTrain(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);
        if (_trains.ContainsKey(train.TrainNumber))
            throw new DuplicateEntityException("Потяг", train.TrainNumber);
        _trains[train.TrainNumber] = train;
    }
 
    /// <summary>Видаляє потяг за номером.</summary>
    public void RemoveTrain(string trainNumber)
    {
        if (!_trains.Remove(trainNumber))
            throw new EntityNotFoundException("Потяг", trainNumber);
    }
 
    /// <summary>Повертає потяг за номером.</summary>
    public Train GetTrain(string trainNumber) =>
        _trains.TryGetValue(trainNumber, out var train)
            ? train
            : throw new EntityNotFoundException("Потяг", trainNumber);
 
    /// <summary>Повертає всі потяги.</summary>
    public IReadOnlyCollection<Train> GetAllTrains() => _trains.Values;
 
    /// <summary>Шукає потяги за ключовим словом.</summary>
    public IEnumerable<Train> SearchTrains(string keyword) =>
        _trains.Values.Where(t => t.MatchesKeyword(keyword));
 
    /// <summary>Додає вагон до потяга.</summary>
    public void AddWagon(string trainNumber, Wagon wagon)
    {
        var train = GetTrain(trainNumber);
        train.AddWagon(wagon);
    }
 
    /// <summary>Видаляє вагон з потяга.</summary>
    public void RemoveWagon(string trainNumber, int wagonNumber)
    {
        var train = GetTrain(trainNumber);
        train.RemoveWagon(wagonNumber);
    }
}
