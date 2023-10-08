using Models;

namespace Services
{
    public interface IApplicantService
    {
        List<Application> Get();
        Application Get(string id);
        void Post(Application person);
        List<Application> Query(QueryClass query);
    }
}