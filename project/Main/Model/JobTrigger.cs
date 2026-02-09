namespace Main.Model
{
	using System.Collections.Generic;
	using System.Linq;

	using Quartz;

	public class JobTrigger
	{
		public string Name;
		public string Group;
		public string Description;
		public string NextFireTime;
		public string TriggerState;
		public bool IsPaused;

		public JobTrigger(ITrigger trigger, IScheduler scheduler)
		{
			Name = trigger.Key.Name;
			Group = trigger.Key.Group;
			Description = trigger.Description;
			NextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().GetValueOrDefault().ToString() : "N/A";
			TriggerState = scheduler.GetTriggerState(trigger.Key).Result.ToString();
			IsPaused = TriggerState == "Paused" ? true : false;
		}
	}

	public class JobTriggerMatch
	{
		public IJobDetail Job;
		public List<JobTrigger> Triggers;

		public JobTriggerMatch(IJobDetail job, IScheduler scheduler)
		{
			Job = job;
			Triggers = scheduler.GetTriggersOfJob(job.Key).Result.Select(x => new JobTrigger(x, scheduler)).ToList();
		}
	}
}
