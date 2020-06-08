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
    public class ClassRoomService : BaseService<ClassRoom, TaviStudentDemoDb>
    {
        TaviStudentDemoDb db = null;
        public ClassRoomService()
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
        public IPagedList<ClassRoom> GetClassRooms(string ClassName, int PageCurrent, int PageSize)
        {

            var list = db.ClassRooms.Where(x => x.Status == true).AsEnumerable();
            if (!string.IsNullOrEmpty(ClassName))
            {
                list = list.Where(x => x.ClassName.Contains(ClassName)).AsEnumerable();
           
            }
            return list.OrderByDescending(x => x.ClassRoomID).ToPagedList(PageCurrent, PageSize);
        }
        public IEnumerable<ClassRoom> GetAllClassRoom(int? DepartmentID)
        {
            var list = db.ClassRooms.Where(x => x.IsDelete == false).AsQueryable();
            if (DepartmentID.HasValue)
            {
                list = list.Where(m => m.DepartmentID == DepartmentID).AsQueryable();
            }
            return list.OrderBy(x => x.SortOrder);
        }
        #endregion
        #region Xem danh sách sinh viên đã xóa
        #endregion
        #region Tìm kiếm theo ID
        public ClassRoom FindByKeys(object ClassRoomID)
        {
            ClassRoom classRoom = new ClassRoom();
            if (ClassRoomID != null)
            {
                classRoom = FindByKey(ClassRoomID);
            }
            return classRoom;
        }
        #endregion
        #region Thêm mới SV
        public void Inserts(ClassRoom classRoom)
        {
            classRoom.IsDelete = false;
            Insert(classRoom);
        }
        #endregion
        #region Sửa SV
        public void Updates(ClassRoom classRoom)
        {
            Update(classRoom);
        }
        #endregion
        #region Xóa SV (Không khôi phục)
        public void Deletes(int? ClassRoomID)
        {
            if (ClassRoomID != null)
            {
                Delete(ClassRoomID);
            }
        }
        #endregion
        #region Xóa SV vào thùng rác(Có thể khôi phục)
        public void DeleteByID(int? ClassRoomID)
        {
            ClassRoom classRoom = new ClassRoom();
            if (ClassRoomID.HasValue)
            {
                classRoom = db.ClassRooms.Find(classRoom);
                classRoom.IsDelete = true;
                Update(classRoom);
            }

        }
        #endregion
        #region Chuyển trạng thái SV
       
        #endregion

    }
}
