using System.ComponentModel.DataAnnotations;

namespace DailyScores.Models
{
    public class EmailAddress : BaseEntity
    {
        [Key]
        public int AddressId { get; set; }
        public int PlayerId { get; set; }
        public string Address { get; set; }

        //Navigation Properties
        public virtual Player Player { get; set; }
    }
}