using System.Net;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SCPSL_Global_Ban
{
    internal class ListUpdateThread
    {
        public bool MyRemoteCertificateValidationCallback(System.Object sender,
    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }
        public ListUpdateThread()
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            // ID
            var web = WebRequest.Create("https://scpsl.net/Global_Ban/api.php?method=id");
            web.Method = "GET";
            var stream = new StreamReader(web.GetResponse().GetResponseStream());
            string str = stream.ReadToEnd();
            string[] str_Array = str.Split(new[] { "<br>" }, StringSplitOptions.None);
            foreach (string s in str_Array)
            {
                if (Int64.TryParse(s, out Int64 id))
                {
                    Events.ban_id.Add(id);
                }
            }

            // IP
            web = WebRequest.Create("https://scpsl.net/Global_Ban/api.php?method=ip");
            web.Method = "GET";
            stream = new StreamReader(web.GetResponse().GetResponseStream());
            str = stream.ReadToEnd();
            str_Array = str.Split(new[] { "<br>" }, StringSplitOptions.None);
            foreach (string s in str_Array)
            {
                Events.ban_ip.Add(s);
            }
        }
    }
}