using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class SpecContentValue : BaseModel
{
    public SpecContentValue()
    {
    }

    [ForeignKey("SpecId")]
    [DisplayName("Spec")]
    public int SpecId { get; set; }
    public virtual Spec Spec { get; set; }

    [ForeignKey("ContentPageId")]

    [DisplayName("İçerik ID")]
    public int ContentPageId { get; set; }
    public virtual ContentPage ContentPage { get; set; }

    [DisplayName("Değer")]
    public string ContentValue { get; set; }



    [DisplayName("Dil")]
    public int? LangId { get; set; }
    public virtual Lang Lang { get; set; }


}

