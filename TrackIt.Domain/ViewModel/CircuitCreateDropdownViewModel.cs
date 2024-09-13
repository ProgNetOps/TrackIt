
namespace TrackIt.Domain.ViewModel
{
    public class CircuitCreateDropdownViewModel
    {
        public CircuitCreateDropdownViewModel()
        {
            Clients = [];
            States = [];
            Services = [];
            CircuitStates = [];
            IPPoPs = [];
            LastMileDevices = [];
        }
        public List<Client> Clients { get; set; }
        public List<State> States { get; set; }
        public List<Service> Services { get; set; }
        public List<CircuitState> CircuitStates { get; set; }
        public List<IPPoP> IPPoPs { get; set; }
        public List<LastMileDevice> LastMileDevices { get; set; }
    }
}
