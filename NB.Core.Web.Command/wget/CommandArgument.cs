using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wget
{
    public enum JobType
    {
        None,
        EarningCalendar,
        FairValue
    }

    public class CommandArgument
    {
        // -e earning
        // -f file download
        // -a analysis
       private readonly CommandLineArgumentsParser _parser;
        private const string JobTypeSetting = "j";
        private const string TargetFileSetting = "t";

        private readonly string _targetFile;
        private readonly string _jobTypeStr;
        public CommandArgument(CommandLineArgumentsParser parser)
        {
            _parser = parser;
            _targetFile = GetValueOrDefault(_parser[TargetFileSetting], "tickerInfo.txt");
            _jobTypeStr = _parser[JobTypeSetting];
        }

        private string GetValueOrDefault (string field, string defaultVal)
        {
            return _parser[field] ?? defaultVal;
        }

        public JobType[] JobTypes
        {
            get
            {
                var jobs = new List<JobType>();
                var jobsStr = _jobTypeStr.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var job in jobsStr)
                {
                    JobType test;
                    var isJob  = Enum.TryParse(job, true, out test);
                    if (isJob)
                    {
                        jobs.Add(test);
                    }
                }
                return jobs.ToArray();
            }
        }

        public string TargetFile
        {
            get { return _targetFile; }
        }
    }
    }
}
