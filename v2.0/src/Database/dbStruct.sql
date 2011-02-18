-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.1.45-community


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema msfa
--

CREATE DATABASE IF NOT EXISTS msfa;
USE msfa;

--
-- Definition of table `results`
--

DROP TABLE IF EXISTS `results`;
CREATE TABLE `results` (
  `resultsid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `testid` int(10) unsigned NOT NULL,
  `testertypeid` int(10) unsigned NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `state` smallint(5) unsigned NOT NULL,
  `configuration` text,
  `starttime` bigint(20) unsigned NOT NULL,
  `endtime` bigint(20) unsigned NOT NULL,
  `totaldownloadscount` int(10) unsigned NOT NULL,
  `totaljsdownloadscount` int(10) unsigned NOT NULL,
  `totalcssdownloadscount` int(10) unsigned NOT NULL,
  `totalimagesdownloadscount` int(10) unsigned NOT NULL,
  `totaldownloadsize` int(10) unsigned NOT NULL,
  `totaljsdownloadsize` int(10) unsigned NOT NULL,
  `totalcssdownloadsize` int(10) unsigned NOT NULL,
  `totalimagesdownloadsize` int(10) unsigned NOT NULL,
  `processortimeavg` int(10) unsigned NOT NULL,
  `usertimeavg` int(10) unsigned NOT NULL,
  `privateworkingsetdelta` int(10) unsigned NOT NULL,
  `workingsetdelta` int(10) unsigned NOT NULL,
  `firstrequesttime` int(10) unsigned NOT NULL,
  `totalrendertime` int(10) unsigned NOT NULL,
  `saved` timestamp NOT NULL DEFAULT '1971-01-01 00:00:00',
  PRIMARY KEY (`resultsid`),
  KEY `FK_results_2` (`testertypeid`),
  KEY `FK_results_1` (`testid`),
  CONSTRAINT `FK_results_1` FOREIGN KEY (`testid`) REFERENCES `tests` (`testid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_results_2` FOREIGN KEY (`testertypeid`) REFERENCES `testertypes` (`testertypeid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3189 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `results`
--

/*!40000 ALTER TABLE `results` DISABLE KEYS */;
/*!40000 ALTER TABLE `results` ENABLE KEYS */;


--
-- Definition of table `testertypes`
--

DROP TABLE IF EXISTS `testertypes`;
CREATE TABLE `testertypes` (
  `testertypeid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `configuration` text,
  `clientid` varchar(45) NOT NULL,
  `clientkey` varchar(45) NOT NULL,
  `lastping` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`testertypeid`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `testertypes`
--

/*!40000 ALTER TABLE `testertypes` DISABLE KEYS */;
/*!40000 ALTER TABLE `testertypes` ENABLE KEYS */;


--
-- Definition of table `tests`
--

DROP TABLE IF EXISTS `tests`;
CREATE TABLE `tests` (
  `testid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `enabled` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `configuration` text,
  `testname` varchar(45) NOT NULL,
  PRIMARY KEY (`testid`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `tests`
--

/*!40000 ALTER TABLE `tests` DISABLE KEYS */;
/*!40000 ALTER TABLE `tests` ENABLE KEYS */;


--
-- Definition of table `triggers`
--

DROP TABLE IF EXISTS `triggers`;
CREATE TABLE `triggers` (
  `triggerid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `triggertype` smallint(5) unsigned NOT NULL,
  `lasttriggered` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `enabled` bit(1) DEFAULT NULL,
  `configuration` text,
  `timeout` int(10) unsigned NOT NULL,
  PRIMARY KEY (`triggerid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `triggers`
--

/*!40000 ALTER TABLE `triggers` DISABLE KEYS */;
/*!40000 ALTER TABLE `triggers` ENABLE KEYS */;


--
-- Definition of table `triggertotestandtestertype`
--

DROP TABLE IF EXISTS `triggertotestandtestertype`;
CREATE TABLE `triggertotestandtestertype` (
  `triggerid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `testertypeid` int(10) unsigned NOT NULL,
  `testid` int(10) unsigned NOT NULL,
  `configuration` text,
  PRIMARY KEY (`triggerid`,`testid`,`testertypeid`),
  KEY `FK_triggertotestandtestertype_2` (`testertypeid`),
  KEY `FK_triggertotestandtestertype_3` (`testid`),
  CONSTRAINT `FK_triggertotestandtestertype_1` FOREIGN KEY (`triggerid`) REFERENCES `triggers` (`triggerid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_triggertotestandtestertype_2` FOREIGN KEY (`testertypeid`) REFERENCES `testertypes` (`testertypeid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_triggertotestandtestertype_3` FOREIGN KEY (`testid`) REFERENCES `tests` (`testid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `triggertotestandtestertype`
--

/*!40000 ALTER TABLE `triggertotestandtestertype` DISABLE KEYS */;
/*!40000 ALTER TABLE `triggertotestandtestertype` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
