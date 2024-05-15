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

USE VeloMax;

DROP TABLE IF EXISTS `MAGASIN`;

CREATE TABLE `MAGASIN` (
  `idMagasin` int NOT NULL,
  `idGerant` int NOT NULL,
  `idAdresse` int NOT NULL,
  `chiffre_affaire` float(7) DEFAULT NULL,
  PRIMARY KEY (`idMagasin`),
  FOREIGN KEY (`idGerant`) REFERENCES `EMPLOYE` (`idEmploye`),
  FOREIGN KEY (`idAdresse`) REFERENCES `ADRESSE` (`idAdresse`)
);

DROP TABLE IF EXISTS `EMPLOYE`;

CREATE TABLE `EMPLOYE` (
  `idEmploye` int NOT NULL AUTO_INCREMENT,
  `nomEmploye` varchar(40) NOT NULL,
  `prenomEmploye` varchar(20) NOT NULL,
  `idMagasin` int NOT NULL,
  `position` ENUM('Gérant', 'Vendeur'),
  `salaire` float(6) DEFAULT NULL,
  `prime` float(6) DEFAULT NULL,
  PRIMARY KEY (`idEmploye`),
  FOREIGN KEY (`idMagasin`) REFERENCES `MAGASIN` (`idMagasin`)
);

DROP TABLE IF EXISTS `ADRESSE`;

CREATE TABLE `ADRESSE` (
`idAdresse` int NOT NULL AUTO_INCREMENT,
`numero` varchar(5) DEFAULT NULL,
`rue` varchar(40) NOT NULL,
`ville` varchar(40) NOT NULL,
PRIMARY KEY (`idAdresse`)
);

DROP TABLE IF EXISTS `ASSEMBLAGE`;

CREATE TABLE `ASSEMBLAGE` (
  `idModele` int NOT NULL,
  `idPiece`  varchar(10) NOT NULL,
  FOREIGN KEY (`idModele`) REFERENCES `MODELE` (`idModele`),
  FOREIGN KEY (`idPiece`) REFERENCES `PIECE` (`idPiece`)
);

DROP TABLE IF EXISTS `MODELE`;

CREATE TABLE `MODELE` (
  `idModele` int NOT NULL,
  `nom` varchar(40) NOT NULL,
  `grandeur` ENUM('Adultes', 'Hommes', 'Dames', 'Jeunes', 'Garcons', 'Filles') NOT NULL,
  `prix` int NOT NULL,
  `ligne` varchar(40) NOT NULL,
  `date_debut` date NOT NULL,
  `date_fin` date DEFAULT NULL,
  PRIMARY KEY (`idModele`)
);

DROP TABLE IF EXISTS `PIECE`;

CREATE TABLE `PIECE` (
`idPiece` varchar(10) NOT NULL,
`libelle` ENUM('Cadre', 'Guidon', 'Frein', 'Selle', 'Dérailleur avant', 
'Dérailleur arrière', 'Roue avant', 'Roue arrière', 'Réflecteur', 'Pédalier',
'Ordinateur','Panier') NOT NULL,
PRIMARY KEY (`idPiece`)
);

DROP TABLE IF EXISTS `FOURNISSEUR`;

CREATE TABLE `FOURNISSEUR` (
`siret` varchar(15) NOT NULL,
`nom_entreprise` varchar(40) NOT NULL,
`idAdresse` int NOT NULL,
`categorie` ENUM('Très bon', 'Bon', 'Moyen', 'Mauvais') DEFAULT NULL,
PRIMARY KEY (`siret`),
FOREIGN KEY (`idAdresse`) REFERENCES `ADRESSE` (`idAdresse`)
);

DROP TABLE IF EXISTS `PIECE_FOURNIT`;

CREATE TABLE `PIECE_FOURNIT` (
  `idPiece` varchar(10) NOT NULL,
  `siret` varchar(15) NOT NULL,
  `prixUnitaire` float(6) NOT NULL,
  `delai` int NOT NULL COMMENT 'en jours',
  PRIMARY KEY (`idPiece`, `siret`),
  FOREIGN KEY (`idPiece`) REFERENCES `PIECE` (`idPiece`),
  FOREIGN KEY (`siret`) REFERENCES `FOURNISSEUR` (`siret`)
);

DROP TABLE IF EXISTS `STOCK_PIECE`;

CREATE TABLE `STOCK_PIECE` (
  `idMagasin` int NOT NULL,
  `idPiece` varchar(10) NOT NULL,
  `quantite` int NOT NULL,
  PRIMARY KEY (`idMagasin`, `idPiece`),
  FOREIGN KEY (`idMagasin`) REFERENCES `MAGASIN` (`idMagasin`),
  FOREIGN KEY (`idPiece`) REFERENCES `PIECE` (`idPiece`)
);

DROP TABLE IF EXISTS `STOCK_VELO`;

CREATE TABLE `STOCK_VELO` (
  `idMagasin` int NOT NULL,
  `idModele` int NOT NULL,
  `quantite` int NOT NULL,
  PRIMARY KEY (`idMagasin`, `idModele`),
  FOREIGN KEY (`idMagasin`) REFERENCES `MAGASIN` (`idMagasin`),
  FOREIGN KEY (`idModele`) REFERENCES `MODELE` (`idModele`)
);


DROP TABLE IF EXISTS `CLIENTELE`;

CREATE TABLE `CLIENTELE` (
`idClient` int NOT NULL AUTO_INCREMENT,
`idParticulier` int DEFAULT NULL,
`siret` varchar(15) DEFAULT NULL,
PRIMARY KEY (`idClient`),
FOREIGN KEY (`idParticulier`) REFERENCES `PARTICULIER` (`idParticulier`),
FOREIGN KEY (`siret`) REFERENCES `ENTREPRISE` (`siret`)
);


DROP TABLE IF EXISTS `PARTICULIER`;

CREATE TABLE `PARTICULIER` (
`idParticulier` int NOT NULL AUTO_INCREMENT,
`nom_client` varchar(40) NOT NULL,
`prenom_client` varchar(20) NOT NULL,
`idAdresse` int NOT NULL,
`no_tel` varchar(10),
`courriel` varchar(50),
`idProgramme` int DEFAULT NULL,
PRIMARY KEY (`idParticulier`),
FOREIGN KEY (`idAdresse`) REFERENCES `ADRESSE` (`idAdresse`),
FOREIGN KEY (`idProgramme`) REFERENCES `FIDELIO` (`idProgramme`)
);

DROP TABLE IF EXISTS `ENTREPRISE`;

CREATE TABLE `ENTREPRISE` (
`siret` varchar(15) NOT NULL,
`nom_entreprise` varchar(40) NOT NULL,
`courriel` varchar(40) DEFAULT NULL,
`idAdresse` int NOT NULL,
`idRabais` int DEFAULT NULL,
PRIMARY KEY (`siret`),
FOREIGN KEY (`idRabais`) REFERENCES `RABAIS` (`idRabais`),
FOREIGN KEY (`idAdresse`) REFERENCES `ADRESSE` (`idAdresse`)
);

DROP TABLE IF EXISTS `LIGNE_COMMANDE_PIECE`;

CREATE TABLE `LIGNE_COMMANDE_PIECE` (
  `idCommande` int NOT NULL,
  `idPiece` varchar(10) DEFAULT NULL,
  `quantite` int NOT NULL,
  `prix_ligne` float(6) DEFAULT NULL,
  FOREIGN KEY (`idCommande`) REFERENCES `COMMANDE` (`idCommande`),
  FOREIGN KEY (`idPiece`) REFERENCES `PIECE` (`idPiece`)
);

DROP TABLE IF EXISTS `LIGNE_COMMANDE_MODELE`;

CREATE TABLE `LIGNE_COMMANDE_MODELE` (
  `idCommande` int NOT NULL,
  `idModele` int DEFAULT NULL,
  `quantite` int NOT NULL,
  `prix_ligne` float(6) DEFAULT NULL,
  FOREIGN KEY (`idCommande`) REFERENCES `COMMANDE` (`idCommande`),
  FOREIGN KEY (`idModele`) REFERENCES `MODELE` (`idModele`)
);

DROP TABLE IF EXISTS `COMMANDE`;

CREATE TABLE `COMMANDE` (
  `idCommande` int NOT NULL AUTO_INCREMENT,
  `idMagasin` int NOT NULL,
  `idEmploye` int NOT NULL,
  `idClient` int NOT NULL,
  `prix_total` float(6) DEFAULT NULL COMMENT 'en €',
  `dateCommande` date NOT NULL,
  `adresseLivraison` int DEFAULT NULL,
  `dateLivraison` date DEFAULT NULL,
  `idRabais` int DEFAULT NULL,
  PRIMARY KEY (`idCommande`),
  FOREIGN KEY (`idMagasin`) REFERENCES `MAGASIN` (`idMagasin`),
  FOREIGN KEY (`idEmploye`) REFERENCES `EMPLOYE` (`idEmploye`),
  FOREIGN KEY (`idClient`) REFERENCES `CLIENTELE` (`idClient`),
  FOREIGN KEY (`adresseLivraison`) REFERENCES `ADRESSE` (`idAdresse`)
);

DROP TABLE IF EXISTS `FIDELIO`;

CREATE TABLE `FIDELIO` (
  `idProgramme` int NOT NULL,
  `description` varchar(100) DEFAULT NULL,
  `cout` float(6) NOT NULL COMMENT 'en €',
  `duree` int DEFAULT NULL COMMENT 'en année',
  `rabais` float(5,2) NOT NULL COMMENT 'en %',
  PRIMARY KEY (`idProgramme`)
);

DROP TABLE IF EXISTS `RABAIS`;

CREATE TABLE `RABAIS` (
  `idRabais` INT NOT NULL AUTO_INCREMENT,
  `quantiteMinPieces` INT DEFAULT NULL,
  `quantiteMinModeles` INT DEFAULT NULL,
  `rabais` FLOAT(5,2) DEFAULT NULL COMMENT 'en %',
  PRIMARY KEY (`idRabais`)
);

DROP TABLE IF EXISTS `ADHESION`;

CREATE TABLE `ADHESION` (
  `idAdhesion` int NOT NULL AUTO_INCREMENT,
  `idClient` int NOT NULL,
  `idProgramme` int NOT NULL,
  `dateDebut` date NOT NULL,
  PRIMARY KEY (`idAdhesion`),
  FOREIGN KEY (`idClient`) REFERENCES `CLIENT_PARTICULIER` (`idClient`),
  FOREIGN KEY (`idProgramme`) REFERENCES `PROGRAMME_FIDELITE` (`idProgramme`)
);

INSERT INTO `FIDELIO` (`idProgramme`, `description`, `cout`, `duree`, `rabais`) 
VALUES (1, 'Fidélio', '15', 1, '5') ,(2, 'Fidélio Or', '25', 2, '8'), (3, 'Fidélio Platine', '60', 2, '10'), (4, 'Fidélio Max', '100', 3, '12');

INSERT INTO `MAGASIN` (`idMagasin`, `idGerant`, `idAdresse`, `chiffre_affaire`) VALUES ('1', '3', '3', '1 503 000');
INSERT INTO `MAGASIN` (`idMagasin`, `idGerant`, `idAdresse`, `chiffre_affaire`) VALUES ('2', '4', '6', '1 115 000');

INSERT INTO `ADRESSE` (`numero`, `rue`, `ville`) VALUES ('8', 'Avenue des Lilas', 'Versailles'),
('12', 'Rue du Pape', 'Cergy'), ('1', 'Place Vendôme', 'Paris I'), ('24', 'Route de la Reine', 'Montrouge'), ('13', 'Rue du Bonheur', 'Montreuil'),
('7', 'Place de la Porte Maillot', 'Paris XVII'), ('7B', 'Rue Lalo', 'Paris XVI'), ('21', 'Avenue Charles de Gaulle', 'Neuilly-sur-Seine'),
('9', 'Boulevard Haussmann', 'Paris IX'), ('15', 'Rue de la Paix', 'Paris II'), ('3', 'Avenue des Champs-Élysées', 'Paris VIII'), ('28', 'Rue de Rivoli', 'Paris I'),
('6', 'Boulevard Saint-Germain', 'Paris VI'), ('17', 'Rue de la Pompe', 'Paris XVI'), ('10', 'Avenue Montaigne', 'Paris VIII'), ('5', 'Boulevard Saint-Michel', 'Paris V');

INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`, `prime`) VALUES ('1', 'Dupont','Jean', '1', 'Vendeur', '2100','800');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`) VALUES ('2', 'Durand','Marie', '2', 'Vendeur', '1800');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`, `prime`) VALUES ('3', 'Bardin','Malo', '2', 'Gérant', '9200','3000');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`, `prime`) VALUES ('4', 'Tarlet','Quentin', '1', 'Gérant', '9200','3000');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`) VALUES ('5', 'Öztürk','Pierre-Antoine', '1', 'Vendeur', '1800');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`, `prime`) VALUES ('6', 'Laroche','Camille', '2', 'Vendeur', '2100','800');
INSERT INTO `EMPLOYE` (`idEmploye`, `nomEmploye`, `prenomEmploye`, `idMagasin`, `position`, `salaire`) VALUES ('7', 'Lecaillou','Louise', '1', 'Vendeur', '1800');

INSERT INTO `PIECE` (`idPiece`,`libelle`)
VALUES
('C32','Cadre'), ('C34','Cadre'), ('C76','Cadre'), ('C43','Cadre'), ('C43f','Cadre'), ('C44f','Cadre'), ('C01','Cadre'), ('C02','Cadre'), ('C15','Cadre'), ('C87','Cadre'),
('C87f','Cadre'), ('C25','Cadre'), ('C26','Cadre'), 
('G7','Guidon'), ('G9','Guidon'), ('G12','Guidon'),
('S88','Selle'), ('S37','Selle'), ('S35','Selle'), ('S02','Selle'), ('S03','Selle'), ('S36','Selle'), ('S34','Selle'), ('S87','Selle'),
('F3','Frein'), ('F9','Frein'),
('DV133','Dérailleur avant'), ('DV17','Dérailleur avant'), ('DV87','Dérailleur avant'), ('DV57','Dérailleur avant'), ('DV15','Dérailleur avant'), ('DV41','Dérailleur avant'), ('DV132','Dérailleur avant'),
('DR56','Dérailleur arrière'), ('DR87','Dérailleur arrière'), ('DR86','Dérailleur arrière'), ('DR23','Dérailleur arrière'), ('DR76','Dérailleur arrière'), ('DR52','Dérailleur arrière'),
('R45','Roue avant'), ('R48','Roue avant'), ('R12','Roue avant'), ('R19','Roue avant'), ('R1','Roue avant'), ('R11','Roue avant'), ('R44','Roue avant'),
('R46','Roue arrière'), ('R47','Roue arrière'), ('R32','Roue arrière'), ('R18','Roue arrière'), ('R2','Roue arrière'),
('R02','Réflecteur'), ('R09','Réflecteur'), ('R10','Réflecteur'),
('O2','Ordinateur'), ('O4','Ordinateur'),
('S01','Panier'), ('S05','Panier'), ('S74','Panier'), ('S73','Panier'),
('P12','Pédalier'), ('P34','Pédalier'), ('P1','Pédalier'), ('P15','Pédalier');


INSERT INTO `ASSEMBLAGE` (`idModele`,`idPiece`)
 VALUES
(101, 'C32'), (101, 'G7'), (101, 'F3'), (101, 'S88'), (101, 'DV133'), (101, 'DR56'), (101, 'R45'), (101, 'R46'), (101, 'P12'), (101, 'O2'),
(102, 'C34'), (102, 'G7'), (102, 'F3'), (102, 'S88'), (102, 'DV17'), (102, 'DR87'), (102, 'R48'), (102, 'R47'), (102, 'P12'),
(103, 'C76'), (103, 'G7'), (103, 'F3'), (103, 'S88'), (103, 'DV17'), (103, 'DR87'), (103, 'R48'), (103, 'R47'), (103, 'P12'), (103, 'O2'),
(104, 'C76'), (104, 'G7'), (104, 'F3'), (104, 'S88'), (104, 'DV87'), (104, 'DR86'), (104, 'R12'), (104, 'R32'), (104, 'P12'),
(105, 'C43'), (105, 'G9'), (105, 'F9'), (105, 'S37'), (105, 'DV57'), (105, 'DR86'), (105, 'R19'), (105, 'R18'), (105, 'R02'), (105, 'P34'),
(106, 'C44f'), (106, 'G9'), (106, 'F9'), (106, 'S35'), (106, 'DV57'), (106, 'DR86'), (106, 'R19'), (106, 'R18'), (106, 'R02'), (106, 'P34'),
(107, 'C43'), (107, 'G9'), (107, 'F9'), (107, 'S37'), (107, 'DV57'), (107, 'DR87'), (107, 'R19'), (107, 'R18'), (107, 'R02'), (107, 'P34'), (107, 'O4'),
(108, 'C43f'), (108, 'G9'), (108, 'F9'), (108, 'S35'), (108, 'DV57'), (108, 'DR87'), (108, 'R19'), (108, 'R18'), (108, 'R02'), (108, 'P34'), (108, 'O4'),
(109, 'C01'), (109, 'G12'), (109, 'S02'), (109, 'R1'), (109, 'R2'), (109, 'R09'), (109, 'P1'), (109, 'S01'),
(110, 'C02'), (110, 'G12'), (110, 'S03'), (110, 'R1'), (110, 'R2'), (110, 'R09'), (110, 'P1'), (110, 'S05'),
(111, 'C15'), (111, 'G12'), (111, 'F9'), (111, 'S36'), (111, 'DV15'), (111, 'DR23'), (111, 'R11'), (111, 'R12'), (111, 'R10'), (111, 'P15'), (111, 'S74'),
(112, 'C87'), (112, 'G12'), (112, 'F9'), (112, 'S36'), (112, 'DV41'), (112, 'DR76'), (112, 'R11'), (112, 'R12'), (112, 'R10'), (112, 'P15'), (112, 'S74'),
(113, 'C87f'), (113, 'G12'), (113, 'F9'), (113, 'S34'), (113, 'DV41'), (113, 'DR76'), (113, 'R11'), (113, 'R12'), (113, 'R10'), (113, 'P15'), (113, 'S73'),
(114, 'C25'), (114, 'G7'), (114, 'F3'), (114, 'S87'), (114, 'DV132'), (114, 'DR52'), (114, 'R44'), (114, 'R47'), (114, 'P15'),
(115, 'C26'), (115, 'G7'), (115, 'F3'), (115, 'S87'), (115, 'DV133'), (115, 'DR52'), (115, 'R44'),(115, 'R47'),(115, 'P12');

 INSERT INTO `MODELE` (`idModele`,`nom`,`grandeur`,`prix` ,`ligne` ,`date_debut`,`date_fin`)
 VALUES
(101, 'Kilimandjaro', 'Adultes', 569,'VTT','2014-01-02','2020-01-02'),
(102, 'NorthPole', 'Adultes', 329,'VTT','2020-01-02', null),
(103, 'MontBlanc', 'Jeunes', 399,'VTT','2015-01-02','2022-01-02'),
(104, 'Hooligan', 'Jeunes', 199,'VTT','2022-01-02', null),
(105, 'Orléans', 'Hommes', 229,'Vélo de course','2018-01-02','2024-01-02'),
(106, 'Orléans', 'Dames', 229,'Vélo de course','2018-01-02','2024-01-02'),
(107, 'BlueJay', 'Hommes', 349,'Vélo de course','2024-01-02',null),
(108, 'BlueJay', 'Dames', 349,'Vélo de course','2024-01-02',null),
(109, 'Trail Explorer', 'Filles', 129,'Classique','2020-01-02',null),
(110, 'Trail Explorer', 'Garçons', 129,'Classique','2020-01-02',null),
(111, 'Night Hawk', 'Jeunes', 189,'Classique','2019-01-02','2023-01-02'),
(112, 'Tierra Verde', 'Hommes', 199,'Classique','2022-01-02',null),
(113, 'Tierra Verde', 'Dames', 199,'Classique','2022-01-02',null),
(114, 'Mud Zinger I', 'Jeunes', 279,'BMX','2018-01-02','2023-01-02'),
(115, 'Mud Zinger II', 'Adultes', 359,'BMX','2019-01-02','2023-01-02');

INSERT INTO `FOURNISSEUR` (`siret`,`nom_entreprise`,`idAdresse`,`categorie`)
VALUES
('12002701600357', 'SuperPieces', 1, 'Très bon'),
('12002701600353', 'Fournitou', 2, 'Bon'),
('12002701600360', 'Vive Les Roues', 4, 'Moyen'),
('12002701600323', 'Alabourre', 5, 'Mauvais');

INSERT INTO `PIECE_FOURNIT` (`idPiece`, `siret`, `prixUnitaire`, `delai`)
VALUES
-- SuperPieces fournit les cadres, les guidons, les selles, les paniers et les réflecteurs
('C32', '12002701600357', 68, 3), ('C34', '12002701600357', 68, 3), ('C76', '12002701600357', 68, 3),
('C43', '12002701600357', 68, 3), ('C43f', '12002701600357', 68, 3), ('C44f', '12002701600357', 68, 3),
('C01', '12002701600357', 68, 3), ('C02', '12002701600357', 68, 3), ('C15', '12002701600357', 68, 3),
('C87', '12002701600357', 68, 3), ('C87f', '12002701600357', 55, 3), ('C25', '12002701600357', 68, 3),
('C26', '12002701600357', 68, 3), ('G7', '12002701600357', 40, 3), ('G9', '12002701600357', 40, 3),
('G12', '12002701600357', 40, 3), ('S88', '12002701600357', 10, 3), ('S37', '12002701600357', 10, 3),
('S35', '12002701600357', 10, 3), ('S02', '12002701600357', 10, 3), ('S03', '12002701600357', 10, 3),
('S36', '12002701600357', 10, 3), ('S34', '12002701600357', 10, 3), ('S87', '12002701600357', 10, 3),
('R02', '12002701600357', 10, 3), ('R09', '12002701600357', 10, 3), ('R10', '12002701600357', 10, 3),
('S01', '12002701600357', 10, 3), ('S05', '12002701600357', 10, 3), ('S74', '12002701600357', 10, 3),
('S73', '12002701600357', 10, 3),
-- Fournitou fournit les freins, les dérailleurs avant et arrière et les pédaliers
('F3', '12002701600353', 10, 4), ('F9', '12002701600353', 10, 4), ('DV133', '12002701600353', 10, 4),
('DV17', '12002701600353', 10, 4), ('DV87', '12002701600353', 10, 4), ('DV57', '12002701600353', 10, 4),
('DR56', '12002701600353', 10, 4), ('DR87', '12002701600353', 10, 4), ('DR86', '12002701600353', 10, 4),
('DR23', '12002701600353', 10, 4), ('DR76', '12002701600353', 10, 4), ('DR52', '12002701600353', 10, 4),
('P12', '12002701600353', 10, 4), ('P34', '12002701600353', 10, 4), ('P1', '12002701600353', 10, 4),
('P15', '12002701600353', 10, 4),
-- Vive Les Roues fournit toutes les roues
('R45', '12002701600360', 40, 5), ('R48', '12002701600360', 40, 5), ('R12', '12002701600360', 40, 5),
('R19', '12002701600360', 40, 5), ('R1', '12002701600360', 40, 5), ('R11', '12002701600360', 40, 5),
('R44', '12002701600360', 40, 5), ('R46', '12002701600360', 40, 5), ('R47', '12002701600360', 40, 5),
('R32', '12002701600360', 40, 5), ('R18', '12002701600360', 40, 5), ('R2', '12002701600360', 40, 5),
-- Alabourre fournit les ordinateurs, des cadres et des guidons
('O2', '12002701600323', 55, 6), ('O4', '12002701600323', 55, 6),
('C87f', '12002701600323', 55, 6), ('C25', '12002701600323', 55, 6), ('C26', '12002701600323', 55, 6),
('G7', '12002701600323', 55, 6), ('G9', '12002701600323', 55, 6), ('G12', '12002701600323', 55, 6);

INSERT INTO `STOCK_PIECE` (idMagasin, idPiece, quantite)
SELECT 1, idPiece, FLOOR(RAND() * 22) 
FROM PIECE
ORDER BY RAND();
INSERT INTO `STOCK_PIECE` (idMagasin, idPiece, quantite)
SELECT 2, idPiece, FLOOR(RAND() * 22) 
FROM PIECE
ORDER BY RAND();

INSERT INTO `CLIENTELE` (idParticulier, siret)
VALUES
(1, NULL), (2, NULL), (3, NULL), (4, NULL), (5, NULL),
(NULL, '12002701600371'), (NULL, '12002701600372'),  (NULL, '12002701600373'), (NULL, '12002701600374'), (NULL, '12002701600375');
    
INSERT INTO `PARTICULIER` (`idParticulier`,`nom_client`,`prenom_client`,`idAdresse`,`no_tel`, `courriel`, `idProgramme`)
VALUES
(1, 'Durand', 'Jean', 7, '0601020304', 'jean.durand@gmail.com', null),
(2, 'Martin', 'Sophie', 8, '0612345678', 'sophie.martin@gmail.com', null),
(3, 'Dubois', 'Pierre', 9, '0654321098', 'pierre.dubois@gmail.com', null),
(4, 'Lefebvre', 'Marie', 10, '0678901234', 'marie.lefebvre@gmail.com', null),
(5, 'Moreau', 'Julien', 11, '0698765432', 'julien.moreau@gmail.com', null);

INSERT INTO `ENTREPRISE` (`siret`, `nom_entreprise`, `idAdresse`, `idRabais`)
VALUES 
('12002701600371', 'Ville de Paris', 12, 1),
('12002701600372', 'Akkodis', 13, 1),
('12002701600373', 'Cycloclub Paris XVI', 14, 1),
('12002701600374', 'RideMaster', 15, 1),
('12002701600375', 'Google France', 16, 1);

INSERT INTO `COMMANDE` (idMagasin, idEmploye, idClient, prix_total, dateCommande, adresseLivraison	)
VALUES
(1, 1, 1, 589, '2024-02-18', 7);

INSERT INTO LIGNE_COMMANDE_PIECE (idCommande, idPiece, quantite, prix_ligne)
VALUES
(1, 'F3', 2, 2 * (SELECT prixUnitaire FROM PIECE_FOURNIT WHERE idPiece = 'F3'));

INSERT INTO LIGNE_COMMANDE_MODELE (idCommande, idModele, quantite, prix_ligne)
VALUES
(1, 101, 1, (SELECT prix FROM MODELE WHERE idModele = 101));
