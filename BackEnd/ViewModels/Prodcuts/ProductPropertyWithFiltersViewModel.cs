namespace ViewModels.Prodcuts
{
    public class ProductPropertyWithFiltersViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<int> Selected { get; set; }
        public List<SelectItem> Filters { get; set; }
    }
}
