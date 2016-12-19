namespace SearchIn
{
    public class CommandIds
    {
        public static readonly int SerchInGithubCommandId = 0x0100;
        public static readonly int SearchInGoogleCommandId = 0x0150;
        public static readonly int SerchInSofCommandId = 0x0350;
        public static readonly int SearchInMsdnCommandId = 0x0250;
    }

    public class BaseUrls
    {
        public static readonly string GoogleBaseUrl = "https://www.google.com.tr/#q={0}";
        public static readonly string GithubBaseUrl = "https://github.com/search?utf8=%E2%9C%93&q={0}";
        public static readonly string MsdnBaseUrl = "https://social.msdn.microsoft.com/Search/en-US?query={0}";
        public static readonly string SofBaseUrl = "http://stackoverflow.com/search?q={0}";
    }
}
