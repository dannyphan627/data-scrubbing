

namespace Models.Requests
{
    public class InsuranceProviderUpdateRequest : InsuranceProviderAddRequest, IModelIdentifier
    {
       
       [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
