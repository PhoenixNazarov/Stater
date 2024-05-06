namespace StaterV
{
    public class FilesInfo
    {
        public const string ProjectExtension = ".stp";
        public const string DiagramExtension = ".xstd";

        public static string DetermineExtension(string path)
        {
            var ind = path.LastIndexOf('.');
            if (ind == -1)
            {
                return "";
            }

            return (path.Substring(ind));
        }
    }
}
