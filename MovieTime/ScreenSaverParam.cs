using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTime.Config;
using MovieTime.PlaylistModel;

namespace MovieTime {
    public class ScreenSaverParam : IHasIndexConfig {
        public int Index { get; set; }
        public AppConfig AppConfig { get; set; }
        public IPlaylistView PlaylistView { get; set; }
        public long InitialTime { get; set; }

        public static IList<ScreenSaverParam> LoadParams(int count) {
            var screenConfigs = LoadScreenConfigs(count);
            var appConfig = AppConfig.Load();
            var screenGroupConfig = ScreenGroupConfig.Load();

            var screenSaverParams = new List<ScreenSaverParam>();
            var groupSourceParams = new Dictionary<int, ScreenSaverParam>();
            for (var index = 0; index < count; ++index) {
                var screenPlaylistConfig = screenConfigs[index].RandomPlaylistConfig();
                var screenState = ScreenState.Load(index);
                var screenSaverParam = new ScreenSaverParam {
                    Index = index,
                    AppConfig = appConfig,
                    InitialTime = screenPlaylistConfig.InitialTime(screenState),
                };
                var groupSourceIndex = screenGroupConfig.GroupSourceIndex(index);
                if (groupSourceIndex == null) { // グループではない
                    screenSaverParam.PlaylistView = screenPlaylistConfig.PlaylistView(screenState);
                    screenSaverParams.Add(screenSaverParam);
                } else if (index == groupSourceIndex) { // グループの最初のスクリーン
                    screenSaverParam.PlaylistView = new SynchronizedPlaylistView(
                        screenPlaylistConfig.PlaylistView(screenState),
                        screenGroupConfig.GroupMemberCount(index, count - 1)
                        );
                    groupSourceParams[index] = screenSaverParam;
                    screenSaverParams.Add(screenSaverParam);
                } else { // グループの2番目以降のスクリーン
                    screenSaverParams.Add(groupSourceParams[(int)groupSourceIndex]);
                }
            }
            return screenSaverParams;
        }

        /// <summary>
        /// スクリーンごとの設定をロードする
        /// (設定がないindexは前のindexと同一の設定になる)
        /// </summary>
        /// <param name="count">ロードする数</param>
        /// <returns>設定リスト</returns>
        static IList<ScreenConfig> LoadScreenConfigs(int count) {
            var screenConfigs = new List<ScreenConfig>();
            var previousScreenConfig = new ScreenConfig();
            for (var index = 0; index < count; ++index) {
                var screenConfig = ScreenConfig.Load(index);
                if (screenConfig == null) {
                    screenConfigs.Add(previousScreenConfig);
                    continue;
                }
                previousScreenConfig = screenConfig;
                screenConfigs.Add(screenConfig);
            }
            return screenConfigs;
        }
    }
}
