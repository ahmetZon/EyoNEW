using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class SpecContentType : BaseModel
{
    public SpecContentType()
    {
    }


    [Required]
    [DisplayName("Spec ID")]
    public int SpecId { get; set; }
    public virtual Spec Spec { get; set; }

    [Required]
    [DisplayName("İçerik Tipi")]
    public int ContentTypesId { get; set; }
    public virtual ContentTypes ContentTypes { get; set; }

   

}

