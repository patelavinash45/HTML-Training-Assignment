﻿using System;
using System.Collections.Generic;

namespace HelloDoc.DataModels;

public partial class PhysicianLocation
{
    public int LocationId { get; set; }

    public int PhysicianId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? PhysicianName { get; set; }

    public string? Address { get; set; }
}
