namespace BN_Project.Domain.ViewModel.Admin
{
    public class CategoriesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
