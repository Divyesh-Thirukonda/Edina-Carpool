using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using TMPro;

public class EmailFactory : MonoBehaviour
{
    public void ActuallySendText () {
        GameObject.Find("UI Script").GetComponent<FBscript>().GetPhoneNumberEnum();
        SendText(gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text); //
    }

    

    public void SendText(string phoneNumber)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;

        mail.From = new MailAddress("divyesh.thirukonda@gmail.com");
        

        mail.To.Add(new MailAddress(phoneNumber + "@txt.att.net"));
        mail.To.Add(new MailAddress(phoneNumber + "@vtext.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@messaging.sprintpcs.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@tmomail.net"));
        mail.To.Add(new MailAddress(phoneNumber + "@vmobl.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@messaging.nextel.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@myboostmobile.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@message.alltel.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@mms.ee.co.uk"));
        mail.To.Add(new MailAddress(phoneNumber + "@mobiletxt.ca"));
        mail.To.Add(new MailAddress(phoneNumber + "@vmpix.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@vzwpix.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@pm.sprint.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@msg.fi.google.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@mms.cricketwireless.net"));
        mail.To.Add(new MailAddress(phoneNumber + "@myboostmobile.com"));
        mail.To.Add(new MailAddress(phoneNumber + "@mms.att.net"));
        mail.To.Add(new MailAddress(phoneNumber + "@mms.alltelwireless.com"));
        // replace with                         + User.PhoneProvider



        mail.Subject = "Edina Uber";

        mail.Body = "";
        SmtpServer.Port = 587;

        SmtpServer.Credentials = new System.Net.NetworkCredential("divyesh.thirukonda@gmail.com", "befegklgestvzwuv") as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        Debug.Log("one");

        StartCoroutine(oooo(mail, SmtpServer));
        
        
    }

    private IEnumerator oooo(MailMessage maill, SmtpClient clintt) {

        yield return new WaitForSeconds(5);

        maill.Body = FBscript.msg;
        clintt.Send(maill);
        Debug.Log("Sent: " + FBscript.msg);



    }
}