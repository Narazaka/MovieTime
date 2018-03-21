using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime {
    namespace Config {
        public class AppConfig {
            public bool Audio { get; set; } = false;
            public bool ExitOnKeyDown { get; set; } = true;
            public bool ExitOnMouseDown { get; set; } = true;
            public bool ExitOnMouseMove { get; set; } = true;

            public bool ExitAnyTime { get => !ExitOnKeyDown && !ExitOnMouseDown && !ExitOnMouseMove; }

            public static AppConfig Load() => FileConfigUtil.Load<AppConfig>(PathBase.AppConfig);
            public void Save() => FileConfigUtil.Save(PathBase.AppConfig, this);
        }
    }
}
