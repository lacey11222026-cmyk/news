using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibGraph
{
    [Serializable]
    public class ApiResponse
    {
        public List<OriginalPart> original_parts { get; set; }
        public List<ReplacementPart> replacement_parts { get; set; }
    }
    public class OriginalPart
    {
        public int mark_id { get; set; }
        public int bl_code { get; set; }
        public bool is_original { get; set; }
        public string mark { get; set; }
        public string part { get; set; }
        public string part_no_raw { get; set; }
        public string name { get; set; }
        public string name_eng { get; set; }
        public string name_rus { get; set; }
        public int price_yen { get; set; }
        public int price_rub { get; set; }
        public double weight { get; set; }
        public bool is_discontinued { get; set; }
        public object discontinued_title { get; set; }
        public bool is_replaced { get; set; }
        public int type_id { get; set; }
    }

    public class ReplacementPart
    {
        public int mark_id { get; set; }
        public int bl_code { get; set; }
        public bool is_original { get; set; }
        public string mark { get; set; }
        public string part { get; set; }
        public string part_no_raw { get; set; }
        public string name { get; set; }
        public string name_eng { get; set; }
        public string name_rus { get; set; }
        public int price_yen { get; set; }
        public int price_rub { get; set; }
        public double weight { get; set; }
        public bool is_discontinued { get; set; }
        public object discontinued_title { get; set; }
        public bool is_replaced { get; set; }
        public int type_id { get; set; }
        public string alt_mark { get; set; }
        public string alt_part { get; set; }
    }

   
}
