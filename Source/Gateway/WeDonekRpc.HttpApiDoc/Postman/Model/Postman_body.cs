namespace WeDonekRpc.HttpApiDoc.Postman.Model
{
        internal class Postman_options
        {
                public Postman__raw_options raw = new Postman__raw_options();
        }
        internal class Postman__raw_options
        {
                public string language = "json";
        }

        internal class Postman_body
        {
                public string mode = "raw";
                public string raw;
                public Postman_options options = new Postman_options();
        }
}
