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
    public class DepartmentController : Controller
    {
        #region Danh sach sien vien


        public ActionResult Index()
        {
            return View();

        }
        public PartialViewResult LoadListDepartment(string DepartmentName , int? PageCurrent, bool? status)
        {
            DepartmentService service = new DepartmentService();
            int pageNumber = PageCurrent ?? 1;
            IPagedList<Department> departments = service.GetDepartments(DepartmentName
                , pageNumber
                , 10
                , status
                );
            ViewBag.DepartmentName = DepartmentName;
            ViewBag.PageCurrent = PageCurrent;
            return PartialView("_List", departments);
        }               
        #endregion

        #region Them moi sinh vien    
        [HttpGet]
        public ActionResult Add(int? id)
        {
            DepartmentService service = new DepartmentService();
            Department department = service.FindByKeys(id);
            return View(department);

        }
        [HttpPost]
        public ActionResult Add(int? Id
            , string DepartmentName           
            , string Description           
            , bool Status
            )
        {
            DepartmentService departmentService = new DepartmentService();
            Department department = departmentService.FindByKeys(Id);
            department.DepartmentName = DepartmentName;
            department.Description = Description;
            department.Status = Status;
            department.IsDelete = false;
            if (Id.HasValue)
            {
                departmentService.Updates(department);
                setAlert("Sửa sinh vien thành công", "success");


            }
            else
            {
                departmentService.Inserts(department);
                setAlert("Thêm sinh vien thành công", "success");

            }
            return View(department);
        }
        [HttpGet]
        public PartialViewResult LoadForm(int? id)
        {
            DepartmentService service = new DepartmentService();
            Department department = service.FindByKeys(id);
            return PartialView("_Form", department);
        }

        [HttpPost]
        public ActionResult LoadForm(int? Id
            , string DepartmentName
            , string Description
            , bool Status
            )
        {
            DepartmentService deparmentService = new DepartmentService();
            Department department = deparmentService.FindByKeys(Id);
            department.DepartmentName = DepartmentName;
            department.Description = Description;
            department.Status = Status;
            if (Id.HasValue)
            {
                deparmentService.Updates(department);
                setAlert("Sửa khoa thành công", "success");

            }
            else
            {
                deparmentService.Inserts(department);
                setAlert("Thêm khoa thành công", "success");


            }
            return RedirectToAction("Index");
        }
        public PartialViewResult LoadDetail(int? id)
        {
            DepartmentService service = new DepartmentService();
            service.FindByKey(id);
            var model = service.FindByKey(id);
            return PartialView("_Detail", model);
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
                    DepartmentService departmentService = new DepartmentService();
                    departmentService.Deletes(item);
                   
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
