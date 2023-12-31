using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSession.Models;
namespace TestSession.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["userPermit"] != null)
            {
                string typeAcc = Session["userPermit"].ToString();
                if (typeAcc == "admin") //Chuyen den trang search user khi da login
                {
                    return RedirectToAction("SearchAccount", "Account");
                }
                if (typeAcc == "staff") //Chuyen den trang search user khi da login
                {
                    return RedirectToAction("SearchTrain", "Account");
                }

            }
            return View();
        }


        [HttpPost]
        public ActionResult Index(AccountDB account) //Login account
        {
            if(account != null)
            {
                if (account.username !=null && account.password !=null)
                {

              
                    using (var db = new dbFPTSystem())
                    {
                        //Check login
                       string passMD5 = account.MD5Pass();
                        var findUser = db.AccountDBs.Where(n=>n.username == account.username).FirstOrDefault<AccountDB>();
                        //Session["userID"] = findUser; //Lay Userfull sau do lay accID sau khi login
                        var user = db.AccountDBs.Where(u => u.username == account.username).Where(p => p.password == passMD5).Count();
                        if (user == 1)
                        {
                            Session["userInfo"] = findUser;
                            //Get type acc
                            var findType = db.InfoAccDBs.Where(n => n.accID == findUser.accID).FirstOrDefault<InfoAccDB>();
                            var type = db.PermitDBs.Find(findType.accID);
                            Session["userPermit"] = type.type_acc;
                            return RedirectToAction("Index");
                        } else
                        {
                            ViewBag.Success = "Account or password is incorrect, please try again!!";
                        }
                    }
                }
                else
                {
                    ViewBag.Success = "Account or password cannot be left blank!";
                }
            }

            return View();
        }

       

    }
}