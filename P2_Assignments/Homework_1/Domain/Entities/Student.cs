using SchoolEntities.Domain.Entities;

public class Student : CommunityMember
{
    public string Grade { get; set; }

    public Student(string name, string lastname, int age, string address, string phone, string grade)
        : base(name, lastname, age, address, phone)
    {
        Grade = grade;
    }

    public override string GetRole() => "Student";
}