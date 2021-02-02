using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Spec : BaseModel
{
    public Spec()
    {
        SpecChilds = new HashSet<Spec>();
        SpecAttrs = new HashSet<SpecAttr>();
        SpecContentValue = new HashSet<SpecContentValue>();

    }

    [ForeignKey("ParentId")]
    [DisplayName("Üst Spec")]
    public int? ParentId { get; set; }


    public virtual Spec Parent { get; set; }

    [DisplayName("Ad")]
    public string Name { get; set; }

    [DisplayName("Tip")]
    public SpecType SpecType { get; set; }

    [DisplayName("Orjinal Id")]
    public int? OrjId { get; set; }
    public virtual Spec Orj { get; set; }

    [DisplayName("Tanım")]
    public bool? IsTanim { get; set; }

    [DisplayName("Dil")]
    [Required()] public int LangId { get; set; }
    public virtual Lang Lang { get; set; }

    public virtual ICollection<Spec> SpecChilds { get; set; }
    public virtual ICollection<SpecAttr> SpecAttrs { get; set; }
    public virtual ICollection<SpecContentValue> SpecContentValue { get; set; }


}

