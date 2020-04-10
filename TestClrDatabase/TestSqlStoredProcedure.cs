//------------------------------------------------------------------------------
// <copyright file="CSSqlStoredProcedure.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TestSqlStoredProcedure()
    {
        SqlContext.Pipe.Send("This text is from the sample SQLCLR procedure MySQLCLR3\n");
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select @@version", connection);
            SqlContext.Pipe.ExecuteAndSend(command);
            //command.CommandText = "insert into Test(Name) values('nnn')";
            //SqlContext.Pipe.ExecuteAndSend(command);

            command.CommandText = "SELECT * FROM Test";
            SqlDataReader reader = command.ExecuteReader();
            SqlContext.Pipe.Send(reader);
        }

        //SqlConnection conn = null;
        //try
        //{
        //    conn = new SqlConnection(@"server = .;integrated security = true;database = Test");

        //    SqlCommand cmdInsertCurrency = new SqlCommand();
        //    cmdInsertCurrency.Connection = conn;

        //    SqlParameter parmCurrencyCode = new SqlParameter("@CCode", SqlDbType.NVarChar, 3);
        //    SqlParameter parmCurrencyName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);

        //    parmCurrencyCode.Value = "";
        //    parmCurrencyName.Value = "";

        //    cmdInsertCurrency.Parameters.Add(parmCurrencyCode);
        //    cmdInsertCurrency.Parameters.Add(parmCurrencyName);

        //    cmdInsertCurrency.CommandText =
        //    "INSERT Sales.Currency (CurrencyCode, CurrencyName, ModifiedCurrencyDate)" +
        //    " VALUES(@CCode, @Name, GetDate())";

        //    conn.Open();

        //    cmdInsertCurrency.ExecuteNonQuery();
        //}

        //catch (SqlException ex)
        //{
        //    SqlContext.Pipe.Send("An error occured" + ex.Message + ex.StackTrace);
        //}

        //finally
        //{
        //    conn.Close();
        //}
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TestSqlStoredGetVersionNumber()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost/EliteCommonWebService.asmx?op=GetVersionNumber");
        request.Method = "GET";
        request.ContentLength = 0;
        request.Credentials = CredentialCache.DefaultCredentials;
        request.ContentType = "application/xml";
        request.Accept = "application/xml";
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (Stream receiveStream = response.GetResponseStream())
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    string strContent = readStream.ReadToEnd();
                    SqlContext.Pipe.Send(strContent);
                    //XmlDocument xdoc = new XmlDocument();
                    //xdoc.LoadXml(strContent);
                    ////Main Logic Begins here
                    //SqlPipe pipe = SqlContext.Pipe;
                    //SqlMetaData[] cols = new SqlMetaData[2];
                    //cols[0] = new SqlMetaData("CurrencyName", SqlDbType.NVarChar, 1024);
                    ////cols[1] = new SqlMetaData("Amount", SqlDbType.Money);
                    //foreach (XmlNode xnCurrency in xdoc.DocumentElement.ChildNodes)
                    //{
                    //    SqlDataRecord record = new SqlDataRecord(cols);
                    //    pipe.SendResultsStart(record);
                    //    record.SetSqlString(0, new SqlString(xnCurrency.Attributes["CurrencyName"].Value));
                    //    record.SetSqlMoney(1, new SqlMoney(decimal.Parse(xnCurrency.Attributes["Amount"].Value)));
                    //}
                    //pipe.SendResultsEnd();
                }
            }
        }

    }
}

