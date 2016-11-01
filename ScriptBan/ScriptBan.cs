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

        //Config Array & DB Connection String
        internal static string[] config = new string[9];
        internal static string[] configLines = null;
        internal static string ConnStr = "";

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

            for (int i = 0; i < 9; i++)
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
                    case "tablename":
                        config[7] = typeArray[1].Trim();
                        break;
                    case "banreason":
                        config[8] = typeArray[1].Trim();
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

            //Database Connection Check
            if (config[2] == "true")
            {
                //Database Connection String
                ConnStr = "server=" + config[3] + ";database=" + config[4] + ";uid=" + config[5] + ";password=" + config[6];

                if (CheckDatabaseConnection(ConnStr))
                {
                    //Table Check & Create Table
                    if (!checkTableExists(ConnStr)) { createTable(ConnStr); }
                } else 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=> Plugin Fehler:\tScriptBan\tVerbindung zur Datenbank konnte nicht aufgebaut werden");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return false;
                }
            }
            
            client.BattlEyeMessageReceived += BattlEyeMessageReceived;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=> Plugin geladen:\tScriptBan");
            Console.ForegroundColor = ConsoleColor.Gray;

            return true;
        }

        //### Database Connection Check Function
        private bool CheckDatabaseConnection(string ConStrg)
        {
            bool isConn = false;
            MySqlConnection check_conn = null;
            try
            {
                check_conn = new MySqlConnection(ConStrg);
                check_conn.Open();
                isConn = true;
            }
            catch (ArgumentException a_ex)
            {
                isConn = false;
            }
            catch (MySqlException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> Plugin Fehler:\tScriptBan\tDatabase Error #"+ex.Number.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
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

        //### Check Function ob Datenbanktabelle existent ist
        private static bool checkTableExists(string ConStrg)
        {
            MySqlConnection tableCheck_conn = new MySqlConnection(ConStrg);
            tableCheck_conn.Open();

            MySqlCommand tableCheck = tableCheck_conn.CreateCommand();
            tableCheck.CommandText = "SELECT* FROM information_schema.tables WHERE table_schema = '" + config[4] + "' AND table_name = '" + config[7] + "' LIMIT 1";
            MySqlDataReader reader = tableCheck.ExecuteReader();

            string row = "";
            while (reader.Read())
            {
                row = reader.GetValue(0).ToString();
            }

            if (row == String.Empty) {
                tableCheck_conn.Close();
                return false;
            };

            tableCheck_conn.Close();

            return true;
        }

        //### Create Table Function
        private static void createTable (string ConStrg)
        {
            MySqlConnection createTable_conn = new MySqlConnection(ConStrg);
            createTable_conn.Open();

            string query = string.Format(@"CREATE TABLE `{0}` (
                                        `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
                                        `player` varchar(50) NOT NULL DEFAULT '',
                                        `guid` varchar(50) NOT NULL DEFAULT '',
                                        `logs` text NOT NULL,
                                        `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        PRIMARY KEY (`id`))
                                        AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8;", config[7]);
            MySqlCommand createTable = new MySqlCommand(query, createTable_conn);
            createTable.ExecuteNonQuery();

            createTable_conn.Close();
        }

        //### BattlEye Message Handler
        private static void BattlEyeMessageReceived(BattlEyeMessageEventArgs args)
        {
            Regex regex = new Regex("(.{0,}) Log: #([0-9]{1,}) (.{0,}) [(]([\\w]{32})[)] - ([#]\\d+) ([\"].{0,})");
            Match match = regex.Match(args.Message);

            if (match.Success)
            {
                string reason       = match.Groups[1].Value.ToLower();
                string beNumber     = match.Groups[2].Value;
                string player       = match.Groups[3].Value;
                string guid         = match.Groups[4].Value;
                string reasonNumber = match.Groups[5].Value;
                string log          = match.Groups[6].Value;

                string[] reasonVariants = {
                    "addbackpackcargo",
                    "addmagazinecargo",
                    "attachto",
                    "addweaponcargo",
                    "createvehicle",
                    "deletevehicle",
                    "mpeventhandler",
                    "publicvariable",
                    "publicvariableval",
                    "remoteexec",
                    "script",
                    "selectplayer",
                    "setdamage",
                    "setvariable",
                    "setvariableval",
                    "teamswitch",
                    "waypointcondition",
                    "waypointstatement"
                };

                if (reasonVariants.Contains(reason))
                {
                    //Autoban
                    beclient.SendCommand(string.Format("addban {0} {1} {2}", guid, 0, config[8]));
                    beclient.SendCommand("loadbans");

                    //Ausgabe
                    DateTime localDate = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}]: Spieler {1} ({2}) wurde gebannt", localDate, player, guid);

                    //Logdatei - Inhalt
                    string time = "[" + localDate.ToString() + "]: ";
                    string timeline = string.Format("Spieler {0} ({1}) wurde für {2} {3} gebannt.", player, guid, reason, reasonNumber);
                    string banlog = string.Format("{0}", log);

                    string[] lines =
                    {
                            (time+timeline),
                            (time+banlog)
                    };

                    //Logdatei erstellen - pro Spieler
                    if (config[0] == "true")
                    {
                        string path = string.Format(@"{0}{1}_{2}.txt", config[1], player, guid);
                        if (!File.Exists(path))
                        {
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                foreach (var line in lines)
                                {
                                    sw.WriteLine(line);
                                }
                            }
                        } else
                        {
                            using (StreamWriter sw = File.AppendText(path))
                            {
                                foreach (var line in lines)
                                {
                                    sw.WriteLine(line);
                                }
                            }
                        }
                    }
               
                    //Datenbank-Log
                    if (config[2] == "true")
                    {
                        string logContent = "";
                        for (int i = 0; i < lines.Length; i++)
                        {
                            logContent += lines[i];
                            if (i > 0) { logContent += "\r"; };
                        }

                        //Datenbankabfrage
                        MySqlConnection insertConn = new MySqlConnection(ConnStr);
                        insertConn.Open();

                        string query = string.Format(@"INSERT INTO `{0}` (`player`, `guid`, `logs`) VALUES
                                                    ('{1}', '{2}', '{3}')", config[7], player, guid, logContent);

                        MySqlCommand insertQuery = new MySqlCommand(query, insertConn);
                        insertQuery.ExecuteNonQuery();
                        insertConn.Close();
                    }
                }
            }
        }
    }
}
