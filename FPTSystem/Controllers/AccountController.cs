using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSession.Models;

namespace TestSession.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult NewAccount(NewAccount account)
        {
            //Dam bao la admin moi duoc reg
            if (Session["userPermit"] != null && Session["userPermit"].Equals("admin"))
            {


                if (account != null)
            {
                if (!account.checkUser())
                {
                    ViewBag.Status = "Username was exist!";
                    return View();
                }
                if (!account.checkTwoPass())
                {
                    ViewBag.Status = "Two passwords do not match!";
                    return View();
                }
                if (!account.checkEmail())
                {
                    ViewBag.Status = "Email was exist!";
                    return View();
                }
                if (account.checkUser() && account.checkTwoPass() && account.checkEmail())
                {
                    //start add database
                    int userID;
                    int PerID;
                    int DetailID;

                    using (var db = new dbFPTSystem())
                    {

                        //add user

                        AccountDB newAcc = new AccountDB();

                        newAcc.username = account.username;
                        newAcc.password = account.MD5Pass();

                        db.AccountDBs.Add(newAcc);

                        db.SaveChanges();

                        userID = newAcc.accID;

                        //add permit

                        string perType = "";

                        if (account.type_acc == 0) perType = "trainer";
                        else perType = "staff";

                        PermitDB newPer = new PermitDB();
                        newPer.type_acc = perType;

                        db.PermitDBs.Add(newPer);
                        db.SaveChanges();

                        PerID = newPer.perID;

                        //add details info
                        InfoDetailsDB newInfo = new InfoDetailsDB();

                        newInfo.date_birthday = "1990-01-01";
                        newInfo.fullname = account.fullname;
                        newInfo.email = account.email;

                        db.InfoDetailsDBs.Add(newInfo);
                         db.SaveChanges();

                        DetailID = newInfo.detailsID;

                        //add InfoAccDB
                        //CouD_ID default null

                        InfoAccDB newAccInfo = new InfoAccDB();

                        newAccInfo.accID = userID;
                        newAccInfo.perID = PerID;
                        newAccInfo.detailsID = DetailID;
                        newAccInfo.couD_ID = null;

                        db.InfoAccDBs.Add(newAccInfo);
                            try
                            {
                                db.SaveChanges();
                            } catch (DbEntityValidationException e)
                            {
                                ViewBag.Status = "Some error:" + e.EntityValidationErrors + ". Please try again later";
                                return View();
                            }
                        



                    }
                        //Reg success
                    }

                    return RedirectToAction("Index");
                }
            }
            return View();



        }


        public ActionResult NewAccount()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("admin"))
            {
                if (Request.QueryString["suggest"] != null)
                {
                    if (IsValidEmail(Request.QueryString["suggest"]))
                    {
                        NewAccount newAcc = new NewAccount()
                        {
                            email = Request.QueryString["suggest"]
                        };
                        return View(newAcc);
                    }
                    else {
                        NewAccount newAcc = new NewAccount()
                        {
                            username = Request.QueryString["suggest"]
                        };
                        return View(newAcc);
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Index()
        {

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            if (Session["userInfo"] != null)
            {
                Session["userInfo"] = null;
            }
            if (Session["userPermit"] != null)
            {
                Session["userPermit"] = null;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout(int zero)
        {
            if (Session["userInfo"] != null)
            {
                if (zero == 0)
                {
                    Session["userInfo"] = null;
                    Session["userPermit"] = null;
                }
            }
            return RedirectToAction("Index");
        }

        //Get account info
        public ActionResult MyAccount()
        {
            if (Session["userInfo"] != null)
            {
                using (var db = new dbFPTSystem())
                {
                    AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                    var myAcc = db.InfoAccDBs.Where(n=>n.accID == _uobj.accID).FirstOrDefault<InfoAccDB>();
                    var myDes = db.InfoDetailsDBs.Where(n => n.detailsID == myAcc.detailsID).FirstOrDefault<InfoDetailsDB>();

                    return View(myDes);
                }
            }
            else {
                return RedirectToAction("Index", "Home");
            }
           
        }

        [HttpPost] //Update
        public ActionResult UpdateInfo(InfoDetailsDB info)
        {
            if (Session["userInfo"] != null)
            {
                //return View();
                if (info != null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                        var myAcc = db.InfoAccDBs.Where(n => n.accID == _uobj.accID).FirstOrDefault<InfoAccDB>();
                        var myDes = db.InfoDetailsDBs.Where(n => n.detailsID == myAcc.detailsID).FirstOrDefault<InfoDetailsDB>();
                        
                        myDes.date_birthday = info.date_birthday;
                        myDes.ex_or_in = info.ex_or_in;
                        myDes.fullname = info.fullname;
                        myDes.job = info.job;
                        myDes.location = info.location;
                        myDes.main_pr_lg = info.main_pr_lg;
                        myDes.telephone = info.telephone;
                        myDes.toeic_score = info.toeic_score;
                        myDes.workplace = info.workplace;

                        db.SaveChanges();
                        return RedirectToAction("MyAccount", new { u = 0 });
                    }
                } 
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost] //ChangePassword
        public ActionResult ChangePassword(string password_change, string password_change_re)
        {
            if (Session["userInfo"] != null)
            {
                if (password_change == password_change_re)
                {
                    //CHange pass
                    using (var db = new dbFPTSystem())
                    {
                        //Check login
                        AccountDB repass = Session["userInfo"] as TestSession.Models.AccountDB;
                        repass.password = password_change;
                        string passMD5 = repass.MD5Pass();
                        var findUser = db.AccountDBs.Where(n => n.username == repass.username).FirstOrDefault<AccountDB>();
                        findUser.password = passMD5;
                        db.SaveChanges();
                      
                        return RedirectToAction("MyAccount", new { u = 1});
                    }
                    } else
                {
                   // ViewBag.Status = "The two changed passwords do not match!";
                    return RedirectToAction("MyAccount", new { u = 2 });
                }
            }
            return RedirectToAction("Index", "Home");
        }
                //Manage for Admin

        public ActionResult ManageAccount()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("admin"))
            {
                if (Request.QueryString["user"] != null)
                {
                    //Neu tai khoan edit la chinh minh thi redict den myacoount
                    AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                    if (_uobj.username == Request.QueryString["user"])
                    {
                        return RedirectToAction("MyAccount");
                    }
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"].Trim();
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                         //   int typeac = (getPer.type_acc == "trainer") ? 0 : 1;
                            EditAdminModel editnew = new EditAdminModel()
                            {
                                username = findAcc.username,
                                fullname = getInfo.fullname,
                                email = getInfo.email,
                                telephone = getInfo.telephone,
                                type_acc = (getPer.type_acc == "trainer") ? 0 : 1
                            };
                            return View(editnew);
                        } else
                        {
                            return RedirectToAction("SearchAccount");
                        }


                    }
                       
                }
                else
                {
                    return RedirectToAction("SearchAccount");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ManageAccount(EditAdminModel edit)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("admin"))
            {
                if (edit != null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"];
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                            //Start check Information
                            if (edit.fullname.Length < 5 || edit.fullname == null)
                            {
                                ViewBag.Status = "Fullname too short or invalid!";
                                return View(edit);
                            }

                            if (IsValidEmail(edit.email))
                            {
                                if (edit.email == getInfo.email)
                                {
                                    //Email not change
                                }
                                else {
                                    var checkMailNew = db.InfoDetailsDBs.Where(n => n.email == edit.email).Count();
                                    if (checkMailNew > 0)
                                    {
                                        ViewBag.Status = "This email is already in use for another account!";
                                        return View(edit);
                                    }
                                }
                            } else
                            {
                                ViewBag.Status = "Incorrect email format!";
                                return View(edit);
                            }

                            if (edit.password != edit.repassword)
                            {
                                ViewBag.Status = "Two passwords do not match!";
                                return View(edit);
                            }

                            //Start update information
                            if (edit.fullname != getInfo.fullname)
                                getInfo.fullname = edit.fullname;
                            if (edit.email != getInfo.email)
                                getInfo.email = edit.email;
                            if (edit.telephone != getInfo.telephone)
                                getInfo.telephone = edit.telephone;

                            //Update permit
                            if(edit.type_acc == 0)
                            {
                                getPer.type_acc = "trainer";
                            } else
                            {
                                getPer.type_acc = "staff";
                            }
                           

                            //Change password if have
                            if(edit.password !=null && edit.repassword !=null)
                            {
                                if(edit.password == edit.repassword)
                                {
                                    findAcc.password = edit.password; //Gan pass moi chua ma hoa vao
                                    findAcc.password = findAcc.MD5Pass(); //Chuyen pass sang MD5
                                }
                            }

                            //Save
                            db.SaveChanges();
                            edit.type_acc = (getPer.type_acc == "trainer") ? 0 : 1;
                            ViewBag.Success = "Information has been updated!";
                            return View(edit);

                        }
                        else
                        {
                            return RedirectToAction("SearchAccount");
                        }


                    }

                }
                else
                {
                    return RedirectToAction("SearchAccount");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //Delete account
        [HttpGet]
        public ActionResult DeleteAccount()
        {
            if (Session["userInfo"] != null && (Session["userPermit"].Equals("admin") || Session["userPermit"].Equals("staff")))
            {
                if (Request.QueryString["del_user"] != null)
                {
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["del_user"].Trim();
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();


                            db.AccountDBs.Remove(findAcc);
                            db.InfoDetailsDBs.Remove(getInfo);
                            db.PermitDBs.Remove(getPer);
                            db.InfoAccDBs.Remove(getCenter);

                            db.SaveChanges();

                            return Content("200");
                        }
                        else
                        {
                            return Content("301");
                        }


                    }

                }
                else
                {
                    return Content("301");
                }
            }
            return Content("503");
        }

        //Manage for Training Staff

        public ActionResult NewTrainee()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (Request.QueryString["suggest"] != null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        var courLS = db.CourseDBs.ToList();
                        int? couIDD;
                        if (IsValidEmail(Request.QueryString["suggest"]))
                    {
                       
                          
                          
                            NewAccount newAcc = new NewAccount()
                            {
                                email = Request.QueryString["suggest"],
                                courseDB = courLS,
                                couID = 0
                            };
                            return View(newAcc);
                        }
                   
                    else
                    {
                        NewAccount newAcc = new NewAccount()
                        {
                            username = Request.QueryString["suggest"],
                             courseDB = courLS,
                            couID = 0
                        };
                        return View(newAcc);
                    }
                    }
                }
                //Normal
                NewAccount newAccNormal = new NewAccount()
                {
                    email = "",
                    courseDB = null,
                    couID = 0
                };
                return View(newAccNormal);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult NewTrainee(NewAccount account)
        {
            //Dam bao la admin moi duoc reg
            if (Session["userPermit"] != null && Session["userPermit"].Equals("staff"))
            {

                if (account != null)
                {
                    if (!account.checkUser())
                    {
                        ViewBag.Status = "Username was exist!";
                        return View();
                    }
                    if (!account.checkEmail())
                    {
                        ViewBag.Status = "Email was exist!";
                        return View();
                    }
                    if (account.checkUser() && account.checkEmail())
                    {
                        //start add database
                        int userID;
                        int PerID;
                        int DetailID;

                        using (var db = new dbFPTSystem())
                        {

                            //add user

                            AccountDB newAcc = new AccountDB();

                            newAcc.username = account.username;
                            newAcc.password = account.MD5Pass();

                            db.AccountDBs.Add(newAcc);

                            db.SaveChanges();

                            userID = newAcc.accID;

                            //add permit

                            string perType = "trainee";

                            PermitDB newPer = new PermitDB();
                            newPer.type_acc = perType;

                            db.PermitDBs.Add(newPer);
                            db.SaveChanges();

                            PerID = newPer.perID;

                            //add details info
                            InfoDetailsDB newInfo = new InfoDetailsDB();

                            if (account.date_birthday != null)
                            {
                                newInfo.date_birthday = account.date_birthday;
                            } else
                            {
                                newInfo.date_birthday = "1990-01-01";
                            }
                            newInfo.fullname = account.fullname;
                            newInfo.email = account.email;
                            newInfo.workplace = account.workplace;
                            newInfo.job = account.job;
                            newInfo.main_pr_lg = account.main_pr_lg;
                            newInfo.telephone = account.telephone;
                            newInfo.toeic_score = account.toeic_score;
                            newInfo.ex_or_in = account.ex_or_in;

                            db.InfoDetailsDBs.Add(newInfo);
                            db.SaveChanges();

                            DetailID = newInfo.detailsID;

                            //add InfoAccDB

                           
                            //CouD_ID default null

                            InfoAccDB newAccInfo = new InfoAccDB();

                            if (account.couID == 0)
                            {
                                newAccInfo.couD_ID = null;
                            }
                            else
                            {
                                var findC = db.CourseDetailsDBs.Where(n => n.couID == account.couID);
                                if (findC.Count() > 0) //Co mon hoc moi them vao duoc
                                {
                                    newAccInfo.couD_ID = findC.FirstOrDefault().couD_ID; //gan link acc
                                }
                            }

                            newAccInfo.accID = userID;
                            newAccInfo.perID = PerID;
                            newAccInfo.detailsID = DetailID;
                            //newAccInfo.couD_ID = null;

                            account.courseDB = db.CourseDBs.ToList();


                            db.InfoAccDBs.Add(newAccInfo);
                            try
                            {
                                db.SaveChanges();
                                ViewBag.Success = "Trainee new account has been created!";
                                return View(account);
                            }
                            catch (DbEntityValidationException e)
                            {
                                ViewBag.Status = "Some error:" + e.EntityValidationErrors + ". Please try again later";
                                return View(account);
                            }
                            return View(account);



                        }
                        //Reg success
                    }

                    return RedirectToAction("Index");
                }
            }
            return View();



        }

        public ActionResult ManageTrainee()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (Request.QueryString["user"] != null)
                {
                    //Neu tai khoan edit la chinh minh thi redict den myacoount
                    AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                    if (_uobj.username == Request.QueryString["user"])
                    {
                        return RedirectToAction("MyAccount");
                    }
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"].Trim();
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                            var courLS = db.CourseDBs.ToList();
                            //Check da tham gia Course chua
                            int? couIDD;
                            if (getCenter.couD_ID == null)
                            {
                                couIDD = 0;
                            } else
                            {
                                var findC = db.CourseDetailsDBs.Find(getCenter.couD_ID);
                                couIDD = findC.couID;
                            }

                            EditAdminModel editnew = new EditAdminModel()
                            {
                                username = findAcc.username,
                                fullname = getInfo.fullname,
                                email = getInfo.email,
                                telephone = getInfo.telephone,
                                type_acc = 0,
                                courseDB = courLS,
                                couID = couIDD
                            };
                            return View(editnew);
                        }
                        else
                        {
                            return RedirectToAction("SearchTrain");
                        }


                    }

                }
                else
                {
                    return RedirectToAction("SearchTrain");
                }
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult ManageTrainee(EditAdminModel edit)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (edit != null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"];
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                            //Start check Information
                            if (edit.fullname.Length < 5 || edit.fullname == null)
                            {
                                ViewBag.Status = "Fullname too short or invalid!";
                                return View(edit);
                            }

                            if (IsValidEmail(edit.email))
                            {
                                if (edit.email == getInfo.email)
                                {
                                    //Email not change
                                }
                                else
                                {
                                    var checkMailNew = db.InfoDetailsDBs.Where(n => n.email == edit.email).Count();
                                    if (checkMailNew > 0)
                                    {
                                        ViewBag.Status = "This email is already in use for another account!";
                                        return View(edit);
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.Status = "Incorrect email format!";
                                return View(edit);
                            }

                            if (edit.password != edit.repassword)
                            {
                                ViewBag.Status = "Two passwords do not match!";
                                return View(edit);
                            }

                            //Start update information
                            if (edit.fullname != getInfo.fullname)
                                getInfo.fullname = edit.fullname;
                            if (edit.email != getInfo.email)
                                getInfo.email = edit.email;
                            if (edit.telephone != getInfo.telephone)
                                getInfo.telephone = edit.telephone;

                            //Update permit
                             getPer.type_acc = "trainee";


                            //Change password if have
                            if (edit.password != null && edit.repassword != null)
                            {
                                if (edit.password == edit.repassword)
                                {
                                    findAcc.password = edit.password; //Gan pass moi chua ma hoa vao
                                    findAcc.password = findAcc.MD5Pass(); //Chuyen pass sang MD5
                                }
                            }

                            if (edit.couID == 0)
                            {
                                getCenter.couD_ID = null;
                            } else
                            {
                                var findC = db.CourseDetailsDBs.Where(n => n.couID == edit.couID);
                                if (findC.Count() > 0) //Co mon hoc moi them vao duoc
                                {
                                    getCenter.couD_ID = findC.FirstOrDefault().couD_ID; //gan link acc
                                }
                            }
                            //Save
                            db.SaveChanges();
                            ViewBag.Success = "Information has been updated!";
                            return View(edit);

                        }
                        else
                        {
                            return RedirectToAction("SearchTrainee");
                        }


                    }

                }
                else
                {
                    return RedirectToAction("SearchTrainee");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchTrain()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult SearchTrain(SearchModel model)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                //Search
               if(model != null)
                {
                    ViewBag.KeyWordSeach = model.keyword;

                    using (var db = new dbFPTSystem()) {

                        if (IsValidEmail(model.keyword)) //Search with email format
                        {
                            //Find all of type if keyword empty
                            var findWith = (model.keyword == null || model.keyword.Length < 1) ?  db.InfoDetailsDBs.OrderBy(n => n.email).Select(n => new{email = n.email,fullname = n.fullname}): db.InfoDetailsDBs.Where(n => n.email.Contains(model.keyword)).OrderBy(n => n.email).Select(n => new{email = n.email, fullname = n.fullname});

                            int firstCount = findWith.Count();
                            findWith = findWith.Take(10);
                            if (firstCount != 0 && findWith != null)
                            {
                                findWith = findWith.Take(10);
                                List<SearchModel> lstSeach = new List<SearchModel>(findWith.Count());
                                for (int i = 0; i < findWith.Count(); i++)
                                {
                                    SearchModel sr = new SearchModel()
                                    {
                                        type_rs = model.type_rs,
                                        email = findWith.Skip(i)?.Take(1).Single().email,
                                        fullname = findWith.Skip(i)?.Take(1).Single().fullname
                                    };
                                    sr.getInfoStaff();
                                    lstSeach.Add(sr);
                                }
                                ViewBag.Success = (model.keyword == null || model.keyword.Length < 1) ? "Result for 'Type All'" : "Result for '" + model.keyword + "'";
                                return View(lstSeach);
                            }
                        }
                        else
                        { //Search with username format 
                            var findWith = (model.keyword == null || model.keyword.Length < 1) ?  db.AccountDBs.OrderBy(n => n.username).Select(n => new { username = n.username, accID = n.accID }) : db.AccountDBs.Where(n => n.username.Contains(model.keyword)).OrderBy(n => n.username).Select(n => new{username = n.username,accID = n.accID});
                            int firstCount = findWith.Count();
                            findWith = findWith.Take(10);
                            if (firstCount != 0 && findWith != null)
                            {
                                List<SearchModel> lstSeach = new List<SearchModel>(findWith.Count());
                                for (int i = 0; i < findWith.Count(); i++)
                                {
                                    SearchModel sr = new SearchModel()
                                    {
                                        type_rs = model.type_rs,
                                        username = findWith.Skip(i)?.Take(1).Single().username,
                                        accID = findWith.Skip(i).Take(1).Single().accID
                                    };
                                    sr.getInfoStaff();
                                    lstSeach.Add(sr);
                                }
                                ViewBag.Success = (model.keyword == null || model.keyword.Length < 1) ?  "Result for 'Type All'" : "Result for '" + model.keyword + "'";
                                return View(lstSeach);
                            }
                        }


                        ViewBag.Status = "The account you requested does not exist!";
                        return View();
                    
                }
                }
            }
            return RedirectToAction("Index", "Home");

        }
        public ActionResult ManageTrainer()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (Request.QueryString["user"] != null)
                {
                    //Neu tai khoan edit la chinh minh thi redict den myacoount
                    AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                    if (_uobj.username == Request.QueryString["user"])
                    {
                        return RedirectToAction("MyAccount");
                    }
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"].Trim();
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                            var topDBB = db.TopicDBs;
                            //get topic
                            int? topIDD;
                            if (getCenter.couD_ID == null)
                            {
                                topIDD = 0;
                            } else
                            {
                                var ckCK = db.CourseDetailsDBs.Find(getCenter.couD_ID); //Check da link acc chua
                                topIDD = ckCK.topID;
                            }
                          

                           
                            

                            EditAdminModel editnew = new EditAdminModel()
                            {
                                username = findAcc.username,
                                fullname = getInfo.fullname,
                                email = getInfo.email,
                                telephone = getInfo.telephone,
                                topDB = topDBB.ToList(),
                                topID = topIDD
                            };
                            return View(editnew);
                        }
                        else
                        {
                            return RedirectToAction("SearchTrain");
                        }


                    }

                }
                else
                {
                    return RedirectToAction("SearchTrain");
                }
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult ManageTrainer(EditAdminModel edit)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (Request.QueryString["user"] != null)
                {
                    //Neu tai khoan edit la chinh minh thi redict den myacoount
                    AccountDB _uobj = Session["userInfo"] as TestSession.Models.AccountDB;
                    if (_uobj.username == Request.QueryString["user"])
                    {
                        return RedirectToAction("MyAccount");
                    }
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        string user_get = Request.QueryString["user"].Trim();
                        var accessTo = db.AccountDBs.Where(n => n.username == user_get);
                        if (accessTo.Count() > 0)
                        {
                            var findAcc = accessTo.FirstOrDefault();
                            var getCenter = db.InfoAccDBs.Where(n => n.accID == findAcc.accID).FirstOrDefault();
                            var getInfo = db.InfoDetailsDBs.Where(n => n.detailsID == getCenter.detailsID).FirstOrDefault();
                            var getPer = db.PermitDBs.Where(n => n.perID == getCenter.perID).FirstOrDefault();

                            //Start check Information
                            if (edit.fullname.Length < 5 || edit.fullname == null)
                            {
                                ViewBag.Status = "Fullname too short or invalid!";
                                return View(edit);
                            }

                            if (IsValidEmail(edit.email))
                            {
                                if (edit.email == getInfo.email)
                                {
                                    //Email not change
                                }
                                else
                                {
                                    var checkMailNew = db.InfoDetailsDBs.Where(n => n.email == edit.email).Count();
                                    if (checkMailNew > 0)
                                    {
                                        ViewBag.Status = "This email is already in use for another account!";
                                        return View(edit);
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.Status = "Incorrect email format!";
                                return View(edit);
                            }
     

                            //Start update information
                            if (edit.fullname != getInfo.fullname)
                                getInfo.fullname = edit.fullname;
                            if (edit.email != getInfo.email)
                                getInfo.email = edit.email;
                            if (edit.telephone != getInfo.telephone)
                                getInfo.telephone = edit.telephone;

                            //Update permit
                            getPer.type_acc = "trainer";


                            if (edit.topID == 0)
                            {
                                getCenter.couD_ID = null;
                            }
                            else
                            {
                                var findC = db.CourseDetailsDBs.Where(n => n.topID == edit.topID && n.couID != null);
                                if (findC.Count() > 0) //Co topic trong course moi them vao duoc
                                {
                                    getCenter.couD_ID = findC.FirstOrDefault().couD_ID; //gan link acc
                                } else
                                {
                                    @ViewBag.Status = "The topic you have selected is not in any Course yet!";
                                }

                            }
                            edit.topDB =  db.TopicDBs.ToList();
                            //Save
                            db.SaveChanges();
                            ViewBag.Success = "Information has been updated!";
                            return View(edit);
                        }
                        else
                        {
                            return RedirectToAction("SearchTrain");
                        }


                    }

                }
                else
                {
                    return RedirectToAction("SearchTrain");
                }
            }
            return RedirectToAction("Index", "Home");

        }

        bool IsValidEmail(string email)
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

        public ActionResult SearchAccount()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("admin"))
            {
                return View(); 
            }
            return RedirectToAction("Index", "Home");

        }
            [HttpPost]
        public ActionResult SearchAccount(SearchModel search) //Toi da danh ca 1 ngay moi code duoc tinh nang nay, no dung de tim kiem!!!
        {

            if (Session["userInfo"] != null && Session["userPermit"].Equals("admin"))
            {
                if (search != null)
                {
                    ViewBag.KeyWordSeach = search.keyword;
                    using (var db = new dbFPTSystem())
                    {
                        if (IsValidEmail(search.keyword)) //Search with email format
                        {
                            //Find all of type if keyword empty
                            var findWith = (search.keyword == null || search.keyword.Length < 1) ? db.InfoDetailsDBs.OrderBy(n => n.email).Select(n => new { email = n.email, fullname = n.fullname }) : db.InfoDetailsDBs.Where(n => n.email.Contains(search.keyword)).OrderBy(n => n.email).Select(n => new { email = n.email, fullname = n.fullname });


                            int firstCount = findWith.Count();
                            findWith = findWith.Take(10);
                            if (firstCount != 0 && findWith != null)
                            {
                                findWith = findWith.Take(10);
                                List<SearchModel> lstSeach = new List<SearchModel>(findWith.Count());
                                for (int i = 0; i < findWith.Count(); i++)
                                {
                                    SearchModel sr = new SearchModel()
                                    {
                                        type_rs = search.type_rs,
                                        email = findWith.Skip(i)?.Take(1).Single().email,
                                        fullname = findWith.Skip(i)?.Take(1).Single().fullname
                                    };
                                    sr.getInfoAvailable();
                                    lstSeach.Add(sr);
                                }
                                ViewBag.Success = (search.keyword == null || search.keyword.Length < 1) ? "Result for 'Type All'" : "Result for '" + search.keyword + "'";
                                return View(lstSeach);
                            }
                        }
                        else { //Search with username format
                            var findWith = (search.keyword == null || search.keyword.Length < 1) ? db.AccountDBs.OrderBy(n => n.username).Select(n => new { username = n.username, accID = n.accID }) : db.AccountDBs.Where(n => n.username.Contains(search.keyword)).OrderBy(n => n.username).Select(n => new { username = n.username, accID = n.accID });

                            int firstCount = findWith.Count();
                            findWith = findWith.Take(10);
                            if (firstCount != 0 && findWith !=null)
                            {   
                                List<SearchModel> lstSeach = new List<SearchModel>(findWith.Count());
                               for (int i = 0; i < findWith.Count(); i++)
                                {
                                    SearchModel sr = new SearchModel()
                                    {
                                        type_rs = search.type_rs,
                                        username = findWith.Skip(i)?.Take(1).Single().username,
                                        accID = findWith.Skip(i).Take(1).Single().accID
                                    };
                                    sr.getInfoAvailable();
                                    lstSeach.Add(sr);
                                }
                                ViewBag.Success = (search.keyword == null || search.keyword.Length < 1) ? "Result for 'Type All'" : "Result for '" + search.keyword + "'";
                                return View(lstSeach);
                            }
                        }
                       
                     
                        ViewBag.Status = "The account you requested does not exist!";
                            return View();
                        }
                }
            } 
                return RedirectToAction("Index", "Home");
            

        }


    


    }
}