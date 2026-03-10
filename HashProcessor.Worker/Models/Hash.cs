using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HashProcessor.Worker.Models
{
    [Table("hashes")]
    public class Hash
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("sha1")]
        public string Sha1 { get; set; } = string.Empty;

        [Required]
        [Column("date")]
        public DateOnly Date { get; set; }
    }
}
