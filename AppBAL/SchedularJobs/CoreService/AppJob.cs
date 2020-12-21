using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppBAL.CoreJobService
{
    public abstract class AppJob
    {
        /// <summary>
        /// Execute the Job itself, one ore repeatedly, depending on
        /// the job implementation.
        /// </summary>
        public async void ExecuteJob()
        {
            OnBeforeJobExecute();
            if (IsRepeatable())
            {
                // execute the job in intervals determined by the methd
                // GetRepetionIntervalTime()
                while (true)
                {
                    try
                    {
                        WriteLog(0, String.Format("The Job \"{0}\" has been started successfully. [{1}]", GetName(), DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                        await DoJob().ConfigureAwait(false);
                        WriteLog(0, String.Format("The Job \"{0}\" has been finished successfully. [{1}]", GetName(), DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                    }
                    catch (Exception ex)
                    {
                        WriteLog(-100, String.Format("The Job \"{0}\" has thrown exception : {1}. [{2}]", GetName(), ex.Message, DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                    }
                    Thread.Sleep(GetRepetitionIntervalTime());
                }
            }
            // since there is no repetetion, simply execute the job
            else
            {
                try
                {
                    WriteLog(0, String.Format("The Job \"{0}\" has been started successfully. [{1}]", GetName(), DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                    await DoJob().ConfigureAwait(false);
                    WriteLog(0, String.Format("The Job \"{0}\" has been finished successfully. [{1}]", GetName(), DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                }
                catch (Exception ex)
                {
                    WriteLog(-100, String.Format("The Job \"{0}\" has thrown exception : {1}. [{2}]", GetName(), ex.Message, DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
                }
            }
        }

        public void LogError(Exception ex)
        {            
            string ServerLogPath = ErrorLogFileName();
            if (!string.IsNullOrEmpty(ServerLogPath))
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                //string path = Server.MapPath("~/ErrorLog/ErrorLog.txt");
                using (StreamWriter writer = new StreamWriter(ServerLogPath, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// If this method is overriden, on can get within the job
        /// parameters set just before the job is started. In this
        /// situation the application is running and the use may have
        /// access to resources which he/she has not during the thread
        /// execution. For instance, in a web application, the user has
        /// no access to the application context, when the thread is running.
        /// Note that this method must not be overriden. It is optional.
        /// </summary>
        /// <returns>Parameters to be used in the job.</returns>
        public virtual Object GetParameters()
        {
            return null;
        }

        /// <summary>
        /// Get the Job´s Name. This name uniquely identifies the Job.
        /// </summary>
        /// <returns>Job´s name.</returns>
        public abstract String GetName();

        /// <summary>
        /// The job to be executed.
        /// </summary>
        public abstract Task DoJob();

        /// <summary>
        /// The pre init job to be executed.
        /// </summary>
        public abstract void OnBeforeJobExecute();

        /// <summary>
        /// Determines whether a Job is to be repeated after a
        /// certain amount of time.
        /// </summary>
        /// <returns>True in case the Job is to be repeated, false otherwise.</returns>
        public abstract bool IsRepeatable();

        /// <summary>
        /// Determines whether a Job is 
        /// currently active.
        /// </summary>
        /// <returns>True in case the Job is active, false otherwise.</returns>
        public abstract bool IsActive();

        /// <summary>
        /// The amount of time, in milliseconds, which the Job has to wait until it is started
        /// over. This method is only useful if IJob.IsRepeatable() is true, otherwise
        /// its implementation is ignored.
        /// </summary>
        /// <returns>Interval time between this job executions.</returns>
        public abstract int GetRepetitionIntervalTime();

        /// <summary>
        /// report log        
        /// </summary>
        /// <returns>return nothing.</returns>
        public abstract void WriteLog(int MsgType, string Msg);

        
        /// <summary>
        /// get the error log file name with path        
        /// </summary>
        /// <returns>True in case the Job is active, false otherwise.</returns>
        public abstract string ErrorLogFileName();
    }
}
