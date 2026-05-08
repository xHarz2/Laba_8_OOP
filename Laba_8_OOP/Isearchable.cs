namespace RailwayStation.Domain.Interfaces;
 
/// <summary>
/// Визначає контракт для об'єктів, що підтримують пошук за ключовим словом.
/// </summary>
public interface ISearchable
{
    /// <summary>Перевіряє, чи відповідає об'єкт вказаному ключовому слову.</summary>
    bool MatchesKeyword(string keyword);
}
