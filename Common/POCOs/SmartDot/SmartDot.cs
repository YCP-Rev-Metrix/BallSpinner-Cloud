namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SmartDot: Poco
{
    public SmartDot() { }

    public SmartDot(string? name, string? macaddress)
    {
        Name = name;
        MacAddress = macaddress;
    }

    public string? Name { get; set; }
    public string? MacAddress { get; set; }
}