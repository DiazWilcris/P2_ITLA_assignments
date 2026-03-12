using SchoolEntities.Domain.Entities;

public class Alumni : CommunityMember
{
    public int GraduationYear { get; set; }

    public Alumni(string name, string lastname, int age, string address, string phone, int graduationYear)
        : base(name, lastname, age, address, phone)
    {
        GraduationYear = graduationYear;
    }

    public override string GetRole() => "Alumni";
}