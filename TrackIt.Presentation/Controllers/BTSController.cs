
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rotativa.AspNetCore;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Presentation.Models.DTO;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;

public class BTSController(IBaseStationService service) : Controller
{
    //The maximun number of BTS returned per page
    private int pageSize = 40;

    [ViewData]
    public string? Title { get; set; } = string.Empty;

    [ViewData]
    public string? PageHeader { get; set; } = string.Empty;

    [ViewData]
    public string? ToolTipText { get; set; }


    [HttpGet]
    public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
    {
        Title = "Base Stations";
        PageHeader = "Base Stations";
        ToolTipText = "Add New BTS";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { "", "Search Filter" },
            { nameof(BTS.BTSName), "Site Id" },
            { nameof(BTS.State.StateName), "State" },
            { nameof(BTS.LocationAddress), "Address" },
        };

        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewData["CurrentSort"] = sortBy;

        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";

        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";

        pageNumber ??= 1;

        //var baseStations = await service.GetSortedPagesAsync(sortBy, pageNumber, pageSize);
        var (baseStations,count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

        ViewBag.TotalCount = count;
        return View(new PagedList<BTS>(baseStations.ToList(), count, pageNumber.Value, pageSize));
    }


    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        ViewBag.EditId = id;
        Title = "BTS";

        var btsDetails = await service.GetBTSDetails(id);
        PageHeader = $"{btsDetails.BTSName} Details";

        if (btsDetails is not null)
        {
            BTSDetailsViewModel bTSDetailsViewModel = new BTSDetailsViewModel()
            {
                BTSName = btsDetails.BTSName,
                LocationAddress = btsDetails.LocationAddress,
                StateId = btsDetails.StateId,
                State = btsDetails.State,
                Coordinates = btsDetails.Coordinates
            };

            return View(bTSDetailsViewModel);
        }
        else
        {
            return View(nameof(NotFound));
        }
    }
    

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        Title = "Create BTS";
        PageHeader = "Create BTS";
        var BTSDropdownData = await service.GetNewBTSDropdownValues();
        ViewBag.States = new SelectList(BTSDropdownData.States, "Id", "StateName");

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(BTSCreateViewModel model)
    {
        Title = "Create BTS";
        PageHeader = "Create BTS";


        if (ModelState.IsValid)
        {
            BTS bTS = new BTS
            {
                BTSName = model.BTSName,
                LocationAddress = model.LocationAddress,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                StateId = model.StateId,
            };

            //Deconstruct the returned valuetuple to get the id of the bts
            (BTS bts, Guid btsId) = await service.AddBTSAsync(bTS);

            //Pass the returned id to the details action
            return RedirectToAction(nameof(Details), btsId);
        }
        else
        {
            //Pass the dropdown values to the view for it to be re-rendered
            //when there is an error with BTS creation
            var BTSDropdownData = await service.GetNewBTSDropdownValues();
            ViewBag.States = new SelectList(BTSDropdownData.States, "Id", "StateName");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        Title = "Edit BTS";
        PageHeader = "Edit BTS";

        var BTSDetails = await service.GetBTSDetails(id);
        if (BTSDetails is null)
        {
            return View("_NotFound");
        }
        else
        {
            var BTSDropdownData = await service.GetNewBTSDropdownValues();
            ViewBag.States = new SelectList(BTSDropdownData.States, "Id", "StateName");

            BTSEditViewModel btsEditViewModel = new BTSEditViewModel
            {
                Id = BTSDetails.Id,
                BTSName = BTSDetails.BTSName,
                Latitude = BTSDetails.Latitude,
                Longitude = BTSDetails.Longitude,
                LocationAddress = BTSDetails.LocationAddress,
                StateId = BTSDetails.StateId
            };

            return View(btsEditViewModel);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BTSEditViewModel model)
    {
        Title = "Edit BTS";
        PageHeader = "Edit BTS";


        if (ModelState.IsValid)
        {
            BTS? bts = await service.GetBTSDetails(model.Id);

            bts.BTSName = model.BTSName;
            bts.LocationAddress = model.LocationAddress;
            bts.Longitude = model.Longitude;
            bts.Latitude = model.Latitude;
            bts.StateId = model.StateId;


            service.UpdateBTS(bts);
            return RedirectToAction("Index");

        }
        else
        {
            var BTSDropdownData = await service.GetNewBTSDropdownValues();
            ViewBag.States = new SelectList(BTSDropdownData.States, "Id", "StateName");
            return View(model);
        }
    }


    public async Task<IActionResult> BTSPdf()
    {
        var baseStations = await service.GetSamplePdf();

        return new ViewAsPdf(nameof(BTSPdf), baseStations, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }

}
   



