using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketScoringAppLibrary
{
    public class Login_DAM
    {
        public class LoginData
        {
            private int uid;
            private string userId;
            private string username;
            private string password;
            private string fname;
            private string lname;
            private string email;
            private string phoneNumber;
            private string age;
            private int gender;
            private DateTime dob;
            private DateTime createdDate;
            private DateTime modifiedDate;
            private string userType;

            public struct IsDirtyLogin
            {
                public bool z_uid;
                public bool z_username;
                public bool z_userId;
                public bool z_fname;
                public bool z_lname;
                public bool z_email;
                public bool z_phonenumber;
                public bool z_password;
                public bool z_age;
                public bool z_gender;
                public bool z_DOB;
                public bool z_createdDate;
                public bool z_modifiedDate;
                public bool z_userType;
            }

            private IsDirtyLogin z_IsDirtyLogin;

            public int Uid
            {
                get
                {
                    z_IsDirtyLogin.z_uid = true;
                    return uid;
                }
                set
                {
                    uid = value;
                }
            }
            public string UserId
            {
                get
                {
                    z_IsDirtyLogin.z_userId = true;
                    return userId;
                }
                set { userId = value; }
            }
            public string Username
            {
                get
                {
                    z_IsDirtyLogin.z_username = true;
                    return username;
                }
                set
                {
                    username = value;
                }
            }
            public string Fname
            {
                get
                {
                    z_IsDirtyLogin.z_fname = true;
                    return fname;
                }
                set
                {
                    fname = value;
                }
            }
            public string Lname
            {
                get
                {
                    z_IsDirtyLogin.z_lname = true;
                    return lname;
                }
                set
                {
                    lname = value;
                }
            }
            public string Password
            {
                get
                {
                    z_IsDirtyLogin.z_password = true;
                    return password;
                }
                set
                {
                    password = value;
                }
            }

            public string Email
            {
                get
                {
                    z_IsDirtyLogin.z_email = true;
                    return email;
                }
                set { email = value; }
            }

            public string PhoneNumber
            {
                get
                {
                    z_IsDirtyLogin.z_phonenumber = true;
                    return phoneNumber;
                }
                set { phoneNumber = value; }
            }
            public string Age
            {
                get
                {
                    z_IsDirtyLogin.z_age = true;
                    return age;
                }
                set { age = value; }
            }
            public int Gender
            {
                get
                {
                    z_IsDirtyLogin.z_gender = true;
                    return gender;
                }
                set { gender = value; }
            }
            public DateTime DOB
            {
                get
                {
                    z_IsDirtyLogin.z_DOB = true;
                    return dob;
                }
                set { dob = value; }
            }
            public DateTime CreatedDate
            {
                get
                {
                    z_IsDirtyLogin.z_createdDate = true;
                    return createdDate;
                }
                set { createdDate = value; }
            }
            public DateTime ModifiedDate
            {
                get
                {
                    z_IsDirtyLogin.z_modifiedDate = true;
                    return modifiedDate;
                }
                set { modifiedDate = value; }
            }
            public string UserType
            {
                get
                {
                    z_IsDirtyLogin.z_userType = true;
                    return userType;
                }
                set
                {
                    userType = value;
                }
            }
            public IsDirtyLogin Z_IsDirtLogin
            {
                get { return z_IsDirtyLogin; }
            }
        }

        public class LoginMgr : LoginData
        {
            public string GetUserData(LoginData LoginData, SqlConnection con, SqlTransaction Trans = null)
            {
                string Result = string.Empty, Qry = string.Empty;
                SqlCommand cmd = new SqlCommand();
                Qry = "Select * from UserData Where Username = '" + LoginData.Username + "' \r\n";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Qry;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                DataSet Ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Ds);
                if (Ds != null)
                {
                    Result = JsonConvert.SerializeObject(Ds);
                }
                return Result;
            }

            public string GetEditData(string UserId, SqlConnection con, SqlTransaction Trans = null)
            {
                string Result = string.Empty, Qry = string.Empty;
                SqlCommand cmd = new SqlCommand();
                Qry = "Select * from UserData Where UserId = @UserId \r\n";
                Qry += "Select Id as ObjId, Name as ObjName From Gender \r\n";
                Qry += "select gn.Id as ObjId, gn.Name as ObjName from UserData ud \r\n " +
                       "Inner Join Gender gn On gn.Id = ud.Gender \r\n " +
                       "Where UserId = @UserId";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Qry;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                DataSet Ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Ds);
                if (Ds != null)
                {
                    Result = JsonConvert.SerializeObject(Ds);
                }
                return Result;
            }

            public string GetLastUserId(SqlConnection con, SqlTransaction Trans = null)
            {
                string Result = string.Empty, Qry = string.Empty;
                SqlCommand cmd = new SqlCommand();
                Qry = "select top 1 UserId from UserData Order By Uid Desc \r\n";
                Qry += "Select Id as ObjId, Name as ObjName From Gender \r\n";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Qry;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                DataSet Ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Ds);
                if (Ds != null)
                {
                    Result = JsonConvert.SerializeObject(Ds);
                }
                return Result;
            }

            public string SaveUserData(LoginData loginData, int Uid, SqlConnection con, SqlTransaction trans = null)
            {
                string Result = string.Empty, Qry = string.Empty;
                if (Uid == 0)
                {
                    Qry = "select email From UserData Where email = @Email";
                    SqlCommand cmd1 = new SqlCommand(Qry, con);
                    cmd1.Parameters.AddWithValue("@Email", loginData.Email);
                    cmd1.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if(dt.Rows.Count > 0 && dt != null)
                    {
                        Result = "0: Email Already Exits";
                        return Result;
                    }
                    else
                    {
                        loginData.CreatedDate = DateTime.Now;
                        Qry = "insert into UserData(UserName,Password,Email, Fname, Lname,PhoneNumber, UserId,UserType, DOB, Age, Gender,CreatedDate) \r\n" +
                            "values(@Username,@Password,@Email, @Fname, @Lname,@PhoneNumber,@UserId,@UserType,@DOB,@Age,@Gender, @CreatedDate)";
                        SqlCommand cmd = new SqlCommand(Qry, con);
                        cmd.Parameters.AddWithValue("@Username", loginData.Username);
                        cmd.Parameters.AddWithValue("@Password", loginData.Password);
                        cmd.Parameters.AddWithValue("@Email", loginData.Email);
                        cmd.Parameters.AddWithValue("@Fname", loginData.Fname);
                        cmd.Parameters.AddWithValue("@Lname", loginData.Lname);
                        cmd.Parameters.AddWithValue("@PhoneNumber", loginData.PhoneNumber);
                        cmd.Parameters.AddWithValue("@UserId", loginData.UserId);
                        cmd.Parameters.AddWithValue("@UserType", loginData.UserType);
                        cmd.Parameters.AddWithValue("@DOB", loginData.DOB);
                        cmd.Parameters.AddWithValue("@Age", loginData.Age);
                        cmd.Parameters.AddWithValue("@Gender", loginData.Gender);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        int rowAffected = cmd.ExecuteNonQuery();
                        //da.InsertCommand = new SqlCommand(Qry, con, trans);
                        //da.InsertCommand.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            Result = "1: SignUp Successfully";
                            this.SendMail(loginData.Email, loginData.Username);
                        }
                    }
                }
                else
                {
                    loginData.ModifiedDate = DateTime.Now;
                    Qry = "Update UserData set Username = @Username, Password = @Password, Email = @Email, Fname = @Fname, Lname = @Lname, \r\n" +
                        "PhoneNumber = @PhoneNumber, DOB = @DOB, Age= @Age, Gender = @Gender,ModifiedDate = @ModifiedDate \r\n" +
                        "Where UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    cmd.Parameters.AddWithValue("@UserId", loginData.UserId);
                    cmd.Parameters.AddWithValue("@Username", loginData.Username);
                    cmd.Parameters.AddWithValue("@Password", loginData.Password);
                    cmd.Parameters.AddWithValue("@Email", loginData.Email);
                    cmd.Parameters.AddWithValue("@Fname", loginData.Fname);
                    cmd.Parameters.AddWithValue("@Lname", loginData.Lname);
                    cmd.Parameters.AddWithValue("@PhoneNumber", loginData.PhoneNumber);
                    cmd.Parameters.AddWithValue("@DOB", loginData.DOB);
                    cmd.Parameters.AddWithValue("@Age", loginData.Age);
                    cmd.Parameters.AddWithValue("@Gender", loginData.Gender);
                    cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                    int rowAffected = cmd.ExecuteNonQuery();
                    //da.UpdateCommand = new SqlCommand(Qry, con, trans);
                    //da.UpdateCommand.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        Result = "1: Record Updated Successfully";
                    }
                    else
                    {
                        Result = "0: Record Updated Failed";
                    }
                }
                return Result;
            }

            public bool SendMail(string Email, string Body)
            {
                string FromMail = "parthjoshi0990@gmail.com";
                string FromPassword = "cokzczfzggwhkzqx";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(FromMail);
                message.Subject = "Credential For Cricket Scoring App";
                message.To.Add(new MailAddress(Email));
                message.Body = "<html><body><h2>Your Username For Login to Your Account is :- " + Body + "</h2><h4> Use this Username to Login your Account. </h4> </body></html>";
                message.IsBodyHtml = true;
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(FromMail,FromPassword),
                    EnableSsl = true
                };

                smtp.Send(message);
                return true;
            }
        }
    }
}
