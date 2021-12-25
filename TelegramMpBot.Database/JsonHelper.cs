using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using TelegramMpBot.Database;

namespace MpstatsSite.Helpers
{
    public static class JsonHelper
    {
        public static string PathToJsonDB { get; set; } = @"C:\хуй.json";

        static JsonSerializer serializer = new JsonSerializer();
        public static void AddViolatorToDB(Violator violator)
        {
            string db = File.ReadAllText(PathToJsonDB);
            var list =  JsonConvert.DeserializeObject<List<Violator>>(db);
            list = list.Where(o => !o.IsWatched).ToList();
            list.Add(violator);
            using (StreamWriter sw = new StreamWriter(PathToJsonDB))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, list);
                }
            }
        }

        public static List<Violator> GetViolatorsFromDB()
        {
            string db = File.ReadAllText(PathToJsonDB);
            var list = JsonConvert.DeserializeObject<List<Violator>>(db);
            list = list.Where(o => !o.IsWatched).ToList();
            foreach (var item in list)
            {
                item.IsWatched = true;
            }
            using (StreamWriter sw = new StreamWriter(PathToJsonDB))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, list.Where(o => !o.IsWatched));
                }
            }
            return list;
        }
    }
}