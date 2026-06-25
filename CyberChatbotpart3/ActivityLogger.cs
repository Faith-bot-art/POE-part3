using System;
using System.Collections.Generic;

namespace CyberbotPart3
{
    public class ActivityLogger
    {
        private List<string> activityLog;
        private int maxSize;

        public ActivityLogger()
        {
            activityLog = new List<string>();
            maxSize = 50;
        }

        public void Log(string action, string details)
        {
            string entry = string.Format("[{0}] {1}: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), action, details);
            activityLog.Insert(0, entry);

            while (activityLog.Count > maxSize)
                activityLog.RemoveAt(activityLog.Count - 1);
        }

        public List<string> GetRecentLogs(int count)
        {
            int take = Math.Min(count, activityLog.Count);
            return activityLog.GetRange(0, take);
        }

        public List<string> GetAllLogs() => new List<string>(activityLog);

        public void Clear() { activityLog.Clear(); Log("Log cleared", "Activity log was reset"); }

        public string GetSummary()
        {
            int tasks = 0, quizzes = 0, reminders = 0;
            foreach (var log in activityLog)
            {
                if (log.Contains("Task added")) tasks++;
                if (log.Contains("Quiz")) quizzes++;
                if (log.Contains("Reminder")) reminders++;
            }
            return string.Format("Summary: {0} tasks, {1} quizzes, {2} reminders", tasks, quizzes, reminders);
        }
    }
}
