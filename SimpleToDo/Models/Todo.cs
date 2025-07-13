using System;
using System.Collections.Generic;

namespace SimpleToDo.Models;

public partial class Todo
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;
}
