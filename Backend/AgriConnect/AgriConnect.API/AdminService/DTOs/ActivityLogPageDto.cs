
using System.Collections.Generic;

namespace AdminService.DTOs
{
    public class ActivityLogPageDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public string SelectedRole { get; set; } = "";

        public List<ActivityLogEntryDto> Entries
        { get; set; } = new();
    }
}