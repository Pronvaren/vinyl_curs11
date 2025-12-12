-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: vynil
-- ------------------------------------------------------
-- Server version	8.0.30

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
-- Table structure for table `Artists`
--

DROP TABLE IF EXISTS `Artists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Artists` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Artists`
--

LOCK TABLES `Artists` WRITE;
/*!40000 ALTER TABLE `Artists` DISABLE KEYS */;
INSERT INTO `Artists` VALUES (1,'London Symphony Orchestra'),(2,'Московский Симфонический Оркестр'),(3,'Нет'),(4,'Нижегородский Оркест Имени Щедрика');
/*!40000 ALTER TABLE `Artists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Composers`
--

DROP TABLE IF EXISTS `Composers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Composers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Composers`
--

LOCK TABLES `Composers` WRITE;
/*!40000 ALTER TABLE `Composers` DISABLE KEYS */;
INSERT INTO `Composers` VALUES (1,'Nirvana'),(2,'Oasis'),(3,'Pearl Jam'),(4,'Soundgarden'),(5,'Alice In Chains'),(6,'Stone Temple Pilots'),(7,'Mother Love Bone'),(8,'Temple of the Dog'),(9,'Smashing Pumpkins'),(10,'Red Hot Chili Peppers'),(11,'Radiohead'),(12,'Green Day'),(13,'The Offspring'),(14,'Lynyrd Skynyrd'),(15,'Blur'),(16,'Linkin Park'),(17,'Limp Bizkit'),(18,'Alice Cooper'),(19,'Guns N Roses'),(20,'Metallica'),(21,'Megadeth'),(22,'Beatles'),(23,'Joy Division'),(24,'Motley Crue'),(25,'AC/DC'),(26,'Led Zeppelin'),(27,'The Rolling Stones'),(28,'The Who'),(29,'Black Sabbath'),(30,'Deep Purple'),(31,'The Doors'),(32,'Queen'),(33,'The Clash'),(34,'Пётр Чайковский'),(35,'Ramones'),(36,'The Cure'),(37,'Depeche Mode'),(38,'Rage Against The Machine'),(39,'Foo Fighters'),(40,'Stone Roses'),(41,'Pink Floyd'),(42,'Ozzy Osbourne'),(43,'The Verve'),(44,'Hole'),(45,'The Jimi Hendrix Experience'),(46,'The Stooges'),(47,'Soundgarden'),(48,'Silverchair'),(49,'Sonic Youth'),(50,'Bee Gees'),(51,'Type O Negative'),(52,'Primus'),(53,'Pantera');
/*!40000 ALTER TABLE `Composers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Employees`
--

DROP TABLE IF EXISTS `Employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Employees` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` text,
  `Role` int DEFAULT NULL,
  `Login` varchar(50) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `PhoneNumber` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `Role` (`Role`),
  CONSTRAINT `employees_ibfk_1` FOREIGN KEY (`Role`) REFERENCES `Roles` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Employees`
--

LOCK TABLES `Employees` WRITE;
/*!40000 ALTER TABLE `Employees` DISABLE KEYS */;
INSERT INTO `Employees` VALUES (1,'Иванов Иван Иванович',1,'ivanov','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000001'),(2,'Петров Пётр Петрович',2,'petrov','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000002'),(3,'Сидоров Сидор Сидорович',3,'sidorov','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000003'),(4,'Кузнецов Алексей Сергеевич',1,'kuznetsov','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000004'),(5,'Смирнова Анна Викторовна',2,'smirnova','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000005'),(7,'Лебедев Михаил Анатольевич',1,'lebedev','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000007'),(8,'Лапина Ольга Ивановна',2,'kozlova','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','+7 (791) 100-0000'),(10,'Морозова Елена Андреевна',1,'morozova','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','79110000010'),(11,'Щедрин Артем Юрьевич',3,'arti','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4','+7 (999) 430-0349');
/*!40000 ALTER TABLE `Employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Genres`
--

DROP TABLE IF EXISTS `Genres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Genres` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Genres`
--

LOCK TABLES `Genres` WRITE;
/*!40000 ALTER TABLE `Genres` DISABLE KEYS */;
INSERT INTO `Genres` VALUES (1,'Гранж'),(2,'Бритпоп'),(3,'Альтернативный рок'),(4,'Фанк-рок'),(5,'Панк-рок'),(6,'Саузерн-рок'),(7,'Ню-метал'),(8,'Хард-рок'),(9,'Хэви-метал'),(10,'Трэш-метал'),(11,'Рок'),(12,'Глэм-метал'),(13,'Психоделический рок'),(14,'Классическая музыка'),(15,'Пост-панк'),(16,'Синтипоп'),(17,'Мэдчестер'),(18,'Прогрессивный рок'),(19,'Готик-метал'),(20,'НеоГенрок');
/*!40000 ALTER TABLE `Genres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Labels`
--

DROP TABLE IF EXISTS `Labels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Labels` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Labels`
--

LOCK TABLES `Labels` WRITE;
/*!40000 ALTER TABLE `Labels` DISABLE KEYS */;
INSERT INTO `Labels` VALUES (1,'Sony Music'),(2,'Universal Music'),(3,'Warner Music'),(4,'EMI'),(5,'Island Records'),(6,'Atlantic Records'),(7,'Capitol Records'),(8,'Columbia Records'),(9,'RCA Records'),(10,'Virgin Records'),(11,'Motown Records'),(12,'Decca Records'),(13,'Epic Records'),(14,'Polydor Records'),(15,'Geffen Records'),(16,'Parlophone'),(17,'Chrysalis Records'),(18,'MCA Records'),(19,'Island Def Jam'),(20,'Blue Note Records'),(21,'Sub Pop'),(22,'Matador Records'),(23,'4AD'),(24,'Rough Trade Records'),(25,'Factory Records'),(26,'Elektra Records'),(27,'Arista Records'),(28,'Sire Records'),(29,'Mercury Records'),(30,'XL Recordings'),(31,'Fat Possum Records'),(32,'Domino Records'),(33,'Mute Records'),(34,'V2 Records'),(35,'Epitaph Records'),(36,'Victory Records'),(37,'Cooking Vinyl'),(38,'Cherry Red Records'),(39,'Koch Records'),(40,'Stax Records'),(41,'Giant Records'),(42,'Virgin EMI'),(43,'Nonesuch Records'),(44,'Concord Records'),(45,'Beggars Banquet'),(46,'Atlantic Records UK'),(47,'Matador UK'),(48,'Rough Trade US'),(49,'Sony Classical'),(50,'London Records');
/*!40000 ALTER TABLE `Labels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Manufacturers`
--

DROP TABLE IF EXISTS `Manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Manufacturers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Manufacturers`
--

LOCK TABLES `Manufacturers` WRITE;
/*!40000 ALTER TABLE `Manufacturers` DISABLE KEYS */;
INSERT INTO `Manufacturers` VALUES (1,'Sony Music Entertainment'),(2,'Universal Music Group'),(3,'Warner Music Group'),(4,'EMI Records'),(5,'Island Records'),(6,'Atlantic Records'),(7,'Capitol Records'),(8,'Columbia Records'),(9,'RCA Records'),(10,'Virgin Records'),(11,'Motown Records'),(12,'Decca Records'),(13,'Epic Records'),(14,'Polydor Records'),(15,'Geffen Records'),(16,'Parlophone'),(17,'Chrysalis Records'),(18,'MCA Records'),(19,'Island Def Jam'),(20,'Blue Note Records'),(21,'Sub Pop'),(22,'Matador Records'),(23,'4AD'),(24,'Rough Trade Records'),(25,'Factory Records'),(26,'Elektra Records'),(27,'Arista Records'),(28,'Sire Records'),(29,'Mercury Records'),(30,'Atlantic Records UK'),(31,'Sony Classical'),(32,'Nonesuch Records'),(33,'Virgin EMI'),(34,'Concord Records'),(35,'Beggars Banquet'),(36,'XL Recordings'),(37,'Fat Possum Records'),(38,'Domino Records'),(39,'Mute Records'),(41,'V2 Records'),(42,'Matador UK'),(43,'Epitaph Records'),(44,'Victory Records'),(45,'Fearless Records'),(46,'Мелодия'),(47,'Cherry Red Records'),(48,'Артем МузЗаписи'),(49,'Stax Records'),(50,'Giant Records'),(51,'Seattle Sound'),(52,'FekaMil Recirds'),(53,'BB Records');
/*!40000 ALTER TABLE `Manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `MediaFormats`
--

DROP TABLE IF EXISTS `MediaFormats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `MediaFormats` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `MediaFormats`
--

LOCK TABLES `MediaFormats` WRITE;
/*!40000 ALTER TABLE `MediaFormats` DISABLE KEYS */;
INSERT INTO `MediaFormats` VALUES (1,'LP'),(2,'2LP'),(3,'3LP'),(4,'LP Box'),(5,'EP'),(6,'Single'),(7,'Flexi');
/*!40000 ALTER TABLE `MediaFormats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `OrderComposition`
--

DROP TABLE IF EXISTS `OrderComposition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OrderComposition` (
  `id` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `Product` int DEFAULT NULL,
  `Cost` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `Description` text,
  `CostSumm` int DEFAULT NULL,
  `ProductID` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Product` (`Product`),
  KEY `ordercomposition_ibfk_3` (`OrderID`),
  KEY `fk_ordercomposition_product` (`ProductID`),
  CONSTRAINT `fk_ordercomposition_product` FOREIGN KEY (`ProductID`) REFERENCES `Products` (`id`),
  CONSTRAINT `ordercomposition_ibfk_1` FOREIGN KEY (`Product`) REFERENCES `Products` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ordercomposition_ibfk_3` FOREIGN KEY (`OrderID`) REFERENCES `Orders` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `OrderComposition`
--

LOCK TABLES `OrderComposition` WRITE;
/*!40000 ALTER TABLE `OrderComposition` DISABLE KEYS */;
/*!40000 ALTER TABLE `OrderComposition` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Orders`
--

DROP TABLE IF EXISTS `Orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Orders` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Date` date DEFAULT NULL,
  `Status` int NOT NULL,
  `Salesman` int DEFAULT NULL,
  `CostSumm` int DEFAULT NULL,
  `DeliveryDate` date NOT NULL,
  PRIMARY KEY (`id`),
  KEY `Salesman` (`Salesman`),
  KEY `orders_ibfk_status` (`Status`),
  CONSTRAINT `orders_ibfk_salesman` FOREIGN KEY (`Salesman`) REFERENCES `Employees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `orders_ibfk_status` FOREIGN KEY (`Status`) REFERENCES `OrderStatus` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Orders`
--

LOCK TABLES `Orders` WRITE;
/*!40000 ALTER TABLE `Orders` DISABLE KEYS */;
INSERT INTO `Orders` VALUES (2,'2025-01-05',1,1,30000,'2025-01-01'),(3,'2025-01-05',2,7,2000,'2025-01-09'),(4,'2025-01-05',3,10,3500,'2025-01-09'),(5,'2025-01-05',4,4,4500,'2025-01-09');
/*!40000 ALTER TABLE `Orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `OrderStatus`
--

DROP TABLE IF EXISTS `OrderStatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OrderStatus` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `OrderStatus`
--

LOCK TABLES `OrderStatus` WRITE;
/*!40000 ALTER TABLE `OrderStatus` DISABLE KEYS */;
INSERT INTO `OrderStatus` VALUES (1,'Оформлен'),(2,'В обработке'),(3,'Отменён'),(4,'Доставлен');
/*!40000 ALTER TABLE `OrderStatus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Products`
--

DROP TABLE IF EXISTS `Products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Composer` int DEFAULT NULL,
  `Artist` int DEFAULT NULL,
  `ReleaseYear` int DEFAULT NULL,
  `ManufactureYear` int DEFAULT NULL,
  `Genre` int DEFAULT NULL,
  `MediaFormat` int DEFAULT NULL,
  `Label` int DEFAULT NULL,
  `OriginCountry` text,
  `Manufacturer` int DEFAULT NULL,
  `Supplier` int DEFAULT NULL,
  `Cost` int DEFAULT NULL,
  `QuantityWarehouse` int DEFAULT NULL,
  `Description` text,
  `Photo` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `MediaFormat` (`MediaFormat`),
  KEY `Label` (`Label`),
  KEY `Manufacturer` (`Manufacturer`),
  KEY `Supplier` (`Supplier`),
  KEY `Genre` (`Genre`),
  KEY `Composer` (`Composer`),
  KEY `Artist` (`Artist`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`MediaFormat`) REFERENCES `MediaFormats` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_2` FOREIGN KEY (`Label`) REFERENCES `Labels` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_3` FOREIGN KEY (`Manufacturer`) REFERENCES `Manufacturers` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_4` FOREIGN KEY (`Supplier`) REFERENCES `Suppliers` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_5` FOREIGN KEY (`Genre`) REFERENCES `Genres` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_6` FOREIGN KEY (`Composer`) REFERENCES `Composers` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_7` FOREIGN KEY (`Artist`) REFERENCES `Artists` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Products`
--

LOCK TABLES `Products` WRITE;
/*!40000 ALTER TABLE `Products` DISABLE KEYS */;
INSERT INTO `Products` VALUES (1,'Nevermind',1,3,1991,1991,1,1,1,'',1,1,1200,50,'Культовый альбом группы Nirvana, один из символов гранжа и поколения X. Включает хиты \"Smells Like Teen Spirit\", \"Come As You Are\", оказавший огромное влияние на рок-сцену 90-х.','1.jpg'),(2,'Definitely Maybe',2,3,1994,1994,2,1,2,'',2,2,1100,40,'Дебютный альбом Oasis, ключевой для британского бритпопа. Треки \"Live Forever\" и \"Supersonic\" сделали группу известной на весь мир.','2.jpg'),(3,'Ten',3,3,1991,1991,1,2,3,'',3,3,1300,30,'Основополагающий альбом Pearl Jam, один из главных представителей гранжа. Содержит песни \"Alive\", \"Even Flow\" и \"Jeremy\", ставшие классикой альтернативного рока.','3.jpg'),(4,'Badmotorfinger',4,3,1991,1991,1,2,4,'',4,4,1250,25,'Альбом Soundgarden, соединяющий хард-рок и гранж. Хиты \"Outshined\" и \"Rusty Cage\" сделали группу культовой для начала 90-х.','4.jpg'),(5,'Dirt',5,3,1992,1992,1,3,5,'',5,5,1200,20,'Второй альбом Alice In Chains, мрачный и эмоционально насыщенный. Песни \"Would?\" и \"Rooster\" стали символами гранжа.','5.jpg'),(6,'Core',6,3,1992,1992,3,1,6,'',6,6,1150,15,'Дебютный альбом Stone Temple Pilots, сочетание альтернативного рока и гранжа. Хиты \"Plush\" и \"Creep\" сделали группу популярной.',''),(7,'Apple',7,3,1990,1990,1,1,7,'',7,7,1100,10,'Альбом Mother Love Bone, предтеча Temple of the Dog. Эмоциональные треки, смесь глэм-рока и гранжа, оказавшие влияние на сэйэтлскую сцену.','7.jpg'),(8,'Temple of the Dog',8,3,1991,1991,1,1,8,'',8,8,1200,20,'Альбом супер-группы Temple of the Dog, посвящённый памяти Эндрю Вуда. Песни \"Hunger Strike\" и \"Say Hello 2 Heaven\" стали гимном гранжа.',''),(9,'Siamese Dream',9,3,1993,1993,3,4,9,'',9,9,1300,25,'Альбом Smashing Pumpkins, сочетание альтернативного рока и психоделии. Хиты \"Today\" и \"Cherub Rock\" сделали пластинку культовой.',''),(10,'Blood Sugar Sex Magik',10,3,1991,1991,4,1,10,'',10,10,1400,30,'Альбом Red Hot Chili Peppers, фанк-рок. Включает \"Under the Bridge\" и \"Give It Away\", оказавшие влияние на рок-сцену 90-х.',''),(11,'OK Computer',11,3,1997,1997,3,1,11,'',11,11,1500,20,'Альбом Radiohead, альтернатива с экспериментальным звучанием. Хиты \"Paranoid Android\" и \"Karma Police\" сделали его культовым.','11.jpg'),(12,'Dookie',12,3,1994,1994,5,2,12,'',12,12,1200,40,'Альбом Green Day, панк-рок классики. Песни \"Basket Case\" и \"When I Come Around\" закрепили группу в истории музыки.',''),(13,'Smash',13,3,1994,1994,5,1,13,'',13,13,1150,30,'Альбом The Offspring, панк-рок. Хиты \"Come Out and Play\" и \"Self Esteem\" сделали пластинку знаковой.',''),(14,'Pronounced Leh-Nerd Skin-Nerd',14,3,1973,1973,6,1,14,'',14,14,1100,20,'Дебютный альбом Lynyrd Skynyrd, южный рок. Содержит хит \"Free Bird\", ставший гимном рок-н-ролла.',''),(15,'Parklife',15,3,1994,1994,2,1,15,'',15,15,1300,25,'Альбом Blur, бритпоп. Песни \"Girls & Boys\" и \"Parklife\" сделали пластинку культовой.',''),(16,'Hybrid Theory',16,3,2000,2000,7,1,16,'',16,16,1500,30,'Дебют Linkin Park, ню-метал. Хиты \"In the End\" и \"Crawling\" сделали альбом успешным мировым релизом.',''),(17,'Chocolate Starfish',17,3,2000,2000,7,1,17,'',17,17,1400,25,'Альбом Limp Bizkit, ню-метал. Содержит треки \"My Generation\" и \"Rollin\'\", оказавшие влияние на альтернативную сцену конца 90-х.',''),(18,'Billion Dollar Babies',18,1,1973,1973,8,1,18,'',18,18,1100,20,'Альбом Alice Cooper, хард-рок. Песни \"Elected\" и \"Hello Hooray\" стали знаковыми для сценического образа группы.',''),(19,'Appetite for Destruction',19,3,1987,1987,8,1,19,'',19,19,1500,30,'Дебютный альбом Guns N Roses. Классика хард-рока, включает \"Welcome to the Jungle\" и \"Sweet Child O\' Mine\".',''),(20,'Master of Puppets',20,3,1986,1986,9,1,20,'',20,20,1600,25,'Альбом Metallica, хэви-метал. Содержит композиции \"Battery\" и \"Master of Puppets\", один из лучших метал-альбомов всех времён.',''),(21,'Rust in Peace',21,3,1990,1990,10,1,21,'',21,21,1550,20,'Альбом Megadeth, трэш-метал. Хиты \"Holy Wars… The Punishment Due\" и \"Hangar 18\" сделали пластинку культовой.',''),(22,'Sgt. Pepper\'s Lonely Hearts Club Band',22,3,1967,1967,11,1,22,'',22,22,1800,15,'Альбом Beatles, классический рок и психоделия. Включает \"Lucy in the Sky with Diamonds\" и \"A Day in the Life\".',''),(23,'Are You Experienced',45,3,1967,2000,13,1,23,'',23,23,1500,20,'Альбом Jimi Hendrix, психоделический рок. Культовый для жанра, включает \"Hey Joe\" и \"Purple Haze\".',''),(24,'Dr. Feelgood',24,3,1989,1989,12,1,24,'',24,24,1400,15,'Альбом Motley Crue, глэм-метал. Содержит треки \"Dr. Feelgood\" и \"Kickstart My Heart\".',''),(25,'Back in Black',25,3,1980,1980,8,1,25,'',25,25,1500,20,'Альбом AC/DC, хард-рок. Легендарные песни \"Back in Black\" и \"Hells Bells\".',''),(26,'Led Zeppelin IV',26,3,1971,1971,8,1,26,'',26,26,1600,25,'Альбом Led Zeppelin, хард-рок. Включает хиты \"Stairway to Heaven\" и \"Black Dog\".',''),(27,'Sticky Fingers',27,3,1971,1971,11,1,27,'',27,27,1400,20,'Альбом The Rolling Stones, рок и блюз. Содержит песни \"Brown Sugar\" и \"Wild Horses\".',''),(28,'Whos Next',28,3,1971,1971,11,1,28,'',28,28,1500,15,'Альбом The Who, классический рок. Включает треки \"Baba O\'Riley\" и \"Behind Blue Eyes\".',''),(29,'Paranoid',29,3,1970,1970,9,1,29,'',29,29,1400,20,'Альбом Black Sabbath, хэви-метал. Включает \"Iron Man\" и \"War Pigs\", повлиял на весь жанр.',''),(30,'Лебединое озеро',34,2,1876,1876,15,1,48,'',34,34,2000,10,'Балет Петра Чайковского, одна из величайших классических композиций. История принцессы, превращённой в лебедя, музыкально передана через выразительные оркестровые партии и эмоциональные арии.',''),(33,'Bloody Kisses',50,3,1993,1993,19,1,2,NULL,50,50,1400,15,'Альбом Type O Negative, один из самых известных в жанре готик-метал. Темная атмосферная музыка, мрачные тексты и глубокий вокал Питера Стилла сделали пластинку культовой для любителей готического рока.',NULL);
/*!40000 ALTER TABLE `Products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Roles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` tinytext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Roles`
--

LOCK TABLES `Roles` WRITE;
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
INSERT INTO `Roles` VALUES (1,'Продавец'),(2,'Товаровед'),(3,'Администратор');
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Suppliers`
--

DROP TABLE IF EXISTS `Suppliers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Suppliers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` text NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Suppliers`
--

LOCK TABLES `Suppliers` WRITE;
/*!40000 ALTER TABLE `Suppliers` DISABLE KEYS */;
INSERT INTO `Suppliers` VALUES (1,'Vinyl Heaven','79161234567'),(2,'Товароведдд','79261234568'),(3,'Spin Records','79361234569'),(4,'Retro Sound','79461234570'),(5,'Needle Drop','79561234571'),(6,'Black Vinyl','79661234572'),(7,'Analog Beats','79761234573'),(8,'Record Vault','79861234574'),(9,'Turntable Kings','79961234575'),(10,'Classic Spins','79061234576'),(11,'Soul Tracks','79161234577'),(12,'Vinyl Collective','79261234578'),(13,'Groove Planet','79361234579'),(14,'Soundwave Records','79461234580'),(15,'The Vinyl Room','79561234581'),(16,'Record Lovers','79661234582'),(17,'Wax & Spin','79761234583'),(18,'Needle & Groove','79861234584'),(19,'Turntable Tales','79961234585'),(20,'Retro Grooves','79061234586'),(21,'Vinyl Factory','79161234587'),(22,'Groove Station','79261234588'),(23,'Spin City','79361234589'),(24,'Analog Dreams','79461234590'),(25,'Black Wax','79561234591'),(26,'Sound Chamber','79661234592'),(27,'Record Planet','79761234593'),(28,'Vinyl Vibes','79861234594'),(29,'Needle & Wax','79961234595'),(30,'Groove Records','79061234596'),(31,'Spin & Sound','79161234597'),(32,'Retro Beats','79261234598'),(33,'Vinyl Kingdom','79361234599'),(34,'Analog Kingdom','79461234600'),(35,'Wax Factory','79561234601'),(36,'Record Cave','79661234602'),(37,'Turntable Nation','79761234603'),(38,'Classic Vinyl','79861234604'),(39,'Groove Alley','79961234605'),(40,'Spin Masters','79061234606'),(41,'Vinyl Lab','79161234607'),(42,'Needle Club','79261234608'),(43,'Retro Lab','79361234609'),(44,'Sound Lab','79461234610'),(45,'Vinyl Hub','79561234611'),(46,'Groove Hub','79661234612'),(47,'Record Hub','79761234613'),(48,'Turntable Hub','79861234614'),(49,'Analoggggggg Hub','+7 (999) 612-3461'),(50,'Wax Hub','79061234616'),(51,'Shed Hub Vynil','+7 (949) 939-4934');
/*!40000 ALTER TABLE `Suppliers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Supplies`
--

DROP TABLE IF EXISTS `Supplies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Supplies` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Date` date DEFAULT NULL,
  `Product` int DEFAULT NULL,
  `Supplier` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `Product` (`Product`),
  KEY `Supplier` (`Supplier`),
  CONSTRAINT `supplies_ibfk_1` FOREIGN KEY (`Product`) REFERENCES `Products` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `supplies_ibfk_2` FOREIGN KEY (`Supplier`) REFERENCES `Suppliers` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Supplies`
--

LOCK TABLES `Supplies` WRITE;
/*!40000 ALTER TABLE `Supplies` DISABLE KEYS */;
INSERT INTO `Supplies` VALUES (1,'2025-01-10',1,1,50),(2,'2025-01-12',2,2,40),(3,'2025-01-15',3,3,30),(4,'2025-01-18',4,4,25),(5,'2025-01-20',5,5,20),(6,'2025-01-22',6,6,15),(7,'2025-01-25',7,7,10),(8,'2025-01-28',8,8,20),(9,'2025-01-30',9,9,25),(10,'2025-02-02',10,10,30),(11,'2025-02-05',11,11,35),(12,'2025-02-08',12,12,40),(13,'2025-02-10',13,13,45),(14,'2025-10-16',3,9,12);
/*!40000 ALTER TABLE `Supplies` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-27  0:55:03
