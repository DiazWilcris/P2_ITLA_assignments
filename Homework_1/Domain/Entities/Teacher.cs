using SchoolEntities.Domain.Entities;

public class Teacher : Employee
{
    public List<string> Subjects { get; set; }

    public Teacher(string name, string lastname, int age, string address, string phone, string role, int salary, List<string> subjects)
        : base(name, lastname, age, address, phone, role, salary)
    {
        Subjects = subjects;
    }
}