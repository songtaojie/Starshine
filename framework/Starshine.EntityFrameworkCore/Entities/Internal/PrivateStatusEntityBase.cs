using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Internal
{
    /// <summary>
    ///  该基类封装了删除，禁用等状态字段（禁止外部继承）
    /// </summary>
    /// <typeparam name="TKeyType">主键的类型</typeparam>
    public abstract class PrivateStatusEntityBase<TKeyType> : Internal.PrivateEntityBase<TKeyType>
    {
        private const string No = "N";
        private const string Yes = "Y";
        #region 删除

        /// <summary>
        /// 是否删除,使用char字段存储，这样查询时不用进行取非的判断，所有都用等于判断
        /// Y:代表删除，N代表没删除，默认值为N
        /// </summary>
        [Column(TypeName = "char(1)")]
        public virtual string Deleted
        {
            get; set;
        } = No;

        /// <summary>
        /// 是否禁用,使用char字段存储，这样查询时不用进行取非的判断，所有都用等于判断
        /// Y:代表禁用，N代表没禁用，默认值为N
        /// </summary>
        [Column(TypeName = "char(1)")]
        public virtual string Disabled { get; set; } = No;
        #endregion

        #region 实体操作

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="modifierId">删除者的id</param>
        /// <param name="modifier">删除者的姓名</param>
        public virtual void SetDelete(string modifierId, string modifier)
        {
            Deleted = Yes;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleted">删除状态</param>
        /// <param name="modifierId">删除者的id</param>
        /// <param name="modifier">删除者的姓名</param>
        public virtual void SetDelete(StatusEntityEnum deleted, string modifierId, string modifier)
        {
            Deleted = deleted == StatusEntityEnum.Yes ? Yes : No;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="modifierId">禁用者的id</param>
        /// <param name="modifier">禁用者的姓名</param>
        public virtual void SetDisable(string modifierId, string modifier)
        {
            Disabled = Yes;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="disabled">禁用状态</param>
        /// <param name="modifierId">禁用者的id</param>
        /// <param name="modifier">禁用者的姓名</param>
        public virtual void SetDisable(StatusEntityEnum disabled, string modifierId, string modifier)
        {
            Disabled = disabled == StatusEntityEnum.Yes ? Yes : No;
            SetModifier(modifierId, modifier);
        }
        #endregion
    }
}
