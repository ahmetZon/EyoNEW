using System;
using System.Collections.Generic; using System.ComponentModel.DataAnnotations;


    public partial class ServiceConfigAuth : BaseModel
    {
       
       
        [Required()] public int ServiceConfigId { get; set; }
        public int? UsersId { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }
        public bool? IsCreate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsUpdate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsList { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
        public virtual ServiceConfig ServiceConfig { get; set; }
        public virtual User Users { get; set; }
    }

