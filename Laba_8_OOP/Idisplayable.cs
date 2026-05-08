namespace RailwayStation.Domain.Interfaces;
 
/// <summary>
/// Визначає контракт для об'єктів, що підтримують виведення інформації.
/// </summary>
public interface IDisplayable
{
    /// <summary>Повертає рядкове представлення об'єкта для відображення.</summary>
    string GetDisplayInfo();
}
