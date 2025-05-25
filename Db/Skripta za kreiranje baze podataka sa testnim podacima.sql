-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: furniture_store
-- ------------------------------------------------------
-- Server version	8.0.36

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250508231616_InitialCreate','8.0.13'),('20250510144751_AddUserPreferences','8.0.13'),('20250511214729_AddProfilePictureToUser','8.0.13'),('20250511231142_AddEmploymentDateToUser','8.0.13'),('20250511234745_AddSalaryToUser','8.0.13'),('20250512145155_SetEmploymentDateToDateOnly','8.0.13'),('20250512145743_AddUniqueConstraintToUsername','8.0.13'),('20250512170907_UpdateSalaryPrecision','8.0.13'),('20250512202131_AddPositionToUser','8.0.13'),('20250513133201_AddDepartmentTable','8.0.13'),('20250513133502_AddCashRegisterTable','8.0.13'),('20250513134235_AddDepartmentRelationToUser','8.0.13'),('20250513135743_AddCategoryTable','8.0.13'),('20250514143651_AddProductTable','8.0.13'),('20250514234527_AddSupplierPriceToProduct','8.0.13'),('20250518225150_AddBillItemClass','8.0.13'),('20250518225441_AddBillItemModel','8.0.13'),('20250519130915_SyncWithDatabase','8.0.13'),('20250524170913_AddIsDeletedToProduct','8.0.13'),('20250524175601_SetSupplierPricePrecision','8.0.13'),('20250524183022_AddIsDeletedToUser','8.0.13'),('20250524194204_AddIsDeletedToCategory','8.0.13');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `billitems`
--

DROP TABLE IF EXISTS `billitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `billitems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Quantity` int NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  `ProductId` int NOT NULL,
  `BillId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_BillItems_BillId` (`BillId`),
  KEY `IX_BillItems_ProductId` (`ProductId`),
  CONSTRAINT `FK_BillItems_Bills_BillId` FOREIGN KEY (`BillId`) REFERENCES `bills` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_BillItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=86 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `billitems`
--

LOCK TABLES `billitems` WRITE;
/*!40000 ALTER TABLE `billitems` DISABLE KEYS */;
INSERT INTO `billitems` VALUES (62,1,1600.00,8,48),(63,1,980.00,9,48),(64,2,200.00,18,48),(65,1,950.00,16,49),(66,1,500.00,17,49),(67,1,1550.00,23,49),(68,1,715.00,25,50),(69,1,770.00,22,51),(70,1,1550.00,23,52),(71,1,660.00,10,53),(72,1,955.00,20,54),(73,1,1600.00,8,55),(74,1,420.00,21,56),(75,2,660.00,10,57),(76,1,450.00,13,57),(79,2,980.00,9,59),(80,2,1950.00,12,59),(81,1,980.00,9,60),(82,1,360.00,19,61);
/*!40000 ALTER TABLE `billitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bills`
--

DROP TABLE IF EXISTS `bills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bills` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DateTime` datetime(6) NOT NULL,
  `PaymentMethod` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `CashRegisterId` int NOT NULL,
  `UserId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Bills_CashRegisterId` (`CashRegisterId`),
  KEY `IX_Bills_UserId` (`UserId`),
  CONSTRAINT `FK_Bills_CashRegisters_CashRegisterId` FOREIGN KEY (`CashRegisterId`) REFERENCES `cashregisters` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bills_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bills`
--

LOCK TABLES `bills` WRITE;
/*!40000 ALTER TABLE `bills` DISABLE KEYS */;
INSERT INTO `bills` VALUES (48,'2025-05-23 19:06:34.802807','Cash',2980.00,1,13),(49,'2025-05-23 19:08:44.469228','Cash',3000.00,2,13),(50,'2025-05-23 19:09:10.112662','Cash',715.00,3,13),(51,'2025-05-23 19:09:36.289042','Cash',770.00,1,13),(52,'2025-05-23 19:17:01.709793','Cash',1550.00,3,13),(53,'2025-05-23 19:20:22.739635','Cash',660.00,1,13),(54,'2025-05-23 19:23:49.285279','Card',955.00,2,13),(55,'2025-05-23 19:24:10.740542','Cash',1600.00,1,13),(56,'2025-05-23 19:32:30.357403','Cash',420.00,2,13),(57,'2025-05-23 19:33:52.488713','Cash',1770.00,1,13),(59,'2025-05-24 19:21:26.065152','Card',5860.00,1,13),(60,'2025-05-24 19:23:09.121374','Card',980.00,2,13),(61,'2025-05-24 19:23:58.669862','Cash',360.00,3,13);
/*!40000 ALTER TABLE `bills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashregisters`
--

DROP TABLE IF EXISTS `cashregisters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cashregisters` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Number` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashregisters`
--

LOCK TABLES `cashregisters` WRITE;
/*!40000 ALTER TABLE `cashregisters` DISABLE KEYS */;
INSERT INTO `cashregisters` VALUES (1,1),(2,2),(3,3);
/*!40000 ALTER TABLE `cashregisters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Dnevna soba',0),(2,'Spavaća soba',0),(3,'Kuhinja',0),(4,'Kupatilo',0),(6,'Hodnik',0),(7,'Radna soba',0),(8,'Dječija soba',0),(9,'Baštenski namještaj',0),(13,'Dekoracije',0),(22,'Kancelarijski namještaj',0);
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departments`
--

LOCK TABLES `departments` WRITE;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
INSERT INTO `departments` VALUES (1,'Prodaja','Kontakt s kupcima, prezentacija proizvoda, fakturisanje.'),(2,'Magacin','Skladištenje robe, upravljanje zalihama.'),(3,'Dizajn','Savjetovanje kupaca o uređenju prostora, 3D vizualizacije.'),(4,'Dostava','Organizacija isporuke.'),(5,'Servis','Montaža namještaja kod kupca, popravke i reklamacije.'),(6,'Marketing','Oglašavanje, društvene mreže, promocije.'),(7,'Računovodstvo','Fakturisanje, obračun plata, finansijski izvještaji.'),(8,'IT podrška','Tehnička podrška, održavanje sistema i opreme.'),(9,'Korisnička podrška','Odgovaranje na upite, rješavanje reklamacija.'),(10,'Uprava','Menadžment, donošenje odluka, strategija rasta.'),(11,'Online prodaja','Upravljanje webshopom, online narudžbe.');
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `AvailableQuantity` int NOT NULL,
  `Composition` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CategoryId` int NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ImagePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SupplierPrice` decimal(10,2) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_Products_CategoryId` (`CategoryId`),
  CONSTRAINT `FK_Products_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (7,'Sofa',850.00,5,'Drvo',1,'Drvena sofa','Images/sofa1.jpg',800.00,0),(8,'Garnitura',1600.00,6,'Koža',1,'Kožna garnitura','Images/sofa2.jpg',1500.00,0),(9,'Sto',980.00,10,'Drvo',3,'Kuhinjski sto','Images/kuhinjskiSto1.jpg',900.00,0),(10,'Stolica',660.00,15,'Drvo',9,'Baštenska stolica','Images/bastenskaStolica.jpg',600.00,0),(12,'Set za kuhinju',1950.00,4,'Drvo i štof',3,'Kuhinjski set','Images/kuhinjskiSto2.jpg',1800.00,0),(13,'Sofa ',450.00,3,'Štof',1,'Sofa za dnevnu','Images/sofa3.jpg',400.00,0),(14,'Set za balkon',950.00,7,'Metal',9,'Balkonski set','Images/setZaBalkon.jpg',860.00,0),(15,'Stolice',1100.00,10,'Drvo i štof',3,'Kuhinjske stolice','Images/kuhinjskeStolice.jpg',900.00,0),(16,'Krevet',950.00,5,'Drvo',8,'Dječiji krevet','Images/djecijiKrevet.jpg',700.00,0),(17,'Fotelja',500.00,6,'Štof',1,'Fotelja za dnevnu','Images/foteljeZaDnevnu.jpg',450.00,0),(18,'Kuhinjske stolice',200.00,8,'Drvo',3,'Drvene stolice','Images/drveneKuhinjskeStolice.jpg',120.00,0),(19,'Radni sto',360.00,4,'Metal',8,'Bijeli radni sto','Images/djecijiSto.jpg',300.00,0),(20,'Ugaona',955.00,3,'Štof',1,'Ugaona garnitura','Images/ugaonaGarnitura.jpg',800.00,0),(21,'Sto',420.00,5,'Metal',1,'Sto za dnevnu','Images/stoZaDnevnu.jpg',360.00,0),(22,'Ormar',770.00,4,'Drvo',2,'Drveni ormar','Images/ormar.jpg',700.00,0),(23,'Krevet',1550.00,7,'Štof',2,'Bračni krevet','Images/bracniKrevet.jpg',1450.00,0),(25,'Fotelja',715.00,5,'Štof',1,'Fotelja za dnevnu','Images/fotelja.jpg',687.00,0),(26,'Set za dnevnu',1600.00,6,'Drvo',1,'Drveni set','Images/setZaDnevnu.jpg',1500.00,0);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FirstName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Shift` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PreferredLanguage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PreferredTheme` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProfilePicturePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `EmploymentDate` date NOT NULL,
  `Salary` decimal(10,2) NOT NULL,
  `Position` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DepartmentId` int DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Users_Username` (`Username`),
  KEY `IX_Users_DepartmentId` (`DepartmentId`),
  CONSTRAINT `FK_Users_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','Admin','Admin','admin','f4035833cb911a3e8634903e498ff84b05d4b66f73c6155af3f58b22ad3503f4','+387 63-998-840','Bulevar Desanke Maksimović 18','Prva','sr','Light',NULL,'0001-01-01',4000.00,'Direktor',2,0),(13,'employee','Tamara','Milanović','tamara','170ae551263015bda383d75d862a3c78f8de383bc6f97f332ae805ee00823c37','+387 65 234 478','Borik ','Prva','sr','Gray','Images/defaultUser.png','2022-10-20',28000.00,'Margeting specijalista',6,0),(15,'employee','Andrej','Kostić','andrej','af90eccf85b4a46a09d636f5c6268db30db49bff8761022a5faa72c9c80a6b37','+387 63 765 432','Obilićevo','Druga','en','Light','Images/user5.png','2025-02-20',16800.00,'Prodavač',1,0),(16,'employee','Sara','Jovanović','sara','b2d6dd436e6c615be2b24f4c30196faad0c1d0d1b1a91f2bac6b3e0adc98a682','+387 65 987 342','Starčevica','Prva','en','Light','Images/defaultUser.png','2024-10-21',17000.00,'Prodavačica',1,0),(17,'employee','Jelena','Vasić','jelena','5a1b796f6524668b21c4b6d8eb6bd41d832d561d70745b39afb0c9f8547cf06a','+387 65 345 621','Boška Tošića 19a','Prva','en','Light','Images/defaultUser.png','2023-12-18',30000.00,'Računovođa',7,0),(19,'employee','Srđan','Kosovac','srđan','b60abd45e4daa06819c8abf073794e3d94e87f4e6cdcde2d642fdbdbdf07ce99','+387 65 432 187','Bulevar Desanke Maksimović 15','Druga','en','Light','Images/user5.png','2024-12-16',30000.00,'Kasir',1,0),(20,'employee','Ilijana','Stanković','ilijana','9af5b8c44fa033890c0f8a251369d1b65009c967601820948dd120937a87c86e','+387 65 435 678','Borik 10','Druga','en','Light','Images/defaultUser.png','2024-05-27',28000.00,'Dizajner enterijera',3,0),(21,'employee','Milijana','Jović','milijana','07c205e137c7c344a510bf808432ee9414990256128ff08542abe7a474cc47c3','+387 65 439 876','Starčevica','Prva','en','Light','Images/defaultUser.png','2024-05-20',50000.00,'Menadžer',10,0),(22,'employee','Viktor','Palalić','viktor','d03a37a761e1c8f0d82d0e6c81bf9960662a13fae5a1aee079b25bcce14cdb09','+387 63 444 657','Mejdan','Prva','en','Light','Images/user5.png','2025-01-20',25000.00,'Vozač',4,0),(23,'employee','Milana','Milanović','milana','96c624fa7265f70d0da4965f6220c08c5ebf4a0ce2f53241f0341024fa92a86d','+387 65 558 129','Ranka Šipke 5','Prva','en','Light','Images/defaultUser.png','2024-08-05',29000.00,'IT tehničar',8,0),(24,'employee','Jovana','Janković','jovana','9114ec4e6928602f6c30d22107932928f17f935353bb291a9bba253447fa7ce1','+387 65 876 342','Slobodana Kusturića 19','Druga','en','Light','Images/defaultUser.png','2024-01-08',25000.00,'Kasirka',1,0),(25,'employee','Ivan','Stamenković','ivan','25ae24e133b22fb04155fb6820fa3f2a96b0a33d2a5699b99c218e92fc9a54da','+387 63 243 998','Krfska 10','Prva','en','Light','Images/user5.png','2023-07-10',50000.00,'Specijalista za korisničku podršku',9,0),(26,'employee','Ivona','Komlenić','ivona','bb6b7318ad85e2742106ad5e4e4e929cb44c3d36e7d8faf49589202b3c2899ad','+387 65 478 564','Sime Matavulja 10','Prva','en','Light','Images/defaultUser.png','2024-07-01',45000.00,'E-commerce menadžer',11,0),(27,'employee','Miljan','Tomašević','miljan','a0d0468605d08c0ea6955d3856491103f8552d262a6717ebb7d3eed026123e96','+387 65 887 534','Đure Daničića','Druga','en','Light','Images/user5.png','2024-09-09',30000.00,'Magacioner',2,0),(28,'employee','Nikola','Simonović','nikola','d0e4fae1496d1cd8a72e7e21ae18a73b3a423a5647899a0e064c7569f1a217e6','+387 65 478 345','Ivana Gorana Kovačića 10','Druga','en','Light','Images/user5.png','2024-08-12',35000.00,'Dizajn enterijera',3,0),(29,'employee','Nikolina','Ivanović','nikolina','cc028670f9c4fb17c799364a7863e8a7ed7a05266b87a619c528f4e6e9005995','+387 65 554 736','Meše Selimovića 14','Druga','en','Light','Images/defaultUser.png','2024-05-06',38000.00,'Prodavač',1,0),(30,'employee','Dejan','Čovakušić','dejan','f24c4a44eccc02e241a8227cb86acfec6c569c97862239a017c68e6e303a3bf8','+387 65 786 362','Bulevar Desanke Maksimović 18','Prva','en','Light','Images/user5.png','2023-07-03',32000.00,'Montažer',5,0);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-25 12:34:35
