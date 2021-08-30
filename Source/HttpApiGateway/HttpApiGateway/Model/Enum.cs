namespace HttpApiGateway.Model
{
        public enum ResponseType
        {
                JSON = 0,
                XML = 1,
                Stream = 2,
                JumpUri = 3,
                HttpStatus = 4,
                String = 5,
        }
    
        internal enum FuncParamType
        {
                参数 = 0,
                返回值 = 1,
                错误信息 = 2,
                数据总数 = 3,
                登陆状态 = 4,
                身份标识 = 5,
                Interface=6
        }
}
