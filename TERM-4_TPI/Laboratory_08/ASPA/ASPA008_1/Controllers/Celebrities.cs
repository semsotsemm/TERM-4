using ANC25_WEBAPI_DLL;
using ASPA008_1.Filters;
using ASPA008_1.Models;
using DAL_Celebrity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASPA008_1.Controllers;

public class CelebritiesController : Controller
{
    private readonly DAL_Celebrity_MSSQL.IRepository _repo;
    private readonly ANC25_WEBAPI_DLL.CelebritiesConfig _config;

    public CelebritiesController(
        DAL_Celebrity_MSSQL.IRepository repo,
        IOptions<ANC25_WEBAPI_DLL.CelebritiesConfig> config)
    {
        _repo = repo;
        _config = config.Value;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new IndexViewModel
        {
            Celebrities = _repo.GetAllCelebrities(),
            PhotosRequestPath = _config.PhotosRequestPath
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Error() => RedirectToAction(nameof(Index));

    [HttpGet]
    [CelebrityAsyncActionFilter("WIKI")]
    public IActionResult Human(int id)
    {
        var celebrity = _repo.GetCelebrityById(id);
        if (celebrity is null) return RedirectToAction(nameof(Index));

        var model = new HumanViewModel
        {
            Celebrity = celebrity,
            Lifeevents = _repo.GetLifeeventsByCelebrityId(id),
            PhotosRequestPath = _config.PhotosRequestPath,
            WikiReferences = HttpContext.Items[CelebrityAsyncActionFilter.Wikipedia] as Dictionary<string, string>
                             ?? new Dictionary<string, string>()
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult NewHumanForm()
    {
        return View(new CelebrityFormModel { SubmitAction = nameof(NewHumanForm), RequirePhoto = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NewHumanForm(CelebrityFormModel formModel, IFormFile? upload)
    {
        if (upload is null || upload.Length == 0)
            ModelState.AddModelError(nameof(upload), "Upload photo is required.");

        if (!ModelState.IsValid)
        {
            formModel.SubmitAction = nameof(NewHumanForm);
            formModel.RequirePhoto = true;
            return View(formModel);
        }

        var reqPhotoPath = SavePhoto(upload!);
        var model = new ConfirmCelebrityViewModel
        {
            Id = 0,
            FullName = formModel.FullName,
            Nationality = formModel.Nationality,
            ReqPhotoPath = reqPhotoPath
        };
        return View("NewHumanConfirm", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NewHumanConfirm(ConfirmCelebrityViewModel model)
    {
        var created = _repo.AddCelebrity(new DAL_Celebrity_MSSQL.Celebrity
        {
            FullName = model.FullName,
            Nationality = model.Nationality,
            ReqPhotoPath = model.ReqPhotoPath
        });
        if (!created) return RedirectToAction(nameof(Index));

        var id = _repo.GetCelebrityIdByName(model.FullName);
        return RedirectToAction(nameof(Human), new { id });
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var celebrity = _repo.GetCelebrityById(id);
        if (celebrity is null) return RedirectToAction(nameof(Index));

        return View(new CelebrityFormModel
        {
            Id = id,
            FullName = celebrity.FullName,
            Nationality = celebrity.Nationality,
            ExistingPhotoPath = celebrity.ReqPhotoPath,
            SubmitAction = nameof(Edit),
            RequirePhoto = false
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CelebrityFormModel formModel, IFormFile? upload)
    {
        if (!ModelState.IsValid)
        {
            formModel.SubmitAction = nameof(Edit);
            formModel.RequirePhoto = false;
            return View(formModel);
        }

        string? reqPhotoPath = formModel.ExistingPhotoPath;
        if (upload is { Length: > 0 }) reqPhotoPath = SavePhoto(upload);

        var updated = _repo.UpdCelebrity(formModel.Id, new DAL_Celebrity_MSSQL.Celebrity
        {
            Id = formModel.Id,
            FullName = formModel.FullName,
            Nationality = formModel.Nationality,
            ReqPhotoPath = reqPhotoPath
        });
        if (!updated) return RedirectToAction(nameof(Index));

        return RedirectToAction(nameof(Human), new { id = formModel.Id });
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var celebrity = _repo.GetCelebrityById(id);
        if (celebrity is null) return RedirectToAction(nameof(Index));

        return View("DeleteHumanConfirm", new ConfirmCelebrityViewModel
        {
            Id = celebrity.Id,
            FullName = celebrity.FullName,
            Nationality = celebrity.Nationality,
            ReqPhotoPath = celebrity.ReqPhotoPath
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteHumanConfirm(int id)
    {
        _repo.DelCelebrity(id);
        return RedirectToAction(nameof(Index));
    }

    private string SavePhoto(IFormFile upload)
    {
        var safeFileName = Path.GetFileName(upload.FileName);
        var ext = Path.GetExtension(safeFileName);
        if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";

        var normalizedName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(_config.PhotoFolder, normalizedName);

        Directory.CreateDirectory(_config.PhotoFolder);
        using var fs = System.IO.File.Create(fullPath);
        upload.CopyTo(fs);
        return normalizedName;
    }
}
