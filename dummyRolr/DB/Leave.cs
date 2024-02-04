namespace dummyRolr.DB
{
	public class Leave
	{
		public int Id { get; set; }
		public string UserEmail { get; set; }
		public int LeaveTypeId { get; set; }

		public string Reason { get; set; }
		public DateTime AppliedDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }


	}
}
