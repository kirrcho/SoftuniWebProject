using MyAnimeWorld.Common.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAnimeWorld.Common.Main.BindingModels
{
    public class ComplaintBindingModel
    {
        [Required]
        [MinLength(NumericConstants.Minimum_Complaint_Name_Length, ErrorMessage = ErrorConstants.Invalid_Minimum_Complaint_Name)]
        [MaxLength(NumericConstants.Maximum_Complaint_Name_Length, ErrorMessage = ErrorConstants.Invalid_Maximum_Complaint_Name)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [MinLength(NumericConstants.Minimum_Complaint_Message_Length,ErrorMessage = ErrorConstants.Invalid_Minimum_Complaint_Message)]
        [MaxLength(NumericConstants.Maximum_Complaint_Message_Length,ErrorMessage = ErrorConstants.Invalid_Maximum_Complaint_Message)]
        public string Message { get; set; }
    }
}
