namespace TestAppSmartWay.Domain.Entities;

public class DepartmentEntity
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Phone { get; private set; }
    
    private DepartmentEntity() {}

    public DepartmentEntity(string name, string phone)
    {
        Name = name;
        Phone = phone;
    }
    
    public void UpdateId(int id)
    {
        Id = id;
    }
}