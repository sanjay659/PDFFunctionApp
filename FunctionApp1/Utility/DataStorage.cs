using FunctionApp1.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1.Utility
{
    public static class DataStorage
    {
        public static List<Student> GetAllEmployess() =>
            new List<Student>
            {
                new Student { Name="Mike", LastName="Turner", Age=35, Gender="Male"},
                new Student { Name="Sonja", LastName="Markus", Age=22, Gender="Female"},
                new Student { Name="Luck", LastName="Martins", Age=40, Gender="Male"},
                new Student { Name="Sofia", LastName="Packner", Age=30, Gender="Female"},
                new Student { Name="John", LastName="Doe", Age=45, Gender="Male"}
            };
    }
}
