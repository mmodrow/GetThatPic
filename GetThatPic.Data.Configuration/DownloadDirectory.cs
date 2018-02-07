namespace GetThatPic.Data.Configuration
{
    public class DownloadDirectory
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public bool IsPathRelative { get; set; } = true;
    }
}
