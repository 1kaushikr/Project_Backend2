using Services;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        public ApplicantController()
        {
            _applicantService = new ApplicantService();
        }


        // GET: <Controller>
        [HttpGet]
        public ActionResult<List<Application>> Get()
        {
            return _applicantService.Get();
        }


        // POST <Controller>
        [HttpPost]
        public void Post(Application value) 
        {
            _applicantService.Post(value);
        }

        // GET: <Controller>/id
        [HttpGet("{id:length(24)}")]
        public ActionResult<Application> Get(string id)
        {
            return _applicantService.Get(id);
        }

        // POST <Controller>/Query
        [HttpPost("Query")]
        public ActionResult<List<Application>> Query(QueryClass query)
        {
            return _applicantService.Query(query);
        }
    }
}