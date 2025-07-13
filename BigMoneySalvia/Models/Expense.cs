using System;
using System.Collections.Generic;

namespace BigMoneySalvia.Models;

public partial class Expense
{
    public int Id { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public double ExpensesAmount { get; set; }

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
