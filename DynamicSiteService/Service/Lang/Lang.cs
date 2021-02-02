using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


public partial class Lang : BaseModel
{
    public Lang()
    {
        ContentPage = new HashSet<ContentPage>();
    }


    [DisplayName("Dil Adı")]
    [Required]
    public string Name { get; set; }

    [DisplayName("Dil Kodu")]
    [Required]
    public string Code { get; set; }

    [DisplayName("Varsayılan Dil")]
    public bool IsDefault { get; set; }

    public virtual ICollection<ContentPage> ContentPage { get; set; }
}

