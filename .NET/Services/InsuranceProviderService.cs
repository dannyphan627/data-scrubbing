

namespace Services
{
    public class InsuranceProviderService : IInsuranceProviderService
    {
        IDataProvider _data = null;

        public InsuranceProviderService(IDataProvider data)
        {
            _data = data;
        }

        public InsuranceProvider Get(int id)
        {
            string procName = "dbo.InsuranceProviders_Select_ByIdV2";

            InsuranceProvider provider = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                provider = mapInsuranceProvider(reader, ref index);
            });
            return provider;
        }

        public Paged<InsuranceProvider> GetAllPagination(int pageIndex, int pageSize)
        {
            Paged<InsuranceProvider> pagedResult = null;
            List<InsuranceProvider> listResult = null;

            int totalCount = 0;
            string procName = "dbo.InsuranceProviders_SelectAllV2";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@pageIndex", pageIndex);
                col.AddWithValue("@pageSize", pageSize);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                InsuranceProvider model = mapInsuranceProvider(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (listResult == null)
                {
                    listResult = new List<InsuranceProvider>();
                }

                listResult.Add(model);
            });
            if (listResult != null)
            {
                pagedResult = new Paged<InsuranceProvider>(listResult, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public int Add(InsuranceProviderAddRequest model, int userId)
        {
            int id = 0;

            string procName = "dbo.InsuranceProviders_InsertV2";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@SiteUrl", model.SiteUrl);
                col.AddWithValue("@CreatedBy", userId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Delete(int id)
        {
            string procName = "dbo.InsuranceProviders_Delete_ById";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public void Update(InsuranceProviderUpdateRequest model, int userId)
        {
            string procName = "dbo.InsuranceProviders_UpdateV2";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@SiteUrl", model.SiteUrl);
                col.AddWithValue("@ModifiedBy", userId);
                col.AddWithValue("@Id", model.Id);
            }, returnParameters: null);
        }

        private static InsuranceProvider mapInsuranceProvider(IDataReader reader, ref int index)
        {
            InsuranceProvider insurance = new InsuranceProvider();

            insurance.Id = reader.GetSafeInt32(index++);
            insurance.Name = reader.GetSafeString(index++);
            insurance.SiteUrl = reader.GetSafeString(index++);
            insurance.CreatedBy = reader.GetSafeInt32(index++);
            insurance.ModifiedBy = reader.GetSafeInt32(index++);
            insurance.DateCreated = reader.GetSafeDateTime(index++);
            insurance.DateModified = reader.GetSafeDateTime(index++);

            return insurance;
        }

        public Paged<InsuranceProvider> SearchInsuranceProviders(string query, int pageSize, int pageIndex)
        {
            string procName = "dbo.InsuranceProviders_SearchV2";
            List<InsuranceProvider> list = null;
            Paged<InsuranceProvider> pagedList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Query", query);
                col.AddWithValue("@pageSize", pageSize);
                col.AddWithValue("@pageIndex", pageIndex);

            }, delegate (IDataReader reader, short set)
             {
                 int index = 0;
                 InsuranceProvider insurance = mapInsuranceProvider(reader, ref index);

                 if (totalCount == 0)
                 {
                     totalCount = reader.GetSafeInt32(index++);
                 }

                 if (list == null)
                 {
                     list = new List<InsuranceProvider>();
                 }

                 list.Add(insurance);
             });
            if (list != null)
            {
                pagedList = new Paged<InsuranceProvider>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public List<InsuranceProvider> GetAllList()
        {
            List<InsuranceProvider> list = null;
            string procName = "dbo.InsuranceProviders_SelectAll_ListV2";

            _data.ExecuteCmd(procName, inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                InsuranceProvider provider = mapInsuranceProvider(reader, ref index);

                if (list == null)
                {
                    list = new List<InsuranceProvider>();
                }

                list.Add(provider);
            });

            return list;
        }

    }
}
