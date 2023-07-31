using Microsoft.AspNetCore.Mvc;

namespace SparkServerLite.Controllers
{
    public class BaseController : Controller
    {
        internal int Page { get; set; }

        internal int ItemsPerPage { get; set; }

        // MARK FOR REMOVAL
        // internal int SkipCount { get { return (this.Page - 1) * this.ItemsPerPage; } }

        internal void SetupPaging(int? page)
        {
            if (page.HasValue)
                this.Page = page.Value;
            else
                this.Page = 1;
        }
    }
}