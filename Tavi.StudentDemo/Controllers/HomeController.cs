using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.StudentDemo.Models;

namespace Tavi.StudentDemo.Controllers
{
   public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TaviStudentDemoDb db = new TaviStudentDemoDb();
            var StudentCount = db.Students.Where(x => x.IsDelete == false).Count();
            var ClassRoomCount = db.ClassRooms.Where(x => x.IsDelete == false).Count();
            var DepartmentCount = db.Departments.Where(x => x.IsDelete == false).Count();
            ViewBag.StudentCount = StudentCount.ToString();
            ViewBag.ClassRoomCount = ClassRoomCount.ToString();
            ViewBag.DepartmentCount = DepartmentCount.ToString();         
            return View();
        }
    }
}
