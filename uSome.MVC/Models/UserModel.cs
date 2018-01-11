using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uSome.Membership.Models
{
    public class LoginModel
    {
       
        public string LoginName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

      
        [DataType(DataType.Password)]
        public string Password { get; set; }

     
        //public bool RememberMe { get; set; }
    }


    public class RegisterModel
    {
        public int Id { get; set; }

      
        public string LoginName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


       
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileModel
    {
        public string RelationToCancer { get; set; }

        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public  DateTime?  Dob{ get; set; }

        public string Image { get; set; }

        public string Gender { get; set; }
    }


    public class MoreAboutMeModel
    {
        public string Motto { get; set; }
        public string ShortBiography { get; set; }
        public string Accommodation { get; set; }
        public string EveryDayLifeDuty { get; set; }
        public string Province { get; set; }
    }



    public class ForgotPasswordModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

 

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class MemberContactsModel
    {
        public int ID { get; set; }
        public int RequestFrom { get; set; }
        public int RequestTo { get; set; }
        public object RequestConfirmed { get; set; }
        public string RequestMessage { get; set; }
        public object RequestDate { get; set; }
        public object ConfirmDate { get; set; }
    }

    public class ResearchModel
    {
        public string Investigation { get; set; }
        public List<String> InvestigationList { get; set; }
    }

    public class ConsequencesModel
    {
        public string Consequences { get; set; }
        public List<String> ConsequencesList { get; set; }
    }


    public class VisibilityModel
    {
        public string visibility { get; set; }
    }

}