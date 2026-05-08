namespace RailwayStation.Domain.Interfaces;
 
/// <summary>
/// Визначає контракт для об'єктів, що підтримують бронювання місць.
/// </summary>
public interface IBookable
{
    /// <summary>Повертає кількість доступних місць.</summary>
    int GetAvailableSeatsCount();
 
    /// <summary>Перевіряє, чи доступне місце з вказаним номером.</summary>
    bool IsSeatAvailable(int seatNumber);
}
