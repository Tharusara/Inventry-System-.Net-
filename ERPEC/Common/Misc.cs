using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Common
{
    namespace Properties
    {
        namespace Database
        {
            namespace ERP
            {
                public struct SM_Types
                {
                    public static int GRN = 1;
                    public static int INV = 2;
                    public static int LTI = 3;
                    public static int LTO = 4;
                }
            }
        }
    }
    namespace Functions
    {
        public static class QueryBuilder
        {
            public static string BuildInsert(string fullTableName, string outPutParam, Dictionary<string, object> SQLparams)
            {
                string Query = "INSERT INTO " + fullTableName;
                if (!string.IsNullOrWhiteSpace(outPutParam))
                {
                    Query += (" OUTPUT INSERTED." + outPutParam);
                }
                Query += " VALUES (";
                SQLparams.ToList().ForEach(p => Query += (p.Key + ", "));
                Query = Query.Remove(Query.Length - 2) + ")";
                return Query;
            }

            public static string BuildUpdate(string fullTableName, string whereClauseColName, object whereClauseValue, Dictionary<string, object> SQLparams)
            {
                string Query = "UPDATE " + fullTableName + " SET ";
                SQLparams.ToList().ForEach(p => Query += (p.Key.Replace("@", "") + " = " + p.Key + ", "));
                Query = Query.Remove(Query.Length - 2) + " WHERE " + whereClauseColName + " = " + whereClauseValue;
                return Query;
            }
        }
    }
}