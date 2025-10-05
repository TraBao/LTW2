using BookWebApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookWebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7204/api";

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private string? GetJwtToken() => HttpContext.Session.GetString("JwtToken");

        private HttpClient CreateApiClient()
        {
            var client = _httpClientFactory.CreateClient();
            var token = GetJwtToken();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private async Task PopulateDropdowns()
        {
            try
            {
                var client = CreateApiClient();
                var authorResponse = await client.GetAsync($"{_apiBaseUrl}/authors/get-all-author");
                if (authorResponse.IsSuccessStatusCode)
                    ViewBag.Authors = await authorResponse.Content.ReadFromJsonAsync<IEnumerable<AuthorDTO>>();

                var publisherResponse = await client.GetAsync($"{_apiBaseUrl}/publishers/get-all-publisher");
                if (publisherResponse.IsSuccessStatusCode)
                    ViewBag.Publishers = await publisherResponse.Content.ReadFromJsonAsync<IEnumerable<PublisherDTO>>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không thể tải dữ liệu cho form. Lỗi: " + ex.Message;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");

            var response = new List<BookWithAuthorAndPublisherDTO>();
            try
            {
                var client = CreateApiClient();
                var httpResponseMessage = await client.GetAsync($"{_apiBaseUrl}/books/get-all-books");

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Auth");

                httpResponseMessage.EnsureSuccessStatusCode();
                var books = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<BookWithAuthorAndPublisherDTO>>();
                if (books != null) response.AddRange(books);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");
            await PopulateDropdowns();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBookRequestDTO request)
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(request);
            }

            try
            {
                var client = CreateApiClient();
                var httpResponseMessage = await client.PostAsJsonAsync($"{_apiBaseUrl}/books/add-book", request);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi từ API ({(int)httpResponseMessage.StatusCode}): {errorContent}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi kết nối: {ex.Message}");
            }
            await PopulateDropdowns();
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");

            await PopulateDropdowns();

            var client = CreateApiClient();
            var book = await client.GetFromJsonAsync<AddBookRequestDTO>($"{_apiBaseUrl}/books/get-book-by-id/{id}");

            if (book != null)
            {
                return View(book);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddBookRequestDTO request)
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");

            try
            {
                var client = CreateApiClient();
                var httpResponseMessage = await client.PutAsJsonAsync($"{_apiBaseUrl}/books/update-book-by-id/{id}", request);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await PopulateDropdowns();
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetJwtToken() == null) return RedirectToAction("Login", "Auth");

            try
            {
                var client = CreateApiClient();
                var httpResponseMessage = await client.DeleteAsync($"{_apiBaseUrl}/books/delete-book-by-id/{id}");
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index", "Books");
        }
    }
}