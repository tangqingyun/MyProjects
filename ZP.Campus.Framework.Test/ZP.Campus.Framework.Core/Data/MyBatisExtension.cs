using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;

namespace ZP.Campus.Framework.Core.Data
{
    class MyBatisExtension
    {
        ///1.6版本
        /// <summary>
        /// 返回Dataset的方法
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static DataSet QueryForDataSet(string statementName, CommandType commandtype, object paramObject)
        {
            DataSet ds = new DataSet();
            ISqlMapper mapper = Mapper.Instance();
            IMappedStatement statement = mapper.GetMappedStatement(statementName);
            if (!mapper.IsSessionStarted)
            {
                mapper.OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
            statement.PreparedCommand.Create(scope, mapper.LocalSession, statement.Statement, paramObject);
            IDbCommand command = mapper.LocalSession.CreateCommand(commandtype);
            command.CommandText = scope.IDbCommand.CommandText;
            foreach (IDataParameter pa in scope.IDbCommand.Parameters)
            {
                command.Parameters.Add(new SqlParameter(pa.ParameterName, pa.Value));
            }
            //foreach (PropertyInfo info in paramObject.GetType().GetProperties())
            //{
            //    command.Parameters.Add(new SqlParameter(info.Name, info.GetValue(paramObject, null)));
            //}
            mapper.LocalSession.CreateDataAdapter(command).Fill(ds);
            return ds;
        }

        public static string GetSql(string statementName, object paramObject)
        {
            ISqlMapper mapper = Mapper.Instance();
            IMappedStatement statement = mapper.GetMappedStatement(statementName);
            if (!mapper.IsSessionStarted)
            {
                mapper.OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
            return scope.PreparedStatement.PreparedSql;
        }

        public static void UpdateSql(string sql)
        {
            Mapper.Instance().Update("ExecuteUpdateSql", sql);
        }

    }

        ///// <summary>
        ///// 获取总公司下所有该简历投递记录信息
        ///// </summary>
        ///// <param name="campusid"></param>
        ///// <returns></returns>
        //public List<PostData> GetPostData(long campusid, long rootcmpid)
        //{
        //    PhaseScoreParameter phaseScoreParameter = new PhaseScoreParameter();
        //    phaseScoreParameter.campusId = campusid.ToString();
        //    phaseScoreParameter.rootCompanyId = rootcmpid;
        //    DataTable dt = MyBatisExtend.QueryForDataSet("PKG_Get_PostRecordInfoModel", CommandType.StoredProcedure, phaseScoreParameter).Tables[0];
        //    return null;
        //}
     
     //public string GetTotalScore(long campusid, long positionid)
     //   {
     //       TotalPhaseScoreParameter phaseScoreParameter = new TotalPhaseScoreParameter();
     //       phaseScoreParameter.campusid = campusid;
     //       phaseScoreParameter.positionid = positionid;
     //       TotalPhaseScore PhaseScore = Mapper.Instance().QueryForObject("XPKG_Get_SelectTotalScoreModel", phaseScoreParameter) as TotalPhaseScore;
     //       return PhaseScore.TotalScore.ToString();
     //   }

}
