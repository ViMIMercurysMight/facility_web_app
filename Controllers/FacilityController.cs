using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test_app.Models;
using System.Collections.Generic;
using System.Text.Json;


namespace test_app.Controllers
{

    public class FacilityController : Controller
    {


        AppModel _appModel;

        public FacilityController(ApplicationContext context) => _appModel = new AppModel(context);


        public IActionResult Index() => View();



        [HttpPost]
        public async Task<ActionResult> Create([FromBody][Bind("Name", "Address", "PhoneNumber", "Email")] Facility facility ) 
            => Json( await _appModel.Create(facility));




        [HttpPut]
        public async Task<ActionResult> Update([FromBody][Bind("Id", "Name", "Address", "PhoneNumber", "Email", "FacilityStatusId")] Facility facility)
            => Json( await _appModel.Update(facility));



        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int? id)
            => Json( await _appModel.DeleteFacility(id) );


        [HttpGet]
        public async Task<string> GetPageItemJson([FromQuery] int page = 0)
          =>  JsonSerializer.Serialize( await _appModel.GetPageItemJson(page));

        

        [HttpGet]
        public async Task<int> GetCountOfPages() => await _appModel.GetCountOfPages();


        [HttpGet]
        public async Task<string> GetFacilityStatuseJson() => await _appModel.GetFacilityStatuseJson();

    }
}
