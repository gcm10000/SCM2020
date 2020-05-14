using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM2020___Utility
{
    class SCMAccess
    {
        public const string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""C:\Users\Gabriel\Documents\SISTEMA STK.MDB""";
        OleDbConnection aConnection;
        OleDbCommand aCommand;
        public SCMAccess(string ConnectionString, string TableName)
        {
            this.aConnection = new OleDbConnection(ConnectionString);
            this.aCommand = new OleDbCommand($"SELECT * FROM {TableName}", aConnection);
        }
        public List<List<KeyValuePair<string, string>>> GetDataFromTable()
        {
            List<List<KeyValuePair<string, string>>> records = new List<List<KeyValuePair<string, string>>>();
            try
            {
                aConnection.Open();
                //cria o objeto datareader para fazer a conexao com a tabela
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Faz a interação com o banco de dados lendo os dados da tabela
                while (aReader.Read())
                {
                    List<KeyValuePair<string, string>> record = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < aReader.FieldCount; i++)
                    {
                        record.Add(new KeyValuePair<string, string>(aReader.GetName(i).ToString(), aReader.GetValue(i).ToString()));
                    }
                    records.Add(record);
                }
                //fecha o reader
                aReader.Close();
                //fecha a conexao
                aConnection.Close();
                return records;
            }
            //Trata a exceção
            catch (OleDbException e)
            {
                Console.WriteLine("Error: {0}", e.Errors[0].Message);
                return null;
            }
        }
    }
}
