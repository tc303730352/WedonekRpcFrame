using System;
using WeDonekRpc.Helper.Area;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 地址数据验证
    /// </summary>
    public class AreaValidate : ValidateAttr
    {
        private readonly bool _IsAllowNull = false;
        private readonly string _ParentAttr = null;

        public AreaType AreaType { get; } = AreaType.未知;
        /// <summary>
        /// 父级ID
        /// </summary>
        public long? ParentId
        {
            get;
        }

        /// <summary>
        /// 地址数据验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="isAllowNull">是否允许为空</param>
        public AreaValidate ( string error, bool isAllowNull = true ) : base(error)
        {
            this._IsAllowNull = isAllowNull;
        }
        /// <summary>
        /// 地址数据验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="areaType">地址类型</param>
        /// <param name="isAllowNull">是否允许为空</param>
        public AreaValidate ( string error, AreaType areaType, bool isAllowNull = true ) : base(error)
        {
            this.AreaType = areaType;
            this._IsAllowNull = isAllowNull;
        }
        /// <summary>
        /// 地址数据验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="parentId">父级ID</param>
        /// <param name="isAllowNull">是否允许空</param>
        public AreaValidate ( string error, long parentId, bool isAllowNull = true ) : base(error)
        {
            this.ParentId = parentId;
            this._IsAllowNull = isAllowNull;
        }
        /// <summary>
        /// 地址数据验证(联动检查)
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="areaType">地址类型</param>
        /// <param name="parent">上级区域属性名</param>
        /// <param name="isAllowNull">是否允许为空</param>
        public AreaValidate ( string error, AreaType areaType, string parent, bool isAllowNull = true ) : base(error)
        {
            this.AreaType = areaType;
            this._ParentAttr = parent;
            this._IsAllowNull = isAllowNull;
        }
        /// <summary>
        /// 地址数据验证(联动检查)
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="parent">上级区域属性名</param>
        /// <param name="isAllowNull">是否允许为空</param>
        public AreaValidate ( string error, string parent, bool isAllowNull = true ) : base(error)
        {
            this._ParentAttr = parent;
            this._IsAllowNull = isAllowNull;

        }
        protected override bool _CheckAttr ( object source, Type type, object data )
        {
            if ( !type.IsValueType || data == null )
            {
                return false;
            }
            int areaId = Convert.ToInt32(data);
            if ( areaId == 0 )
            {
                if ( this._IsAllowNull )
                {
                    return false;
                }
                else if ( this._ParentAttr == null )
                {
                    return true;
                }
                else
                {
                    areaId = source.GetObjectValue<int>(this._ParentAttr);
                    if ( areaId == 0 )
                    {
                        return true;
                    }
                    else if ( !AreaHelper.TryGetArea(areaId, out Area.Area area) )
                    {
                        return true;
                    }
                    else
                    {
                        return area.LowerNum != 0;
                    }
                }
            }
            else if ( !AreaHelper.TryGetArea(areaId, out Area.Area area) || ( this.AreaType != AreaType.未知 && this.AreaType != area.AreaType ) )
            {
                return true;
            }
            else if ( this.ParentId.HasValue && ( area.ParentId == 0 || this.ParentId != area.ParentId ) )
            {
                return true;
            }
            else if ( this._ParentAttr != null && ( area.ParentId == 0 || source.GetObjectValue<int>(this._ParentAttr) != area.ParentId ) )
            {
                return true;
            }
            return false;
        }

    }
}
