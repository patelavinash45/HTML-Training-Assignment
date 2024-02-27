using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc.DataModels;

[Table("PhysicianRegion")]
public partial class PhysicianRegion
{
    [Key]
    public int PhysicianRegionId { get; set; }

    public int PhysicianId { get; set; }

    public int RegionId { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("PhysicianRegions")]
    public virtual Physician Physician { get; set; } = null!;

    [ForeignKey("RegionId")]
    [InverseProperty("PhysicianRegions")]
    public virtual Region Region { get; set; } = null!;
}
