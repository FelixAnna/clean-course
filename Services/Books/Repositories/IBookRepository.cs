﻿using Entities.Entities;
using Services.Books.Models;

namespace Services.Books.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookEntity>> GetAllAsync();
    Task<IEnumerable<BookEntity>> FindAsync(string keywords);
    Task<BookEntity?> GetByIdAsync(int bookId);
    Task<BookEntity> AddAsync(AddBookModel model);
    Task<BookEntity> UpdateAsync(int bookId, AddBookModel model);
    Task<bool> RemoveAsync(int bookId);
}
