namespace SqlExecHelper
{
        public enum SqlIgnoreType
        {
                无 = 0,
                insert = 2,
                update = 4,
                query = 8
        }
        public enum SqlEventPrefix
        {
                deleted = 0,
                inserted = 1
        }
        public enum LikeQueryType
        {
                自定义 = 0,
                左 = 2,
                右 = 4,
                全 = 6
        }
        public enum SqlFuncType
        {
                count = 1,
                sum = 2,
                avg = 3,
                min = 4,
                max = 5
        }
        public enum SqlSetType
        {
                等于 = 0,
                递加 = 1,
                递减 = 2
        }
        public enum SqlColType
        {
                通用 = 14,
                查询 = 2,
                修改 = 4,
                添加 = 8,
                标识 = 16
        }

        public enum QueryType : int
        {
                等号 = 0,
                大于 = 1,
                小于 = 2,
                大等 = 3,
                小等 = 4,
                不等 = 5
        }
}
