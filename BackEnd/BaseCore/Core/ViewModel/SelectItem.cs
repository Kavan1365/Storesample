namespace BaseCore.BaseCore.ViewModel
{
    public class SelectItem
    {
        public SelectItem(string id, string text)
        {
            Id = id;
            Title = text;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }

    }
}
