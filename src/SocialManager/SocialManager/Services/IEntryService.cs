using SocialManager.Shared.DTOs;

namespace SocialManager.Services;

/// <summary>
/// Service interface for managing blog entries
/// </summary>
public interface IEntryService
{
    Task<(List<EntryListItemResponseDto> Items, int TotalCount)> GetEntriesAsync(int page, int pageSize, string? sortField = null, string? sortDirection = null, string? filterText = null);
    Task<PostEditorResponseDto?> GetEntryAsync(string id);
    Task<string> CreateEntryAsync(PostEditorResponseDto entry);
    Task<bool> UpdateEntryAsync(PostEditorResponseDto entry);
    Task<bool> DeleteEntryAsync(string id);
}
