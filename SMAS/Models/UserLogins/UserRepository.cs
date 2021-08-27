using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMAS.Models.UserLogin
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlDbContext db = new SqlDbContext();
        public  Userlogin CheckLogin(Userlogin userlogin)
        {
            try
            {
                Userlogin userlogins = db.UserLogins.Where(a => a.EmailID == userlogin.EmailID).FirstOrDefault();
                if (userlogins == null)
                {
                    return null;
                }
                return userlogins;
            }
            catch
            {
                throw;
            }
        }

        public async Task UploadFileData(FileUpload data)
        {
            db.tblFileUpload.Add(data);
            try
            {
                await db.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}