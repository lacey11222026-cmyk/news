using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.ContentDB
{
    public class Idea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Code { get; set; }
        public string No { get; set; }
        public string FilePath { get; set; }
        public string Proposer { get; set; }
        public string Unit { get; set; }
        public string Mark { get; set; }

        public string Effective { get; set; }
        public string Followers { get; set; }
        public int Status { get; set; }
        public int Progress { get; set; }
        public int ProgressPercent { get; set; }
        public int Result { get; set; }
        public int Region { get; set; }

        //public IdeaConfig FollowersConfig { get; set; }
        //public IdeaConfig ProposerConfig { get; set; }
        public string SPublishDate
        {
            get;
            set;
        }
        public string FileName
        {
            get
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    try
                    {
                        var listf = FilePath.Trim().Split('/');
                        return listf[listf.Length - 1].ToString();
                    }
                    catch
                    {

                        return String.Empty;
                    }
                }
                return String.Empty;
            }
        }
    }
    public class IdeaTemp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string SPublishDate
        {
            get;
            set;
        }
        public string Code { get; set; }
        public string No { get; set; }
        public string FilePath { get; set; }
        public string Proposer { get; set; }
        public string Unit { get; set; }
        public string Mark { get; set; }

        public string Effective { get; set; }
        public string Followers { get; set; }
        //public int Status { get; set; }
        //public int Progress { get; set; }
        public string ProgressPercent { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }

        public string Description { get; set; }
        public int Type { get; set; }
    }
       
    
    //public class IdeaConfig
    //{




    //    public string Role { get; set; }
    //    public string Email { get; set; }
    //    public string Mobile { get; set; }
    //    public string Fullname { get; set; }
    //}
}
