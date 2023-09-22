using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class FileViewModel :BaseViewModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int? Size { get; set; }
        public int StoreId { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }

       
    }
    
    public class FileUpload : ValidationAttribute, IClientModelValidator
    {
        public string FileExtensions { get; set; }
        public int MaxSize { get; set; }
        public bool AutoUpload { get; set; }
        public bool Multiple { get; set; } 
        public string RemoveUrl { get; set; }
        public string SaveUrl { get; set; } 
        public string UpdateUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSize">Enter max file size in KB.</param>
        /// <param name="fileExtentions">File extentions split with ',' </param>
        public FileUpload(int maxSize, string fileExtentions, bool autoUpload, bool multiple, string removeUrl, string updateUrl)
        {
            FileExtensions = fileExtentions;
            MaxSize = maxSize;
            AutoUpload = autoUpload;
            Multiple = multiple;
            RemoveUrl = removeUrl;
            UpdateUrl = updateUrl;
        }
        public override bool IsValid(object value)
        {
            return true;

        }


        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("maxsize", MaxSize + "");
            context.Attributes.Add("extentions", string.Join("|", FileExtensions));
            context.Attributes.Add("ErrorMessage", $"نوع فایل انتخاب شده باید ({FileExtensions}) باشد و حجمش کمتر از ({MaxSize} KB) باشد.");

        }
    }
}
