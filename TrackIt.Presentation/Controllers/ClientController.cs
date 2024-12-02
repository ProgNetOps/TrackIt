using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Presentation.Models.DTO;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers
{
    [Authorize(Roles = "Admin,Super Admin")]
    public class ClientController(IClientService service, IMapper mapper) : Controller
    {
        //The maximun number of Clients returned per page
        private int pageSize = 40;

        [ViewData]
        public string Title { get; set; } = string.Empty;

		[ViewData]
		public string PageHeader { get; set; } = string.Empty;

		[ViewData]
		public string ActionName { get; set; } = string.Empty;

        [ViewData]
        public string ToolTipText { get; set; } = string.Empty;



        [HttpGet]
        public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
        {
            Title = "Clients";
            PageHeader = "Client Management";
            ActionName = "Create";
            ToolTipText = "Add New Client";

            ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { "", "Search Filter" },
                    { nameof(Client.ClientName), "Name" },
                    { nameof(Client.ClientCategory.Name), "Category" }
                };

            /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            ViewData["CurrentSort"] = sortBy;

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";

            ViewData["CategorySortParam"] = sortBy == "clientCategory" ? "clientCategory_desc" : "clientCategory";

            pageNumber ??= 1;

            var (clients,count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

            ViewBag.TotalCount = count;

            return View(new PagedList<Client>(clients.ToList(), count, pageNumber.Value, pageSize));
        }

        
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Title = "Client Details";
            ActionName = "CreateService";
            ToolTipText = "Add New Service";

            var clientDetails = await service.GetByIdAsync(id);
            PageHeader = clientDetails?.ClientName;

            if (clientDetails is not null)
            {
                var clientDetailsViewModel = new ClientDetailsViewModel()
                {
                    Id  = clientDetails.Id,
                    ClientName = clientDetails.ClientName,
                    ClientCategoryId = clientDetails.ClientCategoryId                                       
                };


                if (clientDetails.Circuits.Any())
                {
                    foreach (var circuit in clientDetails.Circuits)
                    {
                        clientDetailsViewModel.CircuitDetailsViewModels.Add(
                            new CircuitDetailsViewModel()
                            {
                                Id = circuit.Id,
                                ClientId = circuit.ClientId,
                                CircuitName = circuit.CircuitName,
                                ServiceAddress = circuit.ServiceAddress,
                                Town = circuit.Town,
                                StateId = circuit.StateId,
                                State = circuit.State,
                                Longitude = circuit.Longitude,
                                Latitude = circuit.Latitude,
                                ServiceId = circuit.ServiceId,
                                AnnualRevenue = circuit.AnnualRevenue,
                                Bandwidth = circuit.Bandwidth,
                                CircuitStateId = circuit.CircuitStateId,
                                IPPoPId = circuit.IPPoPId,
                                AccountManager = circuit.AccountManager,
                                ProjectManager = circuit.ProjectManager,
                                ServiceStartDate = circuit.ServiceStartDate,
                                ClientContactDetails = circuit.ClientContactDetails,
                                LastMileName = circuit.LastMileName,
                                LastMileDeviceId = circuit.LastMileDeviceId,
                                RadioFrequency = circuit.RadioFrequency,
                                TransmissionPath = circuit.TransmissionPath,
                                PathLength = circuit.PathLength,
                                RadioManagementVLAN = circuit.RadioManagementVLAN,
                                ServiceVLAN = circuit.ServiceVLAN,
                                ManagedRadioIPAtPoP = circuit.ManagedRadioIPAtPoP,
                                ManagedRadioIPAtClient = circuit.ManagedRadioIPAtClient,
                                ManagedRadioIPGateway = circuit.ManagedRadioIPGateway,
                                AssignedPublicIP = circuit.AssignedPublicIP,
                                AssignedGateway = circuit.AssignedGateway,
                                AssignedSubnetMask = circuit.AssignedSubnetMask,
                            }
                        );

                    }
                }
                

                return View(clientDetailsViewModel);
            }
            else
            {
                return View("_NotFound");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Title = "Create Client";
            PageHeader = "Create Client";
            var ClientDropdownData = await service.GetNewClientDropdownValues();
            ViewBag.ClientCategories = new SelectList(ClientDropdownData.ClientCategories, "Id", "Name");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateViewModel model)
        {
            Title = "Create Client";
            PageHeader = "Create Client";


            if (ModelState.IsValid)
            {
                var client = new Client()
                {
                    ClientName = model.ClientName,
                    ClientCategoryId = model.ClientCategoryId,
                };

                //Deconstruct the returned valuetuple to get the id of the client
                (Client newClient, Guid clientId) = await service.AddClientAsync(client);

                //Pass the returned id to the details action
                //return RedirectToAction(nameof(Details), clientId);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Pass the dropdown values to the view for it to be re-rendered
                //when there is an error with Client creation
                var ClientDropdownData = await service.GetNewClientDropdownValues();
                ViewBag.ClientCategories = new SelectList(ClientDropdownData.ClientCategories, "Id", "Name");

                return View(model);
            }
        }

    }
}
