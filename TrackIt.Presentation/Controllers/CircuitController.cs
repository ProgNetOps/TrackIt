using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Presentation.Models.DTO;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;

public class CircuitController(ICircuitService service, IMapper mapper) : Controller
{
    //The maximun number of Circuits returned per page
    private int pageSize = 40;

    [ViewData]
    public string? Title { get; set; }

    [ViewData]
    public string? PageHeader { get; set; }

    [ViewData]
    public string? ToolTipText { get; set; }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchBy, string? searchString, string sortBy, int? pageNumber)
    {
        Title = "Services";
        PageHeader = "Services";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { "", "Search Filter" },
                { nameof(Circuit.CircuitName), "Service Address" },
                { nameof(Circuit.State.StateName), "State" },
                { nameof(Circuit.State.Zone.ZoneName), "Zone" },
                { nameof(Circuit.State.Zone.TechnicalRegion.RegionName), "Region" },
                { nameof(Circuit.AccountManager), "Account Manager" },
                { nameof(Circuit.ProjectManager), "Project Manager" },
                { nameof(Circuit.CircuitState.Name), "Circuit State" }
            };

        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewData["CurrentSort"] = sortBy;

        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "name_desc" ? "name" : "name_desc";

        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";

        ViewData["ZoneSortParam"] = sortBy == "zone" ? "zone_desc" : "zone";

        ViewData["RegionSortParam"] = sortBy == "region" ? "region_desc" : "region";

        ViewData["ProjectManagerSortParam"] = sortBy == "projectManager" ? "projectManager_desc" : "projectManager";

        ViewData["AccountManagerSortParam"] = sortBy == "accountManager" ? "accountManager_desc" : "accountManager";

        ViewData["ServiceSortParam"] = sortBy == "service" ? "service_desc" : "service";

        ViewData["BandwidthSortParam"] = sortBy == "bandwidth" ? "bandwidth_desc" : "bandwidth";

        ViewData["AddressSortParam"] = sortBy == "address" ? "address_desc" : "address";

        ViewData["IPPOPTypeSortParam"] = sortBy == "ippop" ? "ippop_desc" : "ippop";
        pageNumber ??= 1;

        var (circuits, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

        ViewBag.TotalCount = count;

        return View(new PagedList<Circuit>(circuits.ToList(), count, pageNumber.Value, pageSize));
    }

    [HttpGet]
    public async Task<IActionResult> CreateService()
    {
        Title = "Create Service";
        PageHeader = "Create Service";


        var CircuitDropdownData = await service.GetNewCircuitDropdownValues();
        ViewBag.CircuitStates = new SelectList(CircuitDropdownData.CircuitStates, "Id", "Name");
        ViewBag.States = new SelectList(CircuitDropdownData.States, "Id", "StateName");
        ViewBag.IPPoPs = new SelectList(CircuitDropdownData.IPPoPs, "Id", "IPPoPName");
        ViewBag.LastMileDevices = new SelectList(CircuitDropdownData.LastMileDevices, "Id", "LastMileDeviceName");
        ViewBag.Services = new SelectList(CircuitDropdownData.Services, "Id", "Name");
        ViewBag.Clients = new SelectList(CircuitDropdownData.Clients, "Id", "ClientName");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(CircuitCreateViewModel model)
    {
        Title = "Create Service";
        PageHeader = "Create Service";

        //Pass the dropdown values to the view for it to be re-rendered
        //when there is an error with Circuit creation
        var CircuitDropdownData = await service.GetNewCircuitDropdownValues();
        ViewBag.CircuitStates = new SelectList(CircuitDropdownData.CircuitStates, "Id", "Name");
        ViewBag.States = new SelectList(CircuitDropdownData.States, "Id", "StateName");
        ViewBag.IPPoPs = new SelectList(CircuitDropdownData.IPPoPs, "Id", "IPPoPName");
        ViewBag.LastMileDevices = new SelectList(CircuitDropdownData.LastMileDevices, "Id", "LastMileDeviceName");
        ViewBag.Services = new SelectList(CircuitDropdownData.Services, "Id", "Name");
        ViewBag.Clients = new SelectList(CircuitDropdownData.Clients, "Id", "ClientName");



        if (ModelState.IsValid)
        {
            Circuit circuit = new Circuit()
            {
                ClientId = model.ClientId,
                CircuitName = model.CircuitName,
                ServiceAddress = model.ServiceAddress,
                Town = model.Town,
                StateId = model.StateId,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                LinkID = model.LinkID,
                ODUSerialNumber = model.ODUSerialNumber,
                IDUSerialNumber = model.IDUSerialNumber,
                ServiceId = model.ServiceId,
                AnnualRevenue = model.AnnualRevenue,
                Bandwidth = model.Bandwidth,
                CircuitStateId = model.CircuitStateId,
                IPPoPId = model.IPPoPId,
                AccountManager = model.AccountManager,  
                ProjectManager = model.ProjectManager,
                ServiceStartDate = model.ServiceStartDate,
                ClientContactDetails = model.ClientContactDetails,
                InstallersContactDetails = model.InstallersContactDetails,
                LastMileName = model.LastMileName,
                LastMileDeviceId = model.LastMileDeviceId,
                RadioFrequency = model.RadioFrequency,
                TransmissionPath = model.TransmissionPath,
                PathLength = model.PathLength,
                RadioManagementVLAN = model.RadioManagementVLAN,
                ServiceVLAN = model.ServiceVLAN,
                ManagedRadioIPAtPoP = model.ManagedRadioIPAtPoP,
                ManagedRadioIPAtClient = model.ManagedRadioIPAtClient,
                ManagedRadioIPGateway = model.ManagedRadioIPGateway,
                AssignedPublicIP = model.AssignedPublicIP,
                AssignedGateway = model.AssignedGateway,
                AssignedSubnetMask = model.AssignedSubnetMask,
            };
            
            //Deconstruct the returned valuetuple to get the id of the circuit
            (Circuit newCircuit, Guid circuitId) =  await service.AddCircuitAsync(circuit);

            //Pass the returned id to the details action
            //return RedirectToAction(nameof(Details), clientId);
            return RedirectToAction(nameof(Details), new {id= circuitId });
        }
        else
        {
            //ALL REQUIRED PROPERTIES OF THE MODEL MUST BE ON THE VIEW OTHERWISE
            //THERE WILL BE NO ERRORS thrown BUT THE CREATION WILL FAIL

            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();
            
            return View(model);
        }

    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Title = "Service Details";
        ViewBag.EditId = id;

        var circuitDetails = await service.GetCircuitDetails(id);

        PageHeader = circuitDetails?.CircuitName;


        if (circuitDetails is not null)
        {
            CircuitDetailsViewModel circuitDetailsViewModel = new CircuitDetailsViewModel()
            {
                Id = circuitDetails.Id,
                ClientId = circuitDetails.ClientId,
                Client = circuitDetails.Client,
                CircuitName = circuitDetails.CircuitName,
                ServiceAddress = circuitDetails.ServiceAddress,
                Town = circuitDetails.Town,
                StateId = circuitDetails.StateId,
                State = circuitDetails.State,
                LinkID = circuitDetails.LinkID,
                ODUSerialNumber = circuitDetails.ODUSerialNumber,
                IDUSerialNumber = circuitDetails.IDUSerialNumber,
                Longitude = circuitDetails.Longitude,
                Latitude = circuitDetails.Latitude,
                ServiceId = circuitDetails.ServiceId,
                Service = circuitDetails.Service,
                AnnualRevenue = circuitDetails.AnnualRevenue,
                InstallersContactDetails = circuitDetails.InstallersContactDetails,
                Bandwidth = circuitDetails.Bandwidth,
                CircuitStateId = circuitDetails.CircuitStateId,
                CircuitState = circuitDetails.CircuitState,
                IPPoPId = circuitDetails.IPPoPId,
                IPPoP = circuitDetails.IPPoP,
                AccountManager = circuitDetails.AccountManager,
                ProjectManager = circuitDetails.ProjectManager,
                ServiceStartDate = circuitDetails.ServiceStartDate,
                ClientContactDetails = circuitDetails.ClientContactDetails,
                LastMileName = circuitDetails.LastMileName,
                LastMileDeviceId = circuitDetails.LastMileDeviceId,
                LastMileDevice = circuitDetails.LastMileDevice,
                RadioFrequency = circuitDetails.RadioFrequency,
                TransmissionPath = circuitDetails.TransmissionPath,
                PathLength = circuitDetails.PathLength,
                RadioManagementVLAN = circuitDetails.RadioManagementVLAN,
                ServiceVLAN = circuitDetails.ServiceVLAN,
                ManagedRadioIPAtPoP = circuitDetails.ManagedRadioIPAtPoP,
                ManagedRadioIPAtClient = circuitDetails.ManagedRadioIPAtClient,
                ManagedRadioIPGateway = circuitDetails.ManagedRadioIPGateway,
                AssignedPublicIP = circuitDetails.AssignedPublicIP,
                AssignedGateway = circuitDetails.AssignedGateway,
                AssignedSubnetMask = circuitDetails.AssignedSubnetMask,
            };

            ViewBag.AnnualRevenue = $"\u20A6 {circuitDetailsViewModel.AnnualRevenue.ToString()}";
            ViewBag.Bandwidth = $"{circuitDetailsViewModel.Bandwidth.ToString()} Mbps";

			return View(circuitDetailsViewModel);
        }
        else
        {
            return View("_NotFound");
        }

    }

	[HttpGet]
	public async Task<IActionResult> Edit(Guid id)
	{
		Title = "Edit Service Details";
		PageHeader = "Edit Service Details";

		var CircuitDetails = await service.GetCircuitDetails(id);
		if (CircuitDetails is null)
		{
			return View("_NotFound");
		}
		else
		{
			var CircuitDropdownData = await service.GetNewCircuitDropdownValues();
			ViewBag.CircuitStates = new SelectList(CircuitDropdownData.CircuitStates, "Id", "Name");
			ViewBag.States = new SelectList(CircuitDropdownData.States, "Id", "StateName");
			ViewBag.IPPoPs = new SelectList(CircuitDropdownData.IPPoPs, "Id", "IPPoPName");
			ViewBag.LastMileDevices = new SelectList(CircuitDropdownData.LastMileDevices, "Id", "LastMileDeviceName");
			ViewBag.Services = new SelectList(CircuitDropdownData.Services, "Id", "Name");
			ViewBag.Clients = new SelectList(CircuitDropdownData.Clients, "Id", "ClientName");


			CircuitEditViewModel circuitEditViewModel = new CircuitEditViewModel
			{
				ClientId = CircuitDetails.ClientId,
				CircuitName = CircuitDetails.CircuitName,
				ServiceAddress = CircuitDetails.ServiceAddress,
				Town = CircuitDetails.Town,
				StateId = CircuitDetails.StateId,
				Longitude = CircuitDetails.Longitude,
				Latitude = CircuitDetails.Latitude,
				ServiceId = CircuitDetails.ServiceId,
				AnnualRevenue = CircuitDetails.AnnualRevenue,
                LinkID = CircuitDetails.LinkID,
                ODUSerialNumber = CircuitDetails.ODUSerialNumber,
                IDUSerialNumber = CircuitDetails.IDUSerialNumber,
				Bandwidth = CircuitDetails.Bandwidth,
				CircuitStateId = CircuitDetails.CircuitStateId,
				IPPoPId = CircuitDetails.IPPoPId,
				AccountManager = CircuitDetails.AccountManager,
                InstallersContactDetails = CircuitDetails.InstallersContactDetails,
                ProjectManager = CircuitDetails.ProjectManager,
				ServiceStartDate = CircuitDetails.ServiceStartDate,
				ClientContactDetails = CircuitDetails.ClientContactDetails,
				LastMileName = CircuitDetails.LastMileName,
				LastMileDeviceId = CircuitDetails.LastMileDeviceId,
				RadioFrequency = CircuitDetails.RadioFrequency,
				TransmissionPath = CircuitDetails.TransmissionPath,
				PathLength = CircuitDetails.PathLength,
				RadioManagementVLAN = CircuitDetails.RadioManagementVLAN,
				ServiceVLAN = CircuitDetails.ServiceVLAN,
				ManagedRadioIPAtPoP = CircuitDetails.ManagedRadioIPAtPoP,
				ManagedRadioIPAtClient = CircuitDetails.ManagedRadioIPAtClient,
				ManagedRadioIPGateway = CircuitDetails.ManagedRadioIPGateway,
				AssignedPublicIP = CircuitDetails.AssignedPublicIP,
				AssignedGateway = CircuitDetails.AssignedGateway,
				AssignedSubnetMask = CircuitDetails.AssignedSubnetMask,
			};
            return View(circuitEditViewModel);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Edit(CircuitEditViewModel model)
	{
		Title = "Edit Service Details";
		PageHeader = "Edit Service Details";


		if (ModelState.IsValid)
		{
			Circuit? circuit = await service.GetCircuitDetails(model.Id);

            circuit.ClientId = model.ClientId;
			circuit.CircuitName = model.CircuitName;
			circuit.ServiceAddress = model.ServiceAddress;
			circuit.Town = model.Town;
			circuit.StateId = model.StateId;
			circuit.Longitude = model.Longitude;
			circuit.Latitude = model.Latitude;
			circuit.ServiceId = model.ServiceId;
			circuit.AnnualRevenue = model.AnnualRevenue;
			circuit.Bandwidth = model.Bandwidth;
			circuit.CircuitStateId = model.CircuitStateId;
			circuit.IPPoPId = model.IPPoPId;
			circuit.AccountManager = model.AccountManager;
			circuit.ProjectManager = model.ProjectManager;
            circuit.InstallersContactDetails = model.InstallersContactDetails;
			circuit.ServiceStartDate = model.ServiceStartDate;
			circuit.ClientContactDetails = model.ClientContactDetails;
			circuit.LastMileName = model.LastMileName;
			circuit.LastMileDeviceId = model.LastMileDeviceId;
			circuit.RadioFrequency = model.RadioFrequency;
			circuit.TransmissionPath = model.TransmissionPath;
			circuit.PathLength = model.PathLength;
			circuit.RadioManagementVLAN = model.RadioManagementVLAN;
			circuit.ServiceVLAN = model.ServiceVLAN;
			circuit.ManagedRadioIPAtPoP = model.ManagedRadioIPAtPoP;
			circuit.ManagedRadioIPAtClient = model.ManagedRadioIPAtClient;
			circuit.ManagedRadioIPGateway = model.ManagedRadioIPGateway;
			circuit.AssignedPublicIP = model.AssignedPublicIP;
			circuit.AssignedGateway = model.AssignedGateway;
			circuit.AssignedSubnetMask = model.AssignedSubnetMask;

			service.UpdateCircuit(circuit);
			return RedirectToAction(nameof(Details), new {id = circuit.Id});

		}
		else
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            var CircuitDropdownData = await service.GetNewCircuitDropdownValues();
			ViewBag.CircuitStates = new SelectList(CircuitDropdownData.CircuitStates, "Id", "Name");
			ViewBag.States = new SelectList(CircuitDropdownData.States, "Id", "StateName");
			ViewBag.IPPoPs = new SelectList(CircuitDropdownData.IPPoPs, "Id", "IPPoPName");
			ViewBag.LastMileDevices = new SelectList(CircuitDropdownData.LastMileDevices, "Id", "LastMileDeviceName");
			ViewBag.Services = new SelectList(CircuitDropdownData.Services, "Id", "Name");
			ViewBag.Clients = new SelectList(CircuitDropdownData.Clients, "Id", "ClientName");

			return View(model);
		}
	}


}