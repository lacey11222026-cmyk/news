using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;
namespace SMS
{
    public class SMSBO
    {
        public int Send(string phonenumber, string content, string code)
        {

            try
            {
                var sms = new SMSContextDataContext();
                var sendObj = new Outbox
                {
                    Content = content,
                    PartnerCode = code,
                    CreatedTime = DateTime.Now,
                    SendTime = DateTime.Now,
                    Status = 0,
                    PhoneNumber = phonenumber

                };

                sms.CommandTimeout = 10;
                sms.Outboxes.InsertOnSubmit(sendObj);

                sms.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                 ExHandler.Handle(ex, "SendSMS");
                 return -1;
            }
        }
        public int MultiSend(string phonenumbers, string content, string code)
        {
            var listphone = phonenumbers.Split(',');
            foreach (var phone in listphone)
            {
                if (phone.Length > 9)
                    Send(phone, content,code);
            }
            return 1;
        }
        public List<SMS_Statistic> GetReport(string code, int year)
        {
            
            var sms = new SMSContextDataContext();
            var data =sms.sp_Outbox_ByReport(code, year).ToList();
            if (data == null)
                return null;
            var result = new List<SMS_Statistic>();
            var lstmonth = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            foreach (var m in lstmonth)
            {
                var item = data.Where(x => x.Month == m).FirstOrDefault();
                if (item != null)
                    result.Add(item);
                else
                    result.Add(new SMS_Statistic { Month = m, Number = 0 });

            }

            return result;
        }
    }
}
