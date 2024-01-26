using TestAppSmartWay.Domain.Enum;

namespace TestAppSmartWay.Domain.Entities;

public class PassportEntity
{
    public int Id { get; private set; }
    
    public PassportType Type { get; private set; }
    
    public string Number { get; private set; }
    
    private PassportEntity() {}

    public static PassportEntity Create(PassportType type, string number)
    {
        return new PassportEntity
        {
            Type = type,
            Number = number
        };
    }

    public void UpdateId(int id)
    {
        Id = id;
    }
}