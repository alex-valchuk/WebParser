using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Mail
/// </summary>
public class Mail
{
    /// <summary>
    /// Send error message to admin
    /// </summary>
    /// <param name="ex"></param>
    public static void SendErrorMessage(Exception ex)
    {
        string to = ConfigurationManager.AppSettings["admin_address"];
        string from = ConfigurationManager.AppSettings["server_address"];
        string subject = "Error in your web site.";
        string body = String.Format("Message: {0} \r\n<br />Source: {1} \r\n<br />StackTrace: {2} \r\n<br />", ex.Message, ex.Source, ex.StackTrace);
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to, subject, body);
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("www.1gb.ru");
        client.Timeout = 10000;
        client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        client.Send(message);
    }
}
