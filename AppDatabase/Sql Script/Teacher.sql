﻿CREATE TABLE lmsdb.tblmteacher (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(50) NOT NULL DEFAULT '',
  RegNo VARCHAR(25) NOT NULL DEFAULT '',
  Address VARCHAR(500) DEFAULT NULL,
  ContactNo VARCHAR(50) NOT NULL DEFAULT '',
  Email VARCHAR(50) NOT NULL DEFAULT '',
  EducationalQualification VARCHAR(500) DEFAULT NULL,
  CreatedOn DATE DEFAULT NULL,
  CreatdBy INT(11) DEFAULT NULL,
  LoginUserId BIGINT(20) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;


CREATE TABLE lmsdb.tblrteacherclassroom (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  TeacherID INT(11) NOT NULL,
  ClassRoomID INT(11) NOT NULL,
  CreatedOn DATE NOT NULL,
  ClosedOn DATE DEFAULT NULL,
  CreatedBy INT(11) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;