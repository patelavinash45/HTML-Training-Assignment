using System;
using System.Collections.Generic;

namespace HelloDoc.DataModels;

public partial class BusinessType
{
    public int BusinessTypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();
}
