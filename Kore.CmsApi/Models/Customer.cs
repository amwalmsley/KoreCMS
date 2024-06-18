using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kore.CmsApi.Models
{
    public record Customer
    {
        [JsonPropertyName("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        
        [JsonPropertyName("firstName")]
        [Required]
        public string? FirstName { get; set; }
        
        [JsonPropertyName("lastName")]
        [Required]
        public string? LastName { get; set; }
        
        [JsonPropertyName("email")]
        [EmailAddress]
        public string? Email {  get; set; }

        [JsonPropertyName("phone")]
        [Phone]
        public string? Phone {  get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("middleInitial")]
        [StringLength(1)]
        public string? MiddleInitial {  get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime ModifiedDate { get; set; }
    }
}
