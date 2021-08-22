using System;
namespace Mvc_Clinic.Models
{
    interface IHomeDAL
    {
        System.Collections.Generic.List<AddTestModel> GetAllTests(int id);
        System.Data.DataSet GetDoctors(string SearchTerm);
        System.Data.DataSet GetDoctorsSpeciality(string searchTerm);
        System.Collections.Generic.List<DoctorModel> SearchAllDoctors(string name, string speciality, string date);
        bool sendFeedback(FeedbackModel model);
        System.Collections.Generic.List<FeedbackModel> showAllVisibleFeedback();
        DoctorModel showDocSchedule(int doctorID);
        System.Collections.Generic.List<FeedbackModel> showVisibleFeedback();
    }
}
