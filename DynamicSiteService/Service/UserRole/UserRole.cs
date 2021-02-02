using System;
using System.Collections.Generic; using System.ComponentModel.DataAnnotations;


    public partial class UserRole : BaseModel
    {
       
        
       
       
        public int? RoleId { get; set; }
        public int? UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }

