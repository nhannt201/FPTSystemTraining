using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestSession.Models
{
    public class NewCourse
    {
        public int type { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Enter name of topic, course, or categoty")]
        public string name { get; set; }

        [StringLength(500)]
        [DisplayName("Enter description of topic, course, or categoty")]
        public string description { get; set; }

        public int id { get; set; }
    }
}