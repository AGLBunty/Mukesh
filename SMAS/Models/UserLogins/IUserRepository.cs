using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAS.Models.UserLogin
{
    public interface IUserRepository
    {
        Userlogin CheckLogin(Userlogin userlogin);

        Task UploadFileData(FileUpload employee);

    }
}
