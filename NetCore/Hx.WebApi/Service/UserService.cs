//using Hx.DatabaseAccessor;
//using Hx.DependencyInjection;
//using Hx.Common;
//using Hx.Test.Entity;
//using Hx.Test.Entity.DbContexts;
//using Hx.Test.Entity.Entities;
//using Hx.UnifyResult;
//using Hx.WebApi.Service.Dtos;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Hx.WebApi.Service
//{
//    //[Autofac.Extras.DynamicProxy.Intercept(typeof(Aop.BlogLogAop))]
//    public class UserService: BaseService<UserInfo>,IUserService, IScopedDependency
//    {
//        public UserService(IRepository<UserInfo> userRepository):base(userRepository)
//        {
//        }
//        /// <summary>
//        /// 新增一条
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task<string> Insert(UserInputDto input)
//        {
//            // 如果不需要返回自增Id，使用 InsertAsync即可
//            var userInfo = new UserInfo
//            {
//                Id = Guid.NewGuid().ToString(),
//                NickName = input.NickName,
//                UserName = input.UserName,
//                PassWord = input.PassWord
//            };
//            var newEntity = await this.Repository.InsertNowAsync(userInfo);
//            return newEntity.Entity.Id;

//            // 还可以直接操作
//            // await personDto.Adapt<Person>().InsertNowAsync();
//        }

//        /// <summary>
//        /// 更新一条
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public async Task Update(UserInputDto input)
//        {
//            var person = await Repository.SingleAsync(u => u.Id == input.Id);
//            person.NickName = input.NickName;
//            person.UserName = input.UserName;
//            person.PassWord = input.PassWord;
//            person.LastModifyTime = DateTime.Now;
//            await Repository.UpdateAsync(person);

//            // 还可以直接操作
//            // await personDto.Adapt<Person>().UpdateAsync();
//        }

//        /// <summary>
//        /// 删除一条
//        /// </summary>
//        /// <param name="id"></param>
//        public async Task Delete(string id)
//        {
//            await Repository.DeleteAsync(id);
//        }

//        /// <summary>
//        /// 查询一条
//        /// </summary>
//        /// <param name="id"></param>
//        public async Task<UserDto> Find(string id)
//        {
//            var person = await Repository.FindAsync(id);
//            return new UserDto
//            {
//                Id = person.Id,
//                NickName = person.NickName,
//                PassWord = person.PassWord,
//                UserName = person.UserName
//            };
//        }

//        /// <summary>
//        /// 查询一条
//        /// </summary>
//        /// <param name="id"></param>
//        public async Task<ApplicationUserDto> FindApplicationUser(string id)
//        {
//           var appRepository = Repository.Change<ApplicationUser, IdsDbContextLocator>();
//            var user =  await appRepository.FindAsync(id);
//            return new ApplicationUserDto
//            {
//                Id = user.Id,
//                UserName = user.UserName
//            };
//        }


//        /// <summary>
//        /// 查询所有
//        /// </summary>
//        /// <returns></returns>
//        public async Task<List<UserDto>> GetAll()
//        {
//            var persons = Repository.AsQueryable(false)
//                .Select(u => new UserDto
//                { 
//                    Id = u.Id,
//                    NickName = u.NickName,
//                    UserName = u.UserName,
//                    PassWord = u.PassWord
//                });
//            return await persons.ToListAsync();
//        }
//        public Task<PagedListResult<UserDto>> GetAllByPage(BasePageParam param)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
