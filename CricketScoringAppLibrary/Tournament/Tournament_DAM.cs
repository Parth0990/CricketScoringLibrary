using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketScoringAppLibrary.Tournament
{
    public class Tournament_DAM
    {
        public class Tournament_Data
        {
            private int tournamentId;
            private string tid;
            private string tName;
            private string city;
            private string organiserName;
            private string phoneNumber;
            private DateTime startDate;
            private DateTime endDate;
            private string ballType;
            private string matchType;
            private string description;
            private DateTime createdDate;
            private DateTime modifiedDate;

            public struct IsDirtyTournamentData
            {
                public bool z_tournamentId;
                public bool z_tid;
                public bool z_tName;
                public bool z_city;
                public bool z_organiserName;
                public bool z_phoneNumber;
                public bool z_startDate;
                public bool z_endDate;
                public bool z_ballType;
                public bool z_matchType;
                public bool z_description;
            }

            public int TournamentId
            {
                get { return tournamentId; }
                set { tournamentId = value; }
            }
            public string TId
            {
                get { return tid; }
                set { tid = value; }
            }
            public string OrganiserName
            {
                get { return organiserName; }
                set { organiserName = value; }
            }
            public string PhoneNumber
            {
                get { return phoneNumber; }
                set { phoneNumber = value; }
            }
            public DateTime StartDate
            {
                get { return startDate; }
                set { startDate = value; }
            }
            public DateTime EndDate
            {
                get { return endDate; }
                set { endDate = value; }
            }
            public string BallType
            {
                get { return ballType; }
                set { ballType = value; }
            }
            public string MatchType
            {
                get { return matchType; }
                set { matchType = value; }
            }
            public string Description
            {
                get { return description; }
                set { description = value; }
            }
            public DateTime CreatedDate
            {
                get { return createdDate; }
                set { createdDate = value; }
            }
            public DateTime ModifiedDate
            {
                get { return modifiedDate; }
                set { modifiedDate = value; }
            }
        }

        public class TournamentMgr : Tournament_Data
        {
            public string GetTournamentData(string UserId, SqlConnection CN, SqlTransaction Trans = null)
            {
                string Result = string.Empty, Qry = string.Empty;
                DataSet Ds = new DataSet();
                SqlCommand cmd = new SqlCommand();

                Qry = " select * from TournamentSummary TS \r\n " +
                      " Left JOIN TournamentDetail TD ON TD.tid = TS.tournamentId \r\n " +
                      " where uid = '" + UserId + "'; \r\n";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Qry;
                cmd.Connection = CN;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Ds);
                if (Ds != null)
                {
                    Result = JsonConvert.SerializeObject(Ds);
                }
                return Result;
            }
        }
    }
}
