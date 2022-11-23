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
    private static readonly ILog _log = LogManager.GetLogger(typeof(MailServiceTest));
    private readonly string smtp_authentication_password = "WINUEHSQUUAANQCN";
    [TestMethod]
    public void SendMailTest()
    {
        // 数据
        var mailFrom = "NorfloxBai20011230@126.com";
        var mailTo = "1762161822@qq.com";
        var mailSubject = "测试邮件";
        var mailBody = "这是一封测试邮件";

        // SmtpClient
        var smtpClient = new SmtpClient("smtp.126.com", 25)
        {
            Credentials = new NetworkCredential(mailFrom, smtp_authentication_password),
            EnableSsl = true
        };

        // MailMessage
        var mailMessage = new MailMessage(mailFrom, mailTo, mailSubject, mailBody);

        // 发送邮件
        try{
            smtpClient.Send(mailMessage);
            _log.Info("发送成功");
            _log.Info("发送方: " + mailFrom);
            _log.Info("接收方: " + mailTo);
            _log.Info("主题: " + mailSubject);
            _log.Info("内容: " + mailBody);
        }
        catch (SmtpException e)
        {
            _log.Error(e);
        }
    }
}

}