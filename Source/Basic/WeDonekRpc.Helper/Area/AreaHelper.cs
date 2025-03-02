using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Area
{
    public class AreaDistance
    {
        public Area Area
        {
            get;
            set;
        }

        public int Distance
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 地区数据
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 地区ID
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 地区名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 区域类型
        /// </summary>
        public AreaType AreaType
        {
            get;
            set;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Lat
        {
            get;
            set;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Lng
        {
            get;
            set;
        }
        /// <summary>
        /// 下级数
        /// </summary>
        public int LowerNum
        {
            get;
            set;
        }
        public bool CheckDistance ( double lat, double lng, int distance, GpsType type, out int num )
        {
            if ( this.Lat == 0 && this.Lng == 0 )
            {
                num = 0;
                return false;
            }
            num = Tools.GetDistance(lat, lng, type, decimal.ToDouble(this.Lat), decimal.ToDouble(this.Lng), GpsType.GPS坐标);
            return num <= distance;
        }
        public bool CheckDistance ( decimal lat, decimal lng, int distance, GpsType type, out int num )
        {
            if ( this.Lat == 0 && this.Lng == 0 )
            {
                num = 0;
                return false;
            }
            num = Tools.GetDistance(lat, lng, type, this.Lat, this.Lng, GpsType.GPS坐标);
            return num <= distance;
        }
        public bool CheckDistance ( decimal lat, decimal lng, GpsType type, out AreaDistance distance )
        {
            if ( this.Lat == 0 && this.Lng == 0 )
            {
                distance = null;
                return false;
            }
            distance = new AreaDistance
            {
                Distance = Tools.GetDistance(lat, lng, type, this.Lat, this.Lng, GpsType.GPS坐标),
                Area = this
            };
            return true;
        }
        public bool CheckDistance ( double lat, double lng, GpsType type, out AreaDistance distance )
        {
            if ( this.Lat == 0 && this.Lng == 0 )
            {
                distance = null;
                return false;
            }
            distance = new AreaDistance
            {
                Distance = Tools.GetDistance(lat, lng, type, decimal.ToDouble(this.Lat), decimal.ToDouble(this.Lng), GpsType.GPS坐标),
                Area = this
            };
            return true;
        }
    }
    /// <summary>
    /// 地区帮助
    /// </summary>
    public class AreaHelper
    {
        private const int _CountryId = 1;
        private static Area[] _Country = Array.Empty<Area>();
        private static Area[] _ProArea = Array.Empty<Area>();

        private static Area[] _CityArea = Array.Empty<Area>();


        static AreaHelper ()
        {
            _ = Task.Factory.StartNew(() =>
            {
                LoadArea();
            });
        }
        private static readonly string[] _AreaType = new string[] { "省", "市", "区", "县" };

        private static Dictionary<int, Area> _AreaDic = [];

        private static volatile bool _IsLoad = false;

        public static Area[] AreaList { get; private set; } = Array.Empty<Area>();
        public static int CountryId => _CountryId;

        public static string Country => _DefCountry.Name;
        private static Area _DefCountry = null;
        /// <summary>
        /// 加载地区文件
        /// </summary>
        /// <param name="path"></param>
        public static void LoadArea ( string path = null )
        {
            if ( _IsLoad )
            {
                return;
            }
            _IsLoad = true;
            path ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Area.json");
            FileInfo file = new FileInfo(path);
            if ( file.Exists )
            {
                string json = null;
                using ( StreamReader ready = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete), Encoding.UTF8) )
                {
                    json = ready.ReadToEnd();
                }
                AreaList = JsonTools.Json<Area[]>(json);
                _InitArea();
            }
            else
            {
                _IsLoad = false;
            }
        }
        private static void _InitArea ()
        {
            AreaList.ForEach(a =>
             {
                 if ( a.AreaType != AreaType.区县 )
                 {
                     a.LowerNum = AreaList.Count(b => b.ParentId == a.Id);
                 }
             });
            _Country = AreaList.FindAll(a => a.ParentId == 0);
            _AreaDic = AreaList.ToDictionary(b => b.Id);
            _ProArea = AreaList.FindAll(a => a.ParentId == _CountryId);
            _CityArea = AreaList.FindAll(b => b.AreaType == AreaType.城市 && Array.FindIndex(_ProArea, a => a.Id == b.ParentId) != -1);
            _DefCountry = AreaList.Find(a => a.Id == _CountryId);
        }
        internal static bool GetCountry ( int id, out Area area )
        {
            area = _Country.Find(a => a.Id == id);
            return area != null;
        }
        /// <summary>
        /// 获取区域名
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static string GetAreaName ( int areaId )
        {
            return areaId == 0 ? null : _AreaDic.TryGetValue(areaId, out Area area) ? area.Name : null;
        }
        public static string GetFullAreaName ( int areaId )
        {
            if ( areaId == 0 )
            {
                return null;
            }
            else if ( !_AreaDic.TryGetValue(areaId, out Area area) )
            {
                return null;
            }
            else if ( area.ParentId == _CountryId || area.ParentId == 0 )
            {
                return area.Name;
            }
            else
            {
                List<string> list = new List<string>(3)
                {
                        area.Name
                };
                do
                {
                    if ( !_AreaDic.TryGetValue(area.ParentId, out Area parent) )
                    {
                        return null;
                    }
                    list.Add(parent.Name);
                    if ( parent.ParentId == _CountryId || parent.ParentId == 0 )
                    {
                        break;
                    }
                    area = parent;
                } while ( true );
                return list.Count == 2 ? string.Concat(list[1], list[0]) : string.Concat(list[2], list[1], list[0]);
            }
        }
        /// <summary>
        /// 通过名字查询国家ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindCountryId ( string name )
        {
            if ( string.IsNullOrEmpty(name) )
            {
                return 0;
            }
            Area area = _Country.Find(a => a.Name.StartsWith(name));
            return area == null ? 0 : area.Id;
        }
        /// <summary>
        /// 查找区域ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindCityId ( string name )
        {
            if ( string.IsNullOrEmpty(name) )
            {
                return 0;
            }
            Area area = _CityArea.Find(a => a.Name.StartsWith(name));
            return area == null ? 0 : area.Id;
        }
        /// <summary>
        /// 查找区域ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindProId ( string name )
        {
            if ( string.IsNullOrEmpty(name) )
            {
                return 0;
            }
            Area area = _ProArea.Find(a => a.Name.StartsWith(name));
            return area == null ? 0 : area.Id;
        }
        /// <summary>
        /// 查找区域ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindAreaId ( string name )
        {
            if ( string.IsNullOrEmpty(name) )
            {
                return 0;
            }
            Area area = AreaList.Find(a => a.Name.StartsWith(name));
            return area == null ? 0 : area.Id;
        }
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool TryGetArea ( int areaId, out Area area )
        {
            return _AreaDic.TryGetValue(areaId, out area);
        }
        public static Area GetArea ( int areaId )
        {
            return _AreaDic[areaId];
        }

        public static int[] GetCityList ( int[] areaId )
        {
            List<int> city = [];
            areaId.ForEach(a =>
             {
                 if ( !_AreaDic.TryGetValue(a, out Area area) )
                 {
                     return;
                 }
                 else if ( area.AreaType == AreaType.城市 )
                 {
                     city.Add(a);
                 }
                 else if ( area.AreaType == AreaType.省份 )
                 {
                     _CityArea.ForEach(b => b.ParentId == a, b =>
                                 {
                                     city.Add(b.Id);
                                 });
                 }
                 else if ( area.AreaType == AreaType.区县 )
                 {
                     city.Add(area.ParentId);
                 }
             });
            return city.Distinct().ToArray();
        }
        public static int[] GetAreaList ( int areaId )
        {
            if ( !_AreaDic.TryGetValue(areaId, out Area obj) )
            {
                return new int[0];
            }
            else if ( obj.AreaType == AreaType.省份 )
            {
                return new int[]
                {
                                        obj.ParentId,
                                         obj.Id
                };
            }
            else if ( obj.AreaType == AreaType.城市 )
            {
                return new int[]
                {
                    _AreaDic[obj.ParentId].ParentId,
                    obj.ParentId,
                    obj.Id
                };
            }
            else
            {
                int proId = _AreaDic[obj.ParentId].ParentId;
                return new int[]
               {
                    _AreaDic[proId].ParentId,
                    proId,
                    obj.ParentId,
                    obj.Id
               };
            }
        }
        public static Area[] GetAreas ( int areaId )
        {
            if ( !_AreaDic.TryGetValue(areaId, out Area obj) )
            {
                return new Area[0];
            }
            else if ( obj.AreaType == AreaType.省份 )
            {
                return new Area[]
                {
                    _AreaDic[obj.ParentId],//国家
                    obj//省份
                };
            }
            else if ( obj.AreaType == AreaType.城市 )
            {
                Area pro = _AreaDic[obj.ParentId];
                return new Area[]
                {
                    _AreaDic[pro.ParentId],
                    pro,
                    obj
                };
            }
            else
            {
                Area city = _AreaDic[obj.ParentId];
                Area pro = _AreaDic[city.ParentId];
                return new Area[]
               {
                    _AreaDic[pro.ParentId],
                    pro,
                    city,
                    obj
               };
            }
        }
        public static int[] GetAreaId ( int areaId )
        {
            if ( !_AreaDic.TryGetValue(areaId, out Area obj) )
            {
                return new int[0];
            }
            else if ( obj.AreaType == AreaType.省份 )
            {
                return new int[]
                {
                    obj.ParentId,//国家
                    obj.Id//省份
                };
            }
            else if ( obj.AreaType == AreaType.城市 )
            {
                Area pro = _AreaDic[obj.ParentId];
                return new int[]
                {
                    pro.ParentId,
                    pro.Id,
                    obj.Id
                };
            }
            else
            {
                Area city = _AreaDic[obj.ParentId];
                Area pro = _AreaDic[city.ParentId];
                return new int[]
               {
                    pro.ParentId,
                    pro.Id,
                    city.Id,
                    obj.Id
               };
            }
        }
        /// <summary>
        /// 查找区域信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Area FindArea ( string name )
        {
            return AreaList.Find(a => a.Name.StartsWith(name));
        }
        public static AreaDistance FindArea ( decimal lat, decimal lng, int distance, GpsType type = GpsType.GPS坐标 )
        {
            List<AreaDistance> areas = [];
            AreaList.ForEach(a =>
             {
                 if ( a.CheckDistance(lat, lng, distance, type, out int num) )
                 {
                     areas.Add(new AreaDistance
                     {
                         Area = a,
                         Distance = num
                     });
                 }
             });
            if ( areas.Count == 1 )
            {
                return areas[0];
            }
            else if ( areas.Count > 1 )
            {
                AreaDistance area = areas.Where(a => a.Area.AreaType == AreaType.区县).OrderBy(a => a.Distance).First();
                return area ?? areas.OrderBy(a => a.Distance).First();
            }
            return new AreaDistance
            {
                Area = _DefCountry
            };
        }
        public static AreaDistance FindCity ( decimal lat, decimal lng, int distance, GpsType type = GpsType.GPS坐标 )
        {
            List<AreaDistance> areas = [];
            _CityArea.ForEach(a =>
             {
                 if ( a.CheckDistance(lat, lng, distance, type, out int num) )
                 {
                     areas.Add(new AreaDistance
                     {
                         Area = a,
                         Distance = num
                     });
                 }
             });
            if ( areas.Count == 1 )
            {
                return areas[0];
            }
            else if ( areas.Count > 1 )
            {
                return areas.OrderBy(a => a.Distance).First();
            }
            return new AreaDistance
            {
                Area = _DefCountry
            };
        }

        public static AreaDistance FindCity ( decimal lat, decimal lng, GpsType type = GpsType.GPS坐标 )
        {
            List<AreaDistance> areas = [];
            Array.ForEach(_CityArea, a =>
             {
                 if ( a.CheckDistance(lat, lng, type, out AreaDistance obj) )
                 {
                     areas.Add(obj);
                 }
             });
            AreaDistance area = areas.OrderBy(a => a.Distance).First();
            return area ?? new AreaDistance
            {
                Area = _DefCountry
            }
;
        }


        /// <summary>
        /// 查找区域信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static Area FindArea ( string name, int parentId = _CountryId )
        {
            return parentId == -1
                    ? Array.Find(AreaList, a => a.Name.StartsWith(name))
                    : Array.Find(AreaList, a => a.Name.StartsWith(name) && a.ParentId == parentId);
        }
        /// <summary>
        /// 查找城市信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pro"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static bool FindCity ( string name, out Area pro, out Area city )
        {
            if ( string.IsNullOrEmpty(name) )
            {
                pro = null;
                city = null;
                return false;
            }
            city = Array.Find(_CityArea, a => a.Name.StartsWith(name));
            if ( city != null )
            {
                return _AreaDic.TryGetValue(city.ParentId, out pro);
            }
            pro = null;
            return false;
        }
        /// <summary>
        /// 查找省份信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Area FindPro ( string name )
        {
            return Array.Find(_ProArea, a => a.Name.StartsWith(name));
        }

        /// <summary>
        /// 查找区域
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="proId"></param>
        /// <param name="cityId"></param>
        /// <param name="disId"></param>
        /// <returns></returns>
        public static bool FindArea ( string areaName, out int proId, out int cityId, out int disId )
        {
            int index = 0;
            int begin = 0;
            string name = null;
            int pId = 0;
            int cId = 0;
            int did = 0;
            int areaId = -1;
            _AreaType.ForEach(a =>
             {
                 index = areaName.IndexOf(a, index) + 1;
                 if ( index != 0 )
                 {
                     name = areaName.Substring(begin, index - begin);
                     begin = index;
                     Area area = FindArea(name, areaId);
                     if ( area != null )
                     {
                         areaId = area.Id;
                         if ( area.AreaType == AreaType.省份 )
                         {
                             pId = area.Id;
                         }
                         else if ( area.AreaType == AreaType.城市 )
                         {
                             cId = area.Id;
                             pId = area.ParentId;
                         }
                         else if ( area.AreaType == AreaType.区县 )
                         {
                             did = area.Id;
                             cId = area.ParentId;
                         }
                     }
                 }
             });
            if ( pId == 0 && cId != 0 )
            {
                pId = _AreaDic[cId].ParentId;
            }
            proId = pId;
            cityId = cId;
            disId = did;
            return pId != 0;
        }
        /// <summary>
        /// 更新区域数据文件
        /// </summary>
        /// <param name="areas"></param>
        /// <param name="savePath"></param>
        public static void UpdateArea ( Area[] areas, string savePath = null )
        {
            _IsLoad = true;
            savePath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Area.json");
            AreaList = areas.ToArray();
            _AreaDic = AreaList.ToDictionary(b => b.Id);
            if ( File.Exists(savePath) )
            {
                File.Delete(savePath);
            }
            string json = JsonTools.Json(AreaList);
            using ( StreamWriter write = new StreamWriter(File.Open(savePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Delete), Encoding.UTF8) )
            {
                write.Write(json);
                write.Flush();
            }
        }

        public static bool FindArea ( string name, out Area pro, out Area city )
        {
            Area area = AreaList.Find(a => a.Name.StartsWith(name));
            if ( area == null )
            {
                pro = null;
                city = null;
                return false;
            }
            if ( area.ParentId == _CountryId )
            {
                pro = area;
                city = null;
                return true;
            }
            if ( _AreaDic.TryGetValue(area.ParentId, out Area pArea) )
            {
                if ( pArea.ParentId == _CountryId )
                {
                    pro = pArea;
                    city = area;
                    return true;
                }
                else if ( _AreaDic.TryGetValue(pArea.ParentId, out pro) )
                {
                    city = pArea;
                    return true;
                }
            }
            pro = null;
            city = null;
            return false;
        }
    }
}
