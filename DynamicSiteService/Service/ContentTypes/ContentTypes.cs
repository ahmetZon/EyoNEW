using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ContentTypes : BaseModel
{
    public ContentTypes()
    {
        ContentPages = new HashSet<ContentPage>();
        SpecContentTypes = new HashSet<SpecContentType>();
    }
      
    [DisplayName("İçerik Tipi")]
    [Required()]
    public string Name { get; set; }

    [DisplayName("Açıklama")]
    [Required()]
    public string Description { get; set; }

    [DisplayName("İçerikler")]
    public virtual ICollection<ContentPage> ContentPages { get; set; }

    [DisplayName("Spec Types")]
    public virtual ICollection<SpecContentType> SpecContentTypes { get; set; }

}

