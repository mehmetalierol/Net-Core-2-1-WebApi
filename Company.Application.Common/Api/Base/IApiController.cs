using Company.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Application.Common.Api.Base
{
    public interface IApiController<TDto, T>
    {
        #region GetMethods

        ApiResult<List<TDto>> GetAll();

        ApiResult<TDto> Find(Guid id);

        Task<ApiResult<TDto>> FindAsync(Guid id);

        ApiResult GetAllWithPaging(PagingParams pagingParams);

        #endregion GetMethods

        #region PostMethods

        ApiResult<TDto> Add(TDto item);

        Task<ApiResult<TDto>> AddAsync(TDto item);

        ApiResult<TDto> Update(TDto item);

        Task<ApiResult<TDto>> UpdateAsync(TDto item);

        ApiResult<string> Delete(TDto item);

        Task<ApiResult<string>> DeleteAsync(TDto item);

        ApiResult<string> DeleteById(Guid Id);

        Task<ApiResult<string>> DeleteByIdAsync(Guid id);

        #endregion PostMethods
    }
}