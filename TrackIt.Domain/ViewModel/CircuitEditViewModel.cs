using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
	public class CircuitEditViewModel
	{
		[Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Client Name is required")]
		[Display(Name = "Client Name")]
		public Guid ClientId { get; init; }

		[StringLength(100, ErrorMessage = "Name too Long, shorten it")]
		[Display(Name = "Service Name")]
		public string? CircuitName { get; init; }

        [StringLength(50, ErrorMessage = "Name too Long, shorten it")]
        [Display(Name = "Link/Service ID")]
        public string? LinkID { get; set; }

        [Display(Name = "ODU Serial Number")]
        public string? ODUSerialNumber { get; set; }

        [Display(Name = "IDU Serial Number")]
        public string? IDUSerialNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
		[StringLength(300, ErrorMessage = "Address too Long, shorten it")]
		[Display(Name = "Service Address")]
		public string? ServiceAddress { get; init; }

		[Required(ErrorMessage = "Town of location is required")]
		[StringLength(50)]
		public string? Town { get; init; }

		[Display(Name = "State")]
		[Required(ErrorMessage = "State is required")]
		public int StateId { get; init; }

		public double? Latitude { get; init; }

		public double? Longitude { get; init; }

		[Display(Name = "Service Type")]
		[Required(ErrorMessage = "Service type is required")]
		public Guid? ServiceId { get; init; }

		[Display(Name = "Annual Revenue")]
		[Required(ErrorMessage = "Annual Revenue is required")]
		public double? AnnualRevenue { get; init; }

		[Display(Name = "Bandwidth")]
		[Required(ErrorMessage = "Bandwidth is required")]
		public double? Bandwidth { get; init; }

		[Display(Name = "Circuit State")]
		[Required(ErrorMessage = "Circuit State is required")]
		public Guid? CircuitStateId { get; init; }

		[Display(Name = "IP PoP")]
		[Required(ErrorMessage = "IP PoP is required")]
		public Guid? IPPoPId { get; init; }

		[StringLength(100, ErrorMessage = "Name too long, shorten it")]
		[Display(Name = "Account Manager")]
		public string? AccountManager { get; init; }

		[StringLength(100, ErrorMessage = "Name too long, shorten it")]
		[Display(Name = "Project Manager")]
		public string? ProjectManager { get; init; }

		[StringLength(1000)]
		[Display(Name = "Installation Engineers/Riggers")]
		[Required(ErrorMessage = "Installers' details is required")]
		public string? InstallersContactDetails { get; init; }

		[DataType(DataType.Date)]
		[Display(Name = "Service Start Date")]
		[Required(ErrorMessage = "Service Start Date IS required")]
		public DateOnly? ServiceStartDate { get; init; }

		[StringLength(1000)]
		[Display(Name = "Client Contact's Information")]
		public string? ClientContactDetails { get; init; }

		//Technical Details

		[Display(Name = "LastMile Name")]
		public string? LastMileName { get; init; }

		[Display(Name = "LastMile Device")]
		[Required(ErrorMessage = "LastMile Device is required")]
		public Guid? LastMileDeviceId { get; init; }

		[Display(Name = "Transmission Path")]
		[Required(ErrorMessage = "Transmission Path is required")]
		public string? TransmissionPath { get; init; }

		[Display(Name = "Path Length")]
		public double? PathLength { get; init; }

		public int? RadioFrequency { get; init; }

		[Display(Name = "Radio Management VLAN")]
		public int? RadioManagementVLAN { get; init; }

		[Display(Name = "Service VLAN")]
		[Required(ErrorMessage = "Service VLAN is required")]
		public int? ServiceVLAN { get; init; }

		[Display(Name = "Radio IP (Glo Side)")]
		public string? ManagedRadioIPAtPoP { get; init; }

		[Display(Name = "Radio IP (Client Side)")]
		public string? ManagedRadioIPAtClient { get; init; }

		[Display(Name = "Radio IP Gateway")]
		public string? ManagedRadioIPGateway { get; init; }

		[Display(Name = "Customer Public IP")]
		public string? AssignedPublicIP { get; init; }

		[Display(Name = "Glo IP Gateway")]
		public string? AssignedGateway { get; init; }

		[Display(Name = "IP Subnet Mask")]
		public string? AssignedSubnetMask { get; init; }
	}
}
