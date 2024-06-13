using Hx.Common;
using Hx.WebApi.Service.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hx.WebApi.Service
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> FindApplicationUser(string id);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedListResult<UserDto>> GetAllByPage(BasePageParam param);
    }
}
