using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Domain;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;

[Authorize(Roles = "Admin,Super Admin")]
public class DCNRouterController(IDCNRouterService service, IMapper mapper) : Controller
{
    //The maximun number of routers returned per page
    private int pageSize = 40;

    [ViewData]
    public string Title { get; set; } = string.Empty;

    [ViewData]
    public string PageHeader { get; set; } = string.Empty;

    [ViewData]
    public string? ToolTipText { get; set; }


    [HttpGet]
    public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
    {
        Title = "MPLS Router";
        PageHeader = "MPLS Routers";
        ToolTipText = "Add New Router";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { "", "Search Filter" },
                    { nameof(DCNRouter.RouterName), "Router Name" },
                    { nameof(DCNRouter.IPPoP.BTS.State), "State" },
                    { nameof(DCNRouter.RouterType), "Router Type" },
                };

        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewData["CurrentSort"] = sortBy;

        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";

        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";

        ViewData["PoPSortParam"] = sortBy == "ippop" ? "ippop_desc" : "ippop";


        pageNumber ??= 1;

        var (dcnRouters, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

        ViewBag.TotalCount = count;

        return View(new PagedList<DCNRouter>(dcnRouters.ToList(), count, pageNumber.Value, pageSize));
    }

}



