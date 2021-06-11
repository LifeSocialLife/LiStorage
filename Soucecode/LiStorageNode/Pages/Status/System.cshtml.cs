using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiStorage.Services;

namespace LiStorageNode.Pages.Status
{
    public class SystemModel : PageModel
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly RundataService _rundata;

        public List<StatusItemModel> StatusItems { get; set; }

        public SystemModel(RundataService rundataService)
        {
            this._rundata = rundataService;

            this.StatusItems = new List<StatusItemModel>();
        }
        public void OnGet()
        {
            this.SetStatusModelData();


            //WebGrid grid = new WebGrid(this.StatusItems, canSort: true, canPage: true, rowsPerPage: 20);

            //var dd = grid.GetHtml();

            //return inventoryList;


        }

        private void  SetStatusModelData()
        {
            this.StatusItems.Add(new StatusItemModel()
            {
                Name = "FrameworkDescription",
                Description = this._rundata.Hardware.FrameworkDescription
            }) ;

            this.StatusItems.Add(new StatusItemModel()
            {
                Name = "OSArchitecture",
                Description = this._rundata.Hardware.OSArchitecture.ToString()
            });
            this.StatusItems.Add(new StatusItemModel()
            {
                Name = "OsPlatform",
                Description = this._rundata.Hardware.OsPlatform
            });
            this.StatusItems.Add(new StatusItemModel()
            {
                Name = "Platform",
                Description = this._rundata.Hardware.Platform.ToString()
            });


            this.StatusItems.Add(new StatusItemModel()
            {
                Name = "zz",
                Description = this._rundata.Hardware.OsPlatform
            });
        }
        public class StatusItemModel
        {
            public string Name { get; set; }
            public string Description { get; set; }

        }


    }
}
