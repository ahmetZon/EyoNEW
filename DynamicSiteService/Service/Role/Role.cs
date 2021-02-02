using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


    public partial class Role : BaseModel
    {
        public Role()
        {
            ParentRoles = new HashSet<Role>();
            Permissions = new HashSet<Permission>();
            ServiceConfigAuth = new HashSet<ServiceConfigAuth>();
            UserRoles = new HashSet<UserRole>();
        }








        [DisplayName("Üst Rol")]
        public int? RoleParentId { get; set; }
        [DisplayName("Ad")]
        [Required]
        public string Name { get; set; }

        public virtual Role RoleParent { get; set; }
        public virtual ICollection<Role> ParentRoles { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<ServiceConfigAuth> ServiceConfigAuth { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
