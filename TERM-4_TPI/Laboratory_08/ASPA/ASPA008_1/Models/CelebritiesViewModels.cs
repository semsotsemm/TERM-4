using System.ComponentModel.DataAnnotations;
using DAL_Celebrity;

namespace ASPA008_1.Models;

public class IndexViewModel
{
    public List<DAL_Celebrity_MSSQL.Celebrity> Celebrities { get; set; } = new();
    public string PhotosRequestPath { get; set; } = "/Photos";
}

public class HumanViewModel
{
    public DAL_Celebrity_MSSQL.Celebrity Celebrity { get; set; } = new();
    public List<DAL_Celebrity_MSSQL.Liveevent> Lifeevents { get; set; } = new();
    public Dictionary<string, string> WikiReferences { get; set; } = new();
    public string PhotosRequestPath { get; set; } = "/Photos";
}

public class CelebrityFormModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Nationality { get; set; } = string.Empty;

    public string? ExistingPhotoPath { get; set; }
    public string SubmitAction { get; set; } = "NewHumanForm";
    public bool RequirePhoto { get; set; }
}

public class ConfirmCelebrityViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string? ReqPhotoPath { get; set; }
}
