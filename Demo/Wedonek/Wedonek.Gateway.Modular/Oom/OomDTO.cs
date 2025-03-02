using System;

namespace Wedonek.Gateway.Modular.Oom
{
    public enum UserSex
    {
        男=1,
        女=2,
        未知=0
    }
    /// <summary>
    /// DTO实体(实体必须公有public的)
    /// </summary>
    public class OomDTO
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
        /// <summary>
        /// 生日
        /// </summary>
        public long Birthday
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public UserSex Sex
        {
            get;
            set;
        }
       /// <summary>
       /// 标签(支持对象，数组等与JSON字符串互转)
       /// </summary>
        public string[] Tags
        {
            get;
            set;
        }
    }
}
