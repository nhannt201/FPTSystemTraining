using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace TestSession.Models
{
    public class NewAccount
    {
        [Required]
        [StringLength(100)]
        [DisplayName("Enter username")]
        public string username { get; set; }

        public bool checkUser()
        {
            using (var db = new dbFPTSystem())
            {
                var checkUser = db.AccountDBs.Where(n => n.username == username).Count();
                if (checkUser == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [Required]
        [StringLength(200)]
        [Display(Name = "Enter password")]
        public string password { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Enter re-password")]
        public string repassword { get; set; }

        public bool checkTwoPass()
        {
            if (password != null && repassword !=null)
            {
                if (password.Equals(repassword)) return true;
                else return false;
            } else
            {
                return false;
            }
           
        }

        public int type_acc { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Enter full name")]
        public string fullname { get; set; }

        [Required]
        [StringLength(200)]
        [DataType("email")]
        [DisplayName("Enter email address")]
        public string email { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Enter date birthday")]
        public string date_birthday { get; set; }

        [StringLength(200)]
        [DisplayName("Enter work place")]
        public string workplace { get; set; }

        [StringLength(200)]
        [DisplayName("Enter job")]
        public string job { get; set; }

        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Enter telephone")]
        public string telephone { get; set; }

        [StringLength(200)]
        [DisplayName("Enter TOEIC score")]
        public string toeic_score { get; set; }

        [StringLength(200)]
        [DisplayName("Enter External or Internal Type")]
        public string ex_or_in { get; set; }

        [StringLength(200)]
        [DisplayName("Enter main programming language")]
        public string main_pr_lg { get; set; }

        public List<CourseDB> courseDB { get; set; }

        public int? couID { get; set; }
        public bool checkEmail()
        {
            using (var db = new dbFPTSystem())
            {
                var checkMail = db.InfoDetailsDBs.Where(n => n.email == email).Count();
                if (checkMail == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string MD5Pass()
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
  
}