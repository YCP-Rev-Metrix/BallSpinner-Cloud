namespace Common.POCOs;

/// <summary>
/// Defines a POCO representing a datetime object
/// </summary>
public class DateTimePoco : POCO
{
    /// <summary>
    /// Creates a new instance of <see cref="DateTimePoco"/> representing the current UTC date & time
    /// </summary>
    public static DateTimePoco UTCNow => new(DateTime.UtcNow);

    public DateTime DateTime { get; set; }

    public DateTimePoco(DateTime dateTime) => DateTime = dateTime;
}
