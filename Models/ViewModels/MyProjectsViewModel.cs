using System.Collections.Generic;

namespace StatusTracker.Models.ViewModels
{
    public class MyProjectsViewModel
    {
        public ICollection<Project> Projects { get; set; }

        public STUser User { get; set; }


    }
}
