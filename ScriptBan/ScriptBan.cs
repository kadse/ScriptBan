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
            Regex regex = new Regex(@"Player #([0-9]{1,}) (.{0,}) [(]([\w]{32})[)] has been kicked by BattlEye: (.{0,}) ([#]\d+)");
            Match match = regex.Match(args.Message);

            if (match.Success)
            {
                string beNumber     = match.Groups[1].Value;
                string player       = match.Groups[2].Value;
                string guid         = match.Groups[3].Value;
                string reason       = match.Groups[4].Value;
                string reasonNumber = match.Groups[5].Value;

                string[] reasonVariants = {
                    "mpeventhandler restriction",
                    "publicVariable restriction",
                    "publicVariable value restriction",
                    "remoteExec restriction",
                    "waypointcondition restriction",
                    "waypointstatement restriction"
                };
                
                if (reasonVariants.Contains(reason))
                {
                    //Autoban
                    beclient.SendCommand(string.Format("addban {0} {1} {2}", guid, 0, "AutoBan | ScriptRestriction | auf TS3 melden"));

                    //Ausgabe
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=> Plugin:\tScriptban Ausgabe: Spieler {0} ({1}) wurde gebant", player, guid);

                    //Logdatei - Inhalt
                    DateTime localDate = DateTime.Now;
                    string time = localDate.ToString("de-DE");

                    string timeline = time + ": " + (string.Format("Spieler {0} ({1}) wurde für {3} {4} gebant.",player, guid, reason, reasonNumber));
                    string banlog = "Testeintrag, hier wird der Banlog stehen.";

                    string[] lines =
                    {
                            timeline,
                            banlog
                    };

                    //Logdatei erstellen - pro Spieler
                    System.IO.File.WriteAllLines(@"C:\GameServer\HackerLogs\" + player + "_" + guid + ".txt", lines);
                }
            }
        }
    }
}
