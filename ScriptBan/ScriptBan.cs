using System;
using System.ComponentModel.Composition;
using BattleNET;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptBan
{
    [Export(typeof(Kekcon.IKekPlugin))]
    public class ScriptBan : Kekcon.IKekPlugin
    {
        internal static BattlEyeClient beclient;

        public bool Init(BattlEyeClient client)
        {
            beclient = client;

            client.BattlEyeMessageReceived += BattlEyeMessageReceived;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=> Plugin geladen:\tScriptban");
            Console.ForegroundColor = ConsoleColor.Gray;

            return true;
        }

        private static void BattlEyeMessageReceived(BattlEyeMessageEventArgs args)
        {
            Regex regex = new Regex(@"Player #([0-9]{1,}) (.{0,}) [(]([\w]{32})[)] has been kicked by BattlEye: (.{0,}) [#](\d+)");
            Match match = regex.Match(args.Message);

            if (match.Success)
            {
                string[] reasons = {
                    "mpeventhandler restriction",
                    "publicVariable restriction",
                    "publicVariable value restriction",
                    "remoteExec restriction",
                    "waypointcondition restriction",
                    "waypointstatement restriction"
                };
                
                if (reasons.Contains(match.Groups[4].Value))
                {
                    //Autoban
                    beclient.SendCommand(string.Format("addban {0} {1} {2}", match.Groups[3].Value, 0, "AutoBan | ScriptRestriction | auf TS3 melden"));

                    //Ausgabe
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=> Plugin:\tScriptban Ausgabe: Spieler {0} ({1}) wurde gebant", match.Groups[2].Value, match.Groups[3].Value);
                }
            }
        }
    }
}
