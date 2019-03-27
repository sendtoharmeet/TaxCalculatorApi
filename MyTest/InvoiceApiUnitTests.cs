using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculator.Controllers;
using Service;
using Service.Model;

namespace MyTest
{
    [TestClass]
    public class InvoiceApiUnitTests
    {
        IMessageProcessor messageProcessor;
        public InvoiceApiUnitTests()
        {
            messageProcessor = new MessageProcessor();
        }

        [TestMethod]
        public void ProcessEmbeddedXmlTest()
        {
            var controller = new InvoiceController(messageProcessor);
            string inputMessage = @"Hi Yvaine, Please create an expense claim for the below. Relevant details are marked up as requested…  <expense><cost_centre>DEV002</cost_centre><total>112.0</total><payment_method>personal card</payment_method> </expense>From: Ivan Castle Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com>Subject: test Hi Antoine, Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team's project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day. Regards, Ivan";

            IHttpActionResult actionResult = controller.ProcessMessage(inputMessage);
            var response = actionResult as OkNegotiatedContentResult<Invoice>;
            Assert.AreEqual(response.Content.ExpenseDetail.GstTax.ToString(), "12.00");
            Assert.AreEqual(response.Content.ExpenseDetail.TotalExcludingGst.ToString(), "100.00");
            Assert.AreEqual(response.Content.ExpenseDetail.TotalIncludingTax.ToString(), "112.0");
            Assert.AreEqual(response.Content.ExpenseDetail.PaymentMethod, "personal card");
            Assert.AreEqual(response.Content.ExpenseDetail.CostCentre, "DEV002");
            Assert.AreEqual(response.Content.Description, "development team's project end celebration dinner");
            Assert.AreEqual(response.Content.DateText, "Tuesday 27 April 2017");
            Assert.AreEqual(response.Content.Vendor, "Viaduct Steakhouse");
        }
        
        [TestMethod]
        public void ProcessEmbeddedXmlMissingCostCentreTest()
        {
            var controller = new InvoiceController(messageProcessor);
            string inputMessage = @"Hi Yvaine, Please create an expense claim for the below. Relevant details are marked up as requested…  <expense><total>112.0</total><payment_method>personal card</payment_method> </expense>From: Ivan Castle Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com>Subject: test Hi Antoine, Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team's project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day. Regards, Ivan";

            IHttpActionResult actionResult = controller.ProcessMessage(inputMessage);
            var response = actionResult as OkNegotiatedContentResult<Invoice>;
            Assert.AreEqual(response.Content.ExpenseDetail.GstTax.ToString(), "12.00");
            Assert.AreEqual(response.Content.ExpenseDetail.TotalExcludingGst.ToString(), "100.00");
            Assert.AreEqual(response.Content.ExpenseDetail.TotalIncludingTax.ToString(), "112.0");
            Assert.AreEqual(response.Content.ExpenseDetail.PaymentMethod, "personal card");
            Assert.AreEqual(response.Content.ExpenseDetail.CostCentre, "UNKNOWN");
            Assert.AreEqual(response.Content.Description, "development team's project end celebration dinner");
            Assert.AreEqual(response.Content.DateText, "Tuesday 27 April 2017");
            Assert.AreEqual(response.Content.Vendor, "Viaduct Steakhouse");
        }

        [TestMethod]
        public void ProcessEmbeddedXmlMissingTotalAmountTest()
        {
            var controller = new InvoiceController(messageProcessor);
            string inputMessage = @"Hi Yvaine, Please create an expense claim for the below. Relevant details are marked up as requested…  <expense><payment_method>personal card</payment_method> </expense>From: Ivan Castle Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com>Subject: test Hi Antoine, Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team's project end celebration dinner on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day. Regards, Ivan";

            IHttpActionResult actionResult = controller.ProcessMessage(inputMessage);
            var response = actionResult as BadRequestErrorMessageResult;
            Assert.AreEqual(response.Message, "Invalid input message");
        }

        [TestMethod]
        public void ProcessEmbeddedXmlMissingClosingTagTest()
        {
            var controller = new InvoiceController(messageProcessor);
            string inputMessage = @"Hi Yvaine, Please create an expense claim for the below. Relevant details are marked up as requested…  <expense><total>112.0</total><payment_method>personal card</payment_method> </expense>From: Ivan Castle Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com>Subject: test Hi Antoine, Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team's project end celebration dinner on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day. Regards, Ivan";

            IHttpActionResult actionResult = controller.ProcessMessage(inputMessage);
            var response = actionResult as BadRequestErrorMessageResult;
            Assert.AreEqual(response.Message, "Invalid input message");
        }

        [TestMethod]
        public void ProcessEmbeddedXmlMissingMessageTest()
        {
            var controller = new InvoiceController(messageProcessor);
            string inputMessage = @"";

            IHttpActionResult actionResult = controller.ProcessMessage(inputMessage);
            var response = actionResult as BadRequestErrorMessageResult;
            Assert.AreEqual(response.Message, "Invalid input message");
        }
    }
}
