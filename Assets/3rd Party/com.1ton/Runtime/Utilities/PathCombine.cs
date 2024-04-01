using System.IO;

namespace nosenfield.Utilities
{
    /// <summary>
    /// This version of PathCombine accounts for inconsistencies in path structures provided by different operating systems (ie. Android vs iOS, forward/backward slashes, leading/trailing slashes)
    /// </summary>
    public static class PathCombine
    {
        public static string Combine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }
    }
}