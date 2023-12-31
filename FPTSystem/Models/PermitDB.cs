namespace TestSession.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermitDB")]
    public partial class PermitDB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PermitDB()
        {
            InfoAccDBs = new HashSet<InfoAccDB>();
        }

        [Key]
        public int perID { get; set; }

        [Required]
        [StringLength(50)]
        public string type_acc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InfoAccDB> InfoAccDBs { get; set; }
    }
}
