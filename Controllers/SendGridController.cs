using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendGridController : Controller
    {
        private readonly ISendGridClient sendGridClient;
        private readonly ILogger<SendGridController> logger;

        public SendGridController(ISendGridClient sendGridClient, ILogger<SendGridController> logger)
        {
            this.sendGridClient = sendGridClient;
            this.logger = logger;
        }
        [Route("Send")]
        [HttpGet]
        public async Task<IActionResult> Send(string toEmail)
        {
            logger.LogDebug("Start Test SendGrid");
            var from = new EmailAddress("javasorn@gmail.com", "Example User");
            var to = new EmailAddress(toEmail, "Example User");
            var subject = "Sending with SendGrid is Fun";
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await sendGridClient.SendEmailAsync(msg);
            logger.LogDebug("response code:" + response.StatusCode
                + ",IsSuccessStatusCode:" + response.IsSuccessStatusCode
                + ",Headers:" + response.Headers
                + ",Body:" + response.Body
                );
            logger.LogDebug("End Test SendGrid");
            return Ok("StatusCode:"+response.StatusCode + ", IsSuccessStatusCode:" + response.IsSuccessStatusCode);
        }
        [Route("SendCal")]
        [HttpGet]
        public async Task<IActionResult> SendCal(string toEmail)
        {
            // refer https://stackoverflow.com/questions/45076896/send-email-as-calendar-invite-appointment-in-sendgrid-c-sharp
            var from = new EmailAddress("javasorn@gmail.com", "Example User");
            var to = new EmailAddress(toEmail, "Example User");
            var subject = "Sending calendar with SendGrid";
            var plainTextContent = "Sending calendar and easy to do anywhere, even with C#";
            var htmlContent = "<strong>Sending calendar and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            string CalendarContent = MeetingRequestString("{ORGANIZER}", new List<string>() 
                { "{ATTENDEE}" }, "{subject}", "{description}", "{location}",DateTime.Now, DateTime.Now.AddDays(2));
            byte[] calendarBytes = Encoding.UTF8.GetBytes(CalendarContent.ToString());
            SendGrid.Helpers.Mail.Attachment calendarAttachment = new SendGrid.Helpers.Mail.Attachment();
            calendarAttachment.Filename = "invite.ics";
            //the Base64 encoded content of the attachment.
            calendarAttachment.Content = Convert.ToBase64String(calendarBytes);
            calendarAttachment.Type = "text/calendar";
            msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>() { calendarAttachment };

            var response = await sendGridClient.SendEmailAsync(msg);

            return Ok("StatusCode:" + response.StatusCode + ", IsSuccessStatusCode:" + response.IsSuccessStatusCode);
        }
        private static string MeetingRequestString(string from, List<string> toUsers, string subject, string desc, string location, DateTime startTime, DateTime endTime, int? eventID = null, bool isCancel = false)
        {
            StringBuilder str = new StringBuilder();

            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
            str.AppendLine("VERSION:2.0");
            str.AppendLine(string.Format("METHOD:{0}", (isCancel ? "CANCEL" : "REQUEST")));
            str.AppendLine("BEGIN:VEVENT");

            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime.ToUniversalTime()));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.Now));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime.ToUniversalTime()));
            str.AppendLine(string.Format("LOCATION: {0}", location));
            str.AppendLine(string.Format("UID:{0}", (eventID.HasValue ? "blablabla" + eventID : Guid.NewGuid().ToString())));
            str.AppendLine(string.Format("DESCRIPTION:{0}", desc.Replace("\n", "<br>")));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", desc.Replace("\n", "<br>")));
            str.AppendLine(string.Format("SUMMARY:{0}", subject));

            str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", from, from));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", string.Join(",", toUsers), string.Join(",", toUsers)));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            return str.ToString();
        }
    }
}
