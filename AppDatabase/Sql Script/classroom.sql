﻿CREATE TABLE lmsdb.tblmclassroom (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  RefID VARCHAR(100) NOT NULL DEFAULT '',
  Name VARCHAR(100) NOT NULL DEFAULT '',
  Description VARCHAR(250) DEFAULT NULL,
  SubjectID INT(11) NOT NULL,
  StandardID INT(11) NOT NULL,
  ClassActivationThreshold INT(11) DEFAULT NULL,
  Scheduler VARCHAR(4000) NOT NULL DEFAULT '',
  CreatedOn DATE NOT NULL,
  CreatedBy VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;