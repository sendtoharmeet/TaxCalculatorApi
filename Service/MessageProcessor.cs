using Service.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Service
{
    public class MessageProcessor : IMessageProcessor
    {
        Dictionary<string, string> keyValidatorElements;
        Dictionary<string, string> keyProcessingElements;

        public MessageProcessor()
        {
            keyValidatorElements = new Dictionary<string, string>();
            keyValidatorElements.Add("Vendor", "vendor");
            keyValidatorElements.Add("Description", "description");
            keyValidatorElements.Add("Date", "date");
            keyValidatorElements.Add("ExpenseDetail", "expense");
            keyValidatorElements.Add("CostCentre", "cost_centre");
            keyValidatorElements.Add("TotalIncludingTax", "total");
            keyValidatorElements.Add("PaymentMethod", "payment_method");

            keyProcessingElements = new Dictionary<string, string>();
            keyProcessingElements.Add("Vendor", "vendor");
            keyProcessingElements.Add("Description", "description");
            keyProcessingElements.Add("Date", "date");
            keyProcessingElements.Add("CostCentre", "cost_centre");
            keyProcessingElements.Add("TotalIncludingTax", "total");
            keyProcessingElements.Add("PaymentMethod", "payment_method");
        }

        public Invoice ProcessMessage(string inputMessage)
        {
            var objInvoice = new Invoice();
            Expense expense = new Expense();

            foreach (var item in keyProcessingElements)
            {
                var beginingTagIndex = inputMessage.IndexOf(string.Format("<{0}>", item.Value)) + item.Value.Length + 2;
                var closingTagIndex = inputMessage.IndexOf(string.Format("</{0}>", item.Value));

                if (closingTagIndex == -1)
                    continue;

                var tagValue = inputMessage.Substring(beginingTagIndex, (closingTagIndex - beginingTagIndex));
                switch (item.Key)
                {
                    case "Vendor":
                        objInvoice.Vendor = tagValue;
                        break;
                    case "Description":
                        objInvoice.Description = tagValue;
                        break;
                    case "Date":
                        objInvoice.DateText = tagValue;
                        break;
                    case "CostCentre":
                        expense.CostCentre = tagValue;
                        break;
                    case "TotalIncludingTax":
                        var total = tagValue;
                        decimal totalInc;
                        expense.TotalIncludingTax = decimal.TryParse(total, out totalInc) ? totalInc : 0;
                        expense.TotalIncludingTax = decimal.Round(expense.TotalIncludingTax, 2);
                        break;
                    case "PaymentMethod":
                        expense.PaymentMethod = tagValue;
                        break;
                }
            }

            objInvoice.ExpenseDetail = expense;
            
            if (string.IsNullOrWhiteSpace(objInvoice.ExpenseDetail.CostCentre))
            {
                objInvoice.ExpenseDetail.CostCentre = "UNKNOWN";
            }
            if (objInvoice.ExpenseDetail.TotalIncludingTax > 0)
            {
                decimal gstRate = 12;
                objInvoice.ExpenseDetail.TotalExcludingGst = decimal.Round(100 / (100 + gstRate) * objInvoice.ExpenseDetail.TotalIncludingTax, 2);
                objInvoice.ExpenseDetail.GstTax = decimal.Round(objInvoice.ExpenseDetail.TotalIncludingTax - objInvoice.ExpenseDetail.TotalExcludingGst, 2);
            }

            return objInvoice;
        }

        public bool ValidateMessage(string message)
        {
            foreach (var item in keyValidatorElements)
            {
                var beginingTag = Regex.Matches(message, string.Format("<{0}>", item.Value)).Count;
                var closingTag = Regex.Matches(message, string.Format("</{0}>", item.Value)).Count;
                if (beginingTag != closingTag)
                {
                    return false;
                }
            }

            if (Regex.Matches(message, "<total>").Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
