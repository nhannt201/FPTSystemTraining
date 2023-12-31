namespace TestSession.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseDB")]
    public partial class CourseDB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseDB()
        {
            CourseDetailsDBs = new HashSet<CourseDetailsDB>();
        }

        [Key]
        public int couID { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseDetailsDB> CourseDetailsDBs { get; set; }
    }
}
