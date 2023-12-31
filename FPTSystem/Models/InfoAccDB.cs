namespace TestSession.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InfoAccDB")]
    public partial class InfoAccDB
    {
        [Key]
        public int infoID { get; set; }

        public int accID { get; set; }

        public int perID { get; set; }

        public int detailsID { get; set; }

        public int? couD_ID { get; set; }

        public virtual AccountDB AccountDB { get; set; }

        public virtual PermitDB PermitDB { get; set; }

        public virtual InfoDetailsDB InfoDetailsDB { get; set; }
    }
}
