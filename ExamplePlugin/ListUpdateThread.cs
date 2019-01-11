using System.Net;
using System;
using System.IO;

namespace SCPSL_Global_Ban
{
    internal class ListUpdateThread
    {
        public ListUpdateThread()
        {
            // ID
            var web = WebRequest.Create("https://scpsl.net/Global_Ban/api.php?method=id");
            web.Method = "GET";
            var stream = new StreamReader(web.GetResponse().GetResponseStream());
            string str = stream.ReadToEnd();
            string[] str_Array = str.Split(new[] { "<br>" }, StringSplitOptions.None);
            foreach (string s in str_Array)
            {
                if (long.TryParse(s, out long id))
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