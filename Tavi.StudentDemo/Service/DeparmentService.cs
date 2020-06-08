using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavi.StudentDemo.Models;

namespace Tavi.StudentDemo.Service
{
    public class DepartmentService : BaseService<Department, TaviStudentDemoDb>
    {
        TaviStudentDemoDb db = null;
        public DepartmentService()
        {
            db = new TaviStudentDemoDb();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StudentCode">Mã sinh viên </param>
        /// <param name="FullName">Họ tên</param>
        /// <param name="DepartmentID">Khoa đang theo học</param>
        /// <param name="PageCurrent">Trang hiện tại</param>
        /// <param name="PageSize">Số bản ghi hiện tại</param>
        /// <returns></returns>
        /// 
        #region Xem danh sách sinh viên
        public IPagedList<Department> GetDepartments(string DepartmentsName, int PageCurrent, int PageSize, bool? status)
        {

            var list = db.Departments.Where(x => x.Status == true).AsEnumerable();
            if (!string.IsNullOrEmpty(DepartmentsName))
            {
                list = list.Where(x => x.DepartmentName.Contains(DepartmentsName)).AsEnumerable();
            }
            if (status.HasValue)
            {
                list = list.Where(x => x.Status == status.Value);
            }

            return list.OrderByDescending(x => x.DepartmentID).ToPagedList(PageCurrent, PageSize);
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            var list = db.Departments.Where(x => x.IsDelete == false).AsEnumerable();
            return list.OrderByDescending(x => x.DepartmentID);
        }
        #endregion
        #region Xem danh sách sinh viên đã xóa
        #endregion
        #region Tìm kiếm theo ID
        public Department FindByKeys(object DepartmentID)
        {
            Department department = new Department();
            if (DepartmentID != null)
            {
                department = FindByKey(DepartmentID);
            }
            return department;
        }
        #endregion
        #region Thêm mới SV
        public void Inserts(Department department)
        {
            department.IsDelete = false;
            Insert(department);
        }
        #endregion
        #region Sửa SV
        public void Updates(Department department)
        {
            Update(department);
        }
        #endregion
        #region Xóa SV (Không khôi phục)
        public void Deletes(int? DepartmentID)
        {
            if (DepartmentID != null)
            {
                Delete(DepartmentID);
            }
        }
        #endregion
        #region Xóa SV vào thùng rác(Có thể khôi phục)
        public void DeleteByID(int? DepartmentID)
        {
            Department department = new Department();
            if (DepartmentID.HasValue)
            {
                department = db.Departments.Find(department);
                department.IsDelete = true;
                Update(department);
            }

        }
        #endregion
        #region Chuyển trạng thái SV
        public bool ChangeStatus(int? id)
        {
            var department = db.Departments.Find(id);
            department.Status = !department.Status;
            db.SaveChanges();
            return true;
        }
        #endregion

    }
}
