using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 框架实体基类Id
    /// </summary>
    [SkipScan]
    public abstract class EntityBase<TKey> : IEntity<TKey>, ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            // 创建当前对象的浅拷贝
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 根据属性的名称获取属性的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected object GetPropertyValue(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                PropertyInfo property = this.GetType().GetProperty(name);
                if (property != null)
                {
                    return property.GetValue(this, null);
                }
            }
            return null;
        }
        /// <summary>
        /// 根据属性的名称设置属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void SetProperty<T>(ref T prop, T value, [CallerMemberName] string propertyName = "")
        {

            if (!EqualityComparer<T>.Default.Equals(prop, value))
            {
                PropertyInfo property = this.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    property.SetValue(this, value);
                    OnPropertyChanged(propertyName);
                }
            }
        }
        /// <summary>
        /// 索引器，根据属性的名称获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                object propertyValue = this.GetPropertyValue(name);
                return propertyValue;
            }
            set
            {
                object propertyValue = this.GetPropertyValue(name);
                this.SetProperty(ref propertyValue, value, name);
            }
        }

        /// <summary>
        /// 属性变化的事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性发生改变时
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 获取所有的主键
        /// </summary>
        /// <returns></returns>
        public object[] GetKeys()
        {
            return new object[] { Id };
        }
    }

    /// <summary>
    /// 框架实体基类Id
    /// </summary>
    [SkipScan]
    public abstract class EntityBase : EntityBase<long>
    {
        /// <summary>
        /// 雪花Id
        /// </summary>
        public override long Id { get; set; }
    }
}
