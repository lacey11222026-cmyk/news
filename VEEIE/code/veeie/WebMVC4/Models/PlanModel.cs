using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    public class PlanItemModel : DATA.PlanItem
    {
        public string Currency1 { get; set; }
        public float CurrencyRate1 { get; set; }
        public float PlanYear1 { get; set; }
        public float PlanCurrent1 { get; set; }
        public float PlanQ1 { get; set; }
        public float BalanceYear1 { get; set; }
        public float Balance1 { get; set; }

        public string Currency2 { get; set; }
        public float CurrencyRate2 { get; set; }
        public float PlanYear2 { get; set; }
        public float PlanCurrent2 { get; set; }
        public float PlanQ2 { get; set; }
        public float BalanceYear2 { get; set; }
        public float Balance2 { get; set; }


        public string Currency3 { get; set; }
        public float CurrencyRate3 { get; set; }
        public float PlanYear3 { get; set; }
        public float PlanCurrent3 { get; set; }
        public float PlanQ3 { get; set; }
        public float BalanceYear3 { get; set; }
        public float Balance3 { get; set; }

        public PlanItemData Item1 { get; set; }
        public PlanItemData Item2 { get; set; }
        public PlanItemData Item3 { get; set; }
    }
    public class PlanItemData
    {
        public  PlanItemData(string currency,float currencyRate, float planYear, float planCurrent, float planQ, float balanceYear, float balance)
        {
            Currency = currency;
            CurrencyRate = currencyRate;
            PlanYear = planYear;
            PlanCurrent = planCurrent;
            PlanQ = planQ;
            BalanceYear = balanceYear;
            Balance = balance;
        }
        public PlanItemData()
        {
            Currency = "VND";
            CurrencyRate = 1;
            PlanYear = 0;
            PlanCurrent = 0;
            PlanQ = 0;
            BalanceYear = 0;
            Balance = 0;
        }
        public string Currency { get; set; }
        public float CurrencyRate { get; set; }
        public float PlanYear { get; set; }
        public float PlanCurrent { get; set; }
        public float PlanQ { get; set; }
        public float BalanceYear { get; set; }
        public float Balance { get; set; }
    }
}