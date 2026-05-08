namespace RailwayStation.Domain.Exceptions;
 
/// <summary>
/// Виникає, коли запитуване місце вже заброньоване або недоступне.
/// </summary>
public class SeatNotAvailableException : Exception
{
    /// <summary>Номер місця, що спричинило виняток.</summary>
    public int SeatNumber { get; }
 
    public SeatNotAvailableException(int seatNumber)
        : base($"Місце №{seatNumber} недоступне або вже заброньоване.")
    {
        SeatNumber = seatNumber;
    }
}
 
/// <summary>
/// Виникає, коли вагон не може бути видалений через наявність броней.
/// </summary>
public class WagonHasBookingsException : Exception
{
    /// <summary>Номер вагона, що спричинив виняток.</summary>
    public int WagonNumber { get; }
 
    public WagonHasBookingsException(int wagonNumber)
        : base($"Вагон №{wagonNumber} не може бути видалений: у ньому є активні бронювання.")
    {
        WagonNumber = wagonNumber;
    }
}
 
/// <summary>
/// Виникає, коли об'єкт із вказаним ідентифікатором не знайдено.
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, string id)
        : base($"{entityName} з ідентифікатором '{id}' не знайдено.")
    {
    }
}
 
/// <summary>
/// Виникає при спробі додати дублікат.
/// </summary>
public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string entityName, string id)
        : base($"{entityName} з ідентифікатором '{id}' вже існує.")
    {
    }
}
