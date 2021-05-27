using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using SimpleTwitchBot;
using RiotAPI.DTOs;

namespace RiotAPI {

    public static class RiotAPIEndpoints {

        public static Dictionary<string, string> regions = new Dictionary<string, string> {
            {"br", "br1"},
            {"eun", "eun1"},
            {"euw", "euw1"},
            {"jp", "jp1"},
            {"kr", "kr"},
            {"lan", "la1"},
            {"las", "la2"},
            {"na", "na1"},
            {"oce", "oc1"},
            {"ru", "ru"},
            {"tr", "tr1"},
        };

        public const string SEARCH_BY_SUMMONER = "https://{0}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{1}";
        public const string MATCH_REQUEST = "https://{0}.api.riotgames.com/lol/spectator/v4/active-games/by-summoner/{1}";

    }

    public class RiotAPIHandler {

        /**
         * Get an account by the region and summoner id
         */
        public static async Task<SummonerDTO> lookupAccountBySummonerId(string region, string summonerName) {
            try {
                RestClient client = new RestClient(string.Format(RiotAPIEndpoints.SEARCH_BY_SUMMONER, RiotAPIEndpoints.regions[region], summonerName));
                client.AddDefaultHeader("X-Riot-Token", GlobalConstants.RIOT_API_KEY);

                IRestResponse response = await client.ExecuteAsync(new RestRequest()); 
                return JsonSerializer.Deserialize<SummonerDTO>(response.Content);
            } catch (Exception e) {
                Logger.log($"Error fetching account information for summoner name '{summonerName}': {e.ToString()}");
            }
            return null;

        }

        /**
         * Steps:
         *  - Search through registers accounts for streamer to find which is ingame
         *  - When an account is found to be ingame, move it to top of the account list and mark as active
         *  - Make Riot API request to get streamer's current match data
         *  - Search database for "notable players" from the account IDs found in the match data
         *  - Make follow up API requests to get found "notable player"s' current ranks (probably need to cache this)
         *  - Format message to display in chat
         *  - Send chat message
         */
        public static async Task<CurrentGameInfo> lookupGame(string region, string encryptedSumId) {
            try {
                RestClient client = new RestClient(string.Format(RiotAPIEndpoints.MATCH_REQUEST, RiotAPIEndpoints.regions[region], encryptedSumId));
                client.AddDefaultHeader("X-Riot-Token", GlobalConstants.RIOT_API_KEY);

                IRestResponse response = await client.ExecuteAsync(new RestRequest()); 
                return JsonSerializer.Deserialize<CurrentGameInfo>(response.Content);
            } catch (Exception e) {
                Logger.log($"Error fetching current game info: {e.ToString()}");
            }
            return null;
        }
        
    }
}