using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test_app.Models;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace test_app.Models
{

    public static class FacilityPager
    {
        static private int ELEMENT_PER_PAGE = 10;

        static async public Task<int> GetCountOfPages(ApplicationContext db)
        {
            var listCount = 0;

            using ( db )
            {

                SqlParameter param = new()
                {
                    ParameterName = "@count",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,

                };
                db.Database.ExecuteSqlRaw("GetCountOfFacilities @count OUT", param);


                listCount = (int)param.Value;
            }


            return listCount % ELEMENT_PER_PAGE == 0
        ? (listCount / ELEMENT_PER_PAGE) - 1
        : listCount / ELEMENT_PER_PAGE;

        }

        static async public Task<List<Facility>> GetPage(ApplicationContext db, int currentPage)
        {
            List<Facility> facilities;

                facilities = await db
                    .Facility
                    .Skip( currentPage * ELEMENT_PER_PAGE )
                    .Except( db.Facility.Skip( (currentPage * ELEMENT_PER_PAGE) + ELEMENT_PER_PAGE ))
                    .Include( e => e.FacilityStatus )
                    .ToListAsync();


            return facilities;
        }

    };

    public class AppModel
    {
        ApplicationContext db;


        public AppModel(ApplicationContext context) => db = context;


        public async Task<Facility> Create( Facility facility ) {
            facility.FacilityStatusId = db.FacilityStatus.Where(e => e.Name == "Inactive").FirstOrDefault().Id;
            db.Facility.Add(facility);
            await db.SaveChangesAsync();
            return facility;

        }


        public async Task<Facility> Update( Facility facility )
        {
            db.Facility.Update(facility);
            await db.SaveChangesAsync();
            return facility;
        }


        public async Task<int?> DeleteFacility( int? id )
        {
            if (id != null)
            {
                Facility facility = await db.Facility.FirstOrDefaultAsync(p => p.Id == id);
                if (facility != null)
                {
                    db.Facility.Remove(facility);
                    await db.SaveChangesAsync();
                }
            }

            return id;
        }



        public async Task<List<Facility>> GetPageItemJson( int page = 0) =>
           await FacilityPager.GetPage( db, page);


        public async Task<int> GetCountOfPages() =>
            await FacilityPager.GetCountOfPages( db );


        public async Task<string> GetFacilityStatuseJson() =>
            JsonSerializer.Serialize( await db.FacilityStatus.ToListAsync() );

    }
}
