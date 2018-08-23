using MyAnimeWorld.Common.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Common.Admin.BindingModels
{
    public class AddAnimeBindingModel
    {
        [Required(ErrorMessage = ErrorConstants.Invalid_Minimum_Title_Length)]
        [MinLength(NumericConstants.Minimum_Title_Length,ErrorMessage = ErrorConstants.Invalid_Minimum_Title_Length)]
        [MaxLength(NumericConstants.Maximum_Title_Length, ErrorMessage = ErrorConstants.Invalid_Maximum_Title_Length)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorConstants.Invalid_Minimum_Description_Length)]
        [MinLength(NumericConstants.Minimum_Description_Length,ErrorMessage = ErrorConstants.Invalid_Minimum_Description_Length)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorConstants.Invalid_Image_Type)]
        [DataType(DataType.ImageUrl, ErrorMessage = ErrorConstants.Invalid_Image_Type)]
        public string ImageUrl { get; set; }

        public List<int> CategoriesIds { get; set; }
    }
}
