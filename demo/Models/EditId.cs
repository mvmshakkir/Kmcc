using demo.Areas.Identity.Data;

namespace demo.Models
{
    public class EditId
    {
        public string userid { get; set; }
		public List<Ward> Ward { get; set; } = new List<Ward>();
		public List<Terms> Terms { get; set; } = new List<Terms>();
		public List<ListCountrie> ListCountrie { get; set; } = new List<ListCountrie>();

		



	}
}
