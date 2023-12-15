using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class PrivacySettings
    {
        public string token { get; set; }
        public bool PrivatePosts { get; set; }
        public bool PrivateFriends { get; set; }
        public bool PrivatePictures { get; set; }
        public bool PrivateCV { get; set; }
        public bool PrivateEmail { get; set; }
    }
}