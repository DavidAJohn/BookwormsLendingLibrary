namespace BookwormsUI.Services
{
    public class CategoryService
    {
        private readonly SettingsService _settings;
        public CategoryService(SettingsService settings)
        {
            _settings = settings;
        }

        public string GetCategoriesApiEndpoint()
        {
            var settings = _settings.GetAppSettingsApiEndpoints();

            var baseUrl = settings.BaseUrl;
            var categoriesEndpoint = settings.CategoriesEndpoint;
            var url = baseUrl + categoriesEndpoint;

            return url;
        }
    }
}