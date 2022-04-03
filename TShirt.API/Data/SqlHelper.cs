using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

/// <summary>
/// </summary>
public class SqlHelper
{
    SqlConnection con;
    public SqlHelper()
	{
        var configuation = GetConfiguration();
        con = new SqlConnection(configuation.GetSection("ConnectionStrings:DBConnection").Value);
    }
   
    public IConfigurationRoot GetConfiguration()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        return builder.Build();
    }

    public async Task<int> ExecuteNonQueryAsync(string query)
    {
        SqlConnection cnn = con;
        SqlCommand cmd = new SqlCommand(query, cnn);

        SqlTransaction trans = null;

        if (query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
        {
            cmd.CommandType = CommandType.Text;
        }
        else
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        int retval;
        try
        {
            cnn.Open();
            trans = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = trans;
            retval = await cmd.ExecuteNonQueryAsync();
            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
        finally
        {
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
        }
        return retval;
    }
    public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
    {
        int retval = 0;
        SqlConnection cnn = con;
        SqlCommand cmd = new SqlCommand(query, cnn);

        SqlTransaction trans = null;
        if (query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
        {
            cmd.CommandType = CommandType.Text;
        }
        else
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        for (int i = 0; i <= parameters.Length - 1; i++)
        {
            cmd.Parameters.Add(parameters[i]);
        }
        try
        {
            cnn.Open();
            trans = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = trans;
            //cmd.Parameters[1].Direction = ParameterDirection.Output;
            //cmd.Parameters[1].Size = 50;
            cmd.CommandTimeout = 0;
            //retval = int.Parse(cmd.Parameters[1].Value.ToString());
            retval = await cmd.ExecuteNonQueryAsync();
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            throw;
        }
        finally
        {
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
        }

        return retval;
    }
    
    public async Task<DataSet> ExecuteDataSetAsync(string query, string dtName)
    {
        SqlConnection cnn = con;
        SqlCommand cmd = new SqlCommand(query, cnn);
         DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        if (query.StartsWith("SELECT") | query.StartsWith("select") | query.StartsWith("Select") | query.StartsWith(" SELECT") | query.StartsWith(" select") | query.StartsWith(" Select"))
        {
            cmd.CommandType = CommandType.Text;
        }
        else
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        try
        {
            cmd.CommandTimeout = 0;
            da.SelectCommand = cmd;
            await Task.Run(() => da.Fill(ds, dtName));
            return ds;
        }
        catch 
        {
            throw ;
        }
        finally
        {
            da.Dispose();
        }
    }

    public async Task<DataTable> ExecuteDataTableAsync(string query, params SqlParameter[] parameters)
    {
        SqlConnection cnn = con;
        SqlCommand cmd = new SqlCommand(query, cnn);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        if (query.StartsWith("SELECT") | query.StartsWith("select") | query.StartsWith("Select") | query.StartsWith(" SELECT") | query.StartsWith(" select") | query.StartsWith(" Select"))
        {
            cmd.CommandType = CommandType.Text;
        }
        else
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        for (int i = 0; i <= parameters.Length - 1; i++)
        {
            cmd.Parameters.Add(parameters[i]);
        }
        try
        {
            cmd.CommandTimeout = 0;
            da.SelectCommand = cmd;
            await Task.Run(() => da.Fill(ds));
            return ds.Tables[0];
        }
        catch 
        {
            throw;
        }
        finally
        {
            da.Dispose();
        }
    }
    public async Task<DataTable> ExecuteDataTableAsync(string query, string dtName)
    {
        SqlConnection cnn = con;
        DataTable temp = new DataTable();
        temp.TableName = dtName;
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand(query, cnn);
            cmd.CommandTimeout = 0;
            da.SelectCommand = cmd;
            await Task.Run(() => da.Fill(temp));
            return temp;
        }
        catch 
        {
            throw;
        }
        finally
        {
            da.Dispose();
        }
    }

    public async Task<SqlCommand> ExecuteNonQueryProcedure_OutPutParameter(string query, params SqlParameter[] parameters)
    {
        int retval = 0;
        SqlConnection cnn = con;
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.CommandTimeout = 300;
        SqlTransaction trans = null;
        if (query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
        {
            cmd.CommandType = CommandType.Text;
        }
        else
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        for (int i = 0; i <= parameters.Length - 1; i++)
        {
            cmd.Parameters.Add(parameters[i]);
        }
        try
        {

            cnn.Open();
            trans = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = trans;
            retval = await cmd.ExecuteNonQueryAsync();
            trans.Commit();
        }
        catch 
        {
            trans.Rollback();
            throw;
        }
        finally
        {
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
        }
        return cmd;
    }
}


