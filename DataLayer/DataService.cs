using MongoDB.Driver;
using Nest;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Models;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private readonly IMongoCollection<Application> _mongoCollection;
        private readonly ElasticClient _elasticClient;
        private readonly HttpClient _pythonClient;

        public DataService()
        {
            MongoDbSetting mongoDbSetting = new MongoDbSetting();
            ElasticDbSetting elasticDbSetting = new ElasticDbSetting();
            PythonApi pythonApi = new PythonApi();
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
            var dataBase = mongoClient.GetDatabase(mongoDbSetting.DatabaseName);
            _mongoCollection = dataBase.GetCollection<Application>(mongoDbSetting.CollectionName);
            var settings = new ConnectionSettings(new Uri(elasticDbSetting.ConnectionString)).DefaultIndex(elasticDbSetting.DefaultIndex);
            _elasticClient = new ElasticClient(settings);
            _pythonClient = new HttpClient();
            _pythonClient.BaseAddress = new Uri(pythonApi.BaseAddress);
            _pythonClient.DefaultRequestHeaders.Accept.Clear();
            _pythonClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<Application> Get()
        {
            try
            {
                var v =  _mongoCollection.Find(_ => true).ToList();
                return v;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get data from to MongoDB due to" + ex.Message);
            }
        }
        public void PostMongo(Application person)
        {
            try
            {
                _mongoCollection.InsertOne(person);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to MongoDB due to" + ex.Message);
            }
        }
        public void PostElastic(elasticResume person)
        {
            try
            {
                var indexResponse = _elasticClient.IndexDocument(person);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to ElasticDB due to" + ex.Message);
            }
        }
        public Application Get(String id)
        {
            try
            {
                return _mongoCollection.Find(x => x._id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to post data to MongoDB due to" + ex.Message);
            }
        }
        public List<Application> ElasticSearch(string ner)
        {
            JObject json = JObject.Parse(ner);
            try
            {
                var searchResponse = _elasticClient.Search<elasticResume>(s => s.Query(q => q
                                                                                    .Term(p => p.Skill, json["SKILL"].ToString()) || q
                                                                                    .Term(p => p.Exp, json["SKILL"].ToString()) || q
                                                                                    .Term(p => p.Exp, json["ORG"].ToString()) || q
                                                                                    .Term(p => p.Edu, json["ORG"].ToString()) || q
                                                                                    .Term(p => p.Edu, json["SKILL"].ToString()) || q
                                                                                    .Term(p => p.Pro, json["SKILL"].ToString()) || q
                                                                                    ));
                var resumeList = new List<Application>();
                var resumes = searchResponse.Documents.ToArray();
                HashSet<string> Ids = new HashSet<string>();
                foreach (var resume in resumes)
                    if (!Ids.Contains(resume.Id))
                    {
                        resumeList.Add(Get(resume.Id));
                    }
                return resumeList;
            }
            catch (Exception ex)
            {
                throw new Exception("Resumes Cannot be fetched due to" + ex.Message);
            }
        }
        public async Task<string> Synthesize(QueryClass query)
        {
            HttpResponseMessage response = await _pythonClient.PostAsJsonAsync("query", query);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
