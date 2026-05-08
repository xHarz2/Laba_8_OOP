using RailwayStation.Domain.Interfaces;
 
namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Представляє пасажира залізничної каси.
/// </summary>
public class Passenger : IDisplayable
{
    /// <summary>Унікальний ідентифікатор пасажира (номер документа).</summary>
    public string DocumentId { get; }
 
    /// <summary>Повне ім'я пасажира.</summary>
    public string FullName { get; }
 
    /// <summary>Контактний телефон пасажира.</summary>
    public string Phone { get; }
 
    public Passenger(string documentId, string fullName, string phone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
 
        DocumentId = documentId;
        FullName = fullName;
        Phone = phone;
    }
 
    /// <inheritdoc/>
    public string GetDisplayInfo() =>
        $"Пасажир: {FullName} | Документ: {DocumentId} | Тел: {Phone}";
}
