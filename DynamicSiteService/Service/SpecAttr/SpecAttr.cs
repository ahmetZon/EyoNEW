using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class SpecAttr : BaseModel
{
    public SpecAttr()
    {
    }

    [ForeignKey("SpecId")]
    [Required]
    [DisplayName("Spec")]
    public int SpecId { get; set; }
    public virtual Spec Spec { get; set; }

    [Required]
    [DisplayName("Değer")]
    public string AttrValue { get; set; }

    [DisplayName("Orjinal Id")]
    public int? OrjId { get; set; }
    public virtual Spec Orj { get; set; }

    [DisplayName("Dil")]
    [Required()] public int LangId { get; set; }
    public virtual Lang Lang { get; set; }


}

