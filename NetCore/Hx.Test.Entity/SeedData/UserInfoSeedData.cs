//using Hx.Sdk.DatabaseAccessor;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Hx.Test.Entity.SeedData
//{
//    public class UserInfoSeedData : IEntitySeedData<UserInfo>
//    {
//        public IEnumerable<UserInfo> HasData(DbContext dbContext, Type dbContextLocator)
//        {
//            return new List<UserInfo>
//            {
//                new UserInfo
//                {
//                    Id = Guid.NewGuid().ToString(),
//                    CreaterId="SuperAdmin", 
//                    Creater = "SuperAdmin",
//                    CreateTime = DateTime.Now,
//                    UserName="songtaojie",
//                    PassWord="123456",
//                    NickName = "宋"
//                }
//            };
//        }
//    }
//}
