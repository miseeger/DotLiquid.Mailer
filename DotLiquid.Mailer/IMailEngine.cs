using System;
using System.Net;

namespace DotLiquid.Mailer
{

    public interface IMailEngine
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string TemplateDir { get; set; }
        string DefaultFromAddress { get; set; }
        bool UseDefaultCredentials { get; set; }
        ICredentialsByHost Credentials { get; set; }
        bool EnableSsl { get; set; }
        bool IsHtml { get; set; }
        bool IsHigh { get; set; }

        void RegisterTag<T>(string tagName) where T : Tag, new();
        void RegisterFilter(Type filter);

        bool SendFromFile<T>(string subject, string templateFile, T data, string to, string from = "");
        bool Send<T>(string subject, string liquidTemplate, T data, string to, string from = "");
    }

}