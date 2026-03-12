using SchoolEntities.Domain.Entities;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        CommunityMember student = new Student("Wilcris", "Diaz", 18, "La Victoria", "8295639124", "6th Grade");

        CommunityMember admin = new Administrative("Yanel", "Gonzales", 26, "Azua", "8497712039", "Administrative Manager", 47250);

        CommunityMember teacher = new Teacher("Maicol", "Suarez", 31, "Villa Mella", "8096843391", "Apointed Professor", 39500, new List<string> { "History", "Mathematics" });

        Console.WriteLine($"Name: {student.Name}, Role: {student.GetRole()}");
        Console.WriteLine($"Name: {admin.Name}, Role: {admin.GetRole()}");
        Console.WriteLine($"Name: {teacher.Name}, Role: {teacher.GetRole()}");
    }
}