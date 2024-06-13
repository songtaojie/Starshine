using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 邮箱发送帮助类
    /// </summary>
    public class EmailHelper
    {
        #region Eail 属性

        private string _mailFrom = "system@hxblog.net";
        /// <summary>
        /// 发送者
        /// </summary>
        public string MailFrom
        {
            get { return _mailFrom; }
            set { _mailFrom = value; }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] MailToArray { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] MailCcArray { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string MailSubject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string MailBody { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string MailPwd { get; set; }

        private string _host = "smtp.163.com";

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        private bool _isbodyHtml = true;
        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool IsbodyHtml
        {
            get { return _isbodyHtml; }
            set { _isbodyHtml = value; }
        }

        private string _nickname = "海星·博客 系统通知";
        /// <summary>
        /// 发送者昵称
        /// </summary>
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
            }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] AttachmentsPath { get; set; }

        //优先级别
        private MailPriority _Priority = MailPriority.Normal;
        /// <summary>
        /// 优先级别  默认正常优先级
        /// </summary>
        public MailPriority Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                _Priority = value;
            }
        }
        /// <summary>
        /// {0}:用户名
        /// {1}{2}{3}:正文内容
        /// </summary>
        public static string TempBody(string userName, string p1 = "", string p2 = "", string p3 = "", bool isShow = true)
        {
            return @"<STYLE type='text/css'>                                 
                                 BODY { font-size: 14px; line-height: 1.5  }   
                              </STYLE>
                       <HEAD>
                     <META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=UTF-8'> </HEAD>
                  <div style='background-color:#ECECEC; padding: 35px;'>
	                       <table cellpadding='0' align='center' style='width: 600px; margin: 0px auto; text-align: left; position: relative; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; font-size: 14px; font-family:微软雅黑, 黑体; line-height: 1.5; box-shadow: rgb(153, 153, 153) 0px 0px 5px; border-collapse: collapse; background-position: initial initial; background-repeat: initial initial;background:#fff;'>
		                <tbody>
			<tr>
				<th valign='middle' style='height: 25px; line-height: 25px; padding: 15px 35px; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #6f5499; background-color: #6f5499; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;'>
					<font face='微软雅黑' size='5' style='color: rgb(255, 255, 255); '>海星·博客</font>
				</th>
			</tr>
			
			<tr>
				<td>
					<div style='padding:25px 35px 40px; background-color:#fff;'>
						<h2 style='margin: 5px 0px; '>
							<font color='#333333' style='line-height: 20px; '>
								<font style='line-height: 22px; ' size='4'>尊敬的 " + userName + @"，您好：</font>
							</font>
						</h2>
						<p>" + p1 + @"</p>
						<p>" + p2 + @"</p>
						<p>" + p3 + @"</p>
						<p>
							" + (isShow ? "如非本人操作，请不要理会此邮件，对此为您带来的不便深表歉意。" : string.Empty) + @"
						</p>
						<p>&nbsp;</p>
						<p align='right'>海星-博客 官方团队</p>
						<p align='right'>" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"</p>
					</div>
				</td>
			</tr>
			
			<tr>
				<td>
					<div style='line-height: 20px;color: #999;background: #f5f5f5;font-size: 12px;border-top: 1px solid #ddd;padding: 10px 20px;'>        				
        				<p> 
        					如有疑问，请发邮件到 <a href='mailto:stjworkemail@163.com' target='_blank'>stjworkemail@163.com</a>，感谢您的支持。
        				</p>
    				</div>
				</td>
			</tr>
		</tbody>
	</table>
</div>
";
        }

        private MailMessage InitMailMessage()
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(MailFrom, Nickname);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();

            //向收件人地址集合添加邮件地址
            if (MailToArray != null)
            {
                for (int i = 0; i < MailToArray.Length; i++)
                {
                    myMail.To.Add(MailToArray[i].ToString());
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (MailCcArray != null)
            {
                for (int i = 0; i < MailCcArray.Length; i++)
                {
                    myMail.CC.Add(MailCcArray[i].ToString());
                }
            }
            //发件人地址
            myMail.From = maddr;

            //电子邮件的标题
            myMail.Subject = MailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = MailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            //邮件优先级
            myMail.Priority = Priority;

            myMail.IsBodyHtml = IsbodyHtml;


            //在有附件的情况下添加附件
            try
            {
                if (AttachmentsPath != null && AttachmentsPath.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in AttachmentsPath)
                    {
                        attachFile = new Attachment(path);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }
            return myMail;
        }

        private SmtpClient GetSmtpClient(bool isAsync = false)
        {
            SmtpClient smtp = new SmtpClient();
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(MailFrom, MailPwd);//115                 //设置SMTP邮件服务器
            smtp.Host = Host;
            smtp.Port = 25;
            if (isAsync)
                smtp.SendCompleted += Smtp_SendCompleted;
            return smtp;
        }

        #endregion
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="CallSuccess">发送成功回调</param>
        /// <param name="CallFailure">发送失败回调</param>
        /// <returns></returns>
        public bool Send(Action<MailMessage> CallSuccess = null, Action<MailMessage, Exception> CallFailure = null)
        {
            MailMessage myMail = InitMailMessage();
            SmtpClient smtp = GetSmtpClient();
            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                CallSuccess?.Invoke(myMail);
                return true;

            }
            catch (SmtpException ex)
            {
                CallFailure?.Invoke(myMail, ex);
                return false;
            }
        }
        #region 异步发送邮件
        private Action<bool, Exception> actionSendCompletedCallback = null;
        private SmtpClient smtpClient = null;
        private void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //同一组件下不需要回调方法,直接在此写入日志即可
            //写入日志
            //return;
            smtpClient = null;
            if (actionSendCompletedCallback == null) return;
            bool success = false;
            Exception ex = null;
            if (e.Cancelled)
            {
                success = false;
            }
            else if (e.Error != null)
            {
                success = false;
                ex = e.Error;
            }
            else
                success = true;
            //执行回调方法
            actionSendCompletedCallback(success, ex);
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="emailCompleted">邮件发送完成时需要调用的方法</param>
        /// <returns></returns>
        public void SendAsync(Action<bool, Exception> emailCompleted)
        {
            if (MailToArray == null || MailToArray.Length == 0) return;
            MailMessage myMail = InitMailMessage();
            actionSendCompletedCallback = emailCompleted;
            SmtpClient smtp = GetSmtpClient(true);
            smtpClient = smtp;
            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.SendAsync(myMail, true);
            }
            catch (SmtpException)
            {
            }
            finally
            {
                myMail = null;
            }
        }
        /// <summary>
        /// 取消异步发送，和SendAsync对应
        /// </summary>
        public void SendAsyncCancel()
        {
            if (smtpClient != null)
            {
                smtpClient.SendAsyncCancel();
            }
        }
        #endregion

    }
}
