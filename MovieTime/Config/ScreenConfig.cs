using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PlaylistsNET.Content;
using PlaylistsNET.Model;
using MovieTime.PlaylistModel;
using YamlDotNet.Serialization;

namespace MovieTime {
    namespace Config {
        public class ScreenConfig : IHasIndexConfig {
            [YamlIgnore]
            public int Index { get; set; }
            public bool ShuffleInPlaylist { get; set; } = false;
            public StartsWith StartsWith { get; set; } = StartsWith.PreviousTime;
            public IList<string> PlaylistPaths { get; set; } = new List<string>();

            public ScreenPlaylistConfig RandomPlaylistConfig() => new ScreenPlaylistConfig {
                Index = Index,
                ShuffleInPlaylist = ShuffleInPlaylist,
                StartsWith = StartsWith,
                PlaylistPath = RandomPlaylistPath(),
            };

            string RandomPlaylistPath() => PlaylistPaths.Count == 0 ? null : PlaylistPaths[new Random().Next(PlaylistPaths.Count)];

            public static ScreenConfig Load(int index) => FileConfigUtil.Load<ScreenConfig>(PathBase.ScreenConfig(index), index, true);
            public void Save() => FileConfigUtil.Save(PathBase.ScreenConfig(Index), this);
        }

        /// <summary>
        /// どこから再生するか
        /// </summary>
        public enum StartsWith {
            /// <summary>
            /// プレイリストの最初から
            /// </summary>
            PlaylistStart,
            /// <summary>
            /// 前回ムービーの最初から
            /// </summary>
            PreviousMovie,
            /// <summary>
            /// 前回ムービーの次のムービーから
            /// </summary>
            NextMovie,
            /// <summary>
            /// 前回終了時刻から
            /// </summary>
            PreviousTime,
        }
    }
}
