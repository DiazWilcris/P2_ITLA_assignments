using SchoolEntities.Domain.Entities;

public class Administrative : Employee
{
    public Administrative(string name, string lastname, int age, string address, string phone, string role, int salary)
        : base(name, lastname, age, address, phone, role, salary) { }

}