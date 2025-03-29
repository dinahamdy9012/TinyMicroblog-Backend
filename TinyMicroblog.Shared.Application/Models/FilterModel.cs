using System.ComponentModel;

namespace TinyMicroblog.Shared.Application.Models
{
    public class FilterModel
    {
        [DefaultValue(1)]
        public int PageIndex { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
        public string? SearchValue { get; set; }

        public int GetPage()
        {
            int page = (PageIndex - 1) * PageSize;
            if (page < 0)
            {
                page = 0;
            }
            return page;
        }
    }
}
