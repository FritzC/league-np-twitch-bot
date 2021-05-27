
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.DTOs;

namespace SimpleTwitchBot.Database {

    public static class DatabaseInfo {

        public const string DB_LOCATION = "np-test-db.chueciaha415.us-east-2.rds.amazonaws.com";
        public const string DB_PORT = "3306";
        public const string DB_USER = "admin";
        public const string DB_PASS = "K3kgniyp#2";

        public const string DB_NAME = "new_schema";

        public const string CHANNELS_TABLE = "twitch_channels";
        public const string CHANNELS_COLUMN_1 = "channel_name";
        
        public const string ACCOUNTS_TABLE = "accounts";
        public const string ACCOUNTS_COLUMN_1 = "encryptedAccountId";
        public const string ACCOUNTS_COLUMN_2 = "regions_id";
        public const string ACCOUNTS_COLUMN_3 = "notable_players_id";

        public const string NOTABLE_PLAYERS_TABLE = "notable_players";
        public const string NOTABLE_PLAYERS_COLUMN_1 = "name";
        public const string NOTABLE_PLAYERS_COLUMN_2 = "lower_case_name";

        public const string REGIONS_TABLE = "regions";

    }

    public class DatabaseManager {

        /**
         * Returns a new MySqlConnection to the DB
         */
        private MySqlConnection getNewConnection() {
            string connStr = $"server={DatabaseInfo.DB_LOCATION};user={DatabaseInfo.DB_USER};database={DatabaseInfo.DB_NAME};port={DatabaseInfo.DB_PORT};password={DatabaseInfo.DB_PASS}";
            return new MySqlConnection(connStr);
        }

        /**
         * Adds a twitch account to the DB
         */
        public async Task<TwitchChannelDTO> addTwitchAccount(TwitchBot bot, string accountName) {
            MySqlConnection conn = getNewConnection();
            TwitchChannelDTO dto = new TwitchChannelDTO();
            
            try {
                conn.Open();

                string sql = $"INSERT INTO {DatabaseInfo.CHANNELS_TABLE} ({DatabaseInfo.CHANNELS_COLUMN_1}) VALUES (@accountName)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@accountName", accountName);  
                MySqlDataReader rdr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

                while (rdr.Read()) {
                    Console.WriteLine(rdr[0] + " " + rdr[1]);
                }
                
            } catch (Exception ex) {
                if (ex.ToString().Contains("Duplicate entry")) {
                    Logger.log($"Bot has already been added to {accountName}'s channel!", this);
                } else {
                    Logger.log(ex.ToString(), this);
                }
                throw ex;
            }

            conn.Close();
            Logger.log($"Bot has been added to {accountName}'s channel!", this);
            return dto;
        }

        /**
         * Removes a twitch account from the DB
         */
        public async Task<bool> removeTwitchAccount(TwitchBot bot, string accountName) {
            MySqlConnection conn = getNewConnection();
            try {
                conn.Open();

                string sql = $"DELETE FROM {DatabaseInfo.CHANNELS_TABLE} WHERE {DatabaseInfo.CHANNELS_COLUMN_1}=@accountName";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@accountName", accountName);  
                await cmd.ExecuteReaderAsync();
                
            } catch (Exception ex) {
                Logger.log(ex.ToString(), this);
                return false;
            }

            conn.Close();
            Logger.log($"Removed bot from {accountName}'s channel!'", this);
            return true;
        }

        /**
         * Gets a list of all twitch accounts from the DB
         */
        public async Task<List<TwitchChannelDTO>> getAllTwitchAccounts() {
            MySqlConnection conn = getNewConnection();
            List<TwitchChannelDTO> accounts = new List<TwitchChannelDTO>();

            try {
                conn.Open();

                string sql = "SELECT * from twitch_channels";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

                while (rdr.Read()) {
                    Console.WriteLine($"Twitch account: {rdr[1]}");
                    accounts.Add(new TwitchChannelDTO() {
                        id = (int) rdr[0],
                        channel_name = rdr[1].ToString()
                    });
                }
                rdr.Close();
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.ToString());
                throw ex;
            }

            conn.Close();
            return accounts;
        }

        /**
         * Adds an account to the DB
         */
        public async Task<AccountDTO> getAccount(TwitchBot bot, string encryptedAccountId) {
            MySqlConnection conn = getNewConnection();
            AccountDTO dto = null;
            
            try {
                conn.Open();

                string sql = $@"SELECT * FROM {DatabaseInfo.ACCOUNTS_TABLE} 
                                WHERE {DatabaseInfo.ACCOUNTS_TABLE}.{DatabaseInfo.ACCOUNTS_COLUMN_1} = @encryptedAccountId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@encryptedAccountId", encryptedAccountId);  
                MySqlDataReader rdr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

                while (rdr.Read()) {
                    dto = new AccountDTO() {
                        id = (int) rdr[0],
                        encryptedAccountId = rdr[1].ToString(),
                        regions_id = (int) rdr[2],
                        notable_players_id = (int) rdr[3]
                    };
                    // Only will get 1 result
                    break;
                }
                
            } catch (Exception ex) {
                Logger.log(ex.ToString(), this);
                throw ex;
            }

            conn.Close();
            return dto;
        }

        public async Task<NotablePlayerDTO> addNotablePlayer(TwitchBot bot, string notablePlayerName) {
            MySqlConnection conn = getNewConnection();
            NotablePlayerDTO dto = null;
            
            try {
                conn.Open();

                string sql = $@"INSERT INTO {DatabaseInfo.NOTABLE_PLAYERS_TABLE} 
                                ({DatabaseInfo.NOTABLE_PLAYERS_COLUMN_1}, {DatabaseInfo.NOTABLE_PLAYERS_COLUMN_2}) 
                                VALUES (@notablePlayerName, @notablePlayerNameLower)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@notablePlayerName", notablePlayerName);  
                cmd.Parameters.AddWithValue("@notablePlayerNameLower", notablePlayerName.ToLower());  
                MySqlDataReader rdr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

                while (rdr.Read()) {
                    dto = new NotablePlayerDTO() {
                        id = (int) rdr[0],
                        name = rdr[1].ToString(),
                        lower_case_name = rdr[2].ToString()
                    };
                    break;
                }
                
            } catch (Exception ex) {
                if (ex.ToString().Contains("Duplicate entry")) {
                    Logger.log($"Notable Player '{notablePlayerName}' already exists!", this);
                } else {
                    Logger.log(ex.ToString(), this);
                }
                throw ex;
            }

            conn.Close();
            Logger.log($"Notable Player '{notablePlayerName}' has been added!", this);
            return dto;
        }

        // public async Task<bool> addTwitchAccount(string accountName) {
        //     string connStr = "server=np-test-db.chueciaha415.us-east-2.rds.amazonaws.com;user=admin;database=new_schema;port=3306;password=K3kgniyp#2";
        //     MySqlConnection conn = new MySqlConnection(connStr);
        //     try {
        //         Console.WriteLine("Connecting to MySQL...");
        //         conn.Open();

        //         string sql = "SELECT * from twitch_channels";
        //         MySqlCommand cmd = new MySqlCommand(sql, conn);
        //         MySqlDataReader rdr = (MySqlDataReader) await cmd.ExecuteReaderAsync();

        //         while (rdr.Read()) {
        //             Console.WriteLine(rdr[0]+" -- "+rdr[1]);
        //         }
        //         rdr.Close();
        //     } catch (Exception ex) {
        //         Console.WriteLine("Error: " + ex.ToString());
        //         return false;
        //     }

        //     conn.Close();
        //     Console.WriteLine("Done.");
        //     return true;
        // }

    }
}