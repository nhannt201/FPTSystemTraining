using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestSession.Models
{
    public class SearchModel
    {

        [Required]
        [DisplayName("Enter username or email address here")]
        public string keyword { get; set; }

        [Required]
        public int type_acc { get; set; }

        //Get info respond after search
        public string username { get; set; }
        public string email { get; set; }

        public string fullname { get; set; }


        public int accID { get; set; }
        public int type_rs { get; set; } //Dung de nhan dang bo loc tai khoan

        public void getInfoAvailable()
        {
            using (var db = new dbFPTSystem())
            {
                if (username != null)
                {
                    var getCenter = db.InfoAccDBs.Where(m => m.accID == accID).FirstOrDefault();
                    var getPer = db.PermitDBs.Where(o => o.perID == getCenter.perID).FirstOrDefault();
                    var getInfo = db.InfoDetailsDBs.Where(u => u.detailsID == getCenter.detailsID).FirstOrDefault();
                    email = getInfo.email;
                    fullname = getInfo.fullname;
                    type_acc = (getPer.type_acc == "trainer") ? 0 : (getPer.type_acc == "staff") ? 1 : -2; //-2 tat la khong nam trong quyen han
                }
                else
                {
                    var getInfo = db.InfoDetailsDBs.Where(u => u.email == email).FirstOrDefault<InfoDetailsDB>();
                    var getCenter = db.InfoAccDBs.Where(m => m.detailsID == getInfo.detailsID).FirstOrDefault<InfoAccDB>();
                    var getPer = db.PermitDBs.Where(o => o.perID == getCenter.perID).FirstOrDefault<PermitDB>();
                    var getAcc = db.AccountDBs.Where(o => o.accID == getCenter.accID).FirstOrDefault<AccountDB>();
                    username = getAcc.username;
                    accID = getAcc.accID;
                    type_acc = (getPer.type_acc == "trainer") ? 0 : (getPer.type_acc == "staff") ? 1 : -2;

                }

            }
        }

        public void getInfoStaff()
        {
            using (var db = new dbFPTSystem())
            {
                if (username != null)
                {
                    var getCenter = db.InfoAccDBs.Where(m => m.accID == accID).FirstOrDefault();
                    var getPer = db.PermitDBs.Where(o => o.perID == getCenter.perID).FirstOrDefault();
                    var getInfo = db.InfoDetailsDBs.Where(u => u.detailsID == getCenter.detailsID).FirstOrDefault();
                    email = getInfo.email;
                    fullname = getInfo.fullname;
                    type_acc = (getPer.type_acc == "trainer") ? 1 : (getPer.type_acc == "trainee") ? 0 : -2; //-2 tat la khong nam trong quyen han
                }
                else
                {
                    var getInfo = db.InfoDetailsDBs.Where(u => u.email == email).FirstOrDefault<InfoDetailsDB>();
                    var getCenter = db.InfoAccDBs.Where(m => m.detailsID == getInfo.detailsID).FirstOrDefault<InfoAccDB>();
                    var getPer = db.PermitDBs.Where(o => o.perID == getCenter.perID).FirstOrDefault<PermitDB>();
                    var getAcc = db.AccountDBs.Where(o => o.accID == getCenter.accID).FirstOrDefault<AccountDB>();
                    username = getAcc.username;
                    accID = getAcc.accID;
                    type_acc = (getPer.type_acc == "trainer") ? 1 : (getPer.type_acc == "trainee") ? 0 : -2;

                }

            }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}