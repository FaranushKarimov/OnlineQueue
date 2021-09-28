using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OnlineQuee.Utils
{
    public class Config
    {
        public string Token { get; set; }
        public Models.Category Categories { get; set; }

        public static Config ReadConfig()
        {
            try
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Utils\Config.json";
                if (!File.Exists(path))
                {
                    throw new Exception(@"Файл настроек \Utils\Config.json не найден!");
                }
                var data = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<Config>(data);
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Ошибка при чтении файла настроек: \Utils\Config.json, {ex.Message}");
            }
        }
        //public static SaveToken()
        //{

        //}
    }
}
