namespace TestSession.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InfoDetailsDB")]
    public partial class InfoDetailsDB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InfoDetailsDB()
        {
            InfoAccDBs = new HashSet<InfoAccDB>();
        }

        [Key]
        public int detailsID { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Enter your date birthday")]
        public string date_birthday { get; set; }
       
        [StringLength(200)]
        [DisplayName("Enter your work place")]
        public string workplace { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Enter your full name")]
        public string fullname { get; set; }

        [StringLength(200)]
        [DisplayName("Enter your job")]
        public string job { get; set; }

        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Enter your telephone")]
        public string telephone { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Enter your e-mail address")]
        public string email { get; set; }

        [StringLength(200)]
        [DisplayName("Enter your TOEIC score")]
        public string toeic_score { get; set; }

        [StringLength(200)]
        [DisplayName("Enter External or Internal Type")]
        public string ex_or_in { get; set; }

        [StringLength(200)]
        [DisplayName("Enter your main programming language")]
        public string main_pr_lg { get; set; }

        [StringLength(200)]
        [DisplayName("Enter your location")]
        public string location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InfoAccDB> InfoAccDBs { get; set; }

      
    }
}
