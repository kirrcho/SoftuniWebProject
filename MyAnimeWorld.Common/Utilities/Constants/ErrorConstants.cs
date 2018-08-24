using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Utilities.Constants
{
    public class ErrorConstants
    {
        public const string Error_Key = "Errors";

        public const string Invalid_Search_Term = "Invalid search input!";

        public const string Invalid_Comment = "Please write a comment with at least 5 symbols.";

        public const string Uanuthorized_Comment_Attempt = "In order to comment you have to log in first.";

        public const string Uanuthorized_Favourite_Add_Attempt = "In order to add to your favourites list you have to log in first.";

        public const string Invalid_Category_Selection = "You must select at least one of the provided categories.";

        public const string Invalid_Title_Already_Exists = "This anime already exists in our database.";

        public const string Invalid_Links_Selection = "Please upload at least one link in order to be able to watch the episode.";

        public const string Invalid_Minimum_Title_Length = "Title must contain at least 3 symbols.";

        public const string Invalid_Maximum_Title_Length = "Title cannot be longer than 35 symbols";

        public const string Invalid_Minimum_Description_Length = "Please write a short description.";

        public const string Invalid_Image_Type = "Please insert a valid image.";

        public const string Invalid_Date = "Please select a valid date.";

        public const string Invalid_Banned_User_Login_Attempt = "User is banned!";

        public const string Invalid_Link_Deletion = "Episodes must have at least 1 link!";

        public const string Invalid_Minimum_Complaint_Name = "Name must be at least 3 symbols.";

        public const string Invalid_Maximum_Complaint_Name = "Name cannot be longer than 30 symbols.";

        public const string Invalid_Minimum_Complaint_Message = "Message must be at least 5 symbols.";
                                                      
        public const string Invalid_Maximum_Complaint_Message = "Message cannot be longer than 350 symbols.";

        public const string Invalid_Subject_Selection = "Please select a subject.";

        public const string Invalid_Episode_Deletion = "In order to delete this episode you have to erase the episodes after it.";
    }
}
