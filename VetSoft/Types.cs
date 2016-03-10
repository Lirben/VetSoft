using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    public static class Types
    {
        public const int COMMAND_PACKAGE_TYPE = 0;
        public const int RESPONSE_PACKAGE_TYPE = 1;
        public const int DATA_PACKAGE_TYPE = 2;

        public enum StreamType { RAW, FILTERED, STEP };
        public enum HoofLocation { FRONT_LEFT, FRONT_RIGHT, HIND_LEFT, HIND_RIGHT};
        public enum SensorLocation { TOP_LEFT, TOP_RIGHT, REAR_LEFT, REAR_RIGHT };
        
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
            public string command { get; set; }
            public string parameter { get; set; }
            public string value { get; set; }
        }

        public struct ResponsePackage
        {
            public int type { get; set; }
            public string hoof { get; set; }
            public string parameter { get; set; }
            public string value { get; set; }
        }

        public struct DataPackage
        {
            public int type { get; set; }
            public string hoof { get; set; }
            public uint time { get; set; }
            public string[] data { get; set; }
        }
    }
}
