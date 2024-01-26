namespace TestAppSmartWay.Domain.Entities;

public class CompanyEntity
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }

    private CompanyEntity() {}
    
    public static CompanyEntity Create(string name)
    {
        return new CompanyEntity
        {
            Name = name
        };
    }
}