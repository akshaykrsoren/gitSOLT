using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.ComponentModel;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace CaregiverLite.Models
{
    // Contains simple details like location, date of birth, etc.
    public class ProfileCommon : ProfileBase
    {
        public string FirstName
        {
            get
            {
                return (string)GetPropertyValue("FirstName");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("FirstName", value);
            }
        }
        public string Emailid
        {
            get
            {
                return (string)GetPropertyValue("EmailID");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("EmailID", value);
            }
        }

        public string LastName
        {
            get
            {
                return (string)GetPropertyValue("LastName");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("LastName", value);
            }
        }

        public string PhoneNo
        {
            get
            {
                return (string)GetPropertyValue("PhoneNo");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("PhoneNo", value);
            }
        }

        public string Address
        {
            get
            {
                return (string)GetPropertyValue("Address");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("Address", value);
            }
        }

        public int CountryId
        {
            get
            {
                return (int)GetPropertyValue("CountryId");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("CountryId", value);
            }
        }

        public int StateId
        {
            get
            {
                return (int)GetPropertyValue("StateId");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("StateId", value);
            }
        }

        public int CityId
        {
            get
            {
                return (int)GetPropertyValue("CityId");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("CityId", value);
            }
        }
        public string City
        {
            get
            {
                return (string)GetPropertyValue("City");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("City", value);
            }
        }
        /*
        public int CityId
        {
            get
            {
                return (int)GetPropertyValue("CityId");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("CityId", value);
            }
        }
        */
        public string Postcode
        {
            get
            {
                return (string)GetPropertyValue("Postcode");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("Postcode", value);
            }
        }

        

        
        public Boolean IsActive
        {
            get
            {
                return (Boolean)GetPropertyValue("IsActive");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("IsActive", value);
            }
        }

        public string LanguageCode
        {
            get
            {
                return (string)GetPropertyValue("LanguageCode");
            }
            set
            {
                if (!IsAnonymous) SetPropertyValue("LanguageCode", value);
            }
        }
        public ProfileCommon() { }

        public static ProfileCommon GetProfile(string username)
        {
            // classic cast throws exceptions which are easier to debug than using the 'as' keyword
            return (ProfileCommon)ProfileBase.Create(username, true);
        }

        public static ProfileCommon GetProfile()
        {
            return (ProfileCommon)ProfileBase.Create(Membership.GetUser().UserName, true);
        }


    }


    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            //FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            FormsAuthentication.SetAuthCookie(userName, true);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

}