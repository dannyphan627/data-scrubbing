

namespace Models.Requests
{
    public class InsuranceProviderAddRequest
    { 
      
        [Required]
        [StringLength(200, MinimumLength = 2 )]
        public string Name { get; set; }
       
        [StringLength(255, MinimumLength = 0)]
        public string SiteUrl { get; set; }


    }
}
