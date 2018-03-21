using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MovieTime.PlaylistModel {
    public static class PlaylistExtension {
        public static string ToMrl(this string path) => Regex.IsMatch(path, @"^\w+://") ? path : "file:///" + path;
    }
}
