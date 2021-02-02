using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BaseModel : IBaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required()] public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreaDate { get; set; }
    [Required()] public int CreaUser { get; set; }
    public int? ModUser { get; set; }
    public DateTime? ModDate { get; set; }
    public int? OrderNo { get; set; }
    public DateTime? IsDeleted { get; set; }

    public int? IsStatus { get; set; }
    public int? LoginCount { get; set; }


    //public Users Users { get; set; }


}

