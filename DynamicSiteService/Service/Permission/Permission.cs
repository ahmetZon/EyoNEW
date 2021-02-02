using System;
using System.Collections.Generic; using System.ComponentModel.DataAnnotations;


    public partial class Permission : BaseModel
    {
        public Permission()
        {
            ServiceConfigAuth = new HashSet<ServiceConfigAuth>();
        }

       
        
       
       
        [Required()] public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<ServiceConfigAuth> ServiceConfigAuth { get; set; }
    }
