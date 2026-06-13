using Company.PL.Helper.MailKitFeature;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions.MailKitFeature;
using Services.Specifications.CourtSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MailKitFeature
{
    public class ReminderJob(IUnitOfWork _unitOfWork, IMailService _mailService) : IReminderJob
    {
        public async Task SendPendingRemindersAsync()
        {
            var now = DateTime.UtcNow;
            var spec = new SessionsDueForReminderSpecification(now);
            // جيب كل السيشنز اللي حل وقتها ولسه ما اتبعتلهاش
            var sessions = await _unitOfWork.GetRepository<int, CourtSession>()
                .GetAllAsync(spec);

            foreach (var session in sessions)
            {
                var lawyer = session.Case.Lawyers.FirstOrDefault();
                var lawyerName = lawyer.FirstName + " " + lawyer.LastName;
                var email = new Email
                {
                    To = lawyer.Email,
                    Subject = $"تذكير بجلسة: {session.Case.Title} — {session.SessionDate:d}",
                    Body = BuildReminderEmailBody(session, lawyerName),
                    IsHtml = true  // ← الـ flag اللي أضفناه
                };

                bool sent = await _mailService.SendMailAsync(email);

                if (sent)
                {
                    session.ReminderSent = true;
                    _unitOfWork.GetRepository<int, CourtSession>().Update(session);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private string BuildReminderEmailBody(CourtSession session, string lawyerName)
        {
            return $@"
                       <!DOCTYPE html>
                            <html dir=""rtl"" lang=""ar"">
                            <head>
                              <meta charset=""UTF-8"">
                              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                              <title>تذكير بموعد جلسة</title>
                              <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
                              <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
                              <link href=""https://fonts.googleapis.com/css2?family=Cairo:wght@400;600;700&display=swap"" rel=""stylesheet"">
                              <style>
                                @import url('https://fonts.googleapis.com/css2?family=Cairo:wght@400;600;700&display=swap');
                                * {{ box-sizing: border-box; font-family: 'Cairo', Arial, sans-serif !important; }}
                                body {{
                                  font-family: 'Cairo', Arial, sans-serif;
                                  background-color: #f0f4f8;
                                  direction: rtl;
                                  text-align: right;
                                  margin: 0; padding: 0;
                                  -webkit-text-size-adjust: 100%;
                                  -ms-text-size-adjust: 100%;
                                }}
                                @media only screen and (max-width: 600px) {{
                                  .email-card {{ border-radius: 0 !important; }}
                                  .header {{ padding: 20px 16px !important; }}
                                  .header-title {{ font-size: 18px !important; }}
                                  .body-td {{ padding: 20px 14px !important; }}
                                  .details-table td {{ padding: 10px 12px !important; font-size: 13px !important; }}
                                  .label-cell {{ width: auto !important; white-space: normal !important; font-size: 12px !important; }}
                                  .footer-td {{ padding: 12px 14px !important; }}
                                }}
                              </style>
                            </head>
                            <body>
 
                            <!-- Outer wrapper -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                   style=""background-color:#f0f4f8; padding:30px 16px;"" dir=""rtl"">
                              <tr>
                                <td align=""center"">
 
                                  <!-- Email Card -->
                                  <table class=""email-card"" width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                         style=""max-width:600px; width:100%; background:#ffffff; border-radius:12px;
                                                border:1px solid #dce6f0; box-shadow:0 4px 20px rgba(0,0,0,0.08);
                                                overflow:hidden;"" dir=""rtl"">
 
                                    <!-- HEADER -->
                                    <tr>
                                      <td class=""header"" align=""center""
                                          style=""background:linear-gradient(135deg,#0d2d50 0%,#1a4a7a 100%);
                                                 padding:28px 32px; text-align:center;"">
                                        <p style=""color:#a8cce8; font-size:11px; letter-spacing:1.5px; margin:0 0 6px;"">
                                          نظام إدارة القضايا القانونية
                                        </p>
                                        <p class=""header-title""
                                           style=""color:#ffffff; font-size:22px; font-weight:700; margin:0;"">
                                          تذكير بموعد جلسة
                                        </p>
                                      </td>
                                    </tr>
 
                                    <!-- BODY -->
                                    <tr>
                                      <td class=""body-td"" dir=""rtl""
                                          style=""padding:32px; direction:rtl; text-align:right;"">
 
                                        <p style=""font-size:15px; color:#1a1a1a; font-weight:600; margin:0 0 10px;"">
                                          الأستاذ / {lawyerName}،
                                        </p>
                                        <p style=""font-size:14px; color:#666; line-height:1.9; margin:0 0 24px;"">
                                          نودّ تذكيركم بموعد الجلسة القادمة المقررة وفقاً للتفاصيل الآتية:
                                        </p>
 
                                        <!-- Details Table -->
                                        <table class=""details-table"" width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                               dir=""rtl""
                                               style=""border-collapse:collapse; border:1px solid #e2eaf3;
                                                      border-radius:8px; font-size:14px; overflow:hidden;"">
 
                                          <tr style=""border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              اسم القضية
                                            </td>
                                            <td style=""padding:13px 16px; color:#1a2e42; font-weight:600;
                                                        word-break:break-word; text-align:right; direction:rtl;"">
                                              {session.Case.Title}
                                            </td>
                                          </tr>
 
                                          <tr style=""background:#f8fafc; border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              المحكمة
                                            </td>
                                            <td style=""padding:13px 16px; color:#1a2e42; font-weight:600;
                                                        word-break:break-word; text-align:right; direction:rtl;"">
                                              {session.CourtName}
                                            </td>
                                          </tr>
 
                                          <tr style=""border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              قاعة الجلسة
                                            </td>
                                            <td style=""padding:13px 16px; color:#1a2e42; font-weight:600;
                                                        word-break:break-word; text-align:right; direction:rtl;"">
                                              {session.CourtRoom}
                                            </td>
                                          </tr>
 
                                          <tr style=""background:#f8fafc; border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              القاضي
                                            </td>
                                            <td style=""padding:13px 16px; color:#1a2e42; font-weight:600;
                                                        word-break:break-word; text-align:right; direction:rtl;"">
                                              {session.JudgeName}
                                            </td>
                                          </tr>
 
                                          <tr style=""border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              نوع الجلسة
                                            </td>
                                            <td style=""padding:13px 16px; color:#1a2e42; font-weight:600;
                                                        word-break:break-word; text-align:right; direction:rtl;"">
                                              {session.SessionType}
                                            </td>
                                          </tr>
 
                                          <tr style=""background:#f8fafc; border-bottom:1px solid #e2eaf3;"">
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; white-space:nowrap;
                                                       direction:rtl;"">
                                              موعد الجلسة
                                            </td>
                                            <td style=""padding:13px 16px; text-align:right; direction:rtl;"">
                                              <span style=""display:inline-block; background:#e3f0fd; color:#0b3d73;
                                                           padding:4px 14px; border-radius:20px; font-size:13px;
                                                           font-weight:700; border:1px solid #b6d4f5;"">
                                                {session.SessionDate:f}
                                              </span>
                                            </td>
                                          </tr>
 
                                          <tr>
                                            <td class=""label-cell""
                                                style=""padding:13px 16px; color:#7a93ad; width:38%;
                                                       font-size:13px; text-align:right; vertical-align:top;
                                                       padding-top:14px; white-space:nowrap; direction:rtl;"">
                                              ملاحظات
                                            </td>
                                            <td style=""padding:13px 16px; color:#4a6070; font-size:13px;
                                                        line-height:1.8; word-break:break-word;
                                                        text-align:right; direction:rtl;"">
                                              {session.Notes}
                                            </td>
                                          </tr>
 
                                        </table>
                                        <!-- End Details Table -->
 
                                        <!-- Warning Box -->
                                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" dir=""rtl""
                                               style=""background:#fffbf0; border:1px solid #f5c842;
                                                      border-radius:8px; margin-top:20px;"">
                                          <tr>
                                            <td style=""padding:14px 16px; font-size:13px; color:#7a4e0d;
                                                        line-height:1.8; text-align:right; direction:rtl;"">
                                              &#9888; يُرجى الحضور قبل موعد الجلسة بوقت كافٍ والتأكد من استيفاء جميع المستندات اللازمة.
                                            </td>
                                          </tr>
                                        </table>
 
                                        <p style=""font-size:13px; color:#999; margin:28px 0 0;
                                                   line-height:1.8; text-align:right; direction:rtl;"">
                                          مع تحيات فريق نظام إدارة القضايا
                                        </p>
 
                                      </td>
                                    </tr>
 
                                    <!-- FOOTER -->
                                    <tr>
                                      <td class=""footer-td""
                                          style=""background:#f8fafc; border-top:1px solid #e2eaf3;
                                                 padding:16px 32px; text-align:center;"">
                                        <p style=""font-size:12px; color:#aab8c5; margin:0;"">
                                          هذا البريد مُرسَل تلقائياً من النظام — لا تردّ عليه
                                        </p>
                                      </td>
                                    </tr>
 
                                  </table>
                                  <!-- End Email Card -->
 
                                </td>
                              </tr>
                            </table>
 
                            </body>
                            </html>";
        }
    }


}
