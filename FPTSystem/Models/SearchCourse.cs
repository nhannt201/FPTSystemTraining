using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestSession.Models
{
    public class SearchCourse
    {
        public string keyword { get; set; }

        public int? id { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        [Required]
        [StringLength(500)]
        public string desc { get; set; }

        public int type_find { get; set; }

        public List<SearchCourse> listrs { get; set; }
    }
}