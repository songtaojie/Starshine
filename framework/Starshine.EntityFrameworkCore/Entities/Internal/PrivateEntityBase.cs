using Starshine.EntityFrameworkCore.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Starshine.EntityFrameworkCore.Internal
{
    /// <summary>
    /// 数据库实体依赖基类（禁止外部继承）
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    public abstract class PrivateEntityBase<TKeyType> : EntityPropertyBase, Internal.IPrivateEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(36)]
        public virtual TKeyType Id
        {
            get;
            set;
        }
        #region 创建
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(36)]
        public string CreaterId { get; set; } = string.Empty;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [MaxLength(36)]
        public string Creater { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        [MaxLength(36)]
        public string LastModifier { get; set; } = string.Empty;

        /// <summary>
        /// 最后修改人的id
        /// </summary>
        [MaxLength(36)]
        public string LastModifierId { get; set; } = string.Empty;

        #region 实体操作

        /// <summary>
        /// 添加创建人信息
        /// </summary>
        /// <param name="createrId">创建人id</param>
        /// <param name="creater">创建人姓名</param>
        public virtual PrivateEntityBase<TKeyType> SetCreater(string createrId, string creater)
        {
            CreaterId = createrId;
            Creater = creater;
            CreateTime = DateTime.Now;
            SetModifier(createrId, creater);
            return this;
        }

        /// <summary>
        /// 添加修改人信息
        /// </summary>
        /// <param name="modifierId">修改者id</param>
        /// <param name="modifier">修改者姓名</param>
        public virtual PrivateEntityBase<TKeyType> SetModifier(string modifierId, string modifier)
        {
            LastModifierId = modifierId;
            LastModifier = modifier;
            LastModifyTime = DateTime.Now;
            return this;
        }

        #endregion
    }
}
