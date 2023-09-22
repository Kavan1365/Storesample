using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Prodcuts
{
    public class ColorViewModel : BaseDto<ColorViewModel, Color>
    {

        [MaxLength(100)] 
        public string Title { get; set; }
        [MaxLength(100)]
        public string HexCode { get; set; }

    }
}
