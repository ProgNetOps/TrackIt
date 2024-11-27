using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TrackIt.Domain;
using TrackIt.Presentation.Models.DTO;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;

public class IPPoPController(IIPPoPService service, IMapper mapper) : Controller
{

   //The maximun number of IPPoPs returned per page
   private int pageSize = 40;

   [ViewData]
   public string? Title { get; set; }

   [ViewData]
   public string? PageHeader { get; set; }

   [ViewData]
   public string? ToolTipText { get; set; }



    public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
   {
        Title = "IP PoPs";
        PageHeader = "IP PoPs";
        ToolTipText = "Add New IP PoP";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { "", "Search Filter" },
            { nameof(IPPoP.IPPoPName), "PoP" },
            { nameof(IPPoP.BTS.State.StateName), "State" }
        };

        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;
        ViewData["CurrentSort"] = sortBy;
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";
        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";

        pageNumber ??= 1;

        var (iPPoPs, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);


        ViewBag.TotalCount = count;

        return View(new PagedList<IPPoP>(iPPoPs.ToList(), count, pageNumber.Value, pageSize));
   }


    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Title = "IP PoP";

        var PoPDetails = await service.GetByIdAsync(id);
        PageHeader = PoPDetails.IPPoPName.ToUpper();

        if (PoPDetails != null)
        {
            IPPoPDTO PoPDetailsDTO = mapper.Map<IPPoPDTO>(PoPDetails);
            return View(PoPDetailsDTO);
        }
        else
        {
            return View(nameof(NotFound));
        }
    }




}

