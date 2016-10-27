using System;
using System.ComponentModel.Composition;
using BattleNET;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ScriptBan
{
    [Export(typeof(Kekcon.IKekPlugin))]
    public class ScriptBan : Kekcon.IKekPlugin
    { 
        internal static BattlEyeClient beclient;

        //Config Array 
        internal static string[] config = new string[7];
        internal static string[] configLines = null;

        public bool Init(BattlEyeClient client)
        {
            beclient = client;

            //Configcheck
            if (!File.Exists("plugins/config/scriptban.cfg"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> Plugin Fehler:\tScriptBan\tscriptban.cfg nicht gefunden");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }

            //Config auslesen
            configLines = File.ReadAllLines("plugins/config/scriptban.cfg");

            for (int i = 0; i < 7; i++)
            {
                string[] typeArray = configLines[i].Split('=');
                switch (typeArray[0].ToLower().Trim())
                {
                    case "filelogging":
                        config[0] = typeArray[1].ToLower().Trim();
                        break;
                    case "filepath":
                        config[1] = typeArray[1].Trim();
                        break;
                    case "dblogging":
                        config[2] = typeArray[1].ToLower().Trim();
                        break;
                    case "dbserver":
                        config[3] = typeArray[1].Trim();
                        break;
                    case "dbname":
                        config[4] = typeArray[1].Trim();
                        break;
                    case "dbuser":
                        config[5] = typeArray[1].Trim();
                        break;
                    case "dbpassword":
                        config[6] = typeArray[1].Trim();
                        break;
                    default:
                        config[i] = "Error";
                        break;
                }
            }

            //Config Check
            foreach (var type in config)
            {
                if (type == "Error") { config = null; };
            }
            if (config == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> Plugin Fehler:\tScriptBan\tSyntax Fehler in Configdatei 'scriptban.cfg'");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }

            //Filepath Check
            if (config[0] == "true" && !Directory.Exists(config[1]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> Plugin Fehler:\tScriptBan\tDateipfad für File-Logs existiert nicht");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }

            //DB Connection String
            string ConnStr = "server=" + config[3] + ";database=" + config[4] + ";uid=" + config[5] + ";password=" + config[6];

            //Database Connection Check
            if (config[2] == "true" && !CheckDatabaseConnection(ConnStr))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> Plugin Fehler:\tScriptBan\tVerbindung zur Datenbank konnte nicht aufgebaut werden");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            
            client.BattlEyeMessageReceived += BattlEyeMessageReceived;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=> Plugin geladen:\tScriptban");
            Console.ForegroundColor = ConsoleColor.Gray;

            return true;
        }

        //Database Connection Check Function
        private bool CheckDatabaseConnection(string CStrg)
        {
            bool isConn = false;
            MySqlConnection check_conn = null;
            try
            {
                check_conn = new MySqlConnection(CStrg);
                check_conn.Open();
                isConn = true;
            }
            catch (ArgumentException a_ex)
            {
                isConn = false;
            }
            catch (MySqlException ex)
            {
                isConn = false;
            }
            finally
            {
                if (check_conn.State == System.Data.ConnectionState.Open)
                {
                    check_conn.Close();
                }
            }
            return isConn;
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
                    DateTime localDate = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}]: Spieler {1} ({2}) wurde gebant", localDate, player, guid);

                    //Logdatei - Inhalt
                    string time = "[" + localDate.ToString() + "]: ";
                    string timeline = string.Format("Spieler {0} ({1}) wurde für {2} {3} gebant.", player, guid, reason, reasonNumber);
                    string banlog = "Testeintrag, hier wird der Banlog stehen.";

                    string[] lines =
                    {
                            (time+timeline),
                            (time+banlog)
                    };

                    //Logdatei erstellen - pro Spieler
                    if (config[0] == "true")
                    {
                        string path = config[1] + player + "_" + guid;
                        System.IO.File.WriteAllLines(@""+path+".txt", lines);
                    }
               
                    //Datenbank-Log
                    if (config[2] == "true")
                    {
                        //Datenbankabfragen
                    }
                }
            }
        }
    }
}
