using Company.Application.Common.Paging;
using System;
using System.Collections.Generic;

namespace Company.Application.Common.Api.Base
{
    public interface IApiController<TDto, T>
    {
        #region GetMethods

        ApiResult<List<TDto>> GetAll();
        ApiResult<TDto> Find(Guid id);
        ApiResult GetAllWithPaging(PagingParams pagingParams);

        #endregion

        #region PostMethods

        ApiResult<string> Add(TDto item);
        ApiResult<string> Update(TDto item);
        ApiResult<string> Delete(TDto item);
        ApiResult<string> DeleteById(Guid Id);

        #endregion
    }
}
