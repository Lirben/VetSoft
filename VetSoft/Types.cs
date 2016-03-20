using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VetSoft
{
    public static class Types
    {
        public const byte XBEE_TRANSMIT_STATUS = 0x89;
        public const byte XBEE_RECIEVE_PACKET_16BIT = 0x81;


        public const int COMMAND_PACKAGE_TYPE = 0;
        public const int RESPONSE_PACKAGE_TYPE = 1;
        public const int DATA_PACKAGE_TYPE = 2;

        public enum HoofLocation { FRONT_LEFT, FRONT_RIGHT, HIND_LEFT, HIND_RIGHT};
        public enum SensorLocation { REAR_LEFT, TOP_LEFT, REAR_RIGHT, TOP_RIGHT };
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public struct CommandPackage
        {
            public int type { get; set; }
            public Types.HoofLocation hoofLocation { get; set; }
            public string command { get; set; }
            public string parameter { get; set; }
            public string value { get; set; }
        }

        public struct ResponsePackage
        {
            [JsonProperty("type")]
            public int type { get; set; }
            [JsonProperty("hoof")]
            public Types.HoofLocation hoofLocation { get; set; }
            [JsonProperty("parameter")]
            public string parameter { get; set; }
            [JsonProperty("value")]
            public string value { get; set; }
        }

        public struct DataPackage
        {
            [JsonProperty("type")]
            public int type { get; set; }
            [JsonProperty("hoof")]
            public Types.HoofLocation hoofLocation { get; set; }
            [JsonProperty("time")]
            public uint time { get; set; }
            [JsonProperty("data")]
            public string[] data { get; set; }
        }
    }
}
