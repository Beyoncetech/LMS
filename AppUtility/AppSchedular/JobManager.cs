using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AppUtility.AppSchedular
{
    public class JobManager
    {
        /// <summary>
        /// Execute all Jobs.
        /// </summary>
        public void ExecuteAllJobs()
        {
            try
            {
                // get all job implementations of this assembly.
                IEnumerable<Type> jobs = GetAllTypesImplementingInterface(typeof(AppJob));
                // execute each job
                if (jobs != null && jobs.Count() > 0)
                {
                    AppJob instanceJob = null;
                    Thread thread = null;
                    foreach (Type job in jobs)
                    {
                        // only instantiate the job its implementation is "real"
                        if (IsRealClass(job))
                        {
                            try
                            {
                                // instantiate job by reflection
                                instanceJob = (AppJob)Activator.CreateInstance(job);
                                //instanceJob.WriteLog(0, String.Format("The Job \"{0}\" has been instantiated successfully.", instanceJob.GetName()));
                                if (instanceJob.IsActive())
                                {
                                    thread = new Thread(new ThreadStart(instanceJob.ExecuteJob));
                                    // start thread executing the job
                                    thread.Start();
                                }
                                //instanceJob.WriteLog(0, String.Format("The Job \"{0}\" has its thread started successfully.", instanceJob.GetName()));
                            }
                            catch (Exception)
                            {
                                //if (instanceJob != null)
                                //    instanceJob.WriteLog(-100, ex.Message);

                            }
                        }
                        else
                        {
                            //instanceJob.WriteLog(0, String.Format("The Job \"{0}\" cannot be instantiated.", instanceJob.GetName()));                            
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log.Error("An error has occured while instantiating " +
                //  "or executing Jobs for the Scheduler Framework.", ex);
            }

            //log.Debug("End Method");
        }

        /// <summary>
        /// Returns all types in the current AppDomain implementing the interface or inheriting the type. 
        /// </summary>
        private IEnumerable<Type> GetAllTypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type));

        }

        /// <summary>
        /// Determine whether the object is real - non-abstract, non-generic-needed, non-interface class.
        /// </summary>
        /// <param name="testType">Type to be verified.</param>
        /// <returns>True in case the class is real, false otherwise.</returns>
        public static bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }
    }
}
