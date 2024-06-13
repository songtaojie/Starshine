//using Hx.Sdk.DatabaseAccessor;
//using System.ComponentModel.DataAnnotations;

//namespace Hx.Test.Entity
//{
//    public class UserInfo: EntityBase
//    {
//        /// <summary>
//        /// 主键
//        /// </summary>
//        [Key]
//        [MaxLength(36)]
//        public override string Id
//        {
//            get => base.Id;
//            set => base.Id = value;
//        }
//        /// <summary>
//        /// 用户名称
//        /// </summary>
//        [MaxLength(36)]
//        [Required]
//        public string UserName
//        {
//            get; set;
//        }
//        /// <summary>
//        /// 密码
//        /// </summary>
//        [Required]
//        [MaxLength(36)]
//        public string PassWord
//        {
//            set;
//            get;
//        }
//        /// <summary>
//        /// 昵称
//        /// </summary>
//        [MaxLength(36)]
//        public string NickName { get; set; }
//    }
//}
