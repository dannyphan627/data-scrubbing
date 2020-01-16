using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IInsuranceProviderService
    {
        int Add(InsuranceProviderAddRequest model, int userId);
        void Delete(int id);
        InsuranceProvider Get(int id);
        Paged<InsuranceProvider> GetAllPagination(int pageIndex, int pageSize);
        void Update(InsuranceProviderUpdateRequest model, int userId);
        Paged<InsuranceProvider> SearchInsuranceProviders(string query, int pageSize, int pageIndex);
        List<InsuranceProvider> GetAllList();
    }
}