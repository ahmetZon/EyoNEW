using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Documents : BaseModel
{
    public Documents()
    {

    }


    public string Types { get; set; }

    public string Name { get; set; }

    public string Link { get; set; }

    public string Guid { get; set; }

    public string Alt { get; set; }

    public string Title { get; set; }

    public string data_class { get; set; }


    public int? DocumentId { get; set; }
    public ContentPage Document { get; set; }


    public int? GalleryId { get; set; }
    public ContentPage Gallery { get; set; }


    public int? ThumbImageId { get; set; }
    public ContentPage ThumbImage { get; set; }


    public int? BannerImageId { get; set; }
    public ContentPage BannerImage { get; set; }


    public int? PictureId { get; set; }
    public ContentPage Picture { get; set; }



}

