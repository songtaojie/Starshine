using Hx.Sdk.DatabaseAccessor;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity.Page;
using Hx.Sdk.Test.Entity;
using Hx.Web.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Web.Service
{
    public class UserService:IUserService,IScopedDependency
    {
        private readonly IRepository<UserInfo> _userRepository;

        public UserService(IRepository<UserInfo> userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// 新增一条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> Insert(UserInputDto input)
        {
            // 如果不需要返回自增Id，使用 InsertAsync即可
            var userInfo = new UserInfo
            {
                Id = Guid.NewGuid().ToString(),
                NickName = input.NickName,
                UserName = input.UserName,
                PassWord = input.PassWord
            };
            var newEntity = await _userRepository.InsertNowAsync(userInfo);
            return newEntity.Entity.Id;

            // 还可以直接操作
            // await personDto.Adapt<Person>().InsertNowAsync();
        }

        /// <summary>
        /// 更新一条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(UserInputDto input)
        {
            var person = await _userRepository.SingleAsync(u => u.Id == input.Id);
            person.NickName = input.NickName;
            person.UserName = input.UserName;
            person.PassWord = input.PassWord;
            person.LastModifyTime = DateTime.Now;
            await _userRepository.UpdateAsync(person);

            // 还可以直接操作
            // await personDto.Adapt<Person>().UpdateAsync();
        }

        /// <summary>
        /// 删除一条
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete(string id)
        {
            await _userRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        public async Task<UserDto> Find(string id)
        {
            var person = await _userRepository.FindAsync(id);
            return new UserDto
            {
                Id = person.Id,
                NickName = person.NickName,
                PassWord = person.PassWord,
                UserName = person.UserName
            };
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDto>> GetAll()
        {
            var persons = _userRepository.AsQueryable(false)
                .Select(u => new UserDto
                { 
                    Id = u.Id,
                    NickName = u.NickName,
                    UserName = u.UserName,
                    PassWord = u.PassWord
                });
            return await persons.ToListAsync();
        }
        public Task<PageModel<UserDto>> GetAllByPage(BasePageParam param)
        {
            throw new NotImplementedException();
        }
    }
}
