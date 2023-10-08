using Models;
using DataLayer;

namespace Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly DataService _dataService;
        public ApplicantService()
        {
            _dataService = new DataService();
        }

        public List<Application> Get()
        {
            return _dataService.Get();
        }


        public void Post(Application person)
        {
            if (person == null)
            {
                throw new Exception("Data Sent to Server is null");
            }
            _dataService.PostMongo(person);
            var exp = "";
            if (person.ExpList != null)
            {
                for (int i = 0; i < person.ExpList.Count; i++)
                {
                    var v1 = person.ExpList[i].Role.Length > 0 ? " as " + person.ExpList[i].Role : "";
                    var v2 = person.ExpList[i].Org.Length > 0 ? " in " + person.ExpList[i].Org : "";
                    var v3 = person.ExpList[i].StartDate.Length > 0 ? " from " + person.ExpList[i].StartDate : "";
                    var v4 = person.ExpList[i].EndDate.Length > 0 ? " to " + person.ExpList[i].EndDate : "";
                    var v5 = person.ExpList[i].Respon.Length > 0 ? " My Responibilities were " + person.ExpList[i].Respon : "";
                    var v6 = v1 + v2 + v3 + v4 + v5;
                    exp = v6.Length > 0 ? exp + " Worked" + v6 : exp;
                }
            }
            var edu = "";
            if (person.EduList != null)
            {
                for (int i = 0; i < person.EduList.Count; i++)
                {
                    var v1 = person.EduList[i].Org.Length > 0 ? " in " + person.EduList[i].Org : "";
                    var v2 = person.EduList[i].StartDate.Length > 0 ? " from " + person.EduList[i].StartDate : "";
                    var v3 = person.EduList[i].EndDate.Length > 0 ? " to " + person.EduList[i].EndDate : "";
                    var v4 = person.EduList[i].Deg.Length > 0 ? " doing " + person.EduList[i].Deg : "";
                    var v5 = person.EduList[i].Major.Length > 0 ? " majoring in " + person.EduList[i].Major : "";
                    var v6 = v1 + v2 + v3 + v4 + v5;
                    edu = v6.Length > 0 ? edu + " Studied" + v6 : edu;
                }
            }
            var pro = "";
            if (person.ProList != null)
            {
                for (int i = 0; i < person.ProList.Count; i++)
                {
                    var v1 = person.ProList[i].Name.Length > 0 ? " on " + person.ProList[i].Name : "";
                    var v2 = person.ProList[i].StartDate.Length > 0 ? " from " + person.ProList[i].StartDate : "";
                    var v3 = person.ProList[i].EndDate.Length > 0 ? " to " + person.ProList[i].EndDate : "";
                    var v4 = person.ProList[i].Respon.Length > 0 ? " My Responibilities were " + person.ProList[i].Respon : "";
                    var v5 = v1 + v2 + v3 + v3 + v4;
                    pro = v5.Length > 0 ? pro + "Worked" + v5 : pro;
                }
            }
            var skill = "";
            if (person.Skill.Count > 0)
            {
                skill = person.Skill[0];
                for (int i = 1; i < person.Skill.Count; i++)
                    skill =skill+", "+ person.Skill[i];
            }
            elasticResume temp = new elasticResume();
            temp.Id = person._id;
            temp.Edu = edu;
            temp.Exp = exp;
            temp.Skill = skill;
            temp.Pro = pro;
            _dataService.PostElastic(temp);
        }

        public Application Get(string id)
        {
            if (id == null)
            {
                throw new Exception("ID Sent to Server is null");
            }
            var arr = _dataService.Get(id);
            if (arr is null)
            {
                throw new Exception("There is not Applicant with this ID");
            }
            return arr;
        }
        public List<Application> Query(QueryClass query)
        {
            if (query.Query == null || query.Query.Length == 0)
            {
                throw new Exception("Invalid length of Query");
            }
            string ners = _dataService.Synthesize(query).Result;
            return _dataService.ElasticSearch(ners);
        }
    }
}
