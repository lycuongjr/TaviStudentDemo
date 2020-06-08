using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.StudentDemo.Helper;
using Tavi.StudentDemo.Models;
using Tavi.StudentDemo.Service;

namespace Tavi.StudentDemo.Controllers
{
    public class ClassRoomController : Controller
    {
        #region Danh sach lop


        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Test(string ClassName, int? PageCurrent)
        {
            ClassRoomService service = new ClassRoomService();
            int pageNumber = PageCurrent ?? 1;
            IPagedList<ClassRoom> students = service.GetClassRooms(ClassName
                , pageNumber
                , 10
                );
            ViewBag.ClassName = ClassName;
            ViewBag.PageCurrent = PageCurrent;
            return View(students);
        }
        public PartialViewResult LoadListClassRoom(string ClassName, int? PageCurrent)
        {
            ClassRoomService service = new ClassRoomService();
            int pageNumber = PageCurrent ?? 1;
            IPagedList<ClassRoom> classRooms = service.GetClassRooms(ClassName
                , pageNumber
                , 10
                );
            ViewBag.ClassName = ClassName;
            ViewBag.PageCurrent = PageCurrent;
            return PartialView("_List", classRooms);
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

        #endregion

        #region Them moi sinh vien    
        [HttpGet]
        public ActionResult Add(int? id)
        {
            ClassRoomService service = new ClassRoomService();
            ClassRoom classRoom = service.FindByKeys(id);
            return View(classRoom);

        }
        [HttpPost]
        public ActionResult Add(int? Id
            , string ClassName
            , string ListDepartmentID
            , string Description
            , bool Status
            )
        {
            ClassRoomService classRoomService = new ClassRoomService();
            ClassRoom classRoom = classRoomService.FindByKeys(Id);
            classRoom.ClassName = ClassName;
            classRoom.Description = Description;
            if (!string.IsNullOrEmpty(ListDepartmentID))
                classRoom.DepartmentID = Convert.ToInt32(ListDepartmentID);
            classRoom.Status = Status;
            classRoom.IsDelete = false;
            if (Id.HasValue)
            {
                classRoomService.Updates(classRoom);
                setAlert("Sửa sinh vien thành công", "success");


            }
            else
            {
                classRoomService.Inserts(classRoom);
                setAlert("Thêm sinh vien thành công", "success");

            }
            return View(classRoom);
        }
        public PartialViewResult LoadDetail(int? id)
        {
            ClassRoomService service = new ClassRoomService();
            service.FindByKey(id);
            var model = service.FindByKey(id);
            return PartialView("_Detail", model);
        }
        public PartialViewResult LoadForm(int? id)
        {
            ClassRoomService classRoomService = new ClassRoomService();
            ClassRoom classRoom = classRoomService.FindByKeys(id);
            return PartialView("_Form", classRoom);
        }
        [HttpPost]
        public ActionResult LoadForm(int? Id
            , string ClassName
            , string ListDepartmentID
            , string Description
            , bool Status
           )
        {
            ClassRoomService classRoomService = new ClassRoomService();
            ClassRoom classRoom = classRoomService.FindByKeys(Id);
            classRoom.ClassName = ClassName;
            classRoom.Description = Description;
            if (!string.IsNullOrEmpty(ListDepartmentID))
                classRoom.DepartmentID = Convert.ToInt32(ListDepartmentID);
            classRoom.Status = Status;
            classRoom.IsDelete = false;
            if (Id.HasValue)
            {
                classRoomService.Updates(classRoom);
                setAlert("Sửa sinh vien thành công", "success");


            }
            else
            {
                classRoomService.Inserts(classRoom);
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
        [HttpPost]

        #endregion
        #region Xoa sinh vien

        public ActionResult Delete(int[] cbxItem)
        {
            if (cbxItem.Count() > 0)
            {
                foreach (int item in cbxItem)
                {
                    ClassRoomService classRoomService = new ClassRoomService();
                    classRoomService.Deletes(item);

                }
            }
            var response = new
            {
                status = true,
                data = cbxItem
            };

            return Json(response, JsonRequestBehavior.AllowGet);
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

