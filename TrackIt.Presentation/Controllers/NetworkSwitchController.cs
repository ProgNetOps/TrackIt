using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Domain;
using TrackIt.Presentation.Models.DTO;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;


[Authorize(Roles = "Admin,Super Admin")]
public class NetworkSwitchController(INetworkSwitchService service, IMapper mapper) : Controller
{

    //The maximun number of BTS returned per page
    private int pageSize = 40;

    [ViewData]
    public string? Title { get; set; }

    [ViewData]
    public string? PageHeader { get; set; }

    [ViewData]
    public string? ToolTipText { get; set; }

    [ViewData]
    public string? ActionName { get; set; }

    [ViewData]
    public string? CrudActionType { get; set; }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
    {
        Title = "Network Switches";
        PageHeader = "Network Switches";
        ToolTipText = "Add New Switch";
        CrudActionType = "Create";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { "", "Search Filter" },
                    { nameof(NetworkSwitch.IPPoP.BTS.State.StateName), "State" },
                    { nameof(NetworkSwitch.IPPoP.IPPoPName), "POP" },
                };

        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewData["CurrentSort"] = sortBy;

        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";

        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";

        ViewData["PoPSortParam"] = sortBy == "ippop" ? "ippop_desc" : "ippop";

        pageNumber ??= 1;

        //var networkSwitches = await service.GetSortedPagesAsync(sortBy, pageNumber, pageSize);
        var (networkSwitches, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);


        ViewBag.TotalCount = count;

        return View(new PagedList<NetworkSwitch>(networkSwitches.ToList(), count, pageNumber.Value, pageSize));
    }


    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Title = "Switch";
        ToolTipText = "Edit Switch";


        var SwitchDetails = await service.GetByIdAsync(id);
        PageHeader = SwitchDetails.SwitchName.ToUpper();

        if (SwitchDetails != null)
        {
            NetworkSwitchDTO SwitchDetailsDTO = mapper.Map<NetworkSwitchDTO>(SwitchDetails);
            return View(SwitchDetailsDTO);
        }
        else
        {
            return View(nameof(NotFound));
        }
    }









}
