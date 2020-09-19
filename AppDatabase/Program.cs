using System;

namespace AppDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            string DatabaseName = "TempDatabase";
            var scrptExecuter = new ScriptExecuter(DatabaseName);

            BaseScript dbscript = new DBCreate(DatabaseName);
            scrptExecuter.ExecuteScript(dbscript);

        }
    }
}
