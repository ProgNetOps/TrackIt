using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel;

/// <summary>
/// A view model for basestation details
/// </summary>
public class BTSDetailsViewModel
{
    public Guid Id { get; set; }

    /// <summary>
    /// Site Id
    /// </summary>
   [Display(Name = "Base Station")]
    public string? BTSName { get; set; }

    /// <summary>
    /// Address or landmark of the location of the base station
    /// </summary>
    [Display(Name = "Address/Landmark")]
    public string? LocationAddress { get; set; }

    [Display(Name = "State")]
    public int StateId { get; set; }
    [ForeignKey(nameof(StateId))]
    public State State { get; set; }

    [StringLength(50)]
    public string? Coordinates { get; set; }
}
