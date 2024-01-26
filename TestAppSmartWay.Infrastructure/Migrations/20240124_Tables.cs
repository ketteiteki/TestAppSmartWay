using FluentMigrator;
using FluentMigrator.Postgres;

namespace TestAppSmartWay.Infrastructure.Migrations;

[Migration(20240124)]
public class Tables : Migration
{
    public override void Up()
    {
        //tables
        Create
            .Table("EmployeeEntity")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Surname").AsString(255).NotNullable()
            .WithColumn("Phone").AsString(255).NotNullable()
            .WithColumn("CompanyId").AsInt32().NotNullable()
            .WithColumn("PassportId").AsInt32().NotNullable()
            .WithColumn("DepartmentId").AsInt32().NotNullable();

        Create
            .Table("CompanyEntity")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(255).NotNullable();

        Create
            .Table("PassportEntity")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("Number").AsString(255).NotNullable();
        
        Create
            .Table("DepartmentEntity")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Phone").AsString(255).NotNullable();
        
        // relations
        Create
            .ForeignKey("fk_EmployeeEntity_CompanyId_CompanyEntity_Id")
            .FromTable("EmployeeEntity").ForeignColumn("CompanyId")
            .ToTable("CompanyEntity").PrimaryColumn("Id");
        
        Create
            .ForeignKey("fk_EmployeeEntity_PassportId_PassportEntity_Id")
            .FromTable("EmployeeEntity").ForeignColumn("PassportId")
            .ToTable("PassportEntity").PrimaryColumn("Id");
        
        Create
            .ForeignKey("fk_EmployeeEntity_DepartmentId_DepartmentEntity_Id")
            .FromTable("EmployeeEntity").ForeignColumn("DepartmentId")
            .ToTable("DepartmentEntity").PrimaryColumn("Id");
        
        // unique constraints
        Create.UniqueConstraint("UQ_EmployeeEntity_PassportId").OnTable("EmployeeEntity").Column("PassportId");
    }

    public override void Down()
    {
        Delete.Table("EmployeeEntity");
        Delete.Table("CompanyEntity");
        Delete.Table("PassportEntity");
        Delete.Table("DepartmentEntity");
    }
}