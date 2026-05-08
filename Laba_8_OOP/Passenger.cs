using RailwayStation.Domain.Interfaces;
 
namespace RailwayStation.Domain.Models;
 
/// <summary>
/// Представляє пасажира залізничної каси.
/// Є нащадком класу <see cref="Person"/> (узагальнення).
/// </summary>
public class Passenger : Person, IDisplayable
{
    /// <summary>Унікальний ідентифікатор пасажира (номер документа).</summary>
    public string DocumentId { get; }
 
    public Passenger(string documentId, string fullName, string phone)
        : base(fullName, phone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        DocumentId = documentId;
    }
 
    /// <inheritdoc/>
    public override string GetDisplayInfo() =>
        $"Пасажир: {FullName} | Документ: {DocumentId} | Тел: {Phone}";
}
