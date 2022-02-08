using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using Pdf.Api.External;
using Pdf.Api.External.Dto;

namespace Pdf.Api.Infrastructure
{
    internal sealed class JobStatusChecker
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly ILogger<JobStatusChecker> _logger;

        private readonly HashSet<String> _processedJobs;
        private readonly IPdfApiExternalClient _pdfApiExternalClient;

        public JobStatusChecker(IPdfApiExternalClient pdfApiExternalClient, ILogger<JobStatusChecker> logger)
        {
            _processedJobs = new HashSet<string>();
            _pdfApiExternalClient = pdfApiExternalClient;

            _tokenSource = new CancellationTokenSource();
            _logger = logger;
        }

        public async Task StartCheckJob(string jobId, Func<Task> onSuccess)
        {
            var info = new JobInfo(jobId, onSuccess);
            if (_processedJobs.Contains(jobId))
            {
                throw new InvalidOperationException($"Job {jobId} is being processed right now");
            }

            ProcessJob(info, _tokenSource.Token);
        }

        private async Task ProcessJob(JobInfo jobInfo, CancellationToken token) 
        {
            while(!token.IsCancellationRequested)
            {
                var response = await _pdfApiExternalClient.CheckJob(jobInfo.JobId);

                if (response.Status == CheckJobStatus.Working)
                {
                    await Task.Delay(3000);
                }
                else if (response.Status == CheckJobStatus.Success)
                {
                    await jobInfo.OnSuccess();

                    _processedJobs.Remove(jobInfo.JobId);
                    break;
                }
                else if (response.Status == CheckJobStatus.Failed
                    || response.Status == CheckJobStatus.Aborted
                    || response.Status == CheckJobStatus.Unknown)
                {
                    _logger.LogError($"Received status {response.Status} for job {jobInfo.JobId}");

                    _processedJobs.Remove(jobInfo.JobId);
                    break;
                }
            }
        }

        private class JobInfo
        {
            public readonly string JobId;
            public readonly Func<Task> OnSuccess;

            public JobInfo(string jobId, Func<Task> onSuccess)
            {
                JobId = jobId;
                OnSuccess = onSuccess;
            }
        }
    }
}