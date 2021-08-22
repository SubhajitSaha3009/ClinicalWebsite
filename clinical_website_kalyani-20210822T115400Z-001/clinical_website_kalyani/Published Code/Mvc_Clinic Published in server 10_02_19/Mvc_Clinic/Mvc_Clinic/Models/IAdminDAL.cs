using System;
namespace Mvc_Clinic.Models
{
    interface IAdminDAL
    {
        string addAdmin(AdminModel admin, System.Web.HttpPostedFileBase img);
        bool AddDepartment(TestDepartmentsModel model, int mID);
        string addDoctors(DoctorModel model);
        string AddStaff(StaffModel sMod, int mID);
        bool AddSubDept(string DeptName, int id, int mID);
        string AddTest(AddTestModel AddT, int mID);
        bool addWorkingDayofDoctor(string days, int did, int modifierID);
        System.Collections.Generic.List<AddTestModel> AllTests();
        bool checkAdminStatus(LoginModel model);
        bool checkDuplicateDepartment(TestDepartmentsModel testDepartmentsModel);
        bool checkDuplicateSubDepartment(TestSub_DepartmentsModel testsubDepartmentsModel);
        bool checkOldPassword(string userID, string sPassword);
        bool DelDept(int id);
        bool DeleteFeedbackById(int id);
        bool DeleteTestById(int id);
        bool DelSubDept(int id);
        bool EditDoctor(SearchDoctorModel model);
        string forgetPasswordBySecurityAnswer(ForgetPasswordModel model);
        bool forgetPasswordToEmail(ForgetPasswordToEmailModel model);
        System.Collections.Generic.List<StaffModel> GetAllActiveStaff();
        System.Collections.Generic.List<StaffModel> GetAllInActiveStaff();
        System.Collections.Generic.List<StaffModel> getAllStaffDuringAPeriod(StaffPeriodModel model);
        System.Collections.Generic.List<TestSub_DepartmentsModel> getAllSubDept(int did);
        System.Collections.Generic.List<TestDepartmentsModel> getAllTestDepartments();
        string getDeptNameById(int id);
        AddTestModel GetTestDetailsByID(int id);
        System.Collections.Generic.List<string> getWorkingDays(int docID);
        bool invisibleFeedback(int feedbackID);
        bool LogException(FailedEventLogModel model);
        Mvc_Clinic.CheckPasswordStatusEnum login(LoginModel model);
        bool LogLoginDetails(LoginLogModel model);
        AdminModel searchAdmin(int adminID);
        System.Collections.Generic.List<SearchDoctorModel> SearchAllDoctors(string name);
        SearchDoctorModel searchDoctor(int doctorID);
        StaffModel SearchStaff(int id);
        System.Collections.Generic.List<FeedbackModel> showAllFeedback();
        bool updateAdminProfile(AdminModel model);
        bool updatePassword(int adminID, ChangePasswordAdmin c);
        string UpdateStaff(StaffModel sm, int mID);
        bool UpdateTest(AddTestModel a_model, int mID);
        bool visibleFeedback(int feedbackID);
    }
}
