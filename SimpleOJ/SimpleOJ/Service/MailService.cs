// 发送邮件部分
using System.Net;
using System.Net.Mail;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleOJ.Service
{

[TestClass]
public class MailServiceTest
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(MailServiceTest));
    private const string SmtpAuthenticationPassword = "WINUEHSQUUAANQCN";
    [TestMethod]
    public void SendMailTest()
    {
        // 数据
        const string mailFrom = "NorfloxBai20011230@126.com";
        var mailTo = "1762161822@qq.com";
        var mailSubject = "测试邮件";
        var mailBody = "这是一封测试邮件";

        // SmtpClient
        var smtpClient = new SmtpClient("smtp.126.com", 25)
        {
            Credentials = new NetworkCredential(mailFrom, SmtpAuthenticationPassword),
            EnableSsl = true
        };

        // MailMessage
        var mailMessage = new MailMessage(mailFrom, mailTo, mailSubject, mailBody);

        // 发送邮件
        try{
            smtpClient.Send(mailMessage);
            Log.Info("发送成功");
            Log.Info("发送方: " + mailFrom);
            Log.Info("接收方: " + mailTo);
            Log.Info("主题: " + mailSubject);
            Log.Info("内容: " + mailBody);
        }
        catch (SmtpException e)
        {
            Log.Error(e);
        }
    }
}

}