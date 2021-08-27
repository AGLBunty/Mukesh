namespace SMAS.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
    }

    public class Requestsendotp
    {
        public string PN_MSPIN { get; set; }
       

    }

    public class Responsesendotp
    {
        public string code { get; set; }
        public string message { get; set; }

        public sendotp result { get; set; }

    }

    public class sendotp
    {
        public string PN_OTP { get; set; }
    }
    

}