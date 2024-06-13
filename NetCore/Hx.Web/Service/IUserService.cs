using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity.Page;
using Hx.Web.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Web.Service
{
    public interface  IUserService
    {
        /// <summary>
        /// 新增一条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> Insert(UserInputDto input);

        /// <summary>
        /// 更新一条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       Task Update(UserInputDto input);

        /// <summary>
        /// 删除一条
        /// </summary>
        /// <param name="id"></param>
        Task Delete(string id);

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        Task<UserDto> Find(string id);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageModel<UserDto>> GetAllByPage(BasePageParam param);
    }
}
