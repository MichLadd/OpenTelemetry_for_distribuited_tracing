using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OTelUseCase.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            _logger.LogInformation("Log: PrivacyModel");
        }

        public void OnGet()
        {
            _logger.LogInformation("Log: Privacy OnGet");
        }
    }
}