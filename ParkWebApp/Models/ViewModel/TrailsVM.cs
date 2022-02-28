using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkWebApp.Models.ViewModel
{
    public class TrailsVM
    {
        public Trail Trail { get; set; }

        public IEnumerable<SelectListItem> NationalParkList { get; set; }
    }
}
