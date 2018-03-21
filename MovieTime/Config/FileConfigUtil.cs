using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace MovieTime.Config {
    public static class FileConfigUtil {
        static Deserializer Deserializer = new DeserializerBuilder().Build();
        static Serializer Serializer = new SerializerBuilder().Build();

        public static T Load<T>(string configPath, bool nullWhenNotFound = false) where T : class, new() {
            if (!File.Exists(configPath)) return nullWhenNotFound ? null : new T();
            return Deserializer.Deserialize<T>(File.ReadAllText(configPath)) ?? (nullWhenNotFound ? null : new T());
        }

        public static T Load<T>(string configPath, int index, bool nullWhenNotFound = false) where T : class, IHasIndexConfig, new() {
            var config = Load<T>(configPath, nullWhenNotFound);
            if (config == null) return null;
            config.Index = index;
            return config;
        }

        public static void Save<T>(string configPath, T config) {
            var configDirectory = Path.GetDirectoryName(configPath);
            if (!Directory.Exists(configDirectory)) Directory.CreateDirectory(configDirectory);
            File.WriteAllText(configPath, Serializer.Serialize(config));
        }
    }
}
