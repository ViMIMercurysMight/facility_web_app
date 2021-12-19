using System.ComponentModel.DataAnnotations;

namespace test_app.Models
{
    public class FacilityStatus
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(255)]
        public string Name { get; set; }

    }
}
