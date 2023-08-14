using BN_Project.Domain.Entities;

namespace BN_Project.Domain.ViewModel.Product
{
    public class ShowProductViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public List<Color> Colors { get; set; }

        public string Description { get; set; }

        public string Features { get; set; }

        public List<Category> Categories { get; set; }

        public long Price { get; set; }

        public string Image { get; set; }

        public List<string> Images { get; set; }

        public int Count { get; set; }
    }
}
