using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;



public class MailModelCustom
{
    public string Konu { get; set; }
    public string SmtpHost { get; set; }
    public string SmtpPort { get; set; }
    public string SmtpMail { get; set; }
    public string SmtpMailPass { get; set; }
    public bool? SmtpUseDefaultCredentials { get; set; }
    public bool? SmtpSSL { get; set; }
    public string MailGorunenAd { get; set; }

    [DataType(DataType.MultilineText)]
    public string Icerik { get; set; }
    public string[] Alicilar { get; set; }
    public string[] cc { get; set; }



}

