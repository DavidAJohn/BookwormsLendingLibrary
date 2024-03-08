using BookwormsUI.Models;

namespace BookwormsUI.Contracts;

public interface ISettingsService
{
    ApiEndpoints GetAppSettingsApiEndpoints();
    string GetAssetBaseUrl();
    string GetApiBaseUrl();
}
