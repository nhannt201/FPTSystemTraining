using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSession.Models
{
    public class AddTo
    {
        public List<TopicDB> topicDB {get; set;}

        public List<CourseDB> courseDB { get; set; }

        public List<CategoryDB> cateDB { get; set; }


        public int topID { get; set; }

        public int couID { get; set; }

        public int cateID { get; set; }
    }
}