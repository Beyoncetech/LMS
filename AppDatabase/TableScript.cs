using System;
using System.Collections.Generic;
using System.Text;

namespace AppDatabase
{
    public interface BaseScript
    {
        string GetScript();
    }
    public class DBCreate : BaseScript
    {
        private string DBName = "";
        private string SqlScript = @"CREATE DATABASE {0}
	                               CHARACTER SET latin1
	                               COLLATE latin1_swedish_ci;";
        public DBCreate(string DB_Name)
        {
            DBName = DB_Name;
        }
        public string GetScript()
        {
            return string.Format(SqlScript, DBName);
        }
    }
    public class TableAppUser : BaseScript
    {
        private string DBName = "";
        private string SqlScript = @"CREATE TABLE {0}.appuser (
                                    ID BIGINT(20) NOT NULL AUTO_INCREMENT,
                                    Name VARCHAR(100) DEFAULT NULL,
                                    UserType VARCHAR(10) NOT NULL DEFAULT '',
                                    UserName VARCHAR(100) NOT NULL DEFAULT '',
                                    Email VARCHAR(50) DEFAULT NULL,
                                    Mobile VARCHAR(255) DEFAULT NULL,
                                    Password VARCHAR(100) NOT NULL DEFAULT '',
                                    UserPerm VARCHAR(2000) DEFAULT NULL,
                                    IsPassReset BIT(1) NOT NULL DEFAULT b'0',
                                    IsActive BIT(1) NOT NULL DEFAULT b'0',
                                    DOB DATE DEFAULT NULL,
                                    CustomData VARCHAR(4000) DEFAULT NULL,
                                    PRIMARY KEY (ID)
                                    )
                                    ENGINE = INNODB,
                                    AUTO_INCREMENT = 2,
                                    CHARACTER SET latin1,
                                    COLLATE latin1_swedish_ci;";
        public TableAppUser(string DB_Name)
        {
            DBName = DB_Name;
        }
        public string GetScript()
        {
            return string.Format(SqlScript, DBName);
        }
    }
    
}
