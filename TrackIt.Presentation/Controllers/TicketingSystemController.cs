using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers
{
    public class TicketingSystemController(ITicketService service, IMapper mapper) : Controller
    {
        private int pageSize = 40;

        [ViewData]
        public string Title { get; set; } = string.Empty;
        [ViewData]
        public string PageHeader { get; set; } = string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
        {
            Title = "Trouble Tickets";
            PageHeader = "Open Tickets";

            //SEARCH
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { "", "Search Filter" },
                { nameof(Ticket.Circuit.Client.ClientName), "Client Name" },
            };

            /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;
            ViewData["CurrentSort"] = sortBy;
            ViewData["LogTimeSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "logTime_desc" ? "logTime" : "logTime_desc";
            ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";
            ViewData["ZoneSortParam"] = sortBy == "zone" ? "zone_desc" : "zone";

            pageNumber ??= 1;

            var (tickets, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

            ViewBag.TotalCount = count;

            return View(new PagedList<Ticket>(tickets.ToList(), count, pageNumber.Value, pageSize));
        }


        [HttpGet]
        public async Task<IActionResult> Create(Client client)
        {
            Title = "Create Ticket";
            PageHeader = "Create Ticket";
            var TicketsDropdownData = await service.GetNewTicketDropdownValues(client);
            ViewBag.Circuits = new SelectList(TicketsDropdownData.Circuits, "Id", "CircuitName");
            ViewBag.TicketStatuses = new SelectList(TicketsDropdownData.TicketStatuses, "Id", "Status");
            ViewBag.TicketTypes = new SelectList(TicketsDropdownData.TicketTypes, "Id", "TypeOfTicket");
            ViewBag.TicketPriorities = new SelectList(TicketsDropdownData.TicketPriorities, "Id", "Priority");
            
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Create(TicketCreateViewModel model)
        //{
        //    Title = "Create Ticket";
        //    PageHeader = "Create Ticket";


        //    if (ModelState.IsValid)
        //    {
        //        Ticket ticket = new Ticket
        //        {
                    
                    
        //        };

        //        //Deconstruct the returned valuetuple to get the id of the bts
        //        (BTS bts, Guid btsId) = await service.AddBTSAsync(bTS);

        //        //Pass the returned id to the details action
        //        return RedirectToAction(nameof(Details), btsId);
        //    }
        //    else
        //    {
        //        //Pass the dropdown values to the view for it to be re-rendered
        //        //when there is an error with BTS creation
        //        var BTSDropdownData = await service.GetNewBTSDropdownValues();
        //        ViewBag.States = new SelectList(BTSDropdownData.States, "Id", "StateName");
        //        return View(model);
        //    }
        //}

    }
}
