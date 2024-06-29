using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models;

public class TripTransport
{
    public int TripTransportId { get; set; }
    
    public Guid TripId { get; set; }
    public virtual Trip Trip { get; set; } = null!;
    
    public byte TransportTypeId { get; set; }
    public virtual TransportType TransportType { get; set; } = null!;

    [MaxLength(10)]
    public string? TransportNumber { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    
    public int? PlaceId { get; set; }
    public virtual Place? Place { get; set; }
    
    public Guid? ExpenseId { get; set; }
    public virtual Expense? Expense { get; set; }

    [MaxLength(255)]
    public string? TicketsUrl { get; set; }
    
    public TransportRole? TransportRole { get; set; }
}

public enum TransportRole : ushort
{
    Arriving = 1,
    Departing = 2,
}