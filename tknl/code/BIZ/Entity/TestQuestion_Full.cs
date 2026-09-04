using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{
    [Serializable]
    public class TestQuestion_Full : DATA.TestQuestion
    {
        public List<AnswerInfo> AnswersInfo
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }
    }
    [Serializable]
    public class TestQuestionArchive : DATA.TestQuestion
    {
        public List<AnswerArchiveInfo> AnswerArchiveInfo
        {
            get;
            set;
        }
    }

    public class ArchiveInfo
    {
        public int Question
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }
    }

    public class AnswerInfo
    {
        public int Order
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool IsCheck
        {
            get;
            set;
        }
    }
    public class AnswerArchiveInfo
    {
        public int Order
        {
            get;
            set;
        }
        //public int Mark
        //{
        //    get;
        //    set;
        //}
        public string Name
        {
            get;
            set;
        }
        public bool IsCheck
        {
            get;
            set;
        }
        public bool IsUserCheck
        {
            get;
            set;
        }
    }
}
