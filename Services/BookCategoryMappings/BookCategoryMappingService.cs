﻿using Services.BookCategories;
using Services.BookCategoryMappings.Models;
using Services.BookCategoryMappings.Repository;
using Services.Books;

namespace Services.BookCategoryMappings;

public class BookCategoryMappingService : IBookCategoryMappingService
{
    private readonly IBookCategoryMappingRepository _repository;
    private readonly IBookCategoryService _categoryService;
    private readonly IBookService _bookService;

    public BookCategoryMappingService(IBookCategoryMappingRepository repository, IBookCategoryService categoryService, IBookService bookService)
    {
        _repository = repository;
        _categoryService = categoryService;
        _bookService = bookService;
    }

    public async Task<BookCategoryMappingsForAddModel> GetBookCategoryMappingsByForAdd(int bookCategoryId, string keywords)
    {
        var mappings = await _repository.GetByBookCategoryIdAsync(bookCategoryId);
        var books = await _bookService.GetAllAsync();

        var relatedBooks = books.Books.ToList();
        if (!string.IsNullOrEmpty(keywords)){
            relatedBooks = relatedBooks.Where(x => x.BookName.Contains(keywords)
                                    || x.Semester.Contains(keywords)
                                    || x.Version.Contains(keywords)
                                    || x.AuditYear.ToString().Contains(keywords)
                                    || x.Grade.Contains(keywords)).ToList();
        }

        var result = new BookCategoryMappingsForAddModel();
        if (!mappings.Any())
        {
            result.BookCategory = await _categoryService.GetByIdAsync(bookCategoryId);
            result.NewBooks = relatedBooks.ToList();
            return result;
        }

        result.BookCategory = new BookCategoryModel(mappings.First()!.BookCategory);
        result.LinkedBooks = mappings.Select(x => new BookModel(x.Book)).ToList();
        result.NewBooks = relatedBooks.Where(x=> !result.LinkedBooks.Any(y=>x.BookId == y.BookId)).ToList();
        return result;
    }

    public async Task AddBookCategoryMappingAsync(int bookCategoryId, int bookId)
    {
        await _repository.AddAsync(bookCategoryId, bookId);
    }

    public async Task RemoveBookCategoryMappingAsync(int bookCategoryId, int bookId)
    {
        await _repository.RemoveAsync(bookCategoryId, bookId);
    }

    public async Task<BookCategoryMappingsResult> GetBookCategoryMappingsById(int bookCategoryId)
    {
        var mappings = await _repository.GetByBookCategoryIdAsync(bookCategoryId);

        var result = new BookCategoryMappingsResult();
        if (!mappings.Any())
        {
            result.BookCategory = await _categoryService.GetByIdAsync(bookCategoryId);
            return result;
        }

        result.BookCategory = new BookCategoryModel(mappings.First()!.BookCategory);
        result.LinkedBooks = mappings.Select(x => new BookModel(x.Book)).ToList();
        return result;
    }
}