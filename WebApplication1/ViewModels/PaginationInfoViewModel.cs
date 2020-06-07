
namespace Microsoft.WebApplication1.ViewModels
{
    public class PaginationInfoViewModel
    {
        public int totalItems { get; set; }
        public int itemsPerPage { get; set; }
        public int actualPage { get; set; }
        public int totalPages { get; set; }
        public string next { get; set; }
        public string previous { get; set; }

        public override string ToString()
        {
            return "totalItems " + totalItems + "itemsPerPage " + itemsPerPage + "actualPage: " + actualPage + "totalPage: " + totalPages;
        }
    }
}

    