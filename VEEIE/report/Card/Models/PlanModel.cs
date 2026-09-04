using Car.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car.CMS.Models
{
    public class PlanDetail
    {
        public Plan Plan { get; set; }
        public List<PlanStuck>  PlanStuck { get; set; }
        public List<PlanRequire> PlanRequire { get; set; }

        public List<PlanItemModel> PlanItemAll { get; set; }
        public List<PlanItemModel> PlanItem { get; set; }
        public List<PlanItemModel> PlanItemBidv { get; set; }
        public List<PlanRequire> PlanRequireBidv { get; set; }
    }


    public class PlanItemModel : PlanItem
    {
        public string Currency1 { get; set; }
        public long CurrencyRate1 { get; set; }
        public long PlanYear1 { get; set; }
        public long PlanCurrent1 { get; set; }
        public long PlanQ1 { get; set; }
        public long BalanceYear1 { get; set; }
        public long Balance1 { get; set; }
        public long Money1 { get; set; }

        public string Currency2 { get; set; }
        public long CurrencyRate2 { get; set; }
        public long PlanYear2 { get; set; }
        public long PlanCurrent2 { get; set; }
        public long PlanQ2 { get; set; }
        public long BalanceYear2 { get; set; }
        public long Balance2 { get; set; }
        public long Money2 { get; set; }

        public string Currency3 { get; set; }
        public long CurrencyRate3 { get; set; }
        public long PlanYear3 { get; set; }
        public long PlanCurrent3 { get; set; }
        public long PlanQ3 { get; set; }
        public long BalanceYear3 { get; set; }
        public long Balance3 { get; set; }
        public long Money3 { get; set; }
        public PlanItemData Item1 { get; set; }
        public PlanItemData Item2 { get; set; }
        public PlanItemData Item3 { get; set; }
    }
    public class PlanItemData
    {
        public PlanItemData(string currency, long currencyRate, long planYear, long planCurrent, long planQ, long balanceYear, long balance, long money)
        {
            Currency = currency;
            CurrencyRate = currencyRate;
            PlanYear = planYear;
            PlanCurrent = planCurrent;
            PlanQ = planQ;
            BalanceYear = balanceYear;
            Balance = balance;
            Money = money;
        }
        public PlanItemData()
        {
            Currency = "VND";
            CurrencyRate = 23000;
            PlanYear = 0;
            PlanCurrent = 0;
            PlanQ = 0;
            BalanceYear = 0;
            Balance = 0;
            Money = 0;
        }
        public string Currency { get; set; }
        public long CurrencyRate { get; set; }
        public long PlanYear { get; set; }
        public long PlanCurrent { get; set; }
        public long PlanQ { get; set; }
        public long BalanceYear { get; set; }
        public long Balance { get; set; }
        public long Money { get; set; }
    }
}