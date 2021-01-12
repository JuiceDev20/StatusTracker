using Microsoft.AspNetCore.Mvc.Rendering;

namespace StatusTracker.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public STUser User { get; set; }

        public MultiSelectList Roles { get; set; }

        public string[] SelectedRoles { get; set; }

    }
}
