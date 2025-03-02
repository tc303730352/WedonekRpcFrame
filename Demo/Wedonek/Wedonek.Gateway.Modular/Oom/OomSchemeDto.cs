using System;

namespace Wedonek.Gateway.Modular.Oom
{
    public class OomSchemeDto
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
        public string Age
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间（时间戳）
        /// </summary>
        public long AddTime
        {
            get;
            set;
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public short Sex
        {
            get;
            set;
        }
        /// <summary>
        /// 标签(支持对象，数组等与JSON字符串互转)
        /// </summary>
        public string Tags
        {
            get;
            set;
        }
    }
}
