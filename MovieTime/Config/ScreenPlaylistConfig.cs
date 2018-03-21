using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTime.PlaylistModel;

namespace MovieTime.Config {
    public class ScreenPlaylistConfig : IHasIndexConfig {
        public int Index { get; set; }
        public bool ShuffleInPlaylist { get; set; }
        public StartsWith StartsWith { get; set; }
        public string PlaylistPath { get; set; }

        Playlist _Playlist = null;
        bool PlaylistLoaded = false;

        public ICanNextSyncPlaylistView PlaylistView(ScreenState screenState) {
            if (Playlist == null) return null;
            var initialEntry = StartsWith == StartsWith.PlaylistStart ? null : screenState.Path;
            var playlistView =
                ShuffleInPlaylist ?
                new ShufflePlaylistView(Playlist, initialEntry) as ICanNextSyncPlaylistView :
                new SequentialPlaylistView(Playlist, initialEntry) as ICanNextSyncPlaylistView;
            if (StartsWith == StartsWith.NextMovie) playlistView.NextSync();
            return playlistView;
        }

        public long InitialTime(ScreenState screenState) => StartsWith == StartsWith.PreviousTime ? screenState.Time : 0;

        public Playlist Playlist {
            get {
                if (!PlaylistLoaded) {
                    PlaylistLoaded = true;
                    if (PlaylistPath != null) _Playlist = Playlist.Read(PlaylistPath);
                }
                return _Playlist;
            }
        }

        public static ScreenConfig Load(int index) => FileConfigUtil.Load<ScreenConfig>(PathBase.ScreenConfig(index), index, true);
        public void Save() => FileConfigUtil.Save(PathBase.ScreenConfig(Index), this);
    }
}
