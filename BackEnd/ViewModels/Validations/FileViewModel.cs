using BaseCore.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Validations
{
    public class FileViewModel : BaseDto<FileViewModel,Core.Entities.Base.File>
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }


    }
}
