using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAliHealthConsoleApp
{

    public class Rootobject
    {
        public Alibaba_Alihealth_Drug_Kyt_Listparts_Response alibaba_alihealth_drug_kyt_listparts_response { get; set; }
    }

    public class Alibaba_Alihealth_Drug_Kyt_Listparts_Response
    {
        public Result result { get; set; }
        public string request_id { get; set; }
    }

    public class Result
    {
        public Model model { get; set; }
        public string msg_code { get; set; }
        public string msg_info { get; set; }
        public bool response_success { get; set; }
    }

    public class Model
    {
        public Result_List result_list { get; set; }
        public int total_num { get; set; }
    }

    public class Result_List
    {
        public P_Ent_Par_Dto[] p_ent_par_dto { get; set; }
    }

    public class P_Ent_Par_Dto
    {
        public string area_name { get; set; }
        public string city_name { get; set; }
        public string crt_date { get; set; }
        public string crt_ic_code { get; set; }
        public string crt_ic_name { get; set; }
        public string ent_id { get; set; }
        public string ent_prov_code { get; set; }
        public string is_network { get; set; }
        public string last_mod_date { get; set; }
        public string p_ent_par_id { get; set; }
        public string partner_capital_name { get; set; }
        public string partner_ent_id { get; set; }
        public string partner_id { get; set; }
        public string partner_name { get; set; }
        public string partner_type { get; set; }
        public string prov_name { get; set; }
        public string ref_ent_id { get; set; }
        public string status { get; set; }
    }
}
