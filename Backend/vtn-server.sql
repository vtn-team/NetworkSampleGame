-- Adminer 4.8.1 MySQL 5.5.5-10.6.18-MariaDB-0ubuntu0.22.04.1 dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

SET NAMES utf8mb4;

DROP TABLE IF EXISTS `ScoreBoard`;
CREATE TABLE `ScoreBoard` (
  `AppName` varchar(64) NOT NULL,
  `UserId` varchar(256) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `Score` int(11) NOT NULL,
  `OtherData` text NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp(),
  `UpdatedAt` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp(),
  PRIMARY KEY (`AppName`,`UserId`),
  KEY `Score` (`Score`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


-- 2025-01-18 07:30:38