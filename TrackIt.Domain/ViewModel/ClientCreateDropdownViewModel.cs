using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class ClientCreateDropdownViewModel
    {
        public ClientCreateDropdownViewModel()
        {
            ClientCategories = [];
        }
        public List<ClientCategory> ClientCategories { get; set; }

    }
}
