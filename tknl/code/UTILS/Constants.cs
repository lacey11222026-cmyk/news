namespace UTILS
{
    public static class Constants
    {
        internal const int DefaultExpire = -1;
        internal const int NeverExpire = 0;
        internal const int TenSecondsDataCacheTime = 10;
        internal const int MinDataCacheTime = 60 / 2;
        internal const int OneMinuteExpire = 60 * 2;
        internal const int FiveMinuteExpire = 300;
        internal const int OneHourExpire = 3600;
        internal const int OneDayExpire = 86400;
        // Connection String
        internal const string SHOP_ONLINE_CONNECTION_STRING = "shoponlineconn";
        internal const string MEMBERSHIP_CONNECTION_STRING = "membershipconn";
        internal const string EXCEPTION_POLICY = "Global Policy";
        public enum NewsStatus
        {
            All = -1,
            Disable = 0,
            Publish = 1,
            Draft = 2,
            EditWait = 3,
            Editting = 4,
            PublishWait = 5,
            Reject=6,
            EditReject = 7,
        }
        public enum NewsAction
        {

            Publish = 1,
            SendPublish = 2,
            Reject = 3,
            Save = 4,
            Delete = 5,
            SendEdit = 6,
            Down = 7,
            Restore = 8,
            RejectBT = 9,
            GetBack = 10,
        }
        //public enum FilterType
        //{
        //    NoFilter = 0,
        //    ByValue = 1,
        //    ByRange = 2,
        //    ByMultiValue = 3
        //}

        //public enum FilterDataType
        //{
        //    String = 0,
        //    Double = 1,
        //    Bit = 2
        //}

        public enum BannerPosition
        {
            None = -1,
            Main = 1,
            Left = 2,
            LeftBottom = 3
            
           
        }

        public enum CategoryPosition
        {
            None = -1,
            Homepage = 0,
            RightCol = 1,
            MainMenu = 2,
            TopMenu = 3,
            Footer = 4
        }

        public enum CategoryType
        {
            
            None = -1,
            Product = 0,
            Intro = 1,
            News =  2,
            Other = 3,

            Album = 4,
            Doc = 5,
             Contact = 6,
            Banner = 7,
            System = 10,
            Cate = 9,
            TestRegistor = 11,
            TestQuestion = 12,
        }

        //public enum BonusType
        //{
        //    DiscountPercent = 0,            
        //}
        //public enum NewsStatus
        //{
        //    All = -1,
        //    Editing = 1,
        //    Waiting = 2,
        //    Reject = 3,
        //    Publish = 4
        //}
        public enum NewsType
        {
            All = -1,
            Normal = 1,
            Video = 2,
            Music = 3
           
        }

    }

    public static class OtherPage
    {
        public static int EngPage
        {
            get
            {
                return 10001;
            }
        }
        public static int PhotoPage
        {
            get
            {
                return 10002;
            }
        }
        public static int ATGTVideoPage
        {
            get
            {
                return 10003;
            }
        }
        public static int ATGTPhotoPage
        {
            get
            {
                return 10004;
            }
        }
    }

    public static class EntityName
    {
        public static string Product
        {
            get
            {
                return "Product";
            }
        }
        public static string Comment
        {
            get
            {
                return "Comment";
            }
        }
        public static string Intro
        {
            get
            {
                return "Intro";
            }
        }
        public static string Products
        {
            get
            {
                return "Products";
            }
        }
        public static string Channel
        {
            get
            {
                return "Channel";
            }
        }
        public static string Category
        {
            get
            {
                return "Category";
            }
        }

        public static string Categories
        {
            get
            {
                return "Categories";
            }
        }

        public static string Article
        {
            get
            {
                return "Article";
            }
        }

        public static string Articles
        {
            get
            {
                return "Articles";
            }
        }

        public static string Attribute
        {
            get
            {
                return "Attribute";
            }
        }

        public static string Attributes
        {
            get
            {
                return "Attributes";
            }
        }
        public static string Album
        {
            get
            {
                return "Album";
            }
        }

        public static string Albums
        {
            get
            {
                return "Albums";
            }
        }
        public static string Support
        {
            get
            {
                return "Support";
            }
        }

        public static string Supports
        {
            get
            {
                return "Supports";
            }
        }



    }

}
