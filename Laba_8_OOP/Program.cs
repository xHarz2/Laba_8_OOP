using System.Text;
using RailwayStation.Domain.Exceptions;
using RailwayStation.Domain.Models;
using RailwayStation.Domain.Services;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding  = Encoding.UTF8;
 
var trainService   = new TrainService();
var bookingService = new BookingService(trainService);
 
while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║        ЗАЛІЗНИЧНА КАСА               ║");
    Console.WriteLine("╠══════════════════════════════════════╣");
    Console.WriteLine("║  1. Управління потягами              ║");
    Console.WriteLine("║  2. Управління вагонами              ║");
    Console.WriteLine("║  3. Управління бронюванням           ║");
    Console.WriteLine("║  4. Пошук                            ║");
    Console.WriteLine("║  0. Вихід                            ║");
    Console.WriteLine("╚══════════════════════════════════════╝");
    Console.Write("\nОберіть пункт: ");
 
    switch (Console.ReadLine()?.Trim())
    {
        case "1": TrainMenu();   break;
        case "2": WagonMenu();   break;
        case "3": BookingMenu(); break;
        case "4": SearchMenu();  break;
        case "0": return;
        default:  ShowMessage("Невірний вибір! Оберіть пункт з меню."); break;
    }
}

void TrainMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║        УПРАВЛІННЯ ПОТЯГАМИ           ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║  1. Переглянути всі потяги           ║");
        Console.WriteLine("║  2. Переглянути дані потяга          ║");
        Console.WriteLine("║  3. Переглянути вагони з % броні     ║");
        Console.WriteLine("║  4. Додати потяг                     ║");
        Console.WriteLine("║  5. Видалити потяг                   ║");
        Console.WriteLine("║  0. Назад                            ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.Write("\nОберіть пункт: ");
 
        switch (Console.ReadLine()?.Trim())
        {
            case "1": ShowAllTrains();         break;
            case "2": ShowTrainDetails();      break;
            case "3": ShowWagonsWithPercent(); break;
            case "4": AddTrain();              break;
            case "5": DeleteTrain();           break;
            case "0": return;
            default:  ShowMessage("Невірний вибір!"); break;
        }
    }
}
 
void ShowAllTrains()
{
    Console.Clear();
    Console.WriteLine("=== УСІ ПОТЯГИ ===\n");
    var trains = trainService.GetAllTrains();
    if (!trains.Any())
    {
        ShowMessage("Список потягів порожній.");
        return;
    }
    foreach (var t in trains)
        Console.WriteLine("  " + t.GetDisplayInfo());
    Pause();
}
 
void ShowTrainDetails()
{
    Console.Clear();
    string? number = ReadString("Введіть номер потяга");
    if (number is null) return;
    try
    {
        var train = trainService.GetTrain(number);
        Console.WriteLine("\n" + train.GetDisplayInfo());
        Console.WriteLine($"\nВагонів: {train.Wagons.Count}");
        foreach (var w in train.Wagons)
            Console.WriteLine("  " + w.GetDisplayInfo());
    }
    catch (EntityNotFoundException ex) { ShowMessage($"{ex.Message}"); return; }
    Pause();
}
 
void ShowWagonsWithPercent()
{
    Console.Clear();
    string? number = ReadString("Введіть номер потяга");
    if (number is null) return;
    try
    {
        var train = trainService.GetTrain(number);
        Console.WriteLine($"\nВагони потяга №{number}:\n");
        foreach (var w in train.Wagons)
            Console.WriteLine($"  Вагон №{w.Number} [{w.Type}] — заброньовано: {w.BookingPercentage:F1}% ({w.BookedSeatsCount}/{w.TotalSeats})");
    }
    catch (EntityNotFoundException ex) { ShowMessage($"{ex.Message}"); return; }
    Pause();
}
 
void AddTrain()
{
    Console.Clear();
    Console.WriteLine("=== ДОДАТИ ПОТЯГ ===\n");
 
    string? number = ReadString("Номер потяга (напр. IC-100)");
    if (number is null) return;
 
    string? from = ReadString("Станція відправлення");
    if (from is null) return;
 
    string? to = ReadString("Станція призначення");
    if (to is null) return;
 
    DateTime? departure = ReadDateTime("Час відправлення (дд.мм.рррр гг:хх)");
    if (departure is null) return;
 
    DateTime? arrival = ReadDateTime("Час прибуття    (дд.мм.рррр гг:хх)");
    if (arrival is null) return;
 
    try
    {
        trainService.AddTrain(new Train(number, from, to, departure.Value, arrival.Value));
        ShowMessage($"Потяг №{number} успішно додано!");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}
 
void DeleteTrain()
{
    Console.Clear();
    string? number = ReadString("Введіть номер потяга для видалення");
    if (number is null) return;
    try
    {
        trainService.RemoveTrain(number);
        ShowMessage($"Потяг №{number} видалено.");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}

void WagonMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║        УПРАВЛІННЯ ВАГОНАМИ           ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║  1. Переглянути місця вагона         ║");
        Console.WriteLine("║  2. Додати вагон до потяга           ║");
        Console.WriteLine("║  3. Видалити вагон з потяга          ║");
        Console.WriteLine("║  0. Назад                            ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.Write("\nОберіть пункт: ");
 
        switch (Console.ReadLine()?.Trim())
        {
            case "1": ShowWagonSeats(); break;
            case "2": AddWagon();       break;
            case "3": DeleteWagon();    break;
            case "0": return;
            default:  ShowMessage("Невірний вибір!"); break;
        }
    }
}
 
void ShowWagonSeats()
{
    Console.Clear();
    string? trainNumber = ReadString("Номер потяга");
    if (trainNumber is null) return;
 
    int? wagonNumber = ReadInt("Номер вагона", 1, 99);
    if (wagonNumber is null) return;
 
    try
    {
        var wagon = trainService.GetTrain(trainNumber).GetWagon(wagonNumber.Value);
        Console.WriteLine($"\nВагон №{wagon.Number} [{wagon.Type}] | Всього місць: {wagon.TotalSeats}\n");
        Console.Write("  Зайняті місця: ");
        Console.WriteLine(wagon.GetBookedSeats().Any()
            ? string.Join(", ", wagon.GetBookedSeats())
            : "немає");
        Console.Write("  Вільні місця:  ");
        var available = wagon.GetAvailableSeats().ToList();
        Console.WriteLine(available.Any() ? string.Join(", ", available) : "немає");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); return; }
    Pause();
}
 
void AddWagon()
{
    Console.Clear();
    Console.WriteLine("=== ДОДАТИ ВАГОН ===\n");
 
    string? trainNumber = ReadString("Номер потяга");
    if (trainNumber is null) return;
 
    int? wagonNumber = ReadInt("Номер вагона", 1, 99);
    if (wagonNumber is null) return;
 
    Console.WriteLine("Тип вагона:");
    Console.WriteLine("  1 — Platzkart (плацкарт)");
    Console.WriteLine("  2 — Coupe     (купе)");
    Console.WriteLine("  3 — Sv        (м'який/СВ)");
    Console.WriteLine("  4 — Seat      (сидячий)");
    Console.Write("Оберіть тип [1-4]: ");
 
    var wagonType = Console.ReadLine()?.Trim() switch
    {
        "1" => (WagonType?)WagonType.Platzkart,
        "2" => WagonType.Coupe,
        "3" => WagonType.Sv,
        "4" => WagonType.Seat,
        _   => null
    };
    if (wagonType is null) { ShowMessage("Невірний тип вагона! Оберіть від 1 до 4."); return; }
 
    int? totalSeats = ReadInt("Кількість місць", 1, 200);
    if (totalSeats is null) return;
 
    try
    {
        trainService.AddWagon(trainNumber, new Wagon(wagonNumber.Value, wagonType.Value, totalSeats.Value));
        ShowMessage($"Вагон №{wagonNumber} додано до потяга №{trainNumber}!");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}
 
void DeleteWagon()
{
    Console.Clear();
    string? trainNumber = ReadString("Номер потяга");
    if (trainNumber is null) return;
 
    int? wagonNumber = ReadInt("Номер вагона", 1, 99);
    if (wagonNumber is null) return;
 
    try
    {
        trainService.RemoveWagon(trainNumber, wagonNumber.Value);
        ShowMessage($"Вагон №{wagonNumber} видалено з потяга №{trainNumber}.");
    }
    catch (WagonHasBookingsException ex) { ShowMessage($"{ex.Message}"); }
    catch (Exception ex)                 { ShowMessage($"Помилка: {ex.Message}"); }
}

void BookingMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║      УПРАВЛІННЯ БРОНЮВАННЯМ          ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║  1. Переглянути всі бронювання       ║");
        Console.WriteLine("║  2. Переглянути бронювання за ID     ║");
        Console.WriteLine("║  3. Додати бронювання                ║");
        Console.WriteLine("║  4. Скасувати бронювання             ║");
        Console.WriteLine("║  5. Змінити бронювання               ║");
        Console.WriteLine("║  0. Назад                            ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.Write("\nОберіть пункт: ");
 
        switch (Console.ReadLine()?.Trim())
        {
            case "1": ShowAllBookings();   break;
            case "2": ShowBookingById();   break;
            case "3": AddBooking();        break;
            case "4": CancelBooking();     break;
            case "5": ModifyBooking();     break;
            case "0": return;
            default:  ShowMessage("Невірний вибір!"); break;
        }
    }
}
 
void ShowAllBookings()
{
    Console.Clear();
    Console.WriteLine("=== УСІ БРОНЮВАННЯ ===\n");
    var bookings = bookingService.GetAllBookings();
    if (!bookings.Any())
    {
        ShowMessage("Список бронювань порожній.");
        return;
    }
    foreach (var b in bookings)
        Console.WriteLine("  " + b.GetDisplayInfo());
    Pause();
}
 
void ShowBookingById()
{
    Console.Clear();
    Guid? id = ReadGuid("Введіть ID бронювання");
    if (id is null) return;
    try
    {
        Console.WriteLine("\n" + bookingService.GetBooking(id.Value).GetDisplayInfo());
    }
    catch (EntityNotFoundException ex) { ShowMessage($" {ex.Message}"); return; }
    Pause();
}
 
void AddBooking()
{
    Console.Clear();
    Console.WriteLine("=== ДОДАТИ БРОНЮВАННЯ ===\n");
 
    string? fullName = ReadString("ПІБ пасажира (напр. Іваненко Олена Миколаївна)");
    if (fullName is null) return;
 
    string? documentId = ReadString("Номер документа (напр. АА123456)");
    if (documentId is null) return;
 
    string? phone = ReadPhone("Телефон (напр. +380501234567)");
    if (phone is null) return;
 
    string? trainNumber = ReadString("Номер потяга");
    if (trainNumber is null) return;
 
    int? wagonNumber = ReadInt("Номер вагона", 1, 99);
    if (wagonNumber is null) return;
 
    int? seatNumber = ReadInt("Номер місця", 1, 200);
    if (seatNumber is null) return;
 
    try
    {
        var passenger = new Passenger(documentId, fullName, phone);
        var booking   = bookingService.CreateBooking(passenger, trainNumber, wagonNumber.Value, seatNumber.Value);
        ShowMessage($"Бронювання створено!\n   ID: {booking.Id}");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}
 
void CancelBooking()
{
    Console.Clear();
    Guid? id = ReadGuid("Введіть ID бронювання");
    if (id is null) return;
    try
    {
        bookingService.CancelBooking(id.Value);
        ShowMessage("Бронювання скасовано.");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}
 
void ModifyBooking()
{
    Console.Clear();
    Guid? id = ReadGuid("Введіть ID бронювання");
    if (id is null) return;
 
    int? wagonNumber = ReadInt("Новий номер вагона", 1, 99);
    if (wagonNumber is null) return;
 
    int? seatNumber = ReadInt("Новий номер місця", 1, 200);
    if (seatNumber is null) return;
 
    try
    {
        bookingService.ModifyBooking(id.Value, wagonNumber.Value, seatNumber.Value);
        ShowMessage("Бронювання змінено.");
    }
    catch (Exception ex) { ShowMessage($"Помилка: {ex.Message}"); }
}

void SearchMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║              ПОШУК                   ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║  1. Пошук потягів за ключовим словом ║");
        Console.WriteLine("║  2. Пошук броні за датою             ║");
        Console.WriteLine("║  0. Назад                            ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.Write("\nОберіть пункт: ");
 
        switch (Console.ReadLine()?.Trim())
        {
            case "1": SearchTrains();       break;
            case "2": SearchBookingsByDate(); break;
            case "0": return;
            default:  ShowMessage("Невірний вибір!"); break;
        }
    }
}
 
void SearchTrains()
{
    Console.Clear();
    string? keyword = ReadString("Введіть ключове слово (номер або назва станції)");
    if (keyword is null) return;
    var results = trainService.SearchTrains(keyword).ToList();
    Console.WriteLine($"\nЗнайдено: {results.Count}\n");
    foreach (var t in results)
        Console.WriteLine("  " + t.GetDisplayInfo());
    Pause();
}
 
void SearchBookingsByDate()
{
    Console.Clear();
    DateTime? date = ReadDateTime("Введіть дату (дд.мм.рррр)");
    if (date is null) return;
    var results = bookingService.SearchByDate(date.Value).ToList();
    Console.WriteLine($"\nБронювань за {date.Value:dd.MM.yyyy}: {results.Count}\n");
    foreach (var b in results)
        Console.WriteLine("  " + b.GetDisplayInfo());
    Pause();
}

/// <summary>Зчитує непорожній рядок. Повертає null після 3 невдалих спроб.</summary>
string? ReadString(string prompt)
{
    for (int attempt = 0; attempt < 3; attempt++)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(value))
            return value;
        Console.WriteLine("Поле не може бути порожнім. Спробуйте ще раз.");
    }
    ShowMessage(" Введення скасовано після 3 невдалих спроб.");
    return null;
}
 
/// <summary>Зчитує ціле число в діапазоні [min, max]. Повертає null після 3 невдалих спроб.</summary>
int? ReadInt(string prompt, int min, int max)
{
    for (int attempt = 0; attempt < 3; attempt++)
    {
        Console.Write($"{prompt} ({min}–{max}): ");
        var input = Console.ReadLine()?.Trim();
        if (int.TryParse(input, out int result) && result >= min && result <= max)
            return result;
        Console.WriteLine($"Введіть ціле число від {min} до {max}.");
    }
    ShowMessage("Введення скасовано після 3 невдалих спроб.");
    return null;
}
 
/// <summary>Зчитує дату. Повертає null після 3 невдалих спроб.</summary>
DateTime? ReadDateTime(string prompt)
{
    for (int attempt = 0; attempt < 3; attempt++)
    {
        Console.Write($"{prompt}: ");
        var input = Console.ReadLine()?.Trim();
        if (DateTime.TryParse(input, out var result))
            return result;
        Console.WriteLine("Невірний формат дати. Приклад: 01.08.2025 08:00");
    }
    ShowMessage("Введення скасовано після 3 невдалих спроб.");
    return null;
}
 
/// <summary>Зчитує GUID. Повертає null після 3 невдалих спроб.</summary>
Guid? ReadGuid(string prompt)
{
    for (int attempt = 0; attempt < 3; attempt++)
    {
        Console.Write($"{prompt}: ");
        var input = Console.ReadLine()?.Trim();
        if (Guid.TryParse(input, out var result))
            return result;
        Console.WriteLine("Невірний формат ID. Скопіюйте ID з бронювання.");
    }
    ShowMessage("Введення скасовано після 3 невдалих спроб.");
    return null;
}
 
/// <summary>Зчитує номер телефону. Повертає null після 3 невдалих спроб.</summary>
string? ReadPhone(string prompt)
{
    for (int attempt = 0; attempt < 3; attempt++)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(value) &&
            System.Text.RegularExpressions.Regex.IsMatch(value, @"^\+?[\d\s\-\(\)]{7,15}$"))
            return value;
        Console.WriteLine("Невірний формат телефону. Приклад: +380501234567");
    }
    ShowMessage("Введення скасовано після 3 невдалих спроб.");
    return null;
}

void ShowMessage(string text)
{
    Console.WriteLine($"\n{text}");
    Pause();
}
 
void Pause()
{
    Console.WriteLine("\nНатисніть будь-яку клавішу...");
    Console.ReadKey();
}
