using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.Demo.G2.Service;
using Tavi.StudentDemo.Helper;
using Tavi.StudentDemo.Models;
using Tavi.StudentDemo.Service;

namespace Tavi.StudentDemo.Controllers
{
    public class StudentController : Controller
    {
        #region Danh sach sien vien


        public ActionResult Index(string Status)
        {
             ListByStatus(Status);
            return View();

        }
        public ActionResult Test(string sortOrder, string StudentCode, string FullName, int? DepartmentID, int? PageCurrent)
        {
            TaviStudentDemoDb db = new TaviStudentDemoDb();
            var students = db.Students.AsQueryable();
            int pageNumber = PageCurrent ?? 1;
            if (string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name_desc"))
            {
                ViewBag.NameSortParm = "name_asc";
            }else 
            {
                ViewBag.NameSortParm = "name_desc";
            }
            
                switch (sortOrder)
            {
                case "name_asc":
                    students = students.OrderBy(s => s.StudentID);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.StudentID);
                    break;
                default:
                    students = students.OrderByDescending(s => s.StudentID);
                    break;
            }
            return View(students.ToList());
        }

        
        public PartialViewResult LoadListStudent(string StudentCode, string FullName, int? DepartmentID, int? PageCurrent)
        {
            StudentService service = new StudentService();
            int pageNumber = PageCurrent ?? 1;
            IPagedList<Student> students = service.GetStudents(StudentCode
                , FullName
                , DepartmentID
                , pageNumber
                , 10
                );
            ViewBag.StudentCode = StudentCode;
            ViewBag.FullName = FullName;
            ViewBag.PageCurrent = PageCurrent;          
            return PartialView("_List", students);
        }
        public PartialViewResult LoadDetail(int? id)
        {
            StudentService service = new StudentService();
            service.FindByKey(id);
            var model = service.FindByKey(id);
            return PartialView("_Detail", model);
        }

        //[HttpPost]
        //public ActionResult Index(string StudentCode, string FullName, int? DepartmentID)
        //{
        //    StudentService service = new StudentService();
        //    int pageNumber =   1;
        //    IPagedList<Student> students = service.GetStudents(StudentCode
        //        , FullName
        //        , DepartmentID
        //        , pageNumber
        //        , 10
        //        );
        //    ViewBag.StudentCode = StudentCode;
        //    ViewBag.FullName = FullName;
        //    ViewBag.PageCurrent = pageNumber;
        //    return View(students);

        //}
        [HttpGet]
        public ActionResult Deleted(string StudentCode, string FullName, int? DepartmentID, int? PageCurrent)
        {
            StudentService service = new StudentService();
            int pageNumber = PageCurrent ?? 1;
            IPagedList<Student> students = service.GetDeleted(StudentCode
                , FullName
                , DepartmentID
                , pageNumber
                , 10
                );
            ViewBag.StudentCode = StudentCode;
            ViewBag.FullName = FullName;
            ViewBag.PageCurrent = PageCurrent;
            return View(students);
        }
        [HttpPost]
        public ActionResult Deleted(string StudentCode, string FullName, int? DepartmentID)
        {
            StudentService service = new StudentService();
            int pageNumber = 1;
            IPagedList<Student> students = service.GetStudents(StudentCode
                , FullName
                , DepartmentID
                , pageNumber
                , 10
                );
            ViewBag.StudentCode = StudentCode;
            ViewBag.FullName = FullName;
            ViewBag.PageCurrent = pageNumber;
            return View(students);

        }
        #endregion

        #region Them moi sinh vien    
        [HttpGet]
        public ActionResult Add(int? id)
        {
            StudentService service = new StudentService();
            Student student = service.FindByKeys(id);
            return View(student);

        }
        [HttpPost]
        public ActionResult Add(int? Id
            , string StudentCode
            , string FullName
            , string Birthday
            , string Address
            , string Phone
            , string Email
            , string Description
            , string ListDepartmentID
            , string ListClassRoomID
            , bool Status
            )
        {
            StudentService studentService = new StudentService();
            Student student = studentService.FindByKeys(Id);
            student.StudentCode = StudentCode;
            student.FullName = FullName;
            if (!string.IsNullOrEmpty(Birthday))
                student.Birthday = ConvertEx.ToDate(Birthday);
            student.Address = Address;
            student.Phone = Phone;
            student.Email = Email;
            if (!string.IsNullOrEmpty(ListDepartmentID))
                student.DepartmentID = Convert.ToInt32(ListDepartmentID);
            if (!string.IsNullOrEmpty(ListClassRoomID))
                student.ClassRoomID = Convert.ToInt32(ListClassRoomID);
            student.Description = Description;
            student.Status = Status;
            student.IsDelete = false;
            if (Id.HasValue)
            {
                studentService.Updates(student);
                setAlert("Sửa sinh vien thành công", "success");


            }
            else
            {
                studentService.Inserts(student);
                setAlert("Thêm sinh vien thành công", "success");

            }
            return View(student);
        }
        public PartialViewResult LoadForm(int? id)
        {
            StudentService service = new StudentService();
            Student student = service.FindByKeys(id);
            return PartialView("_Form", student);
        }
        [HttpPost]
        public ActionResult LoadForm(int? Id
           , string StudentCode
           , string FullName
           , string Birthday
           , string Address
           , string Phone
           , string Email
           , string Description
           , string ListDepartmentID
           , string ListClassRoomID
           , bool Status
           )
        {
            StudentService studentService = new StudentService();
            Student student = studentService.FindByKeys(Id);
            student.StudentCode = StudentCode;
            student.FullName = FullName;
            if (!string.IsNullOrEmpty(Birthday))
                student.Birthday = ConvertEx.ToDate(Birthday);
            student.Address = Address;
            student.Phone = Phone;
            student.Email = Email;
            if (!string.IsNullOrEmpty(ListDepartmentID))
                student.DepartmentID = Convert.ToInt32(ListDepartmentID);
            if (!string.IsNullOrEmpty(ListClassRoomID))
                student.ClassRoomID = Convert.ToInt32(ListClassRoomID);
            student.Description = Description;
            student.Status = Status;
            student.IsDelete = false;
            if (Id.HasValue)
            {
                studentService.Updates(student);
                setAlert("Sửa sinh vien thành công" ,"success");
            }
            else
            {
                studentService.Inserts(student);
                setAlert("Thêm sinh vien thành công", "success");
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult ListDeparment(int? DeparmentID)
        {
            DepartmentService service = new DepartmentService();

            ViewBag.ListDepartmentID = new SelectList(service.GetAllDepartments(), "DepartmentID", "DepartmentName", DeparmentID);
            return PartialView("_Department");
        }
        public void ListByStatus(string Status)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Đang hoạt động",
                Value = "1",
                Selected = string.Equals("1", Status)
            });
            items.Add(new SelectListItem
            {
                Text = "Ngưng hoạt động",
                Value = "0",
                Selected = string.Equals("0", Status)
            });

            ViewBag.ListDepartmentStatus = items;// new SelectList(service.GetAllDepartments(), "DepartmentID", "DepartmentName", DeparmentID);
          
        }
        public PartialViewResult ListClassRoom(int? DepartmentID, int? ClassRoomID)
        {
            ClassRoomService service = new ClassRoomService();

            ViewBag.ListClassRoomID = new SelectList(service.GetAllClassRoom(DepartmentID), "ClassRoomID", "ClassName", ClassRoomID);
            return PartialView("_ClassRoom");
        }
        [HttpPost]

        #endregion
        #region Xoa sinh vien

        public ActionResult Delete(int[] cbxItem)
        {
            if (cbxItem.Count() > 0)
            {
                foreach (int item in cbxItem)
                {
                    StudentService studentService = new StudentService();
                    studentService.Deletes(item);
                }
            }

            var response = new
            {
                status = true,
                data = cbxItem
            };

            return Json(response,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Recycle(int? StudentID)
        {
            if (StudentID > 0)
            {
                StudentService service = new StudentService();
                service.DeleteByID(StudentID);
            }
            return RedirectToAction("Index");
        }   
        #endregion
        #region alert
        public void setAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;

            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";

            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        #endregion
    }
}
