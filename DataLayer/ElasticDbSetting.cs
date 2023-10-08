namespace DataLayer
{
    public class ElasticDbSetting
    {
        public string ConnectionString { get; set; } = "http://localhost:9200/";
        public string DefaultIndex { get; set; } = "applicants";
    }
}
