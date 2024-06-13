using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.DatabaseAccessor.Base
{
    /// <summary>
    /// 基本的模型，封装了索引器登方法
    /// </summary>
    [Serializable]
    public abstract class EntityPropertyBase : INotifyPropertyChanged
    {
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
        /// 根据属性的名称获取属性的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetPropertyValue(string name)
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
        /// 把指定的模型中值复制到当前模型中
        /// </summary>
        /// <param name="model">要复制数据的模型</param>
        public void CopyFrom(EntityPropertyBase model)
        {
            if (model == null) return;
            Type destType = this.GetType();
            Type sourcetype = model.GetType();
            if (sourcetype != null && destType != null)
            {
                foreach (PropertyInfo sourceInfo in sourcetype.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (sourceInfo.GetIndexParameters().Length > 0) continue;
                    if (sourceInfo.CanRead && sourceInfo.CanWrite)
                    {
                        PropertyInfo destInfo = destType.GetProperty(sourceInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                        object sourceValue = sourceInfo.GetValue((object)model, null);
                        if (destInfo != null && destInfo.CanWrite && destInfo.CanRead && sourceValue != null)
                        {
                            destInfo.SetValue((object)this, sourceValue, null);
                        }
                    }
                }
            }
        }
    }
}
