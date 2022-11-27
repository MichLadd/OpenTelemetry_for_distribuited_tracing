using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OTelUseCase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _logger.LogInformation("Log: IndexModel");
        }

        public void OnGet()
        {
            _logger.LogInformation("Log: Index OnGet");
        }
    }
}