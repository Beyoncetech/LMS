﻿--
-- Script was generated by Devart dbForge Studio 2020 for MySQL, Version 9.0.304.0
-- Product home page: http://www.devart.com/dbforge/mysql/studio
-- Script date 12/20/2020 12:03:08 AM
-- Server version: 10.4.13
-- Client version: 4.1
--

-- 
-- Disable foreign keys
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Set SQL mode
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

--
-- Set default database
--
USE lmsdb;

--
-- Drop table `activitylog`
--
DROP TABLE IF EXISTS activitylog;

--
-- Drop table `appsetting`
--
DROP TABLE IF EXISTS appsetting;

--
-- Drop table `appuser`
--
DROP TABLE IF EXISTS appuser;

--
-- Drop table `mjob`
--
DROP TABLE IF EXISTS mjob;

--
-- Drop table `tblmclassroom`
--
DROP TABLE IF EXISTS tblmclassroom;

--
-- Drop table `tblmstandard`
--
DROP TABLE IF EXISTS tblmstandard;

--
-- Drop table `tblmstudent`
--
DROP TABLE IF EXISTS tblmstudent;

--
-- Drop table `tblmsubject`
--
DROP TABLE IF EXISTS tblmsubject;

--
-- Drop table `tblmteacher`
--
DROP TABLE IF EXISTS tblmteacher;

--
-- Drop table `tblrstudentclassroom`
--
DROP TABLE IF EXISTS tblrstudentclassroom;

--
-- Drop table `tblrteacherclassroom`
--
DROP TABLE IF EXISTS tblrteacherclassroom;

--
-- Set default database
--
USE lmsdb;

--
-- Create table `tblrteacherclassroom`
--
CREATE TABLE tblrteacherclassroom (
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

--
-- Create table `tblrstudentclassroom`
--
CREATE TABLE tblrstudentclassroom (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  StudentID INT(11) NOT NULL,
  ClassRoomID INT(11) NOT NULL,
  CreatedOn DATE NOT NULL,
  ClosedOn DATE DEFAULT NULL,
  CreatedBy VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `tblmteacher`
--
CREATE TABLE tblmteacher (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(50) NOT NULL DEFAULT '',
  Address VARCHAR(500) DEFAULT NULL,
  ContactNo VARCHAR(50) NOT NULL DEFAULT '',
  Email VARCHAR(50) NOT NULL DEFAULT '',
  EducationalQualification VARCHAR(500) DEFAULT NULL,
  CreatedOn DATE DEFAULT NULL,
  CreatdBy INT(11) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `tblmsubject`
--
CREATE TABLE tblmsubject (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(100) NOT NULL DEFAULT '',
  CreatedOn DATE NOT NULL,
  CreatedBy INT(11) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `tblmstudent`
--
CREATE TABLE tblmstudent (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(50) NOT NULL DEFAULT '',
  RegNo INT(11) NOT NULL,
  Address VARCHAR(255) DEFAULT NULL,
  ContactNo VARCHAR(50) NOT NULL DEFAULT '',
  Email VARCHAR(50) NOT NULL DEFAULT '',
  StandardID INT(11) NOT NULL,
  CreatedOn DATE NOT NULL,
  CreatedBy INT(11) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `tblmstandard`
--
CREATE TABLE tblmstandard (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(50) DEFAULT NULL,
  CreatedOn DATE NOT NULL,
  CreatedBy VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
AUTO_INCREMENT = 3,
AVG_ROW_LENGTH = 16384,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `tblmclassroom`
--
CREATE TABLE tblmclassroom (
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

--
-- Create table `mjob`
--
CREATE TABLE mjob (
  JobId BIGINT(20) NOT NULL AUTO_INCREMENT,
  RefNo VARCHAR(50) NOT NULL DEFAULT '',
  Command VARCHAR(25) NOT NULL DEFAULT '',
  CommandData VARCHAR(5000) DEFAULT NULL,
  Priority VARCHAR(2) NOT NULL DEFAULT '',
  CreatedOn DATETIME DEFAULT NULL,
  CreatedBy BIGINT(20) DEFAULT NULL,
  FinishedOn DATETIME DEFAULT NULL,
  ErrorCode VARCHAR(10) DEFAULT NULL,
  ErrorMsg VARCHAR(255) DEFAULT NULL,
  Status TINYINT(4) DEFAULT NULL,
  ValidFrom DATETIME DEFAULT NULL,
  ValidTo DATETIME DEFAULT NULL,
  PRIMARY KEY (JobId)
)
ENGINE = INNODB,
AUTO_INCREMENT = 8,
AVG_ROW_LENGTH = 2340,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `appuser`
--
CREATE TABLE appuser (
  ID BIGINT(20) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(100) DEFAULT NULL,
  UserType VARCHAR(10) NOT NULL DEFAULT '',
  UserID VARCHAR(100) NOT NULL DEFAULT '',
  Email VARCHAR(50) DEFAULT NULL,
  Mobile VARCHAR(255) DEFAULT NULL,
  Password VARCHAR(100) NOT NULL DEFAULT '',
  UserPerm VARCHAR(2000) DEFAULT NULL,
  IsPassReset BIT(1) NOT NULL DEFAULT b'0',
  ResetPassValidity DATE DEFAULT NULL,
  ResetPassContext VARCHAR(255) DEFAULT NULL,
  IsActive BIT(1) NOT NULL DEFAULT b'0',
  DOB DATE DEFAULT NULL,
  CustomData VARCHAR(4000) DEFAULT NULL,
  Gender VARCHAR(1) DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
AUTO_INCREMENT = 16,
AVG_ROW_LENGTH = 3276,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `appsetting`
--
CREATE TABLE appsetting (
  ID INT(11) NOT NULL AUTO_INCREMENT,
  AppKey VARCHAR(255) NOT NULL DEFAULT '',
  AppVal VARCHAR(5000) NOT NULL DEFAULT '',
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
AUTO_INCREMENT = 17,
AVG_ROW_LENGTH = 8192,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `activitylog`
--
CREATE TABLE activitylog (
  ID BIGINT(20) NOT NULL AUTO_INCREMENT,
  ActivityType TINYINT(4) NOT NULL,
  ActivityTime DATETIME NOT NULL,
  UserID VARCHAR(100) NOT NULL DEFAULT '',
  UserName VARCHAR(255) NOT NULL DEFAULT '',
  Origin VARCHAR(255) NOT NULL DEFAULT '',
  Description VARCHAR(255) DEFAULT NULL,
  IsRead TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (ID)
)
ENGINE = INNODB,
AUTO_INCREMENT = 88,
AVG_ROW_LENGTH = 188,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

-- 
-- Dumping data for table tblrteacherclassroom
--
-- Table lmsdb.tblrteacherclassroom does not contain any data (it is empty)

-- 
-- Dumping data for table tblrstudentclassroom
--
-- Table lmsdb.tblrstudentclassroom does not contain any data (it is empty)

-- 
-- Dumping data for table tblmteacher
--
-- Table lmsdb.tblmteacher does not contain any data (it is empty)

-- 
-- Dumping data for table tblmsubject
--
-- Table lmsdb.tblmsubject does not contain any data (it is empty)

-- 
-- Dumping data for table tblmstudent
--
-- Table lmsdb.tblmstudent does not contain any data (it is empty)

-- 
-- Dumping data for table tblmstandard
--
INSERT INTO tblmstandard VALUES
(2, 'class 5', '0001-01-01', NULL);

-- 
-- Dumping data for table tblmclassroom
--
-- Table lmsdb.tblmclassroom does not contain any data (it is empty)

-- 
-- Dumping data for table mjob
--
INSERT INTO mjob VALUES
(1, '8bc36994-7e78-4de5-a567-85c1f9a18ba4', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>debabrata.dutta@gmail.com</string></To><Subject>User: [sudipta] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src='''' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;sudipta&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''Account/ResetUserPass/1d372b252a574979a072574a88c60d47''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-11-06 23:56:55', 0, '2020-12-19 21:44:24', 'E05', 'Job is expired', -1, '2020-11-06 23:56:55', '2020-11-07 23:56:55'),
(2, '0a6255e9-ce3a-49a7-be0f-694135952133', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>d@dp.com</string></To><Subject>User: [shamal] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src='''' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;shamal&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''Account/ResetUserPass/ba8b7d282ee444b6b86639c772b98654''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-11-07 00:14:11', 2, '2020-12-19 21:44:29', 'E05', 'Job is expired', -1, '2020-11-07 00:14:11', '2020-11-08 00:14:11'),
(3, '00f80b28-53f7-4e8a-85a1-054d971d44bd', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>d@gm.in</string></To><Subject>User: [sonali] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src=''D:\\Final Projects\\LMS\\BTWebFrameWorkCore\\wwwroot/img/Comp_logo.png'' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;sonali&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''D:\\Final Projects\\LMS\\BTWebFrameWorkCore\\wwwroot/Account/ResetUserPass/d68ba373aa2447628108d8e76a72742f''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-11-07 00:39:25', 2, '2020-12-19 21:44:29', 'E05', 'Job is expired', -1, '2020-11-07 00:39:25', '2020-11-08 00:39:25'),
(4, 'ae7955a2-111a-419d-be78-8d63a074f3c3', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>d@dp.com</string></To><Subject>User: [kunal] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/img/Comp_logo.png'' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;kunal&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/Account/ResetUserPass/38e21cc857f645618ecc82ba3b5689ed''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-12-10 10:48:21', 2, '2020-12-19 21:44:29', 'E05', 'Job is expired', -1, '2020-12-10 10:48:21', '2020-12-11 10:48:21'),
(5, 'be555f4e-1996-440b-8fdd-9c93dc63f63a', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>debintheweb@gmail.com</string></To><Subject>User: [kunal] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/img/Comp_logo.png'' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;kunal&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/Account/ResetUserPass/1da093542ca548468f826c2f8fa3a3ae''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-12-19 21:48:25', 2, '2020-12-19 21:48:44', NULL, NULL, 1, '2020-12-19 21:48:25', '2020-12-20 21:48:25'),
(6, 'ae8d6d57-c283-4985-afba-fa56a3046445', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>debintheweb@gmail.com</string></To><Subject>User: [sourav] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/img/Comp_logo.png'' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;sourav&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''D:\\Final Projects\\LMS_Project\\LMS\\BTWebFrameWorkCore\\wwwroot/Account/ResetUserPass/446e3ee68e504752a94f4411d9cd2428''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-12-19 22:29:33', 2, '2020-12-19 22:30:09', NULL, NULL, 1, '2020-12-19 22:29:33', '2020-12-20 22:29:33'),
(7, '4d605e71-976f-477f-af78-5b65f4d5af68', 'ScheduleMail', '<ScheduleEmailInfoBM><To><string>debintheweb@gmail.com</string></To><Subject>User: [tarit] has been created.</Subject><MailBody>&lt;h1&gt;&lt;img src=''localhost:6941/img/Comp_logo.png'' alt=''...'' /&gt;&amp;nbsp;&lt;/h1&gt;\r\n                            &lt;h2&gt;Hi&amp;nbsp;&amp;nbsp;tarit&lt;/h2&gt;\r\n                            &lt;h2&gt;&lt;span style=''background-color: #00ff00;''&gt;Your Login is created by Admin.&lt;/span&gt;&amp;nbsp;&lt;/h2&gt;\r\n                            Click the below link to reset your password. This link will be active for 1 day. &lt;br /&gt;&lt;br /&gt;\r\n                            &lt;a href=''localhost:6941/Account/ResetUserPass/86be9a0a73af4014b0c2957bec75f0b8''&gt;Click here to Reset your Password&lt;/a&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;\r\n                            &lt;p&gt;&lt;strong&gt;Send by Beyoncetech Team&lt;/strong&gt;&lt;/p&gt;\r\n                            &lt;p&gt;&amp;nbsp;&lt;/p&gt;</MailBody></ScheduleEmailInfoBM>', '1', '2020-12-19 23:12:40', 2, '2020-12-19 23:13:17', NULL, NULL, 1, '2020-12-19 23:12:40', '2020-12-20 23:12:40');

-- 
-- Dumping data for table appuser
--
INSERT INTO appuser VALUES
(2, 'deb', 'A', 'dd', 'debabrata.dutta@gmail.com', '5646233338', '123', NULL, False, NULL, NULL, True, '2020-10-22', NULL, 'M'),
(12, 'kunal', 'S', 'kk', 'd@dp.com', NULL, '123', NULL, True, '2020-12-11', '4737df72RP6e07RP4324RP8cb7RP6af0b87e1eb7', True, NULL, NULL, 'M'),
(13, 'kunal', 'S', 'pth', 'debintheweb@gmail.com', '5646233456', '123', NULL, True, '2020-12-20', 'c6c33f1dRPb8c9RP4dd0RP9347RP1dc22c3f7585', True, NULL, NULL, 'M'),
(14, 'sourav', 'S', 'sd', 'debintheweb@gmail.com', '5646233338', '123', NULL, True, '2020-12-20', '46299bdaRPbcddRP43efRP9fddRPc35a15da9413', True, NULL, NULL, 'M'),
(15, 'tarit', 'A', 'tr', 'debintheweb@gmail.com', '5646233338', '123', NULL, True, '2020-12-20', '1e02abdaRPa219RP4f08RPb349RPb67c7e313511', True, NULL, NULL, 'M');

-- 
-- Dumping data for table appsetting
--
INSERT INTO appsetting VALUES
(14, 'App-GeneralSetup', '<GeneralSettingBM><ClassActiveThresholdTime>70</ClassActiveThresholdTime><SupportMailID>deb@gmail.com</SupportMailID></GeneralSettingBM>'),
(16, 'Mail-AppMailSetup', '<MailSettingBM><FromMailID>contact@beyoncetechsolutions.com</FromMailID><SmtpServer>mail.beyoncetechsolutions.com</SmtpServer><MailUserID>contact@beyoncetechsolutions.com</MailUserID><MailPassword>OyyQ!w{#{vG3</MailPassword><SmtpServerPort>465</SmtpServerPort><MailPasswordEx>************</MailPasswordEx></MailSettingBM>');

-- 
-- Dumping data for table activitylog
--
INSERT INTO activitylog VALUES
(1, 2, '2020-10-08 18:43:04', 'dd', 'deb', 'User Profile', 'Profile deb has updated', 1),
(2, 2, '2020-10-09 04:48:14', 'ss', 'Super', 'User Profile', 'Profile Super has updated', 1),
(3, 2, '2020-10-09 05:10:52', 'uu', 'User', 'User Profile', 'Updated user profile', 1),
(4, 2, '2020-10-09 05:11:49', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(5, 2, '2020-10-09 05:12:28', 'ss', 'Super', 'User Profile', 'Updated user profile', 1),
(6, 2, '2020-10-09 05:12:53', 'ss', 'Super', 'User Profile', 'Updated user profile', 1),
(7, 2, '2020-10-09 13:42:05', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(8, 2, '2020-10-09 17:02:02', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(9, 2, '2020-10-09 17:13:19', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(10, 2, '2020-10-09 17:14:13', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(11, 2, '2020-10-14 06:45:21', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(12, 2, '2020-10-14 10:31:12', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(13, 2, '2020-10-14 11:05:48', 'dd', 'deb', 'Change Password', 'Changed user password', 1),
(14, 2, '2020-10-14 11:48:30', 'dd', 'deb', 'Change Password', 'Changed user password', 1),
(15, 2, '2020-10-14 11:54:24', 'dd', 'deb', 'Change Password', 'Changed user password', 1),
(16, 2, '2020-10-14 11:55:02', 'dd', 'deb', 'Change Password', 'Changed user password', 1),
(17, 2, '2020-10-14 11:55:22', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(18, 2, '2020-10-14 11:55:29', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(19, 2, '2020-10-14 11:55:36', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(20, 2, '2020-10-14 12:01:13', 'dd', 'deb', 'Change Password', 'Changed user password', 1),
(21, 2, '2020-10-14 12:01:27', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(22, 2, '2020-10-14 12:07:31', 'uu', 'User', 'Change Password', 'Changed user password', 1),
(23, 2, '2020-10-17 13:57:11', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(24, 2, '2020-10-17 15:43:07', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(25, 2, '2020-10-17 17:34:58', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(26, 2, '2020-10-17 17:37:17', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(27, 2, '2020-10-17 17:38:23', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(28, 2, '2020-10-17 17:39:59', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(29, 2, '2020-10-17 17:41:51', 'dd', 'deb', 'Settings', 'Changed Settings', 1),
(30, 2, '2020-10-17 17:49:11', 'dd', 'deb', 'Settings', 'Changed App Settings', 1),
(31, 2, '2020-10-17 17:51:09', 'dd', 'deb', 'Settings', 'Changed App Settings', 1),
(32, 2, '2020-10-17 17:52:02', 'dd', 'deb', 'Settings', 'Changed Email Settings', 1),
(33, 2, '2020-10-29 14:39:35', 'dd', 'deb', 'User Profile', 'Updated user profile', 1),
(34, 2, '2020-10-30 12:07:16', 'dd', 'deb', 'Update User', 'Updated user : deb info.', 1),
(35, 2, '2020-10-30 12:09:17', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 1),
(36, 2, '2020-10-30 12:15:51', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(37, 2, '2020-10-30 12:16:17', 'dd', 'deb', 'Update User', 'Updated user : User info.', 0),
(38, 2, '2020-10-30 12:16:34', 'dd', 'deb', 'Update User', 'Updated user : User info.', 0),
(39, 2, '2020-10-30 12:17:12', 'dd', 'deb', 'Update User', 'Updated user : User info.', 0),
(40, 2, '2020-10-30 12:18:39', 'dd', 'deb', 'Update User', 'Updated user : User info.', 0),
(41, 2, '2020-10-30 12:24:58', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(42, 2, '2020-10-30 12:25:23', 'dd', 'deb', 'Update User', 'Updated user : User info.', 0),
(43, 2, '2020-10-30 12:30:25', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(44, 2, '2020-10-30 12:39:09', 'dd', 'deb', 'Update User', 'Updated user : deb info.', 0),
(45, 2, '2020-10-30 12:45:55', 'dd', 'deb', 'Update User', 'Updated user : deb info.', 0),
(46, 2, '2020-10-30 12:46:26', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(47, 1, '2020-10-30 12:47:16', 'dd', 'deb', 'Save User', 'Save new user : kunal info.', 0),
(48, 2, '2020-10-30 12:47:37', 'dd', 'deb', 'Update User', 'Updated user : kunal info.', 0),
(49, 1, '2020-10-30 12:48:22', 'dd', 'deb', 'Save User', 'Save new user : debdas info.', 0),
(50, 1, '2020-10-30 12:57:38', 'dd', 'deb', 'Save User', 'Save new user : sudipta info.', 0),
(51, 2, '2020-10-30 12:57:49', 'dd', 'deb', 'Update User', 'Updated user : kunal info.', 0),
(52, 2, '2020-10-30 12:57:59', 'dd', 'deb', 'Update User', 'Updated user : sudipta info.', 0),
(53, 2, '2020-10-30 12:58:11', 'dd', 'deb', 'Update User', 'Updated user : sudipta info.', 0),
(54, 2, '2020-10-30 13:43:02', 'dd', 'deb', 'Update User', 'Updated user : kunal info.', 0),
(55, 2, '2020-10-30 13:43:17', 'dd', 'deb', 'Update User', 'Updated user : kunal info.', 0),
(56, 1, '2020-10-30 14:18:57', 'dd', 'deb', 'Delete User', 'Deleted user : sudipta.', 0),
(57, 1, '2020-10-30 14:21:02', 'dd', 'deb', 'Delete User', 'Deleted user : debdas.', 0),
(58, 1, '2020-10-30 14:21:48', 'dd', 'deb', 'Save User', 'Save new user : kamak info.', 0),
(59, 2, '2020-10-30 14:22:06', 'dd', 'deb', 'Update User', 'Updated user : kamak info.', 0),
(60, 1, '2020-10-30 14:22:15', 'dd', 'deb', 'Delete User', 'Deleted user : kamak.', 0),
(61, 2, '2020-10-31 16:12:59', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(62, 2, '2020-10-31 18:09:07', 'dd', 'deb', 'User Profile', 'Updated user profile', 0),
(63, 2, '2020-10-31 18:09:23', 'dd', 'deb', 'Update User', 'Updated user : Super info.', 0),
(64, 1, '2020-11-06 18:26:59', 'dd', 'deb', 'Save User', 'Save new user : sudipta info.', 0),
(65, 1, '2020-11-06 18:44:15', 'dd', 'deb', 'Save User', 'Save new user : shamal info.', 0),
(66, 1, '2020-11-06 19:09:25', 'dd', 'deb', 'Save User', 'Save new user : sonali info.', 0),
(67, 1, '2020-11-06 19:11:03', 'dd', 'deb', 'Delete User', 'Deleted user : shamal.', 0),
(68, 1, '2020-11-06 19:11:10', 'dd', 'deb', 'Delete User', 'Deleted user : sonali.', 0),
(69, 2, '2020-11-28 12:03:47', 'dd', 'deb', 'Update User', 'Updated user : sudipta info.', 0),
(70, 1, '2020-11-28 12:03:56', 'dd', 'deb', 'Delete User', 'Deleted user : sudipta.', 0),
(71, 1, '2020-11-28 12:08:18', 'dd', 'deb', 'Delete User', 'Deleted user : Super.', 0),
(72, 1, '2020-11-28 20:12:11', 'dd', 'deb', 'Delete User', 'Deleted user : User.', 0),
(73, 2, '2020-12-10 05:16:38', 'dd', 'deb', 'Settings', 'Changed Email Settings', 0),
(74, 2, '2020-12-10 05:17:07', 'dd', 'deb', 'Settings', 'Changed App Settings', 0),
(75, 2, '2020-12-10 05:17:21', 'dd', 'deb', 'Settings', 'Changed Email Settings', 0),
(76, 1, '2020-12-10 05:18:21', 'dd', 'deb', 'Save User', 'Save new user : kunal info.', 0),
(77, 2, '2020-12-10 05:18:47', 'dd', 'deb', 'Update User', 'Updated user : kunal info.', 0),
(78, 2, '2020-12-19 16:15:35', 'dd', 'deb', 'Settings', 'Changed App Settings', 0),
(79, 2, '2020-12-19 16:16:13', 'dd', 'deb', 'Settings', 'Changed Email Settings', 0),
(80, 2, '2020-12-19 16:16:42', 'dd', 'deb', 'Settings', 'Changed Email Settings', 0),
(81, 1, '2020-12-19 16:18:25', 'dd', 'deb', 'Save User', 'Save new user : kunal info.', 0),
(82, 1, '2020-12-19 16:59:33', 'dd', 'deb', 'Save User', 'Save new user : sourav info.', 0),
(83, 1, '2020-12-19 17:42:41', 'dd', 'deb', 'Save User', 'Save new user : tarit info.', 0),
(84, 1, '2020-12-19 18:26:25', 'dd', 'deb', 'Standard Master', 'Add Standard', 0),
(85, 2, '2020-12-19 18:26:47', 'dd', 'deb', 'Standard Master', 'Update Standard', 0),
(86, 1, '2020-12-19 18:27:13', 'dd', 'deb', 'Standard Master', 'Add Standard', 0),
(87, 1, '2020-12-19 18:27:18', 'dd', 'deb', 'Delete Standard', 'Deleted Standard', 0);

-- 
-- Restore previous SQL mode
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Enable foreign keys
-- 
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;