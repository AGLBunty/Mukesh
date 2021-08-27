using SMAS.Models.List;
using SMAS.Models.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SMAS.Controllers.UserLogins
{
    public class AdminApiController : ApiController
    {
        private readonly IUserRepository _iUserRepository = new UserRepository();

        [HttpPost]
        [Route("api/Admin/CheckLogin")]
         public Userlogin CheckLogin([FromBody]Userlogin userlogin)
        {
            var result = _iUserRepository.CheckLogin(userlogin);
            return result;
        }


        [HttpPost]
        [Route("api/Admin/AddFile")]
        public async Task CreateAsync([FromBody]FileUpload filedata)
        {
            if (ModelState.IsValid)
            {
                await _iUserRepository.UploadFileData(filedata);
            }
        }


    }
}
