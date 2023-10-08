using Models;

namespace DataLayer
{
    public interface IDataService
    {
        List<Application> ElasticSearch(string ner);
        List<Application> Get();
        Application Get(string id);
        void PostElastic(elasticResume person);
        void PostMongo(Application person);
        Task<string> Synthesize(QueryClass query);
    }
}