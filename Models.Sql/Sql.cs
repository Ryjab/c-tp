using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAirport.Pim.Entities;

namespace MyAirport.Pim.Models
{
    public class Sql : AbstractDefinition
    {
        private string strCnx = ConfigurationManager.ConnectionStrings["MyAirport.Pim.Settings.DbConnect"].ConnectionString;

        private string commandGetBagageIata = "SELECT b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.ligne,  b.DATE_CREATION, b.ESCALE, b.CLASSE," +
            "CAST(iif(bp.ID_PARTICULARITE is null, 0, 1) as bit) as 'RUSH'," + "CAST(iif(b.EN_CONTINUATION like 'N', 0,1) as bit) as 'CONT'," +
            "CAST(iif(b.PRIORITAIRE like '0', 0,1) as bit) as 'PRIO'" + "FROM BAGAGE b left outer join BAGAGE_A_POUR_PARTICULARITE bp on(bp.ID_BAGAGE > 0 and bp.ID_PARTICULARITE = 15) where b.CODE_IATA like '%@code_iata%';";

        private string commandGetBagageId = "SELECT b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.ligne,  b.DATE_CREATION, b.ESCALE, b.CLASSE," +
            "CAST(iif(bp.ID_PARTICULARITE is null, 0, 1) as bit) as 'RUSH'," + "CAST(iif(b.EN_CONTINUATION like 'N', 0,1) as bit) as 'CONT'," +
            "CAST(iif(b.PRIORITAIRE like '0', 0,1) as bit) as 'PRIO'" + "FROM BAGAGE b left outer join BAGAGE_A_POUR_PARTICULARITE bp on(bp.ID_BAGAGE > 0 and bp.ID_PARTICULARITE = 15) where b.ID_BAGAGE = '@id';";



        private List<BagageDefinition> listBagage;
        public override BagageDefinition GetBagage(int idBagage)
        {
            BagageDefinition bagRes = null;
            using (SqlConnection cnx = new SqlConnection(strCnx)) { 
                    SqlCommand cmd = new SqlCommand(this.commandGetBagageId, cnx);
                cmd.Parameters.AddWithValue("@id", idBagage);
                cnx.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if(sdr.Read())
                {
                    bagRes = new BagageDefinition()
                    {
                        IdBagage = sdr.GetInt32(sdr.GetOrdinal("ID_BAGAGE")),
                        CodeIata = sdr.GetString(sdr.GetOrdinal("CODE_IATA")),
                        EnContinuation = sdr.GetBoolean(sdr.GetOrdinal("CONT")),
                        Compagnie = sdr.GetString(sdr.GetOrdinal("COMPAGNIE")),
                        Rush = sdr.GetBoolean(sdr.GetOrdinal("RUSH")),
                        Prioritaire = sdr.GetBoolean(sdr.GetOrdinal("PRIO")),
                        DateVol = sdr.GetDateTime(sdr.GetOrdinal("DATE_CREATION")),
                        Ligne = sdr.GetString(sdr.GetOrdinal("LIGNE")),
                        Jour_Exploitation = sdr.GetInt32(sdr.GetOrdinal("JOUR_EXPLOITATION"))
                    };
                }
            }
            return bagRes;
        }

        public override List<BagageDefinition> GetBagage(string codeIataBagage)
        {
            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageIata, cnx);
                cmd.Parameters.AddWithValue("@code_iata", codeIataBagage);
                cnx.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                listBagage = new List<BagageDefinition>();
                while (sdr.Read())
                {
                    listBagage.Add(new BagageDefinition()
                    {
                        IdBagage = sdr.GetInt32(sdr.GetOrdinal("ID_BAGAGE")),
                        CodeIata = sdr.GetString(sdr.GetOrdinal("CODE_IATA")),
                        EnContinuation = sdr.GetBoolean(sdr.GetOrdinal("CONT")),
                        Compagnie = sdr.GetString(sdr.GetOrdinal("COMPAGNIE")),
                        Rush =  sdr.GetBoolean(sdr.GetOrdinal("RUSH")),
                        Prioritaire = sdr.GetBoolean(sdr.GetOrdinal("PRIO")),
                        DateVol = sdr.GetDateTime(sdr.GetOrdinal("DATE_CREATION")),
                        Ligne = sdr.GetString(sdr.GetOrdinal("LIGNE")),
                        Jour_Exploitation = sdr.GetInt32(sdr.GetOrdinal("JOUR_EXPLOITATION"))
                    });
                }
            }
            return listBagage;
        }
    }
}
