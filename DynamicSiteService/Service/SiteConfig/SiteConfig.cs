using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public partial class SiteConfig : BaseModel
{


    [Required()] public string Title { get; set; }
    [Required()] public string StartPage { get; set; }
    [Required()] public string StartAction { get; set; }

    [Required()] public string version { get; set; }
    [Required()] public string layoutID { get; set; }
    [Required()] public string layoutUrlBase { get; set; }
    [Required()] public string layoutUrl { get; set; }
    [Required()] public string Logo { get; set; }

    public string Map { get; set; }
    public string DefaultImage { get; set; }
    [Required()] public string BaseUrl { get; set; }
    public string ImageUrl { get; set; }
    [Required()] public string JokerPass { get; set; }

    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }

    public string Adress { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string MailGorunenAd { get; set; }
    public string SmtpHost { get; set; }
    public string SmtpPort { get; set; }
    public string SmtpMail { get; set; }
    public string SmtpMailPass { get; set; }
    public bool? SmtpSSL { get; set; }


    public string Instagram { get; set; }
    public string Twitter { get; set; }
    public string Facebook { get; set; }
    public string Youtube { get; set; }
    public string GooglePlus { get; set; }
    public string Tumblr { get; set; }

    public string HeadScript { get; set; }
    public string HeadStyle { get; set; }
    public string BodyScript { get; set; }
    public string FooterScript { get; set; }
    public string FooterStyle { get; set; }


}

