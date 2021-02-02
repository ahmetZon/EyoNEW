using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class FormType : BaseModel
{
    public FormType()
    {
        Forms = new HashSet<Forms>();
    }


    [DisplayName("Ad")]
    [Required]
    public string Name { get; set; }


    [DisplayName("Formlar")]
    public virtual ICollection<Forms> Forms { get; set; }


}

