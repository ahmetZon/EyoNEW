using System;
using System.Collections.Generic; using System.ComponentModel.DataAnnotations;


    public partial class ServiceConfig : BaseModel
    {
        public ServiceConfig()
        {
            InverseParent = new HashSet<ServiceConfig>();
            Role = new HashSet<Role>();
            ServiceConfigAuth = new HashSet<ServiceConfigAuth>();
        }

        
       
       
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string UrlTarget { get; set; }

        public virtual ServiceConfig Parent { get; set; }
        public virtual ICollection<ServiceConfig> InverseParent { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<ServiceConfigAuth> ServiceConfigAuth { get; set; }
    }

