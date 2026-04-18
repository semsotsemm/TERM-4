using System;

namespace DAL003
{
    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityById(int id);
        Celebrity[] getCelebritiesBySurname(string Surname);
        string? getPhotoPathById(int id);
    }
}