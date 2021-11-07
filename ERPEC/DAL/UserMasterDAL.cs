using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class UserMasterDAL
    {
        public UserMasterDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, UserMasterDOM User)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@USER_name", User.USER_name);
            SQLparams.Add("@USER_username", User.USER_userName);
            SQLparams.Add("@USER_password", Enc_Dec_P_Class.ComputeHash(User.USER_password,HashAlgorithms.SHA512,null));
            SQLparams.Add("@USER_email", User.USER_email);
            SQLparams.Add("@USER_contactnumber", User.USER_contact);
            SQLparams.Add("@USER_Admin", 1);

            string Query = @"SELECT * FROM UserMaster WHERE USER_id = " + User.USER_id;
            DataTable UserMaster = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (UserMaster != null && UserMaster.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("UserMaster", "USER_id", User.USER_id, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("UserMaster", "USER_id", SQLparams);
                User.USER_id = (int)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return User;
        }
        public List<UserMasterDOM> GetUsers(SqlConnection SqlCon, SqlTransaction SqlTran,int? userID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<UserMasterDOM> UserMaster = new List<UserMasterDOM>();

            string Query = @"SELECT * FROM UserMaster";
            if (userID != null)
                Query += " WHERE USER_id=" + userID;
            DataTable DT_UserMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_UserMaster != null && DT_UserMaster.Rows.Count > 0)
            {
                DT_UserMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    UserMaster.Add(new UserMasterDOM()
                    {
                        USER_id = r.Field<int>("USER_id"),
                        USER_name = r.Field<string>("USER_name"),
                        USER_userName = r.Field<string>("USER_username"),
                        USER_password = r.Field<string>("USER_password"),
                        USER_contact = r.Field<string>("USER_contactnumber"),
                        USER_email = r.Field<string>("USER_email"),
                        USER_Admin = r.Field<bool>("USER_Admin"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return UserMaster;
        }

        public UserMasterDOM ChekUser(SqlConnection SqlCon, SqlTransaction SqlTran, UserMasterDOM user)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }
            //string Query = @"SELECT * FROM UserMaster WHERE USER_username='" + user.USER_userName + "' AND USER_password='" + user.USER_password + "'";
            string Query = @"SELECT * FROM UserMaster WHERE USER_username='" + user.USER_userName+"'";
            DataTable DT_USER = DBCon.GetData(SqlCon, SqlTran, Query, null);


            if (DT_USER != null && DT_USER.Rows.Count > 0)
            {
                //user.isValidUser = Enc_Dec_P_Class.Enc_Dec_P_Class.VerifyHash(us);
                string passwordHash = DT_USER.Rows[0].Field<string>("USER_password");
                user.USER_name = DT_USER.Rows[0].Field<string>("USER_name");
                user.isValidUser = Enc_Dec_P_Class.VerifyHash(user.USER_password,HashAlgorithms.SHA512, passwordHash);
            }
            else
            user.isValidUser = false;

            return user;

        }
    }
}