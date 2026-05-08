namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Базовий клас, що представляє фізичну особу.
/// Є батьківським для класу <see cref="Passenger"/>.
/// </summary>
public abstract class Person
{
    /// <summary>Повне ім'я особи.</summary>
    public string FullName { get; }
 
    /// <summary>Контактний телефон особи.</summary>
    public string Phone { get; }
 
    protected Person(string fullName, string phone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
 
        FullName = fullName;
        Phone = phone;
    }
 
    /// <summary>Повертає рядкове представлення особи.</summary>
    public virtual string GetDisplayInfo() =>
        $"Особа: {FullName} | Тел: {Phone}";
}
