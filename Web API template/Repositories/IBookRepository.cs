﻿using WebAPI_simple.Helpers;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks();
        BookWithAuthorAndPublisherDTO? GetBookById(int id);
        Result<Books> AddBook(AddBookRequestDTO addBookRequestDTO);

        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Books? DeleteBookById(int id);
    }
}