using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BaseCore.Helper.Validations
{
    public class NationalId : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string text = (string)value;
            if (text.Length < 10 || text.Length > 10 || text == "0000000000" || text == "1111111111" || text == "2222222222" || text == "3333333333" || text == "4444444444" || text == "5555555555" || text == "6666666666" || text == "7777777777" || text == "8888888888" || text == "9999999999")
            {
                return false;
            }
            int num = 10;
            int num2 = 0;
            int num3 = int.Parse(text.Substring(9, 1));
            int i = 0;
            while (i < text.Length - 1)
            {
                num2 += int.Parse(text.Substring(i, 1)) * num;
                i++;
                num--;
            }
            int num4 = num2 % 11;
            return (num3 == num4 && num4 == 0) || (num3 == 1 && num4 == 1) || (num4 > 1 && num3 == 11 - num4);
        }
        public void AddValidation(ClientModelValidationContext context)
        {
       
            context.Attributes.Add("ErrorMessage", $"کد ملی وارد شده صحیح نمی‌باشد.");

        }


       
    }
}
