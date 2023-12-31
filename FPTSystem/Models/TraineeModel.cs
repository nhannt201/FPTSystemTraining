using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace TestSession.Models
{
    public class TraineeModel
    {

        [Required]
        [DisplayName("Enter username or email address here")]
        public string keyword { get; set; }
        public int accID { get; set; }

        [Required]
        [StringLength(100)]
        public string username { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string repassword { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Enter date birthday")]
        public string date_birthday { get; set; }

        [StringLength(200)]
        [DisplayName("Enter work place")]
        public string workplace { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Enter full name")]
        public string fullname { get; set; }

        [StringLength(200)]
        [DisplayName("Enter job")]
        public string job { get; set; }

        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Enter telephone")]
        public string telephone { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Enter e-mail address")]
        public string email { get; set; }

        [StringLength(200)]
        [DisplayName("Enter TOEIC score")]
        public string toeic_score { get; set; }

        [StringLength(200)]
        [DisplayName("Enter External or Internal Type")]
        public string ex_or_in { get; set; }

        [StringLength(200)]
        [DisplayName("Enter main programming language")]
        public string main_pr_lg { get; set; }

        [StringLength(200)]
        [DisplayName("Enter location")]
        public string location { get; set; }

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