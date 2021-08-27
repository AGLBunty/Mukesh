using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMAS.Models.UserLogin
{
    public class Userlogin
    {
        public int Id { get; set; }

        //[Display(Name = "Email ID")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string UserType { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }


    }
}