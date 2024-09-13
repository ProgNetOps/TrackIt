using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class NewBTSDropdownsVM
    {
        public List<State> States { get; set; }

        public NewBTSDropdownsVM()
        {
            States = new List<State>();
        }
    }
}
