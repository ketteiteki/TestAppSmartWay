using TestAppSmartWay.Domain.Enum;

namespace TestAppSmartWay.Domain.Entities;

public class PassportEntity
{
    public int Id { get; private set; }
    
    public PassportType Type { get; private set; }
    
    public string Number { get; private set; }
    
    private PassportEntity() {}

    public PassportEntity(PassportType type, string number)
    {
        Type = type;
        Number = number;
    }

    public void UpdateId(int id)
    {
        Id = id;
    }
}