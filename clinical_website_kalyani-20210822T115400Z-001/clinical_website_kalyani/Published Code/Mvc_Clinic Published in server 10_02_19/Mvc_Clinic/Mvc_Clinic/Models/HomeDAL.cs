using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Mvc_Clinic.Models
{
    public class HomeDAL : Mvc_Clinic.Models.IHomeDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Constr"].ToString());

        //////////////////////////////search for doctors name's suggestion////////////////////////
        public DataSet GetDoctors(string SearchTerm)
        {
            try
            {
                SqlCommand com_getAllDoctors = new SqlCommand("select * from Doctors where doctorName like '%'+@term+'%' and doctorStatus='Present'", con);
                com_getAllDoctors.Parameters.AddWithValue("@term", SearchTerm);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(com_getAllDoctors);
                da.Fill(ds);
                return ds;
            }
            finally
            {
                //con.Close();
            }

        }

        ///////////////////////////////////search doctor and speciality and day//////////////////////////////
        public List<DoctorModel> SearchAllDoctors(string name, string speciality, string date)
        {
            try
            {
                List<DoctorModel> list = new List<DoctorModel>();
                SqlCommand com_searchAllDoctors;
                if (name == "" && speciality == "" && date == "")
                {
                    com_searchAllDoctors = new SqlCommand("select * from Doctors where doctorStatus='Present'", con);
                    con.Open();
                    SqlDataReader dr = com_searchAllDoctors.ExecuteReader();

                    while (dr.Read())
                    {
                        DoctorModel d_model = new DoctorModel();
                        d_model.doctorID = dr.GetInt32(0);
                        d_model.doctorName = dr.GetString(1);
                        d_model.doctorDesignation = dr.GetString(2);
                        d_model.doctorSpeciality = dr.GetString(3);
                        d_model.doctorDegree = dr.GetString(4);
                        d_model.imageAddress = dr.GetString(5);
                        list.Add(d_model);
                    }
                    //con.Close();
                }
                if (name != "" && speciality == "" && date == "")
                {
                    com_searchAllDoctors = new SqlCommand("select * from Doctors where doctorName like '%'+@term+'%' and doctorStatus='Present'", con);
                    com_searchAllDoctors.Parameters.AddWithValue("@term", name);
                    con.Open();
                    SqlDataReader dr = com_searchAllDoctors.ExecuteReader();

                    while (dr.Read())
                    {
                        DoctorModel d_model = new DoctorModel();
                        d_model.doctorID = dr.GetInt32(0);
                        d_model.doctorName = dr.GetString(1);
                        d_model.doctorDesignation = dr.GetString(2);
                        d_model.doctorSpeciality = dr.GetString(3);
                        d_model.doctorDegree = dr.GetString(4);
                        d_model.imageAddress = dr.GetString(5);
                        list.Add(d_model);
                    }
                    con.Close();
                }
                if (name == "" && speciality != "" && date == "")
                {
                    com_searchAllDoctors = new SqlCommand("select * from Doctors where doctorSpeciality like '%'+@term+'%' and doctorStatus='Present'", con);
                    com_searchAllDoctors.Parameters.AddWithValue("@term", speciality);
                    con.Open();
                    SqlDataReader dr = com_searchAllDoctors.ExecuteReader();

                    while (dr.Read())
                    {
                        DoctorModel d_model = new DoctorModel();
                        d_model.doctorID = dr.GetInt32(0);
                        d_model.doctorName = dr.GetString(1);
                        d_model.doctorDesignation = dr.GetString(2);
                        d_model.doctorSpeciality = dr.GetString(3);
                        d_model.doctorDegree = dr.GetString(4);
                        d_model.imageAddress = dr.GetString(5);
                        list.Add(d_model);
                    }
                    con.Close();
                }
                if (name != "" && speciality != "" && date == "")
                {
                    com_searchAllDoctors = new SqlCommand("select * from Doctors where doctorName like '%'+@termName+'%' AND doctorSpeciality like '%'+@termSpeciality+'%' and doctorStatus='Present'", con);
                    com_searchAllDoctors.Parameters.AddWithValue("@termName", name);
                    com_searchAllDoctors.Parameters.AddWithValue("@termSpeciality", speciality);
                    con.Open();
                    SqlDataReader dr = com_searchAllDoctors.ExecuteReader();

                    while (dr.Read())
                    {
                        DoctorModel d_model = new DoctorModel();
                        d_model.doctorID = dr.GetInt32(0);
                        d_model.doctorName = dr.GetString(1);
                        d_model.doctorDesignation = dr.GetString(2);
                        d_model.doctorSpeciality = dr.GetString(3);
                        d_model.doctorDegree = dr.GetString(4);
                        d_model.imageAddress = dr.GetString(5);
                        list.Add(d_model);
                    }
                    con.Close();
                }

                if (name == "" && speciality == "" && date != "")
                {
                    //abcTest(date);
                    //get the ids of the doctors who are working on that particular day
                    List<int> lst_id = new List<int>();
                    if (date == "Monday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Monday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Tuesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Tuesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Wednesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Wednesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Thursday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Thursday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Friday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Friday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Saturday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Saturday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Sunday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Sunday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }

                    con.Close();





                    //now get all doctors whole profile
                    foreach (int id in lst_id)
                    {
                        SqlCommand com_getDoc = new SqlCommand("select * from doctors where doctorID=@did and doctorStatus='Present'", con);
                        com_getDoc.Parameters.AddWithValue("@did", id);

                        con.Open();
                        SqlDataReader drnew = com_getDoc.ExecuteReader();
                        DoctorModel doc = new DoctorModel();
                        if (drnew.Read())
                        {
                            doc.doctorID = drnew.GetInt32(0);
                            doc.doctorName = drnew.GetString(1);
                            doc.doctorDesignation = drnew.GetString(2);
                            doc.doctorSpeciality = drnew.GetString(3);
                            doc.doctorDegree = drnew.GetString(4);
                            doc.imageAddress = drnew.GetString(5);
                            doc.workingTiming = drnew.GetString(7);
                            doc.numberForAppointment = drnew.GetString(8);
                            doc.LnumberForAppointment = drnew.GetString(9);
                        }
                        con.Close();
                        if (doc.doctorID != 0)
                        {
                            list.Add(doc);
                        }

                    }

                    con.Close();

                }

                ////////////////////////////////////////////////// subha 23/02/18

                if (name != "" && speciality == "" && date != "")
                {
                    //abcTest(date);
                    //get the ids of the doctors who are working on that particular day
                    List<int> lst_id = new List<int>();
                    if (date == "Monday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Monday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Tuesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Tuesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Wednesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Wednesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Thursday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Thursday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Friday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Friday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Saturday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Saturday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Sunday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Sunday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }

                    con.Close();





                    //now get all doctors whole profile
                    foreach (int id in lst_id)
                    {
                        SqlCommand com_getDoc = new SqlCommand("select * from doctors where doctorName like '%'+@termName+'%' and doctorID=@did and doctorStatus='Present'", con);
                        com_getDoc.Parameters.AddWithValue("@did", id);
                        com_getDoc.Parameters.AddWithValue("@termName", name);

                        con.Open();
                        SqlDataReader drnew = com_getDoc.ExecuteReader();
                        DoctorModel doc = new DoctorModel();
                        if (drnew.Read())
                        {
                            doc.doctorID = drnew.GetInt32(0);
                            doc.doctorName = drnew.GetString(1);
                            doc.doctorDesignation = drnew.GetString(2);
                            doc.doctorSpeciality = drnew.GetString(3);
                            doc.doctorDegree = drnew.GetString(4);
                            doc.imageAddress = drnew.GetString(5);
                            doc.workingTiming = drnew.GetString(7);
                            doc.numberForAppointment = drnew.GetString(8);
                            doc.LnumberForAppointment = drnew.GetString(9);
                        }
                        con.Close();
                        if (doc.doctorID != 0)
                        {
                            list.Add(doc);
                        }
                    }

                    con.Close();

                }

                if (name == "" && speciality != "" && date != "")
                {
                    //abcTest(date);
                    //get the ids of the doctors who are working on that particular day
                    List<int> lst_id = new List<int>();
                    if (date == "Monday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Monday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Tuesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Tuesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Wednesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Wednesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Thursday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Thursday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Friday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Friday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Saturday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Saturday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Sunday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Sunday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }

                    con.Close();





                    //now get all doctors whole profile
                    foreach (int id in lst_id)
                    {
                        SqlCommand com_getDoc = new SqlCommand("select * from doctors where doctorSpeciality like '%'+@term+'%' and doctorID=@did and doctorStatus='Present'", con);
                        com_getDoc.Parameters.AddWithValue("@did", id);
                        com_getDoc.Parameters.AddWithValue("@term", speciality);

                        con.Open();
                        SqlDataReader drnew = com_getDoc.ExecuteReader();
                        DoctorModel doc = new DoctorModel();
                        if (drnew.Read())
                        {
                            doc.doctorID = drnew.GetInt32(0);
                            doc.doctorName = drnew.GetString(1);
                            doc.doctorDesignation = drnew.GetString(2);
                            doc.doctorSpeciality = drnew.GetString(3);
                            doc.doctorDegree = drnew.GetString(4);
                            doc.imageAddress = drnew.GetString(5);
                            doc.workingTiming = drnew.GetString(7);
                            doc.numberForAppointment = drnew.GetString(8);
                            doc.LnumberForAppointment = drnew.GetString(9);
                        }
                        con.Close();
                        if (doc.doctorID != 0)
                        {
                            list.Add(doc);
                        }
                    }

                    con.Close();

                }
                if (name != "" && speciality != "" && date != "")
                {
                    //abcTest(date);
                    //get the ids of the doctors who are working on that particular day
                    List<int> lst_id = new List<int>();
                    if (date == "Monday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Monday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Tuesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Tuesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Wednesday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Wednesday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Thursday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Thursday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Friday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Friday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Saturday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Saturday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }
                    else if (date == "Sunday")
                    {
                        SqlCommand com_getDocIDs = new SqlCommand("select doctorID from DoctorWorkingDays  where Sunday='Y'", con);
                        com_getDocIDs.Parameters.AddWithValue("@day", date);

                        con.Open();
                        SqlDataReader dr = com_getDocIDs.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            lst_id.Add(id);
                        }
                    }

                    con.Close();





                    //now get all doctors whole profile
                    foreach (int id in lst_id)
                    {
                        SqlCommand com_getDoc = new SqlCommand("select * from doctors where doctorName like '%'+@termName+'%' and doctorSpeciality like '%'+@term+'%' and doctorID=@did and doctorStatus='Present'", con);
                        com_getDoc.Parameters.AddWithValue("@did", id);
                        com_getDoc.Parameters.AddWithValue("@termName", name);
                        com_getDoc.Parameters.AddWithValue("@term", speciality);

                        con.Open();
                        SqlDataReader drnew = com_getDoc.ExecuteReader();
                        DoctorModel doc = new DoctorModel();
                        if (drnew.Read())
                        {
                            doc.doctorID = drnew.GetInt32(0);
                            doc.doctorName = drnew.GetString(1);
                            doc.doctorDesignation = drnew.GetString(2);
                            doc.doctorSpeciality = drnew.GetString(3);
                            doc.doctorDegree = drnew.GetString(4);
                            doc.imageAddress = drnew.GetString(5);
                            doc.workingTiming = drnew.GetString(7);
                            doc.numberForAppointment = drnew.GetString(8);
                            doc.LnumberForAppointment = drnew.GetString(9);
                        }
                        con.Close();
                        if (doc.doctorID != 0)
                        {
                            list.Add(doc);
                        }
                    }

                    con.Close();

                }

                return list;
            }
            finally
            {
                con.Close();
            }
        }


        ////////////////////////////////////////search suggestion for doctor's speciality////////////////////////
        public DataSet GetDoctorsSpeciality(string searchTerm)
        {
            try
            {
                SqlCommand com_getAllDoctorsSpeciality = new SqlCommand("select doctorSpeciality from Doctors group by doctorSpeciality having  doctorSpeciality like '%'+@term+'%' ", con);
                com_getAllDoctorsSpeciality.Parameters.AddWithValue("@term", searchTerm);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(com_getAllDoctorsSpeciality);
                da.Fill(ds);
                return ds;
            }
            finally
            {
                con.Close();
            }
        }



        ///////////////////////////////////souvik 20/10//////////////////////

        public DoctorModel showDocSchedule(int doctorID)
        {
            try
            {
                SqlCommand com_getDoc = new SqlCommand("select * from doctors where doctorID=@did", con);
                com_getDoc.Parameters.AddWithValue("@did", doctorID);

                con.Open();
                SqlDataReader dr = com_getDoc.ExecuteReader();
                DoctorModel doc = new DoctorModel();
                if (dr.Read())
                {
                    doc.doctorID = dr.GetInt32(0);
                    doc.doctorName = dr.GetString(1);
                    doc.doctorDesignation = dr.GetString(2);
                    doc.doctorSpeciality = dr.GetString(3);
                    doc.doctorDegree = dr.GetString(4);
                    doc.imageAddress = dr.GetString(5);
                    doc.workingTiming = dr.GetString(7);
                    doc.numberForAppointment = dr.GetString(8);
                    doc.LnumberForAppointment = dr.GetString(9);
                }
                //con.Close();
                return doc;
            }
            finally
            {
                con.Close();
            }
        }



        public bool sendFeedback(FeedbackModel model)
        {
            // var senderEmail = new MailAddress("developers.clinic@gmail.com", "Developers");
            // var receiverEmail = new MailAddress("souviksaha127@gmail.com", "User");



            // var fromPassword = "AtaSouSub@123";
            // var subject = "Feedback";
            //// var body = "Your new password is -  " + newPassword + "\n Please reset your password";
            // var body = "Sender Mobile number is :" + model.mobileNumber + "\nFeedback Message-----------------------\n" + model.body;

            // var smtp = new SmtpClient
            // {
            //     Host = "smtp.gmail.com",
            //     Port = 587,
            //     EnableSsl = true,
            //     DeliveryMethod = SmtpDeliveryMethod.Network,
            //     UseDefaultCredentials = false,
            //     Credentials = new NetworkCredential(senderEmail.Address, fromPassword)
            // };

            // using (var message = new MailMessage(senderEmail, receiverEmail)
            // {
            //     Subject = subject,
            //     Body = body

            // })
            // {
            //     smtp.Send(message);
            // }
            // return true;

            try
            {

                SqlCommand com_feedback = new SqlCommand("insert feedback values(@name,@mbl,@msg,'Invisible')", con);
                com_feedback.Parameters.AddWithValue("@name", model.name);
                com_feedback.Parameters.AddWithValue("@mbl", model.contactNumber);
                com_feedback.Parameters.AddWithValue("@msg", model.feedbackMessage);


                con.Open();
                int count = com_feedback.ExecuteNonQuery();
                //con.Close();
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                con.Close();
            }
        }





        public List<FeedbackModel> showVisibleFeedback()
        {
            try
            {
                SqlCommand com_showFeedback = new SqlCommand("select top 3 * from feedback where feedbackStatus='Visible' order by feedbackID desc", con);
                con.Open();
                SqlDataReader dr = com_showFeedback.ExecuteReader();
                List<FeedbackModel> list_feed = new List<FeedbackModel>();
                while (dr.Read())
                {
                    FeedbackModel model = new FeedbackModel();
                    model.feedbackID = dr.GetInt32(0);
                    model.name = dr.GetString(1);
                    model.contactNumber = dr.GetString(2);
                    model.feedbackMessage = dr.GetString(3);
                    model.feedbackStatus = dr.GetString(4);
                    list_feed.Add(model);
                }

                con.Close();
                return list_feed;
            }
            finally
            {
                con.Close();
            }
        }

        /////////////////////////////////////////Getting all testimonials////////////////////////////
        public List<AddTestModel> GetAllTests(int id)
        {
            try
            {
                SqlCommand com_getAllTests = new SqlCommand("Select * from Tests where testDeptID=@id", con);
                com_getAllTests.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = com_getAllTests.ExecuteReader();
                List<AddTestModel> list_allTests = new List<AddTestModel>();
                while (dr.Read())
                {
                    AddTestModel t_model = new AddTestModel();
                    t_model.TestId = dr.GetInt32(0);
                    t_model.TestName = dr.GetString(1);
                    t_model.TestDesc = dr.GetString(2);
                    t_model.imageAddress = dr.GetString(3);
                    t_model.TestSchedule = dr.GetString(4);
                    list_allTests.Add(t_model);
                }
                con.Close();
                return list_allTests;
            }
            finally
            {
                con.Close();
            }
        }



        //////////////////////////////showing all visible feedback for people////////////////////
        public List<FeedbackModel> showAllVisibleFeedback()
        {
            try
            {
                SqlCommand com_showFeedback = new SqlCommand("select * from feedback where feedbackStatus='Visible' order by feedbackID desc", con);
                con.Open();
                SqlDataReader dr = com_showFeedback.ExecuteReader();
                List<FeedbackModel> list_feed = new List<FeedbackModel>();
                while (dr.Read())
                {
                    FeedbackModel model = new FeedbackModel();
                    model.feedbackID = dr.GetInt32(0);
                    model.name = dr.GetString(1);
                    model.contactNumber = dr.GetString(2);
                    model.feedbackMessage = dr.GetString(3);
                    model.feedbackStatus = dr.GetString(4);
                    list_feed.Add(model);
                }

                con.Close();
                return list_feed;
            }
            finally
            {
                con.Close();
            }
        }

        //public bool abcTest(string date)
        //{
        //    SqlCommand com_getDocIDs = new SqlCommand("select * from DoctorWorkingDays where Sunday='Y'", con);
        //    com_getDocIDs.Parameters.AddWithValue("@day", date);

        //    con.Open();
        //    SqlDataReader dr = com_getDocIDs.ExecuteReader();
        //    List<int> lst_id = new List<int>();
        //    while (dr.Read())
        //    {
        //        int id = dr.GetInt32(0);
        //        lst_id.Add(id);
        //    }
        //    con.Close();
        //    return true;
        //}
    }
}