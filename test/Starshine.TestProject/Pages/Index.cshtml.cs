using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Starshine.TestProject.Repositorys;

namespace Starshine.TestProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITestEfCoreRepository _testEfCoreRepository;

        public IndexModel(ILogger<IndexModel> logger, ITestEfCoreRepository testEfCoreRepository)
        {
            _logger = logger;
            _testEfCoreRepository = testEfCoreRepository;
        }

        public async void OnGet()
        {
            var count = await _testEfCoreRepository.CountAsync();
        }
    }
}
