using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ContentPage : BaseModel
{
    public ContentPage()
    {
        Documents = new HashSet<Documents>();
        ContentPageChilds = new HashSet<ContentPage>();
        Gallery = new HashSet<Documents>();
        SpecContentValue = new HashSet<SpecContentValue>();
    }

    //1. Sayfa Yapısı
    //[Display(Description ="", Order = 1)]

    [Column(Order = 1)]
    [DisplayName("Başlık")]
    [Required()]
    public string Name { get; set; }

    [Column(Order = 1)]
    [DisplayName("Üst Kategori")]
    public int? ContentPageId { get; set; }

    [Column(Order = 1)]
    [DisplayName("İçerik Tipi")]
    [Required()]
    public int ContentTypesId { get; set; }
    public virtual ContentTypes ContentTypes { get; set; }
    [Column(Order = 1)]
    [DisplayName("Şablon Tipi")]
    [Required()]
    public TemplateType TemplateType { get; set; }


    [Column(Order = 1)]
    [DisplayName("Sayfa Url")]
    [Required()]
    public string Link { get; set; }
   

    [Column(Order = 1)]
    [DisplayName("Dış Url")]
    public string ExternalLink { get; set; }
 
     
    [Column(Order = 3)]
    [DisplayName("Bayi")]
    public bool? IsBayi { get; set; }
    [Column(Order = 3)]
    [DisplayName("Bireysel")]
    public bool? IsBireysel { get; set; }
    [Column(Order = 3)]
    [DisplayName("Endustri")]
    public bool? IsEndustri { get; set; }
    [Column(Order = 3)]
    [DisplayName("Mimar")]
    public bool? IsMimar { get; set; }
     

    [Column(Order = 2)]
    [DisplayName("Ön Görsel")]
    public Documents ThumbImage { get; set; }

    [Column(Order = 2)]
    [DisplayName("Görsel")]
    public Documents Picture { get; set; }

    [Column(Order = 2)]
    [DisplayName("Banner Görsel")]
    public Documents BannerImage { get; set; }

    [Column(Order = 2)]
    [DisplayName("Banner Yazı")]
    public string BannerText { get; set; }

    [Column(Order = 2)]
    [DisplayName("Banner Button Yazı")]
    public string BannerButtonText { get; set; }

    [Column(Order = 2)]
    [DisplayName("Button Yazı")]
    public string ButtonText { get; set; }

    [Column(Order = 2)]
    [DisplayName("Button Link")]
    public string ButtonLink { get; set; }


    //2. Sayfa İçeriği
    [Column(Order = 2)]
    [DataType("text")]
    [DisplayName("Açıklama")]
    public string Description { get; set; }

    [Column(Order = 2)]
    [DataType("text")]
    [DisplayName("Kısa İçerik")]
    public string ContentShort { get; set; }


    [Column(Order = 2)]
    [DataType("text")]
    [DisplayName("İçerik")]
    public string ContentData { get; set; }



    [Column(Order = 2)]
    [DisplayName("Video Link")]
    public string VideoLink { get; set; }
    [Column(Order = 3)]
    [DisplayName("Form Tipi")]
    public int? FormTypeId { get; set; }

    [Column(Order = 3)]
    [DisplayName("Galeri")]
    public bool? IsGallery { get; set; }

    [Column(Order = 3)]
    [DisplayName("Harita")]
    public bool? IsMap { get; set; }


    //3. Sayfa Ayarları

    [Column(Order = 3)]
    [DisplayName("Üst Menü")]
    public bool? IsHeaderMenu { get; set; }

    [Column(Order = 3)]
    [DisplayName("Alt Menü")]
    public bool? IsFooterMenu { get; set; }

    [Column(Order = 3)]
    [DisplayName("Hamburger Menü")]
    public bool? IsHamburgerMenu { get; set; }
    [Column(Order = 3)]
    [DisplayName("Yan Menü")]
    public bool? IsSideMenu { get; set; }

    [Column(Order = 3)]
    [DisplayName("Tıklanabilir")]
    public bool? IsClick { get; set; }



    [Column(Order = 3)]
    [DisplayName("Meta Title")]
    public string MetaTitle { get; set; }

    [Column(Order = 3)]
    [DisplayName("Meta Keywords")]
    public string MetaKeywords { get; set; }

    [Column(Order = 3)]
    [DisplayName("Meta Description")]
    public string MetaDescription { get; set; }

    [Column(Order = 3)]
    [DisplayName("İçerik Sırası")]
    public int? ContentOrderNo { get; set; }


    [Column(Order = 3)]
    [DisplayName("interal")]
    public bool? IsInteral { get; set; }
    [Column(Order = 3)]
    [DisplayName("interax")]
    public bool? IsInterax { get; set; }
    [Column(Order = 3)]
    [DisplayName("interwall")]
    public bool? IsInterwall { get; set; }
    [Column(Order = 3)]
    [DisplayName("intersecure")]
    public bool? IsIntersecure { get; set; }
  

    [Column(Order = 3)]
    [DisplayName("Aktiflik Durumu")]
    public bool? IsActive { get; set; }

    [Column(Order = 3)]
    [DisplayName("Yayına Alma Durumu")]
    public bool? IsPublish { get; set; }





    [DisplayName("Form Tipi")]
    public virtual FormType FormType { get; set; }


 




    [Column(Order = 3)]
    [DisplayName("Galeri")]
    public virtual ICollection<Documents> Gallery { get; set; }

    [Column(Order = 3)]
    [DisplayName("Dökümanlar")]
    public virtual ICollection<Documents> Documents { get; set; }

    [Column(Order = 3)]
    [DisplayName("Alt Liste")]
    public virtual ICollection<ContentPage> ContentPageChilds { get; set; }

    [Column(Order = 3)]
    [DisplayName("Üst Liste")]
    public virtual ContentPage Parent { get; set; }


    [Column(Order = 3)]
    [DisplayName("Orjinal Id")]
    public int? OrjId { get; set; }

    [Column(Order = 3)]
    [DisplayName("Orjinal")]
    public virtual ContentPage Orj { get; set; }

    [Column(Order = 3)]
    [DisplayName("Dil")]
    [Required()] public int LangId { get; set; }
    public virtual Lang Lang { get; set; }



    public virtual ICollection<SpecContentValue> SpecContentValue { get; set; }

}

