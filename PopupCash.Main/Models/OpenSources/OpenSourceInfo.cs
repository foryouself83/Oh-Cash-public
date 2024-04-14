namespace PopupCash.Main.Models.OpenSources
{
    public class OpenSourceInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public OpenSourceInfo(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
