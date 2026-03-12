namespace SchoolEntities.Domain.Entities
{
    public abstract class CommunityMember
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        protected CommunityMember(string name, string lastname, int age, string address, string phone)
        {
            Name = name;
            Lastname = lastname;
            Age = age;
            Address = address;
            Phone = phone;
        }

        public abstract string GetRole();
    }
}