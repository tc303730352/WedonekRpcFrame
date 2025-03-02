using System;
using System.Text;

namespace WeDonekRpc.Helper.Reflection
{
    public interface IReflectionBody
    {
        Type Type { get; }
        ReflectionType ClassType { get; }
        /// <summary>
        /// 获取属性或成员内部属性值
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="name">属性或成员名字典</param>
        /// <returns></returns>
        object GetValue (object source, string[] name);
        /// <summary>
        /// 获取数组或集合的值
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="index">数组或集合索引</param>
        /// <returns></returns>
        object GetValue (object source, int index);
        /// <summary>
        /// 尝试获取属性或成员
        /// </summary>
        /// <param name="name">属性或成员名</param>
        /// <param name="property">属性或成员</param>
        /// <returns>是否获取成功</returns>
        bool TryGet (string name, out IReflectionProperty property);
        /// <summary>
        /// 属性列表
        /// </summary>
        IReflectionProperty[] Properties { get; }
        /// <summary>
        /// 获取属性或成员
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IReflectionProperty GetProperty (string name);
        /// <summary>
        /// 获取属性或成员值
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="name">属性或成员名</param>
        /// <returns></returns>
        object GetValue (object source, string name);
        /// <summary>
        /// 检查2个不相同相近对象是否相等
        /// </summary>
        /// <param name="otherClass">另一个对象的结构体</param>
        /// <param name="source">原对象</param>
        /// <param name="other">检查的对象</param>
        /// <param name="isExclude">是否排除或指定属性范围</param>
        /// <param name="pros">排除或指定的属性名</param>
        /// <returns></returns>
        bool IsEquals (IReflectionBody otherClass, object source, object other, bool? isExclude, string[] pros);
        /// <summary>
        /// 覆盖对象
        /// </summary>
        /// <param name="source">被覆盖的对象</param>
        /// <param name="cover">覆盖的对象</param>
        /// <returns></returns>
        string[] Cover (object source, object cover);
        /// <summary>
        /// 覆盖对象
        /// </summary>
        /// <param name="otherClass">覆盖的对象类型</param>
        /// <param name="source">被覆盖的对象</param>
        /// <param name="cover">覆盖的对象</param>
        /// <returns></returns>
        string[] Cover (IReflectionBody otherClass, object source, object cover);

        void Init (IReflectionBody sourceBody, object source, object obj);
        /// <summary>
        /// 检查2个相同对象是否相等
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="other">检查的对象</param>
        /// <param name="isExclude">是否排除或指定属性范围</param>
        /// <param name="pros">排除或指定的属性名</param>
        /// <returns></returns>
        bool IsEquals (object source, object other, bool? isExclude, string[] pros);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="obj">设置的值</param>
        /// <param name="name">属性或成员名</param>
        void SetValue (object source, object obj, string name);
        /// <summary>
        /// 设置数组或集合值
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="obj">设置的值</param>
        /// <param name="index">数组或集合索引</param>
        void SetValue (object source, int index, object obj);
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="remove">排除的属性或成员</param>
        /// <returns>字符串</returns>
        string ToString (object source, params string[] remove);
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="source">原对象</param>
        void ToString (object source, StringBuilder write);
        void SetDicValue (object source, object key, object obj);
        object GetDicValue (object source, object key);
    }
}