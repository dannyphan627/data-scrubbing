using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests
{
    public class InsuranceProviderUpdateRequest : InsuranceProviderAddRequest, IModelIdentifier
    {
       
       [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
