namespace SchoolEntities.Domain.Entities
{
    public abstract class Employee : CommunityMember
    {
        public string Role { get; set; } 
        public int Salary { get; set; }

        protected Employee(string name, string lastname, int age, string address, string phone, string role, int salary)
            : base(name, lastname, age, address, phone)
        {
            Role = role;
            Salary = salary;
        }

        public override string GetRole()
        {
            return Role;
        }
    }
}