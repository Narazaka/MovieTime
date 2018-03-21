using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;

namespace MovieTime {
    namespace Config {
        public class ScreenState : IHasIndexConfig {
            [YamlIgnore]
            public int Index { get; set; }
            public string Path { get; set; } = null;
            public long Time { get; set; } = 0;

            public static ScreenState Load(int index) => FileConfigUtil.Load<ScreenState>(PathBase.ScreenState(index), index);
            public void Save() => FileConfigUtil.Save(PathBase.ScreenState(Index), this);
        }
    }
}
