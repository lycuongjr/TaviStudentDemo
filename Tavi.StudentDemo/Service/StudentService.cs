using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tavi.StudentDemo.Models;
using Tavi.StudentDemo.Service;

namespace Tavi.Demo.G2.Service
{
    public class StudentService : BaseService<Student, TaviStudentDemoDb>
    {
        TaviStudentDemoDb db = null;
        public StudentService()
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
        public IPagedList<Student> GetStudents(string StudentCode, string FullName, int? DepartmentID, int PageCurrent, int PageSize)
        {
            
            var list = db.Students.Where(x => x.Status == true).AsEnumerable();
            if (!string.IsNullOrEmpty(StudentCode))
            {
                list = list.Where(x => x.StudentCode.Contains(StudentCode)).AsEnumerable();
            }
            if (!string.IsNullOrEmpty(FullName))
            {
                list = list.Where(x => x.FullName.Contains(FullName)).AsEnumerable();
            }
            if (DepartmentID.HasValue)
            {
                list = list.Where(x => x.DepartmentID == DepartmentID);
            }

            return list.OrderByDescending(x => x.StudentID).ToPagedList(PageCurrent, PageSize);
        }
        #endregion
        #region Xem danh sách sinh viên đã xóa
        public IPagedList<Student> GetDeleted(string StudentCode, string FullName, int? DepartmentID, int PageCurrent, int PageSize)
        {

            var list = db.Students.Where(x => x.IsDelete == true).AsEnumerable();
            if (!string.IsNullOrEmpty(StudentCode))
            {
                list = list.Where(x => x.StudentCode.Contains(StudentCode)).AsEnumerable();
            }
            if (!string.IsNullOrEmpty(FullName))
            {
                list = list.Where(x => x.StudentCode.Contains(FullName)).AsEnumerable();
            }
            if (DepartmentID.HasValue)
            {
                list = list.Where(x => x.DepartmentID == DepartmentID);
            }

            return FindListByPage(PageCurrent, PageSize);
        }
        #endregion
        #region Tìm kiếm theo ID
        public Student FindByKeys(object StudentID)
        {
            Student student = new Student();
            if (StudentID != null)
            {
                student = FindByKey(StudentID);
            }
            return student;
        }
        #endregion
        #region Thêm mới SV
        public void Inserts(Student student)
        {
            student.IsDelete = false;
            Insert(student);
        }
        #endregion
        #region Sửa SV
        public void Updates(Student student)
        {
            Update(student);
        }
        #endregion
        #region Xóa SV (Không khôi phục)
        public void Deletes(int? StudentID)
        {
            if (StudentID != null)
            {
                Delete(StudentID);
            }
        }
        #endregion
        #region Xóa SV vào thùng rác(Có thể khôi phục)
        public void DeleteByID(int? StudentID)
        {
            Student student = new Student();
            if (StudentID.HasValue)
            {
                student = db.Students.Find(student);
                student.IsDelete = true;
                Update(student);
            }

        }
        #endregion
        #region Chuyển trạng thái SV
        public bool ChangeStatus(int? id)
        {
            var student = db.Students.Find(id);
            student.Status = !student.Status;
            db.SaveChanges();
            return true;
        }
        #endregion

    }
}
