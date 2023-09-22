using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BaseCore.ViewModels.ViewModel
{
    public class TokenRequest
    {
        //  [Required]
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        public string refresh_token { get; set; }

        public string tokengoogle { get; set; }
        public string token { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public Guid Guid { get; set; }

    }
}
