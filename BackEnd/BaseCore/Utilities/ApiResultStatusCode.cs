using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BaseCore.Utilities
{


  public enum ApiResultStatusCode
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeSuccess))]
        Success = 0,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeServerError))]
        ServerError = 1,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeBadRequest))]
        BadRequest = 2,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeNotFound))]
        NotFound = 3,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeListEmpty))]
        ListEmpty = 4,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeLogicError))]
        LogicError = 5,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ApiResultStatusCodeUnAuthorized))]
        UnAuthorized = 6
    }
}
