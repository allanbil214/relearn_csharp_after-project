using System;
using System.Collections.Generic;

namespace SimpleToDo.Models;

public partial class Motivation
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;
}
