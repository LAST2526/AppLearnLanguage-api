using System;

namespace Last02.Models
{
    public class PagerViewModel
    {
        public PagerViewModel(string action, int page, int size)
        {
            Action = action;
            Page = page;
            Size = size;
        }

        public string Action { get; set; }

        public string? Search { get; set; }
        public string? OrderField { get; set; }

        public bool OrderSort { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 30;
        public int DisplayPage { get; set; } = 5;
        public int TotalItem { get; set; }
        public int TotalItemInPage { get; set; }
    }
}
