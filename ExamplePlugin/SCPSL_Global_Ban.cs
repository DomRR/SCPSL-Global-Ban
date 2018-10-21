using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;

namespace SCPSL_Global_Ban
{
    [PluginDetails(
        author = "DomRR",
        name = "SCPSL Global Ban",
        description = "",
        id = "rbq.global.ban",
        version = "1.01",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0
        )]
    class Events : Plugin, IEventHandlerRoundStart, IEventHandlerPlayerJoin
    {
        public static List<Int64> ban_id = new List<Int64>();
        public static List<string> ban_ip = new List<string>();
        public override void OnDisable()
        {
        }
        public override void OnEnable()
        {

            Info("SCPSL Global Ban v1.01 插件已加载 :)");
        }
        public override void Register()
        {
            Thread updateThread = new Thread(new ThreadStart(() => new ListUpdateThread()));
            updateThread.Start();
            AddEventHandler(typeof(IEventHandlerPlayerJoin), this, Priority.Normal);
            AddEventHandler(typeof(IEventHandlerRoundStart), this, Priority.Normal);
        }
        public void OnRoundStart(RoundStartEvent ev)
        {
            ban_id = new List<Int64>();
            ban_ip = new List<string>();
            Info("正在更新数据.");
            Thread updateThread = new Thread(new ThreadStart(() => new ListUpdateThread()));
            updateThread.Start();
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            Player player = ev.Player;
            Int64.TryParse(player.SteamId, out Int64 id);
            string ip = player.IpAddress.Split(':')[3];
            /*Info(ip);
            foreach (Int64 i in ban_id)
            {
                Info(i.ToString());
            }
            foreach (string s in ban_ip)
            {
                Info(s);
            }*/
            if (ban_id.Contains(id))
            {
                Info("踢出被封禁的 Id: " + id.ToString());
                player.Ban(1);
            }
            if (ban_ip.Contains(ip))
            {
                Info("踢出被封禁的 IP: " + ip);
                player.Ban(1);
            }
        }
    }
}