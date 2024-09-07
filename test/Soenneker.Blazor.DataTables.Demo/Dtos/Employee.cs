using System;

namespace Soenneker.Blazor.DataTables.Demo.Dtos;

public class Employee
{
    public string? Name { get; set; }

    public string? Position { get; set; }

    public string? Office { get; set; }

    public int Age { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Salary { get; set; }
}