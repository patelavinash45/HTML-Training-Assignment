using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc.DataModels;

public partial class AspNetUserRole
{
    [Key]
    public int UserId { get; set; }

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("AspNetUserRoles")]
    public virtual AspNetRole Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AspNetUserRole")]
    public virtual AspNetUser User { get; set; } = null!;
}
