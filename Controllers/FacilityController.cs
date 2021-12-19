using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test_app.Models;
using System.Collections.Generic;
using System.Text.Json;


namespace test_app.Controllers
{

    public static class FacilityPager
    {
        static private int  ELEMENT_PER_PAGE = 10;


        static public int GetCountOfPages(List<Facility> list) => list.Count() / ELEMENT_PER_PAGE;

        static public List<Facility> GetPage(List<Facility> _list, int currentPage )
        {

            if (_list.Count() == 0 || _list.Count() <= ELEMENT_PER_PAGE )
                return _list;
            else
                if (((currentPage * ELEMENT_PER_PAGE) + ELEMENT_PER_PAGE) < _list.Count())
                    return _list.GetRange(currentPage * ELEMENT_PER_PAGE, ELEMENT_PER_PAGE);
                else
                    return _list.GetRange(currentPage * ELEMENT_PER_PAGE, (_list.Count() - currentPage * ELEMENT_PER_PAGE ) );              
        }

    };


    public class FacilityController : Controller
    {

        ApplicationContext db;





        public FacilityController(ApplicationContext context) => db = context;


        public IActionResult Index() => View();


        [HttpPost]
        public async Task<ActionResult> Create([FromBody][Bind("Name", "Address", "PhoneNumber", "Email")] Facility facility )
        {
            facility.FacilityStatusId = db.FacilityStatus.Where( e => e.Name == "Inactive").FirstOrDefault().Id;
            db.Facility.Add(facility);
            await db.SaveChangesAsync();
            return Json(facility);
        }



        [HttpPut]
        public async Task<ActionResult> Update([FromBody][Bind("Id", "Name", "Address", "PhoneNumber", "Email", "FacilityStatusId")]Facility facility )
        {
            db.Facility.Update(facility);
            await db.SaveChangesAsync();

            return Json(facility);
        }



        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery]int? id)
        {
          
            if (id != null)
            {
                Facility user = await db.Facility.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Facility.Remove(user);
                    await db.SaveChangesAsync();
                }
            }


            return Json(id);
         
        }


        [HttpGet]
        public async Task<string> GetPageItemJson([FromQuery]int page = 0)
            => JsonSerializer.Serialize(FacilityPager.GetPage(await db.Facility.Include(s => s.FacilityStatus).ToListAsync(), page));


        [HttpGet]
        public async Task<int> GetCountOfPages() => FacilityPager.GetCountOfPages( await db.Facility.ToListAsync() );


        public async Task<string> GetFacilityStatuseJson() => JsonSerializer.Serialize(await db.FacilityStatus.ToArrayAsync());

    }
}
