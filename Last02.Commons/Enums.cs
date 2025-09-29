using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons.Extensions;

namespace Last02.Commons
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 0,
        [Display(Name = "Female")]
        Female = 1
    }

    public enum Language
    {
        [Display(Name = "Vietnamese (Tiếng Việt)")]
        [LanguageCode("vi")]
        Vietnamese,
        [Display(Name = "English (Tiếng Anh)")]
        [LanguageCode("en")]
        English
    }

    public enum Provider
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Google")]
        Google = 1,
        [Display(Name = "Facebook")]
        Facebook = 2
    }

    public enum FlashcardStatus
    {
        New = 0,
        InProgress = 1,
        Completed = 2
    }

    public enum FlashcardAction
    {
        Forgot,
        Remembered
    }

    public enum CourseLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }

    public enum AudioType
    {
        Grammar = 0,
        Kaiwa = 1
    }
}
