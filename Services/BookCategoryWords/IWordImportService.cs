﻿using Services.BookCategoryWords.Models;

namespace Services.BookCategoryWords
{
    public interface IWordImportService
    {
        Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria criteria);
        Task<bool> DeleteWordAsync(int wordId);
        Task<WordModel> UpdateWordAsync(int id, AddWordModel model);
        Task<WordModel> GetByIdAsync(int id);
    }
}
