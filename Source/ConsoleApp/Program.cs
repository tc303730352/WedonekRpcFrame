using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace ConsoleApp
{
    public class ModuleSortSet
    {
        /// <summary>
        /// 模版中的模块ID
        /// </summary>
        public long Id { get; set; }

    }
    internal class Program
    {
        public static void SetSort ( [NullValidate("public.param.null")] ModuleSortSet[] sorts )
        {

        }
        private static void Main ( string[] args )
        {
            string json = new ModuleSortSet
            {
                Id = 9043647030625557
            }.ToJson();
            Console.Write(json);
            _ = Console.ReadLine();
        }
    }
}
