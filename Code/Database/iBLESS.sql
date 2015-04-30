-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: ibless.cx7whwbxrpt3.us-east-1.rds.amazonaws.com:3306
-- Generation Time: Apr 29, 2015 at 08:11 PM
-- Server version: 5.6.19-log
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `iBLESS`
--

-- --------------------------------------------------------

--
-- Table structure for table `1294_Arch`
--

CREATE TABLE IF NOT EXISTS `1294_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1294_Arch`
--

INSERT INTO `1294_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Admin2', 1),
(2, 'Admin3', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1294_SPL`
--

CREATE TABLE IF NOT EXISTS `1294_SPL` (
  `ID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1294_Users`
--

CREATE TABLE IF NOT EXISTS `1294_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1294_Users`
--

INSERT INTO `1294_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1628, 1, NULL, 1294, '4/12/2015', '4:32:19 AM', '1234567890', 1233, 'Miami', 'AL', 'f'),
(1631, 1, NULL, 1294, '4/14/2015', '2:39:32 AM', '2132568902', 1289991, 'El Segundo', 'CA', 'Dude');

-- --------------------------------------------------------

--
-- Table structure for table `1294_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1294_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `1294_VibrationSettings`
--

INSERT INTO `1294_VibrationSettings` (`ID`, `Low`, `High`, `String`) VALUES
(1, 100, 122, 'Medium;2,2,2,2;8'),
(2, 222, 1000, 'Low;1,1,1,1;6');

-- --------------------------------------------------------

--
-- Table structure for table `1327_Arch`
--

CREATE TABLE IF NOT EXISTS `1327_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_Arch`
--

INSERT INTO `1327_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Manager', 1),
(2, 'Great Employee', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1327_BLEInformation`
--

CREATE TABLE IF NOT EXISTS `1327_BLEInformation` (
  `ID` int(11) NOT NULL,
  `IsOn` tinyint(1) NOT NULL,
  `IMEI` int(11) NOT NULL,
  `MAC` char(100) NOT NULL,
  `MSISDN` int(11) NOT NULL,
  `IMSI` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_BLEInformation`
--

INSERT INTO `1327_BLEInformation` (`ID`, `IsOn`, `IMEI`, `MAC`, `MSISDN`, `IMSI`) VALUES
(1511, 1, 0, '', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `1327_Errors`
--

CREATE TABLE IF NOT EXISTS `1327_Errors` (
  `ID` int(11) NOT NULL,
  `Date` char(100) NOT NULL,
  `Time` char(100) NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `ErrorID` int(11) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_Errors`
--

INSERT INTO `1327_Errors` (`ID`, `Date`, `Time`, `Weather`, `Phone`, `GPS_Loc`, `ErrorID`) VALUES
(1510, '3/17/2015', '7:21:52 PM', 'sunny', '7869084442', '23', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1327_Notifications`
--

CREATE TABLE IF NOT EXISTS `1327_Notifications` (
  `ID` int(11) NOT NULL,
  `Date` char(100) NOT NULL,
  `Time` char(100) NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `CellID` int(11) NOT NULL,
  `AccessoryStatus` char(100) NOT NULL,
  `VibrationSource` char(100) NOT NULL,
  `VibrationEffect` char(100) NOT NULL,
  `TimeVibrated` double NOT NULL,
  `Accelerometer` char(100) NOT NULL,
  `InGeofence` tinyint(1) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_Notifications`
--

INSERT INTO `1327_Notifications` (`ID`, `Date`, `Time`, `Weather`, `Phone`, `GPS_Loc`, `CellID`, `AccessoryStatus`, `VibrationSource`, `VibrationEffect`, `TimeVibrated`, `Accelerometer`, `InGeofence`) VALUES
(1511, '3/24/2015', '6:53:18 PM', 'JIji', '4495267000', 'Jojo', 151, 'Jeje', 'Jeje', 'Jeje', 23, 'Jojo', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1327_PhoneInformation`
--

CREATE TABLE IF NOT EXISTS `1327_PhoneInformation` (
  `ID` int(11) NOT NULL,
  `IMEI` int(11) NOT NULL,
  `MSISDN` int(11) NOT NULL,
  `IMSI` int(11) NOT NULL,
  `MAC` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_PhoneInformation`
--

INSERT INTO `1327_PhoneInformation` (`ID`, `IMEI`, `MSISDN`, `IMSI`, `MAC`) VALUES
(1511, 200, 1511, 213213, '1sda11');

-- --------------------------------------------------------

--
-- Table structure for table `1327_SPL`
--

CREATE TABLE IF NOT EXISTS `1327_SPL` (
  `ID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_SPL`
--

INSERT INTO `1327_SPL` (`ID`, `Date`, `Time`, `Weather`, `Phone`, `GPS_Loc`, `SPL`, `WindSpeed`, `WindDirection`) VALUES
(1511, '2015-04-10', '06:11:22', 'sasa', '4495267000', 'sasa', 999, 22, 'NorthEast'),
(1511, '2015-04-10', '06:12:56', 'sasa', '4495267000', '25.65114388;-80.35995236', 66, 22, 'NorthEast'),
(1516, '2015-04-10', '06:13:30', 'sasa', '4495267000', '25.65114388;-80.35995236', 90, 22, 'NorthEast'),
(1511, '2015-04-10', '06:13:59', 'sasa', '4495267000', '25.65114388;-80.35995236', 90, 22, 'NorthEast'),
(1511, '2015-04-10', '06:14:17', 'sasa', '4495267000', '25.65114388;-80.35995236', 90, 22, 'NorthEast'),
(1511, '2015-04-10', '06:16:38', 'sasa', '4495267000', '25.65114388;-80.35995236', 90, 22, 'NorthEast'),
(1511, '2015-04-11', '01:05:01', 'sada', '4495267000', 'sad', 200, 212, 'South'),
(1511, '2015-04-11', '01:05:45', 'sada', '4495267000', 'sad', 100, 212, 'South'),
(1511, '2015-04-11', '01:06:49', 'sada', '4495267000', 'sad', 101, 212, 'South'),
(1511, '2015-04-11', '01:08:44', 'sada', '4495267000', 'sad', 101, 212, 'South'),
(1511, '2015-04-11', '01:10:22', 'sada', '4495267000', 'sad', 101, 212, 'South'),
(1511, '2015-04-11', '03:56:15', '1511', '4495267000', '1511', 100, 20.3, 'NorthEast'),
(1511, '2015-04-11', '05:52:00', '1511', '4495267000', '1511', 100, 20.3, 'NorthEast'),
(1511, '2015-04-12', '08:12:24', '1511', '4495267000', '1511', 100, 20.3, 'NorthEast'),
(1511, '2015-04-13', '05:07:32', '1511', '4495267000', '1511', 100, 20.3, 'NorthEast'),
(1516, '2015-04-13', '05:08:12', '1511', '7869084441', '1511', 200, 20.3, 'NorthEast'),
(1516, '2015-04-13', '05:13:55', '1511', '7869084441', '1511', 100, 20.3, 'NorthEast'),
(1516, '2015-04-23', '04:28:26', 'sa', '7869084441', 'sa', 5, 231, 'NorthEast'),
(1511, '2015-04-25', '03:39:24', '', '4495267000', 'Updating...', 74.54, 20, 'North'),
(1511, '2015-04-26', '11:05:58', 'Sunny', '4495267000', '123;2323', 100, 20, 'NorthEast'),
(1511, '2015-04-26', '11:20:26', 'Sunny', '4495267000', '123;2323', 100, 20, 'North'),
(1511, '2015-04-27', '02:21:13', 'sa', '4495267000', 'sa', 95, 123, 'North'),
(1511, '2015-04-27', '02:25:56', 'sa', '4495267000', 'sa', 95, 123, 'North'),
(1511, '2015-04-27', '02:26:02', 'sa', '4495267000', 'sa', 6, 123, 'North');

-- --------------------------------------------------------

--
-- Table structure for table `1327_Users`
--

CREATE TABLE IF NOT EXISTS `1327_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`),
  UNIQUE KEY `Phone` (`Phone`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1327_Users`
--

INSERT INTO `1327_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1510, 1, NULL, 1327, '3/9/2015', '9:50:49 PM', '7869084442', 123, 'Miami', 'FL', 'Boss'),
(1511, 1, 1510, 1327, '3/10/2015', '4:23:40 PM', '4495267000', 1, 'Lima', 'FL', 'Director'),
(1516, 1, 1510, 1327, '3/10/2015', '7:58:34 PM', '7869084441', 1234, 'Miami', 'FL', 'BOSS'),
(1547, 2, 1510, 1327, '3/11/2015', '8:58:37 PM', '1234567890', 12311, 'Miami', 'FL', '33193'),
(1597, 1, NULL, 1327, '4/10/2015', '6:45:22 PM', '1234567821', 12222, 'Miami', 'FL', 'Boss'),
(1630, 2, 1597, 1327, '4/13/2015', '9:40:34 PM', '2345678901', 123213, 'Miami', 'AL', 'f'),
(1667, 1, NULL, 1327, '2015/4/29', '12:17:40 AM', '9999999999', 13222, 'Miami', 'AL', 'f');

-- --------------------------------------------------------

--
-- Table structure for table `1327_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1327_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=56 ;

--
-- Dumping data for table `1327_VibrationSettings`
--

INSERT INTO `1327_VibrationSettings` (`ID`, `Low`, `High`, `String`) VALUES
(1, 5, 7, 'Low;1,1,1,1;6'),
(2, 7, 9, 'Low;1,1,1,1;6'),
(8, 3.4, 3.5, 'Low;1,1,1,1;6'),
(9, 3.5, 3.6, 'Low;1,1,1,1;6'),
(10, 3.6, 3.7, 'Low;1,1,1,1;6'),
(11, 3.7, 3.8, 'Low;1,1,1,1;6'),
(12, 3.8, 3.9, 'Low;1,1,1,1;6'),
(13, 3.9, 3.91, 'Low;1,1,1,1;6'),
(34, 95, 99, 'Fast Alert;16,138,16,138,15,138,15,138;7'),
(42, 3.951, 4, 'Low;1,1,1,1;6'),
(43, 99.6, 100.05, 'Fast Alert;16,138,16,138,15,138,15,138;7'),
(44, 10, 12, 'Low;1,1,1,1;6'),
(45, 4, 4.99, 'Low;1,1,1,1;6'),
(48, 3.399999, 3.4, 'Low;1,1,1,1;6'),
(51, 100.1, 103, 'Fast Alert;16,138,16,138,15,138,15,138;7'),
(55, 50, 51, 'Low;1,1,1,1;6');

-- --------------------------------------------------------

--
-- Table structure for table `1337_Arch`
--

CREATE TABLE IF NOT EXISTS `1337_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1337_Arch`
--

INSERT INTO `1337_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Admin', 1),
(2, 'A2', 0),
(3, 'A3', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1337_PhoneInformation`
--

CREATE TABLE IF NOT EXISTS `1337_PhoneInformation` (
  `ID` int(11) NOT NULL,
  `IMEI` int(11) NOT NULL,
  `MSISDN` int(11) NOT NULL,
  `IMSI` int(11) NOT NULL,
  `MAC` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1337_Users`
--

CREATE TABLE IF NOT EXISTS `1337_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` int(11) NOT NULL,
  `EmployeeID` int(11) NOT NULL,
  `City` varchar(100) NOT NULL,
  `State` varchar(100) NOT NULL,
  `Title` varchar(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1337_Users`
--

INSERT INTO `1337_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1501, 1, NULL, 1337, '3/9/2015', '3:47:12 PM', 0, 0, '', '', '');

-- --------------------------------------------------------

--
-- Table structure for table `1451_Arch`
--

CREATE TABLE IF NOT EXISTS `1451_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1451_Arch`
--

INSERT INTO `1451_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Test1', 1),
(2, 'Test2', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1451_Users`
--

CREATE TABLE IF NOT EXISTS `1451_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` varchar(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1451_Users`
--

INSERT INTO `1451_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`) VALUES
(1452, 1, NULL, 1451, '2/27/2015', '7:59:39 PM', '3055882899');

-- --------------------------------------------------------

--
-- Table structure for table `1458_Arch`
--

CREATE TABLE IF NOT EXISTS `1458_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1458_Arch`
--

INSERT INTO `1458_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'dsad', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1458_Users`
--

CREATE TABLE IF NOT EXISTS `1458_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1467_Arch`
--

CREATE TABLE IF NOT EXISTS `1467_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1467_Users`
--

CREATE TABLE IF NOT EXISTS `1467_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1483_Arch`
--

CREATE TABLE IF NOT EXISTS `1483_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1483_Arch`
--

INSERT INTO `1483_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Dean', 1),
(2, 'Special Head', 1),
(3, 'Head Professor', 1),
(4, 'Peasant', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1483_Users`
--

CREATE TABLE IF NOT EXISTS `1483_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1483_Users`
--

INSERT INTO `1483_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`) VALUES
(1485, 1, NULL, 1483, '3/6/2015', '8:10:06 PM'),
(1486, 4, 1485, 1483, '3/6/2015', '8:10:42 PM');

-- --------------------------------------------------------

--
-- Table structure for table `1516_Users`
--

CREATE TABLE IF NOT EXISTS `1516_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1549_Arch`
--

CREATE TABLE IF NOT EXISTS `1549_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1549_Arch`
--

INSERT INTO `1549_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Dsad', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1549_Users`
--

CREATE TABLE IF NOT EXISTS `1549_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1549_Users`
--

INSERT INTO `1549_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1551, 1, NULL, 1549, '3/12/2015', '11:59:22 PM', '1234567993', 1233333, 'Miami', 'FL', '33123');

-- --------------------------------------------------------

--
-- Table structure for table `1563_Arch`
--

CREATE TABLE IF NOT EXISTS `1563_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1563_Arch`
--

INSERT INTO `1563_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Admin', 1),
(2, 'Admin2', 0),
(3, 'Admin3', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1563_SPL`
--

CREATE TABLE IF NOT EXISTS `1563_SPL` (
  `ID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1563_Users`
--

CREATE TABLE IF NOT EXISTS `1563_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1563_Users`
--

INSERT INTO `1563_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1636, 1, NULL, 1563, '2015/4/19', '3:54:40 PM', '9548611588', 888, 'dfsfsd', 'GA', 'fsdfs');

-- --------------------------------------------------------

--
-- Table structure for table `1563_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1563_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `1590_Arch`
--

CREATE TABLE IF NOT EXISTS `1590_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1590_Arch`
--

INSERT INTO `1590_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Employee', 0);

-- --------------------------------------------------------

--
-- Table structure for table `1590_SPL`
--

CREATE TABLE IF NOT EXISTS `1590_SPL` (
  `ID` int(11) NOT NULL,
  `Date` char(100) NOT NULL,
  `Time` char(100) NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1590_SPL`
--

INSERT INTO `1590_SPL` (`ID`, `Date`, `Time`, `Weather`, `Phone`, `GPS_Loc`, `SPL`, `WindSpeed`, `WindDirection`) VALUES
(1592, '4/3/2015', '8:38:47 PM', '11', '1234567890', '11', 11, 11, 'East'),
(1592, '4/3/2015', '8:41:18 PM', '11', '1234567890', '11', 11, 11, 'East'),
(1592, '4/3/2015', '8:42:15 PM', '11', '1234567890', '11', 11, 11, 'East'),
(1592, '4/3/2015', '8:42:39 PM', '11', '1234567890', '11', 11, 11, 'East');

-- --------------------------------------------------------

--
-- Table structure for table `1590_Users`
--

CREATE TABLE IF NOT EXISTS `1590_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1590_Users`
--

INSERT INTO `1590_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1592, 1, NULL, 1590, '4/3/2015', '8:36:32 PM', '1234567890', 1233, 'Miami', 'FL', '33193');

-- --------------------------------------------------------

--
-- Table structure for table `1590_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1590_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------

--
-- Table structure for table `1610_Arch`
--

CREATE TABLE IF NOT EXISTS `1610_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1610_Arch`
--

INSERT INTO `1610_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Safety Manager', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1610_Users`
--

CREATE TABLE IF NOT EXISTS `1610_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1644_Arch`
--

CREATE TABLE IF NOT EXISTS `1644_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1644_Arch`
--

INSERT INTO `1644_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'Supreme', 1);

-- --------------------------------------------------------

--
-- Table structure for table `1644_SPL`
--

CREATE TABLE IF NOT EXISTS `1644_SPL` (
  `ID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1644_Users`
--

CREATE TABLE IF NOT EXISTS `1644_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1644_Users`
--

INSERT INTO `1644_Users` (`ID`, `Type`, `Parent`, `CreatedBy`, `Date`, `Time`, `Phone`, `EmployeeID`, `City`, `State`, `Title`) VALUES
(1645, 1, NULL, 1644, '2015/4/27', '1:32:50 AM', '7869084442', 123, 'Mimami', 'AL', 'f');

-- --------------------------------------------------------

--
-- Table structure for table `1644_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1644_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `1644_VibrationSettings`
--

INSERT INTO `1644_VibrationSettings` (`ID`, `Low`, `High`, `String`) VALUES
(1, 0, 5, 'Low;1,1,1,1;6');

-- --------------------------------------------------------

--
-- Table structure for table `1649_Arch`
--

CREATE TABLE IF NOT EXISTS `1649_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1649_SPL`
--

CREATE TABLE IF NOT EXISTS `1649_SPL` (
  `ID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Weather` text,
  `Phone` char(100) DEFAULT NULL,
  `GPS_Loc` char(100) DEFAULT NULL,
  `SPL` double NOT NULL,
  `WindSpeed` double NOT NULL,
  `WindDirection` char(100) NOT NULL,
  PRIMARY KEY (`Date`,`Time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1649_Users`
--

CREATE TABLE IF NOT EXISTS `1649_Users` (
  `ID` int(11) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Parent` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date` char(100) DEFAULT NULL,
  `Time` char(100) DEFAULT NULL,
  `Phone` char(100) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `City` char(100) DEFAULT NULL,
  `State` char(100) DEFAULT NULL,
  `Title` char(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `1649_VibrationSettings`
--

CREATE TABLE IF NOT EXISTS `1649_VibrationSettings` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Low` double NOT NULL,
  `High` double NOT NULL,
  `String` char(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=9 ;

--
-- Dumping data for table `1649_VibrationSettings`
--

INSERT INTO `1649_VibrationSettings` (`ID`, `Low`, `High`, `String`) VALUES
(2, 1, 2, 'Low;1,1,1,1;6'),
(7, 33.01927660099444, 33.01928660099444, 'Low;1,1,1,1;6');

-- --------------------------------------------------------

--
-- Table structure for table `1663_Arch`
--

CREATE TABLE IF NOT EXISTS `1663_Arch` (
  `ID` int(11) NOT NULL,
  `Type` char(50) DEFAULT NULL,
  `Can_Create` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `1663_Arch`
--

INSERT INTO `1663_Arch` (`ID`, `Type`, `Can_Create`) VALUES
(1, 'dsad', 1);

-- --------------------------------------------------------

--
-- Table structure for table `Companies`
--

CREATE TABLE IF NOT EXISTS `Companies` (
  `ID` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Date` date NOT NULL,
  `Time` time NOT NULL,
  `Address` varchar(100) NOT NULL,
  `Type` text NOT NULL,
  `Subscription` text NOT NULL,
  `Chargify_ID` int(11) NOT NULL,
  `SPL_Type` varchar(7) NOT NULL,
  `TagLine` varchar(100) NOT NULL,
  `Website` varchar(100) NOT NULL,
  `Description` varchar(1000) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Companies`
--

INSERT INTO `Companies` (`ID`, `Name`, `Date`, `Time`, `Address`, `Type`, `Subscription`, `Chargify_ID`, `SPL_Type`, `TagLine`, `Website`, `Description`) VALUES
(1294, 'ffsf', '2015-04-12', '05:31:23', 'fsfs', 'sfsfs', 'Bronze', 8241076, 'OSHA', 'Tractouch', 'www.tractouchmobile.com', 'tractouchmobile.com'),
(1327, 'Test', '2015-04-06', '04:05:42', 'Test', 'Test', 'Black', 8241076, 'OSHA', 'Be great and the best :)!', 'www.hello_world.com', 'Best website for creating hello world programs! '),
(1333, '1', '2015-04-02', '07:11:34', '1', '1', 'Bronze', 7760654, 'NIOSH', '', '', ''),
(1337, 'Kuh-nect Inc', '2015-04-04', '03:41:39', '123ifajbnfba', 'Wbe Dev.', 'Bronze', 7889638, 'OSHA', '', '', ''),
(1451, '12', '2015-04-09', '07:57:39', '12', '12', 'Bronze', 7793215, 'OSHA', '', '', ''),
(1458, '23', '2015-04-01', '05:25:50', '23', '23', 'Bronze', 7798800, 'OSHA', '', '', ''),
(1467, 'SomeCompany', '2015-04-02', '09:36:58', '1234 SW 101 Lane', 'Finances', 'Bronze', 7834826, 'OSHA', '', '', ''),
(1483, 'SomeCompany', '2015-04-03', '08:05:26', '12342 SW 101 Lane', 'Financial', 'Bronze', 7869445, 'OSHA', '', '', ''),
(1549, 'Joe', '2015-04-07', '03:54:26', 'Joe', 'Joe', 'Bronze', 7932294, 'OSHA', '', '', ''),
(1563, 'Test1', '2015-04-19', '03:51:34', 'A', 'Test', 'Black', 8346466, 'OSHA', 'We Are Awesome', 'www.google.com', 'aararaerea'),
(1590, 'dsa', '2015-04-08', '08:17:16', 'dsad', 'dsad', 'Bronze', 8161809, 'NIOSH', '', '', ''),
(1644, 'SomeCompany', '2015-04-27', '01:06:59', 'SomeCompany', 'SomeCompany', 'Bronze', 8422721, 'OSHA', 'SomeCompany', 'SomeCompany', 'SomeCompany'),
(1649, 'Selenium', '2015-04-28', '12:21:17', '101 SW 101 lane', 'Finances!!!', 'Silver', 8432714, 'NIOSH', 'WE are selenium!', 'www.selenium.com', 'Super selenium!'),
(1663, 'new Info', '2015-04-28', '02:23:19', '1', '1', 'Silver', 8433976, 'OSHA', '1', '1', '1111');

-- --------------------------------------------------------

--
-- Table structure for table `Errors`
--

CREATE TABLE IF NOT EXISTS `Errors` (
  `ID` int(11) NOT NULL,
  `Type` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Errors`
--

INSERT INTO `Errors` (`ID`, `Type`) VALUES
(0, 'BLE Low'),
(1, 'Cellular Low'),
(2, 'Fell'),
(3, 'Signal Drop');

-- --------------------------------------------------------

--
-- Table structure for table `Password_Recovery`
--

CREATE TABLE IF NOT EXISTS `Password_Recovery` (
  `E-mail` varchar(100) NOT NULL,
  `Code` text NOT NULL,
  PRIMARY KEY (`E-mail`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `Password_Recovery`
--

INSERT INTO `Password_Recovery` (`E-mail`, `Code`) VALUES
('12333@gmail.com', 'NWXF9507J8'),
('aedwards@kuh-nect.com', 'CWUFIPE524'),
('cursedknight@live.com.mx', '9KTSO3MZ2E'),
('faudeveloper@gmail.com', '6IWS744S06'),
('jaime@tractouchmobile.com', '76M6VD1J89'),
('lcarr081@fiu.edu', 'O11GS00Q94'),
('luis34091@gmail.com', '3K859QJ1NE'),
('marioxkal@hotmail.com', '9B98TMGPYB'),
('mauricio@tractouchmobile.com', 'QM2KTFU46W'),
('t@gmail.com', '32WDTJNR21'),
('test@test.com', '4PAH63EGLT');

-- --------------------------------------------------------

--
-- Table structure for table `User`
--

CREATE TABLE IF NOT EXISTS `User` (
  `FirstName` text NOT NULL,
  `LastName` text NOT NULL,
  `UserName` varchar(100) NOT NULL,
  `Password` text NOT NULL,
  `E-mail` varchar(100) NOT NULL,
  `CustomerNumber` int(11) NOT NULL AUTO_INCREMENT,
  `Guid` text NOT NULL,
  `BossID` int(11) DEFAULT NULL,
  `IsInactive` tinyint(1) NOT NULL,
  PRIMARY KEY (`CustomerNumber`),
  UNIQUE KEY `Foreign_Customer` (`CustomerNumber`),
  UNIQUE KEY `E-mail` (`E-mail`),
  UNIQUE KEY `UserName` (`UserName`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1668 ;

--
-- Dumping data for table `User`
--

INSERT INTO `User` (`FirstName`, `LastName`, `UserName`, `Password`, `E-mail`, `CustomerNumber`, `Guid`, `BossID`, `IsInactive`) VALUES
('Mauricio', 'Bendeck', 'mauricio', 'C473590E43644AAB2D2EC94DC8B8F30510796BB6', 'mauricio@tractouchmobile.com', 1294, 'cf269ed6-4e09-47e4-8a1c-836694dc88b2', 1294, 0),
('<script type=''text/javascript''>alert("hello lui");</script>', 'last name', 'lexyto', 'C243C25966791AF4F4E24CBBC7B57BED0D97BA83', 'lexy.feito@gmail.com', 1296, '88aa8fb6-6cf0-4460-b3fb-c82f6fe9916a', NULL, 0),
('Luis Miguel', 'Carrillo', 'Satrofu', 'A2F42EFA765985B2F48378012CD6AE1794466838', 'luis34091@gmail.com', 1327, 'c6d375d8-3bf8-4adf-98d9-01c5dd317421', 1327, 0),
('Jaime', 'Borras', 'jaime@tractouchmobile.com', '64CFCF8EF68BDAF210F4D343B8A53D4B2A0AF49A', 'jaime@tractouchmobile.com', 1332, '4abc97ba-d5d6-4bb8-8606-12469f8c479d', NULL, 0),
('Johan', 'Carrillo', 'User123', '03913CF3D122EC4FF9DE5D0B68A1572F195B2FBF', '123@gmail.com', 1333, '6665f6a3-749e-4867-bfa7-c2aa2e9ad8ab', NULL, 0),
('aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', 'cxz', 'cxz', '2C5CE9802129086F7A96481C44B05C227FCCADF4', 'cxz@cxz.com', 1334, 'daf3ce76-25ea-4dd6-bc20-f351934384c8', NULL, 0),
('123', '123', '123', '71DE094E997A99BA9A9CFA50D6EFBDE64A824D68', '121113@gmail.com', 1336, '7f5d6d62-8ef3-476d-a1bc-0eb8dcb8552c', NULL, 0),
('Alain', 'Edwards', 'alain', '3FEA83151BF5CA01059465E54B15B389BD5FAEF9', 'hmack1141@gmail.com', 1337, '5107c0f8-e57e-497d-8b1c-e77d2f72a3f3', 1337, 0),
('Juan', 'Juan', 'Juan', 'ADD5B5BD52B3E7DE94DBF26DFC9BA1CAC78740C8', 'Juan@gmail.com', 1338, '3a87a0f2-c248-4cec-a0f9-ac2812ae800a', NULL, 0),
('uuuu', 'uuuu', 'uuuu', 'C4C62E5A2E08E101CD3BBA42621E59042513E745', 'uuuu@gmail.com', 1442, '62358d90-03c1-4817-944f-7c42b953eb19', NULL, 0),
('222', '222', '222', 'AD6B2E46EB7A2F4B657656BE7132F88B689B68A9', '222@gmail.com', 1451, '158a93df-5ee4-426f-aa4c-3a7e04ef06a4', 1451, 0),
('t', 't', 'Test1', '463EE05E9D5F222C1F1576FE02091BDF9AC0DA33', 't@gmail.com', 1452, '25f29eaa-ad9e-4744-9e1b-7038e6612125', 1451, 0),
('122', '122', '122', '0F5797AE064AEFA8A7EF27DF457F2E6FD87D7DFB', '122@gmail.com', 1458, 'ab9caedd-e506-4dad-ac25-554bcc16393b', 1458, 0),
('sa', 'pruebita', 'pruebita', 'D323498ED6B06077BB3304E6AEEADA564ACA9777', 'pruebita@gmail.com', 1463, '6a09d743-5912-4c1e-bbac-4301db5474c7', NULL, 0),
('Dandroid', 'The Master', 'dandroid', '2240052D72B6E4DA7164D19EE7F0C7113CA8E56E', 'dagarroyo@gmail.com', 1466, '7ae5eeb1-fdd1-4872-9fc7-51f264802590', NULL, 0),
('My Name', 'My Last Name', 'SomeUsername', '1567C4D597AA4DE12A0F7C9DC3E55AC235857975', 'SomeUsername@gmail.com', 1467, 'd89ddb83-8cd0-4732-af9d-15a5692acc87', 1467, 0),
('Randomuser', 'Randomuser', 'Randomuser', '1AA35508864F623425B3F7BB2AB8AF7F6F3B5197', 'Randomuser@gmail.com', 1469, 'c5e434a3-8c34-48a6-9044-326553812507', NULL, 0),
('Gerardo', 'Estrada', 'Ragera', 'AB8FB31521BC9649BA0FC2D4A04451BBE858F1EB', 'cursedknight@live.com.mx', 1480, '40122b51-40b4-4763-860f-3889bebf9e73', NULL, 0),
('123123', '123123', '123123', '0D4CAD9D82BC418924F0BAC1278C6CE978D4C486', '123123@gmail.com', 1481, 'dc41225b-29fc-46e9-b308-1fa75ae5f2bc', NULL, 0),
('Giancarlo', 'Boccoleri', 'gian6667', '6DFECA8566AA12CFBEA00D67E28B9B793945FEE9', 'gian_boccoleri_7@hotmail.com', 1482, '81b85240-b56b-4cbe-bb98-c8c30ef0c013', NULL, 0),
('Juan', 'Juan', 'someaccount', '88E8F2F84B18D952F93BFBDECEDCDDE193523B5A', 'someaccount@gmail.com', 1483, '9ba65e8b-6ca0-47ae-9bcd-b7ff1ba693da', 1483, 0),
('Alain', 'Edwards', 'Fla', '7B9E649B5F19BFF536D816506C5EFC90204E4EF1', 'faudeveloper@gmail.com', 1485, '37ea4177-3042-42e7-ae63-1b0fcbdf4b23', 1483, 0),
('12333', '12333', '12333', '39A2561DA77F5CFCA8ABA3673C5D8B1CE0D2007F', '12333@gmail.com', 1486, 'a966724a-383f-4558-96a8-bfcdc737c845', 1483, 0),
('Joe', 'Gonz', 'Jg3', '05352311478A7B00EB9E3D4C669D8DEF4447F9B3', 'josephg94@gmail.com', 1488, '2a34806a-df70-4125-be4f-4bafb4ed836c', NULL, 0),
('A', 'E', 'Akuhn', '5658665AA51DA27017FB3FDB6DD849981AFFF004', 'aedwards@kuh-nect.com', 1501, 'a8191900-7dda-41d6-8e01-32eaf52f8a77', 1337, 0),
('MingZhao', 'MingZhao', 'MingZhao', 'A9E06E48C95BD24376B19A26DD86AEC4B6C53341', 'MingZhao@gmail.com', 1510, '20ef8f38-582d-448e-ab60-591cdb70c4dd', 1327, 0),
('Mario', 'Santillana', '444', '51794C71990C375320261062DC1F68DF04E62C9E', 'marioxkal@hotmail.com', 1511, '34620538-7034-4d3c-b643-1421ed6f7726', 1327, 0),
('Pii', 'dsad', 'Piii', '403F2ADC67291018DB278D592118F9CBB8F69700', 'lcarr081@fiu.edu', 1516, '583c46db-4354-4e9f-b07e-817837cd1801', 1327, 0),
('Jotaro', 'Kujo', 'JOoo', 'D0C30EB092E22A64CF49459C445F8DA037C2BECA', 'JOoo@gmail.com', 1547, 'b6a93235-a34b-42e6-b855-c9823e155576', 1327, 0),
('3444', '3444', '3444', '799D473AD18A78FED629CE1FA5F2724F3B044B5C', '3444@gmail.com', 1549, 'e1ef35cc-264d-4865-b49a-fc6733fce91c', 1549, 0),
('JoeUsers', 'JoeUser', 'JoeUser', '5DD0FD04D8ED0D426C3B6E46E21465E91C77AFAC', 'JoeUser@gmail.com', 1551, '09c7d60b-27be-4caf-917c-c9f877eeff6e', 1549, 0),
('577', '577', '577', 'AE69B1DA6E6872D3424CF483F77351A08E66FAA6', '577@gmail.com', 1558, '7d377dd0-2ab8-4dd2-b329-991c7205361c', NULL, 0),
('Alain', 'E', 'test', 'F3397256DAE678F37B5333901DA752168CE17964', 'hmack1141@yahoo.com', 1563, 'cbd74be4-1137-42e6-8cc6-a4fa87248df3', 1563, 0),
('sasasa', 'Satrofu1', 'sasasa', '8FC49DEF30870026CD6A71905FB60894F3F29720', 'luis340921@gmail.com', 1567, '54f7a965-2de2-4af0-93a3-34094cf9bde5', NULL, 0),
('sasasa', 'sasasa', 'Satrofu3', 'B7AF984811520BA1CB25AC263A08C4484A1FAA1C', 'luis3422091@gmail.com', 1570, 'a1e81745-9e93-4a79-a2cb-e010b8f62f59', NULL, 0),
('sasasa', 'sasasa', 'Satrofu4', '0A98D41E9EFADAECDE098C0DD76E498A55DDB706', 'luis34022921@gmail.com', 1575, 'e75ce3f8-cf88-40cd-aa16-4bdb9ac0afbb', NULL, 0),
('sasasa', 'Satrofu24313421312', 'sasasa2', 'EE4C798427B9448FB2AA329811CA8FB136F1190F', 'sasas32232a@gmail.com', 1583, '983b400e-b80f-4f8a-9b0e-d2f9eba14be0', NULL, 0),
('pan1potito', 'pan1potito', 'Satrofu22', '51C495F532FD3739C9952206E00C7493ACAD0FC1', 'sasasa@gmail.com', 1585, 'd78583c7-7a62-44b7-b7b1-fd9be00ec92e', NULL, 0),
('hola', 'hola', 'test333', '7CA35F50E2AB50A28E00D550157A00768BCA6F50', 'sasasssadsa@gmail.com', 1587, 'f5aa81a9-1e16-421c-8f15-b6b2d7d1cc4e', NULL, 0),
('sasasa', 'sasasa', 'kdksjakdj', '4223B46937C8B3C3CB72C232CE00DFDC406BC6E3', 'sasas22222a@gmail.com', 1589, '2e1e43be-aa16-48ee-bdd1-b9ef973a0425', NULL, 0),
('3040', '3040', 'Test3040', 'B93ED76637B02FACE28EF2326A9175C4AC24E16E', '3040@gmail.com', 1590, 'e2ec8939-853a-4d15-865a-021c2da5486c', 1590, 0),
('3040', '3040', 'Testing3040', 'EB0D94DC6255D84F0D0CC53DE5D359C48DF6CCE7', '30404@gmail.com', 1592, 'aa87732e-4f7d-4edf-95f9-973da4d62e4e', 1590, 0),
('F', 'L', 'test@test.com', '9AAA3FD07FEC5A9633E553183F8E7555EF08AE46', 'test@test.com', 1595, 'bc1bff90-33fe-4c47-bb47-f24e8aad4ac0', NULL, 0),
('Carmen', 'Carmen', 'Carmen', 'C35D3186B14DB7B457E164761BD53362F33CFE51', 'carmen@gmail.com', 1597, 'b1ba0525-8e05-46e2-82e3-82811c046a52', 1327, 0),
('Test333', 'Test333', 'Test33344', '99E61EA777E57C6CDAB69C4460F8D3C7911A459A', 'Test33344@gmail.com', 1610, '54f5a0d1-4887-48f6-b2ca-088c95d71604', NULL, 0),
('Bob2', 'Smith', 'Bob2', '874B756C3E44EA3D41FDA24182CB052A87A8250B', 'something@gmail.com', 1628, 'c8014956-6286-4db3-8c47-28befcca8ccf', 1294, 0),
('Miamis', 'florida', 'Username', 'B09FD3BD1D4D9E58EE5E36A3BC3D0CAA55F136F3', 'dlorasd@gmail.com', 1630, '9e0935cc-9fb9-42e1-b2f0-93755aa68a27', 1327, 0),
('Tommy', 'Jones', 'jones', '1738856BD142747A71D16C06EDD4E87203C4D53F', 'jones@tommy.com', 1631, '365f4e3b-f14d-4227-ae05-06fd206fdf26', 1294, 0),
('sfsf', 'sfdsf', 'User1', '0E2669000A005CB91DE006556A2976EBF41AEC12', 'fsdff@fafa.com', 1636, '9473f0bf-75d5-44df-8b77-1081f518b23f', 1563, 0),
('Randomuser', 'Randomuser', 'joe+269@gmail.com', '9710BAF408E4E0C51471B3D453CC1611B12530ED', 'joe+3459@gmail.com', 1638, '42a31740-8c28-4732-a295-dad6716305b0', NULL, 0),
('Randomuser', 'Randomuser', 'Randomuser2', 'FDBEF72D4A3E35A57B268CF89BE89C05EEE0854E', 'Randomuse2r@gmail.com', 1639, '12bab528-d96a-495e-af18-953c660af996', NULL, 0),
('Randomuser', 'Randomuser', 'joe+10681', 'B1173303B686A23A5206EE72A61329781B58DDA7', 'joe+5510@gmail.com', 1643, '8e50758c-c49a-4a13-ab37-8df2c0105561', NULL, 0),
('New Name!', 'Test3355', 'Test3355', '2913385403A9BFF6DC5CFABA8FA0821B8397E753', 'Test3355@gmail.com', 1644, '2bd73e7a-a916-427a-91d9-a2e8673b2f60', 1644, 0),
('Usercito', 'Usercito', 'Usercito', 'EC29523B09BDFE0FD6E39D81D8F8BAF76AD2E3B7', 'Usercito@gmail.com', 1645, '5a5bc932-835e-4830-90fe-da797ca146cd', 1644, 1),
('WSUser', 'WSUser', 'WSUser', 'FA35FDAD02E0779EE48D5DBAE082939B5E1370DB', 'WSUser@gmail.com', 1646, '70e887b8-2a87-446f-a866-8a36a9517d84', NULL, 0),
('ra', 'ra', 'RandomUser22', 'C4B1CE15CF10C34DAAA1E344897A7EA99BCAEF56', 'Randomuser33@gmail.com', 1647, 'd1078af5-3f9e-4daa-a7e9-7b974b02dbb7', NULL, 0),
('ra', 'ra', 'joe+8821', '1636AC70529C765177274961AA0FC62AF2AD9EE1', 'joe+5782@gmail.com', 1648, '74b22533-8525-4380-a37a-b57769a8b664', NULL, 0),
('ra', 'ra', 'joe+6431', '318EB1214EA5877B5C02400E11C538AC387075DE', 'joe+2409@gmail.com', 1650, 'fd68fc96-7bd3-427a-bced-2d830f4fa08a', NULL, 0),
('New Name', 'New Last Name', 'Rarara', '2D885B49ADF5CC6662AF461E98DA12699B4157E1', 'Rarara@gmail.com', 1663, '0850752c-3ff6-495d-839f-be6106102569', 1663, 0),
('Rarara', 'Rarara', 'joe+10740', 'BA1CDB81DE85CBD4ED3890395CC413D0642F2932', 'joe+8522@gmail.com', 1664, 'b509038f-2e6d-4ee5-8c4b-e86127947d69', NULL, 0),
('Rarara', 'Rarara', 'joe+3666', '24CD29B586A45C3017200F137388AB67627F0C36', 'joe+9836@gmail.com', 1665, 'ac6333cf-7dd2-4f0d-b7fa-aa1d4324ef20', NULL, 0),
('Rarara', 'Rarara', 'joe+10482', '54985896DAB4F63F59BEB0C45D521DDEB7B4B955', 'joe+6241@gmail.com', 1666, '6eb45556-7370-485a-93ee-86770af1b159', NULL, 0),
('John', 'Stuart', 'Test Selenium', 'AE26A4DD2E1B234003C2CDCB0DBDE85CA241BDB9', 'john.lm.stuart@gmail.com', 1667, '5a499511-107d-40d7-a146-95d31e6514cb', 1327, 0);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `Password_Recovery`
--
ALTER TABLE `Password_Recovery`
  ADD CONSTRAINT `Password_Recovery_ibfk_1` FOREIGN KEY (`E-mail`) REFERENCES `User` (`E-mail`) ON DELETE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
