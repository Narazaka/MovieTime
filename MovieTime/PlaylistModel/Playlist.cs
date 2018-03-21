using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PlaylistsNET.Content;
using PlaylistsNET.Model;

namespace MovieTime {
    namespace PlaylistModel {
        public class Playlist : List<string> {
            public Playlist(IEnumerable<string> entries) : base(entries) { }

            public static Playlist Read(string playlistPath) {
                if (!File.Exists(playlistPath)) return null;
                var ext = Path.GetExtension(playlistPath).ToLower();
                var playlistStream = new FileStream(playlistPath, FileMode.Open);
                IEnumerable<BasePlaylistEntry> playlistEntries;
                switch (ext) {
                    case ".pls":
                        playlistEntries = new PlsContent().GetFromStream(playlistStream).PlaylistEntries;
                        break;
                    case ".m3u":
                        playlistEntries = new M3uContent().GetFromStream(playlistStream).PlaylistEntries;
                        break;
                    case ".m3u8":
                        playlistEntries = new M3u8Content().GetFromStream(playlistStream).PlaylistEntries;
                        break;
                    case ".wpl":
                        playlistEntries = new WplContent().GetFromStream(playlistStream).PlaylistEntries;
                        break;
                    case ".zpl":
                        playlistEntries = new ZplContent().GetFromStream(playlistStream).PlaylistEntries;
                        break;
                    default:
                        throw new InvalidOperationException("unknown playlist format");
                }
                playlistStream.Close();
                if (playlistEntries.Count() == 0) return null;
                return new Playlist(playlistEntries.Select(e => e.Path));
            }

            public void Save(string playlistPath) {
                File.WriteAllText(playlistPath, ToString());
            }

            public override string ToString() {
                var playlist = new M3uPlaylist();
                playlist.PlaylistEntries.AddRange(this.Select(entry => new M3uPlaylistEntry { Path = entry }));
                return new M3u8Content().Create(playlist);
            }
        }
    }
}
