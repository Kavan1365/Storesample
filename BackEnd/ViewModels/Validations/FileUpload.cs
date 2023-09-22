using BaseCore.Core.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Validations
{

    public class FileUpload : ValidationAttribute, IClientModelValidator
    {
        public string FileExtensions { get; set; }
        public int MaxSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSize">Enter max file size in KB.</param>
        /// <param name="fileExtentions">File extentions split with ',' </param>
        public FileUpload(int maxSize, string fileExtentions)
        {
            FileExtensions = fileExtentions;
            MaxSize = maxSize;
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var model = value as FileViewModel;
            if (!FileExtensions?.Split(',').Select(x => x.Trim()).Any(t => t.ToLower() == model.Extension?.ToLower()) ?? true)
                return false;

            if (model.Size > MaxSize * 1024)
                return false;

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
