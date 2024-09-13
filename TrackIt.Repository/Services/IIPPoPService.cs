using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain;
using TrackIt.Domain.Contract;
using TrackIt.Repository.Base;

namespace TrackIt.Repository.Services;

   public interface IIPPoPService : IBaseRepository<IPPoP>, IFilterSortPaginate<IPPoP>
   {

   }

