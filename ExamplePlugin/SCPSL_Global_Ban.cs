using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Threading;
using System.Collections.Generic;

namespace SCPSL_Global_Ban
{
    [PluginDetails(
        author = "DomRR",
        name = "SCPSL Global Ban",
        description = "",
        id = "rbq.global.ban",
        version = "1.0.4",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0
        )]
    class Events : Plugin, IEventHandlerRoundStart, IEventHandlerPlayerJoin, IEventHandlerLateUpdate
    {
        public static List<long> ban_id = new List<long>();
        public static List<string> ban_ip = new List<string>();

        IConfigFile Config => ConfigManager.Manager.Config;

        bool first = false;

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            Info("SCPSL Global Ban v" + Details.version + " 插件已加载 :)");
        }

        public override void Register()
        {
            new Thread(new ThreadStart(() => new ListUpdateThread())).Start();
            AddEventHandler(typeof(IEventHandlerPlayerJoin), this, Priority.Normal);
            AddEventHandler(typeof(IEventHandlerRoundStart), this, Priority.Normal);
            AddEventHandler(typeof(IEventHandlerLateUpdate), this, Priority.Normal);
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            ban_id = new List<long>();
            ban_ip = new List<string>();
            Info("[RoundStart] 正在更新数据.");
            new Thread(new ThreadStart(() => new ListUpdateThread())).Start();
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            Player player = ev.Player;
            string message = Config.GetStringValue("global_ban_message", "你已经被封禁, 如有疑问请加QQ群: 437224732");
            if (long.TryParse(player.SteamId, out long id) && ban_id.Contains(id))
            {
                Info("踢出被封禁的 Id: " + id.ToString());
                player.Ban(0, message);
            }
            string ip = player.IpAddress.Split(':')[3];
            if (ban_ip.Contains(ip))
            {
                Info("踢出被封禁的 IP: " + ip);
                player.Ban(0, message);
            }
        }

        public void OnLateUpdate(LateUpdateEvent ev)
        {
            if (!first)
            {
                first = true;
                Info("[First] 正在更新数据.");
                new Thread(new ThreadStart(() => new ListUpdateThread())).Start();
            }
        }
    }
}