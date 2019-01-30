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

        private string commandGetBagageIata = "SELECT b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.CLASSE," +
            "b.EN_CONTINUATION, cast(iif(bp.ID_PARTICULARITE is null, 0, 1) as bit) as 'RUSH', cast(iif(b.EN_CONTINUATION like 'N', 0, 1) as bit ) as 'CONT', " +
            "cast(iif(b.PRIORITAIRE like '0', 0, 1) as bit) as 'PRIO' " +
            "FROM BAGAGE b left outer join BAGAGE_A_POUR_PARTICULARITE bp on (bp.ID_BAGAGE > 0 and bp.ID_PARTICULARITE = 15) where b.code_iata = @code_iata";

        private string commandGetBagageId = "SELECT b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.CLASSE," +
            "b EN_CONTINUATION, cast(iif(bp.ID_PARTICULARITE is null, 0, 1) as bit) as 'RUSH', cast(iif(b.EN_CONTINUATION like 'N', 0, 1) as bit ) as 'CONT', " +
            "cast(iif(b.PRIORITAIRE like '0', 0, 1) as bit) as 'PRIO' " +
            "FROM BAGAGE b left outer join BAGAGE_A_POUR_PARTICULARITE bp on (bp.ID_BAGAGE > 0 and bp.ID_PARTICULARITE = 15) where b.ID_BAGAGE = @id";

        private List<BagageDefinition> listBagage;
        public override BagageDefinition GetBagage(int idBagage)
        {
            BagageDefinition bagRes = null;
            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
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
                        EnContinuation = sdr.GetBoolean(sdr.GetOrdinal("CONTINUATION")),
                        Compagnie = sdr.GetString(sdr.GetOrdinal("COMPAGNIE"))
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
                        EnContinuation = sdr.GetBoolean(sdr.GetOrdinal("CONTINUATION")),
                        Compagnie = sdr.GetString(sdr.GetOrdinal("COMPAGNIE"))
                    });
                }
            }
            return listBagage;
        }
    }
}
