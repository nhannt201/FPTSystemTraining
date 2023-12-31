namespace TestSession.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseDetailsDB")]
    public partial class CourseDetailsDB
    {
        [Key]
        public int couD_ID { get; set; }

        public int? couID { get; set; }

        public int? cateID { get; set; }

        public int? topID { get; set; }

        public virtual CategoryDB CategoryDB { get; set; }

        public virtual CourseDB CourseDB { get; set; }

        public virtual TopicDB TopicDB { get; set; }
    }
}
