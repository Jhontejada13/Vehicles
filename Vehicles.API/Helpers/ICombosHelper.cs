using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetCombosDocumentsType();

        IEnumerable<SelectListItem> GetCombosProcedures();

        IEnumerable<SelectListItem> GetCombosVehicleTypes();

        IEnumerable<SelectListItem> GetBrands();

    }
}
