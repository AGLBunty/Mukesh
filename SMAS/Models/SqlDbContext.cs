using SMAS.Models.ExceptionDT;
using SMAS.Models.List;
using SMAS.Models.UserLogin;
using System.Data.Entity;

namespace SMAS.Models
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext() : base("name=SqlConn")
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Userlogin> UserLogins { get; set; }
        public DbSet<Company> tblcompany { get; set; }
        public DbSet<Designation> tbldesignation { get; set; }
        public DbSet<Vertical> tblvertical { get; set; }
        public DbSet<Assign> Assigns { get; set; }
		public DbSet<Languages> tblLanguages { get; set; }
		public DbSet<Model> tblModel { get; set; }
		public DbSet<FileVersions> tblFileVersions { get; set; }
		public DbSet<ServiceManuals> tblServiceManuals { get; set; }
		public DbSet<FileUpload> tblFileUpload { get; set; }
		public DbSet<EXCEPTION_DETAILS> EXCEPTION_DETAILS { get; set; }
		public DbSet<FileUploadPublishUpdate> FileUploadModelUpdate { get; set; }
		public DbSet<Subscribe> tblSubsribe { get; set; }
		public DbSet<Feedbacks> tblFeedbacks { get; set; }

        //public DbSet<SubscribeUpdate> UpdateSubData { get; set; }
        public DbSet<SUBSUCRIBE_FINAL> tblSUBSUCRIBE_FINAL { get; set; }

       // add 25-08-2021
        public DbSet<DropdownValues> tblDropdownList { get; set; }
       // add 26-08-2021
        public DbSet<tblFeedback_Form> tblFeedback_Form { get; set; }
    }
}