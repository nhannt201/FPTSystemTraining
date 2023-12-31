using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestSession.Models
{
    public class EditAdminModel
    {
        [Required]
        [StringLength(100)]
        public string username { get; set; }

        [Required]
        [DisplayName("Enter fullname for this account")]
        [StringLength(200)]
        public string fullname { get; set; }

        [Required]
        [DisplayName("Enter email address for this account")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DisplayName("Enter telephone number for this account")]
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string telephone { get; set; }

        [DisplayName("Enter a new password, leave it blank if you do not want to change the password.")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [DisplayName("Re-password above")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string repassword { get; set; }

        public int? type_acc { get; set; }

        public List<CourseDB> courseDB { get; set; }

        public int? couID { get; set; }

        public List<TopicDB> topDB { get; set; }
        public int? topID { get; set; }
    }
}