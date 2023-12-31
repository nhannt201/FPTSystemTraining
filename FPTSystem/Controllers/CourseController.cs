using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSession.Models;
namespace TestSession.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        //Trainer

        public ActionResult TrainerCourseView()
        {
            if (Session["userInfo"] != null && (Session["userPermit"].Equals("trainer")))
            {
                using (var db = new dbFPTSystem())
                {
                    AccountDB uo = Session["userInfo"] as TestSession.Models.AccountDB;

                    var dbF = db.AccountDBs.Where(n => n.username == uo.username).FirstOrDefault();
                    var dbC = db.InfoAccDBs.Where(n => n.accID == dbF.accID);
                    if(dbC.Count() > 0 || dbC !=null)
                    {
                        if (dbC.FirstOrDefault().couD_ID != null)
                        {
                            var dbX = db.CourseDetailsDBs.Where(n => n.couD_ID == dbC.FirstOrDefault().couD_ID);
                            if (dbX.FirstOrDefault().topID !=null)
                            {
                                
                                List<TrainerView> newT = new List<TrainerView>(dbX.Count());
                             //   for (int i=0; i < dbX.Count(); i++)
                              //  {
                                    var dbU = db.CourseDBs.Where(n => n.couID == dbX.FirstOrDefault().couID).FirstOrDefault();
                                    var dbTT = db.TopicDBs.Where(n => n.topID == dbX.FirstOrDefault().topID).FirstOrDefault();

                                TrainerView neww = new TrainerView()
                                    {
                                        course_name = dbU.name,
                                        course_details = dbU.description,
                                        topic_name = dbTT.name,
                                        topic_details = dbTT.description
                                    };
                                newT.Add(neww);
                                return View(newT);
                               // }

                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        //Training Staff

        public ActionResult DeleteCourse()
        {
            if (Session["userInfo"] != null && (Session["userPermit"].Equals("staff")))
            {
                if (Request.QueryString["id_del"] != null && Request.QueryString["type"] != null)
                {
                    //Normal
                    using (var db = new dbFPTSystem())
                    {
                        var id_type = int.Parse(Request.QueryString["id_del"].Trim());
                        var type_data = int.Parse(Request.QueryString["type"].Trim());
                       if (type_data == 0)
                        {
                            var course = db.CourseDBs.Find(id_type);
                            if (course !=null)
                            {
                                var findD = db.CourseDetailsDBs.Where(n => n.couID == course.couID).FirstOrDefault();
                                var findA = db.InfoAccDBs.Where(n =>n.couD_ID == findD.couD_ID).FirstOrDefault();
                                //Co Couse nhung chua co Account so huu
                                if (findD !=null && findA == null)
                                {
                                    db.CourseDetailsDBs.Remove(findD);
                                    db.CourseDBs.Remove(course);

                                    db.SaveChanges();
                                }
                                //Account da so huu course, thi go bo khoi acc truoc khi xoa
                                if (findD != null && findA != null)
                                {
                                    findA.couD_ID = null;
                                    db.CourseDetailsDBs.Remove(findD);
                                    db.CourseDBs.Remove(course);

                                    db.SaveChanges();
                                }
                                //Khong co truong hop else vi khi tao Course dong thoi tao luon CourseDetails
                            }
                            return Content("200");

                        }
                        else if (type_data ==1)
                        {
                            var course = db.CategoryDBs.Find(id_type);
                            if (course != null)
                            {
                                var findD = db.CourseDetailsDBs.Where(n => n.cateID == course.cateID).FirstOrDefault();
     
                                if (findD != null)
                                {
                                    findD.cateID = null; //Go cate ra khoi center
                                    db.CategoryDBs.Remove(course); //Xoa cate

                                    db.SaveChanges();
                                } else
                                {
                                    db.CategoryDBs.Remove(course); //Xoa cate

                                    db.SaveChanges();
                                }
                               
                            }
                            return Content("200");

                        }
                        else if (type_data ==2)
                        {
                            var course = db.TopicDBs.Find(id_type);
                            if (course != null)
                            {
                                var findD = db.CourseDetailsDBs.Where(n => n.topID == course.topID).FirstOrDefault();
                                if (findD != null)
                                {
                                    findD.topID = null; //Go top ra khoi center
                                    db.TopicDBs.Remove(course); //Xoa top

                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.TopicDBs.Remove(course); //Xoa top

                                    db.SaveChanges();
                                }

                            }
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

        public ActionResult ManageCourse()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (Request.QueryString["id_type"] == null || Request.QueryString["type"] == null)
                {
                    return RedirectToAction("SearchCourse", "Course");
                }  else
                {
                    var type_find = Request.QueryString["type"];
                    var id_type = int.Parse(Request.QueryString["id_type"]); //It is ID
                    if (type_find == "0")
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.CourseDBs.Find(id_type); 
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                NewCourse neww = new NewCourse()
                                {
                                    id = id_type,
                                    type = int.Parse(type_find),
                                    name  = findit.name,
                                    description = findit.description
                                };
                                return View(neww);
                            }
                           
                        }

                       
                    } else if (type_find == "1")
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.CategoryDBs.Find(id_type);
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                NewCourse neww = new NewCourse()
                                {
                                    id = id_type,
                                    type = int.Parse(type_find),
                                    name = findit.name,
                                    description = findit.description
                                };
                                return View(neww);
                            }

                        }
                    }
                    else if (type_find == "2")
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.TopicDBs.Find(id_type);
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                NewCourse neww = new NewCourse()
                                {
                                    id = id_type,
                                    type = int.Parse(type_find),
                                    name = findit.name,
                                    description = findit.description
                                };
                                return View(neww);
                            }

                        }
                    } else
                    {
                        return RedirectToAction("SearchCourse", "Course");

                    }
                   
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ManageCourse(NewCourse course)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (course == null)
                {
                    return RedirectToAction("SearchCourse", "Course");
                }
                else
                {
                    var id_type = int.Parse(Request.QueryString["typeid"]); //Ben cshtml gui lai ID
                    var type_find = course.type; 
                    if (type_find == 0)
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.TopicDBs.Find(id_type);
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                if (course.name.Length > 1)
                                {
                                    findit.name = course.name;
                                    findit.description = course.description;
                                    db.SaveChanges();
                                }
                                ViewBag.Success = "Topic has been updated!";
                                return View(course);
                            }

                        }


                    }
                    else if (type_find == 1)
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.CategoryDBs.Find(id_type);
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                if (course.name.Length > 1)
                                {
                                    findit.name = course.name;
                                    findit.description = course.description;
                                    db.SaveChanges();
                                }
                                ViewBag.Success = "Category has been updated!";
                                return View(course);
                            }

                        }
                    }
                    else if (type_find == 2)
                    {
                        using (var db = new dbFPTSystem())
                        {
                            var findit = db.CourseDBs.Find(id_type);
                            if (findit == null)
                            {
                                return RedirectToAction("SearchCourse", "Course");
                            }
                            else
                            {
                                if (course.name.Length > 1)
                                {
                                    findit.name = course.name;
                                    findit.description = course.description;
                                    db.SaveChanges();
                                }
                                ViewBag.Success = "Course has been updated!";
                                return View(course);
                            }

                        }
                    }
                    else
                    {
                        return RedirectToAction("SearchCourse", "Course");

                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateCourse()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateCourse(NewCourse course)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                using (var db = new dbFPTSystem())
                {
                    if (course.name == null || course.name.Length < 1)
                    {
                        ViewBag.Status = "Please enter all required information!";
                        return View();
                    }
                    //Add Topic
                    if (course.type == 0)
                    {
                        //Check topic available
                        var findTopic = db.TopicDBs.Where(n=>n.name.Contains(course.name)).Count();
                        if (findTopic > 0) {
                            ViewBag.Status = "This topic already exists!";
                            return View();
                        } else
                        {

                            TopicDB newTP = new TopicDB()
                            {
                                name = course.name,
                                description = course.description
                            };
                            db.TopicDBs.Add(newTP);
                            db.SaveChanges();

                            ViewBag.Success = "Added New Topic!";
                            return View();
                        }
                    }
                    //Add Category
                    if (course.type == 1)
                    {
                        //Check Category available
                        var findCate = db.CategoryDBs.Where(n => n.name.Contains(course.name)).Count();
                        if (findCate > 0)
                        {
                            ViewBag.Status = "This category already exists!";
                            return View();
                        }
                        else
                        {

                            CategoryDB newCT = new CategoryDB()
                            {
                                name = course.name,
                                description = course.description
                            };
                            db.CategoryDBs.Add(newCT);
                            db.SaveChanges();

                            ViewBag.Success = "Added New Category!";
                            return View();
                        }
                    }
                    //Add Course
                    if (course.type == 2)
                    {
                        //Check course available
                        var findCourse = db.CourseDBs.Where(n => n.name.Contains(course.name)).Count();
                        if (findCourse > 0 )
                        {
                            ViewBag.Status = "This course already exists!";
                            return View();
                        }
                        else
                        {
                            int couIDD;
                            CourseDB newC = new CourseDB()
                            {
                                name = course.name,
                                description = course.description
                            };
                            db.CourseDBs.Add(newC);
                            db.SaveChanges();
                            couIDD = newC.couID;//db.CourseDBs.Where(n=>n.name == course.name).FirstOrDefault().couID;
                            //Add to Details Course
                            CourseDetailsDB newDTT = new CourseDetailsDB() { 
                                couID = couIDD
                            };
                            db.CourseDetailsDBs.Add(newDTT);
                            db.SaveChanges();

                            ViewBag.Success = "Added New Course!";
                            return View();
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddTo()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                using (var db = new dbFPTSystem())
                {
                    var getTopic = db.TopicDBs;
                    var getCate = db.CategoryDBs;
                    var getCou = db.CourseDBs;
                   
                    AddTo newAdd = new AddTo()
                    {
                        topicDB = getTopic.ToList(),
                        cateDB = getCate.ToList(),
                        courseDB = getCou.ToList()
                    };
                    return View(newAdd);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchCourse()
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                SearchCourse search = new SearchCourse();
                return View(search);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult SearchCourse(SearchCourse search)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (search !=null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        int type_find =  search.type_find;
                        switch(type_find)
                        {
                            case 0: //Course Name
                                var findCourse = (search.keyword == null || search.keyword.Length < 1) ? db.CourseDBs.OrderBy(n => n.name) : db.CourseDBs.Where(n => (n.description != null) ? (n.description.Contains(search.keyword) || n.name.Contains(search.keyword)) : n.name.Contains(search.keyword)).OrderBy(n => n.name);

                                if (findCourse.Count() > 0)
                                {
                                    findCourse.Take(10);
                                    List<SearchCourse> newS = new List<SearchCourse>(findCourse.Count());
                                    for(int i = 0; i<findCourse.Count(); i++)
                                    {
                                        SearchCourse newSS = new SearchCourse()
                                        {
                                            id = findCourse.Skip(i)?.Take(1).Single().couID,
                                            name = findCourse.Skip(i)?.Take(1).Single().name,
                                            desc = findCourse.Skip(i)?.Take(1).Single().description,
                                            type_find = search.type_find
                                        };
                                        newS.Add(newSS); 
                                    }
                                    ViewBag.Success = (search.keyword == null || search.keyword.Length < 1) ? "Result for 'Course'" : "Result for '" + search.keyword + "'";

                                    search.listrs = newS;                                  
                                } else
                                {
                                    ViewBag.Status = "No search results found!";
                                }
                                break;
                            case 1: //Category Name
                                var findCate = (search.keyword == null || search.keyword.Length < 1) ? db.CategoryDBs.OrderBy(n => n.name) : db.CategoryDBs.Where(n => (n.description != null) ? (n.description.Contains(search.keyword) || n.name.Contains(search.keyword)) : n.name.Contains(search.keyword)).OrderBy(n => n.name);

                                if (findCate.Count() > 0)
                                {
                                    findCate.Take(10);
                                    List<SearchCourse> newS = new List<SearchCourse>(findCate.Count());
                                    for (int i = 0; i < findCate.Count(); i++)
                                    {
                                        SearchCourse newSS = new SearchCourse()
                                        {
                                            id = findCate.Skip(i)?.Take(1).Single().cateID,
                                            name = findCate.Skip(i)?.Take(1).Single().name,
                                            desc = findCate.Skip(i)?.Take(1).Single().description,
                                            type_find = search.type_find
                                        };
                                        newS.Add(newSS);
                                    }
                                    ViewBag.Success = (search.keyword == null || search.keyword.Length < 1) ? "Result for 'Category'" : "Result for '" + search.keyword + "'";

                                    search.listrs = newS;
                                }
                                else
                                {
                                    ViewBag.Status = "No search results found!";
                                }
                                break;
                            case 2: //Topic
                                var findTop = (search.keyword == null || search.keyword.Length < 1) ? db.TopicDBs.OrderBy(n => n.name) : db.TopicDBs.Where(n => (n.description != null) ? (n.description.Contains(search.keyword) || n.name.Contains(search.keyword)) : n.name.Contains(search.keyword)).OrderBy(n => n.name);

                                if (findTop.Count() > 0)
                                {
                                    findTop.Take(10);
                                    List<SearchCourse> newS = new List<SearchCourse>(findTop.Count());
                                    for (int i = 0; i < findTop.Count(); i++)
                                    {
                                        SearchCourse newSS = new SearchCourse()
                                        {
                                            id = findTop.Skip(i)?.Take(1).Single().topID,
                                            name = findTop.Skip(i)?.Take(1).Single().name,
                                            desc = findTop.Skip(i)?.Take(1).Single().description,
                                            type_find = search.type_find
                                        };
                                        newS.Add(newSS);
                                    }
                                    ViewBag.Success = (search.keyword == null || search.keyword.Length < 1) ? "Result for 'Topic'" : "Result for '" + search.keyword + "'";
                                    search.listrs = newS;
                                }
                                else
                                {
                                    ViewBag.Status = "No search results found!";
                                }
                                break;
                        }
                        return View(search);

                    }
                }
               
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CourseToCate(AddTo add)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            { 
                if (add !=null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        if (add.cateID == 0 || add.couID == 0)
                        {
                            return RedirectToAction("AddTo", "Course", new { az = "1" });
                            //No cate to add
                        }
                        else
                        {
                            //Check course has center
                            var findCenter = db.CourseDetailsDBs.Where(n => n.couID == add.couID).FirstOrDefault();
                            if (findCenter != null)
                            {
                               // if (findCenter.cateID == null)
                              //  {
                                    findCenter.cateID = add.cateID; //ghi de len caate hien tai luon (neu co)
                                    db.SaveChanges();
                                    return RedirectToAction("AddTo", "Course", new { az = "0" });
                                    //Add Couse to Cate success

                            }
                        }
                        
                    }
                }
            }
            return RedirectToAction("Index",  "Home");
        }

        [HttpPost]
        public ActionResult TopicToCourse(AddTo add)
        {
            if (Session["userInfo"] != null && Session["userPermit"].Equals("staff"))
            {
                if (add != null)
                {
                    using (var db = new dbFPTSystem())
                    {
                        if (add.topID == 0 || add.couID == 0)
                        {
                            return RedirectToAction("AddTo", "Course", new { az = "3" });
                            //No topic or course to add
                        }
                        else
                        {
                            //Check course has center
                            var findCenter = db.CourseDetailsDBs.Where(n => n.couID == add.couID).FirstOrDefault();
                            if (findCenter != null)
                            {
                                    findCenter.topID = add.topID; //ghi de len topic hien tai luon (neu co)
                                    db.SaveChanges();
                                    return RedirectToAction("AddTo", "Course", new { az = "2" });
      
                                //Add Topic to Course success

                            }
                        }

                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}