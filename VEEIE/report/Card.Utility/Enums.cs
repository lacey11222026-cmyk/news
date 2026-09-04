using System;
using System.Configuration;
namespace Car.Utility
{
    public class Enums
    {
        public enum FunctionType
        {
            IsView = 0,
            IsInsert = 1,
            IsUpdate = 2,
            IsDelete = 3,
            IsFullControl = 4,
        }

        public enum UserType
        {
            Admin = 1,
            Level1 = 2,
            Level2 = 3,
            Level3 = 4
        }
        public enum Status
        {
            Lock = -2,
            Success = 3,
        }
        public enum CampaignStatus
        {
            Init = 0,
            Waiting = 1,
            InProcess = 2,
            Success = 3,
            Lock = 4,
        }
        public enum TelcoType
        {
            VTT = 1,
            VMS = 2,
            VNP = 3,

        }
    }
    public static class FunctionCode
    {
        public const string Users = "users";
        public const string Revenue = "Revenue";
        public const string Function = "function";
        public const string UserLog = "userlog";
        public const string Project = "project";
        public const string Report = "report";
        public const string BankReport = "bankreport";
        public const string AdminReport = "adminreport";
        public const string Pay = "pay";
        public const string Finance = "finance";
        public const string Group = "group";
    }

}
