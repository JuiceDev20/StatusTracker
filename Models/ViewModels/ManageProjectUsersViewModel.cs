using Microsoft.AspNetCore.Mvc.Rendering;

namespace StatusTracker.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }

        public MultiSelectList Users { get; set; }

        public string[] SelectedUsers { get; set; }

    }
}
