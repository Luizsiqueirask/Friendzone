		----------------------------------------------------------
				 /* Creating Database */
----------------------------------------------------------

-- Create

CREATE DATABASE Friendzone;

-- Drop

DROP DATABASE Friendzone

DROP TABLE Contacts
DROP TABLE Pictures;
DROP TABLE Countries;
DROP TABLE Flags;
DROP TABLE Friends;
DROP TABLE Friendship;
DROP TABLE States;
DROP TABLE Person;

-- User

use Friendzone;

----------------------------------------------------------
				  /* Creating Tables */
----------------------------------------------------------

-- Creating Pictures	

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Pictures' and type in (N'U'))

CREATE TABLE Pictures(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] NVARCHAR(255) NOT NULL,
	[Path] NVARCHAR(255) NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Pictures - Exists this table !!!'
GO

-- Creating Flags

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Flags' and type in (N'U'))

CREATE TABLE Flags(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] NVARCHAR(255) NOT NULL,
	[Path] NVARCHAR(255) NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Flags - Exists this table !!!'
GO

-- Creating Contacts

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Contacts' and type in (N'U'))

CREATE TABLE Contacts(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Email NVARCHAR(250) NOT NULL,
	Mobile NVARCHAR(250) NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Contacts - Exists this table !!!'
GO

-- Creating Countries

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Countries' and type in (N'U'))

CREATE TABLE Countries (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) UNIQUE NOT NULL,
	FlagId INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
); 
ELSE
	PRINT 'Countries - Exists this table !!!'
GO

-- Creating States

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'States' and type in (N'U'))

CREATE TABLE States (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) UNIQUE NOT NULL,
	FlagId INT NOT NULL,
	CountryId INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (FlagId) REFERENCES Flags(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries(Id),
); 
ELSE
	PRINT 'States - Exists this table !!!'
GO

-- Creating Person

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Person' and type in (N'U'))

CREATE TABLE Person (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(250) NOT NULL,
	LastName NVARCHAR(250) NOT NULL,
	Birthday DATETIME NOT NULL,
	Age INT NOT NULL,
	PictureId INT NOT NULL,
	CountryId INT UNIQUE NOT NULL,
	ContactId INT UNIQUE NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES Pictures(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries(Id),
	FOREIGN KEY (ContactId) REFERENCES Contacts(Id)
);
ELSE
	PRINT 'Person - Exists this table !!!'
GO

-- Creating Friends

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Friends' and type in (N'U'))

CREATE TABLE Friends (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(250) NOT NULL,
	LastName NVARCHAR(250) NOT NULL,
	Birthday DATETIME NOT NULL,
	Age INT NOT NULL,
	PictureId INT NOT NULL,
	CountryId INT UNIQUE NOT NULL,
	ContactId INT UNIQUE NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES Pictures(Id),
	FOREIGN KEY (CountryId) REFERENCES Countries(Id),
	FOREIGN KEY (ContactId) REFERENCES Contacts(Id),
);
ELSE
	PRINT 'Friends - Exists this table !!!'
GO

-- Creating Friendship

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Friendship' and type in (N'U'))

CREATE TABLE Friendship (
	-- Id INT IDENTITY(1,1) NOT NULL,
	PersonId INT UNIQUE NOT NULL,
	FriendId INT PRIMARY KEY NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PersonId) REFERENCES Person (Id),
    FOREIGN KEY (FriendId) REFERENCES Friends (Id)
);
ELSE
	PRINT 'Friendship - Exists this table !!!'
GO

----------------------------------------------------------
				  /* Listing Tables */
----------------------------------------------------------

-- Listing Pictures

SELECT * FROM Pictures;

-- Listing Flags

SELECT * FROM Flags;

-- Listing Contacts

SELECT * FROM Contacts;

-- Listing Countries

SELECT * FROM Countries;

-- Listing States

SELECT * FROM States;

-- Listing Person

SELECT * FROM Person;

-- Listing Friend

SELECT * FROM Friends;

-- Listing Friendship

SELECT * FROM Friendship; 
GO;


----------------------------------------------------------
			  /* Crud Procedure Operation */
----------------------------------------------------------


/* ********************* Country ********************* */

/* List */

CREATE PROCEDURE ListedCountry
	-- @CountryId INT,
	-- @FlagId INT
AS BEGIN
	SELECT
	-- Country	
		c1.Id,
		c1.[Label],
		c1.FlagId,
	-- Flag
		f1.Id,
		f1.Symbol,
		f1.[Path]
	FROM Countries c1
	LEFT JOIN Flags f1
	ON f1.Id = c1.FlagId
	ORDER BY c1.[Label];
END
GO;

/* Detail */

CREATE PROCEDURE DetailedCountry
	@Id INT
AS BEGIN
	SELECT 
	-- Country	
		c1.Id,
		c1.[Label],
		c1.FlagId,
	-- Flag
		f1.Id,
		f1.Symbol,
		f1.[Path]
	FROM Countries c1
	LEFT JOIN Flags f1
	ON f1.Id = c1.FlagId
	WHERE c1.Id = @Id;
END
GO;

/* Create */
					
CREATE PROCEDURE CreatedCountry
	-- Country
	@Label NVARCHAR(255),
	@FlagId INT,
	-- Flag
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255)
AS BEGIN	
	-- Country
	INSERT INTO Countries([Label], FlagId) 
	Values (@Label, @FlagId);
	-- Flag
	INSERT INTO Flags([Symbol], [Path]) 
	VALUES (@Symbol, @Path)
END
GO;

/* Update */

CREATE PROCEDURE UpdatedCountry
	-- Country
	@Id INT,
	@Label NVARCHAR(250),
	@FlagId INT,
	-- Flag
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255)
AS BEGIN
	-- Flag
	UPDATE Flags SET [Symbol] = @Symbol, [Path] = @Path	
    -- WHERE Id = @FlagId
	-- Country
	UPDATE Countries SET [Label] = @Label, FlagId = @FlagId
	WHERE Id = @Id
END
GO;

/* Delete */

CREATE PROCEDURE DeletedCountry
	@Id INT
AS BEGIN
	-- Country
	DELETE c1 FROM Countries c1
	-- Flag
	LEFT JOIN Flags f1
	ON f1.Id = c1.FlagId
	WHERE c1.Id = @Id
END 
GO;


/* ********************* States ********************* */

/* List */

CREATE PROCEDURE ListedStates
	-- @Id INT
AS BEGIN
	-- States
	SELECT * FROM States s1
	-- Flags
	LEFT JOIN Flags f1
	ON f1.Id = s1.FlagId
	-- Country
	LEFT JOIN Countries c1
	ON c1.Id = s1.CountryId
END 
GO;

/* Detail */

CREATE PROCEDURE DetailedStates	
	-- Flag States 
	@Id INT
AS BEGIN
	-- States
	SELECT 
		s1.Id,
		s1.[Label],
		s1.FlagId,
		s1.CountryId, 
		f1.Id,
		f1.[Symbol],
		f1.[Path],
		c1.Id,
		c1.[Label],
		c1.FlagId
	FROM States s1
	-- Flags
	LEFT JOIN Flags f1
	ON f1.Id = s1.FlagId
	-- Country
	LEFT JOIN Countries c1
	ON c1.Id = s1.CountryId
	WHERE c1.Id = @Id
END
GO;

/* Create */

CREATE PROCEDURE CreatedStates
	-- States
	@Label NVARCHAR(250),
	@FlagId INT,
	@CountryId INT,
	-- Flag 
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255)

AS BEGIN
	-- States
	INSERT INTO States([Label], FlagId, CountryId) 
	VALUES (@Label, @FlagId, @CountryId);
	-- Flag
	INSERT INTO Flags([Symbol], [Path]) 
	VALUES (@Symbol, @Path);
END 
GO;

/* Update */
		
CREATE PROCEDURE UpdatedStates
	-- States
	@Id INT,
	@Label NVARCHAR(250),
	@FlagId INT,
	@CountryId INT,
	-- Flag
	@IdFlag INT,
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255)
	
AS BEGIN
	-- Flag States
	UPDATE Flags SET [Symbol] = @Symbol, [Path] = @Path
	WHERE Id = @IdFlag
	-- States
	UPDATE States SET [Label] = @Label, FlagId = @FlagId, CountryId = @CountryId
	WHERE Id = @Id
END 
GO;

/* Delete */

CREATE PROCEDURE DeletedStates
	@Id INT
AS BEGIN
	-- States
	DELETE s1 FROM States s1
	LEFT JOIN Flags f1
	-- Flag
	ON f1.Id = s1.FlagId
	-- Country
	LEFT JOIN Countries c1
	ON c1.Id = s1.CountryId
	WHERE s1.Id = @Id
END
GO;


/* ********************* Person ********************* */

/* List */

CREATE PROCEDURE ListedPerson
	-- @Id INT
AS BEGIN
	-- Person
	SELECT * FROM Person p1
	-- Pictures
	LEFT JOIN Pictures p2
	ON p2.Id = p1.Id
	LEFT JOIN Countries c1
	-- Country
	ON c1.Id = p1.CountryId
	-- Contacts
	LEFT JOIN Contacts c2
	ON c2.Id = c1.Id
END
GO;

/* Detail */

CREATE PROCEDURE DetailedPerson
	-- Person
	@Id INT
AS BEGIN	
	SELECT
		-- Person
		p1.Id,
		p1.FirstName,
		p1.LastName,
		p1.Age,
		p1.Birthday,
		-- Pictures
		p2.Id,
		p2.[Symbol],
		p2.[Path],
		-- Country
		c1.Id,
		c1.[Label],
		-- Contacts
		c2.Id,
		c2.Email,
		c2.Mobile
	FROM Person p1
	LEFT JOIN Pictures p2
	ON p1.Id = p1.PictureId
	LEFT JOIN Countries c1
	ON c1.Id = p1.CountryId
	LEFT JOIN Contacts c2
	ON c2.Id = p1.ContactId
	WHERE p1.Id = @Id
END
GO;

/* Create */

CREATE PROCEDURE CreatedPerson
	-- Pictures
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255),
	-- Contacts
	@ContactId INT,
	@Email NVARCHAR(255),
	@Mobile NVARCHAR(255),
	-- Person
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Birthday DATETIME,
	@Age INT,
	@PictureId INT,
	-- Country
	@CountryId INT
AS BEGIN
	-- Pictures
	INSERT INTO Pictures([Symbol], [Path]) 
	VALUES (@Symbol, @Path);
	-- Country
	SELECT c2.Id, c2.[Label], c2.FlagId FROM Countries c2
	WHERE c2.Id = @ContactId;	
	-- Contacts
	INSERT INTO Contacts(Email, Mobile) 
	VALUES (@Email, @Mobile);
	-- Person
	INSERT INTO Person(FirstName, LastName, Birthday, Age, PictureId, CountryId, ContactId)
	VALUES (@FirstName, @LastName, @Birthday, @Age, @PictureId, @CountryId, @ContactId);
END
GO;

/* Update */

CREATE PROCEDURE UpdatedPerson
	-- Pictures
	@IdPictures INT,
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255),
	-- Country
	@IdCountry INT,
	-- Contacts
	@IdContact INT,
	@Email NVARCHAR(255),
	@Mobile NVARCHAR(255),
	-- Person
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Birthday DATETIME,
	@Age INT,
	@PictureId INT,
	@CountryId INT,
	@ContactId INT
AS BEGIN
	-- Pictures
	UPDATE Pictures SET [Symbol] = @Symbol, [Path] = @Path
	WHERE Id = @PictureId;
	-- Country
	SELECT c2.Id, c2.[Label], c2.FlagId FROM Countries c2
	WHERE c2.Id = @CountryId;
	-- Contacts
	UPDATE Contacts SET Email = @Email, Mobile = @Mobile 
	WHERE Id = @ContactId;	
	-- Person
	UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Birthday = @Birthday, Age = @Age,
	PictureId = @PictureId, CountryId = @CountryId, ContactId = @ContactId
	WHERE Id = @IdPictures;
END
GO;

/* Delete */

CREATE PROCEDURE DeletedPerson
	@Id INT
AS BEGIN
	-- Person
	DELETE p1 FROM Person p1
	-- Country
	LEFT JOIN  Countries c1
	ON c1.Id = p1.Id
	-- Pictures
	LEFT JOIN Pictures p2
	ON p2.Id = p1.Id
	-- Contacts
	LEFT JOIN Contacts c2
	ON c2.Id = p1.ContactId
	WHERE p1.Id = @Id;
END
GO;

/* ********************* Friends ********************* */


/* List */

CREATE PROCEDURE ListedFriends
	-- @Id INT
AS BEGIN
	-- Friend
	SELECT * FROM Friends f1
	-- Pictures
	LEFT JOIN Pictures p2
	ON p2.Id = f1.Id
	LEFT JOIN Countries c1
	-- Country
	ON c1.Id = f1.CountryId
	-- Contacts
	LEFT JOIN Contacts c2
	ON c2.Id = c1.Id
END
GO;

/* Detail */

CREATE PROCEDURE DetailedFriends
	@Id INT
AS BEGIN
	SELECT
		-- Friend
		f1.Id,
		f1.FirstName,
		f1.LastName,
		f1.Age,
		f1.Birthday,
		-- Pictures
		p1.Id,
		p1.[Symbol],
		p1.[Path],
		-- Country
		c1.Id,
		c1.[Label],
		-- Contacts
		c2.Id,
		c2.Email,
		c2.Mobile
	FROM Friends f1
	LEFT JOIN Pictures p1
	ON p1.Id = f1.PictureId
	LEFT JOIN Countries c1
	ON c1.Id = f1.CountryId
	LEFT JOIN Contacts c2
	ON c2.Id = f1.ContactId
	WHERE f1.Id = @Id
END 
GO;

/* Create */

CREATE PROCEDURE CreatedFriends
	-- Pictures
	@IdPictures INT,
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255),
	-- Country
	@IdCountry INT,
	-- Contacts
	@IdContact INT,
	@Email NVARCHAR(255),
	@Mobile NVARCHAR(255),
	-- Friends
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Birthday DATETIME,
	@Age INT,
	@PictureId INT,
	@CountryId INT,
	@ContactId INT

AS BEGIN
	-- Pictures
	UPDATE Pictures SET [Symbol] = @Symbol, [Path] = @Path
	WHERE Id = @PictureId;
	-- Country
	SELECT c2.Id, c2.[Label], c2.FlagId FROM Countries c2
	WHERE c2.Id = @CountryId;
	-- Contacts
	UPDATE Contacts SET Email = @Email, Mobile = @Mobile 
	WHERE Id = @ContactId;	
	-- Friends
	UPDATE Friends SET FirstName = @FirstName, LastName = @LastName, Birthday = @Birthday, Age = @Age,
	PictureId = @PictureId, CountryId = @CountryId, ContactId = @ContactId
	WHERE Id = @IdPictures;
END
GO;
	
/* Update */

CREATE PROCEDURE UpdatedFriends
	-- Pictures
	@IdPictures INT,
	@Symbol NVARCHAR(255),
	@Path NVARCHAR(255),
	-- Country
	@IdCountry INT,
	-- Contacts
	@IdContact INT,
	@Email NVARCHAR(255),
	@Mobile NVARCHAR(255),
	-- Friends
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Birthday DATETIME,
	@Age INT,
	@PictureId INT,
	@CountryId INT,
	@ContactId INT

AS BEGIN
	-- Pictures
	UPDATE Pictures SET [Symbol] = @Symbol, [Path] = @Path
	WHERE Id = @PictureId;
	-- Country
	SELECT c2.Id, c2.[Label], c2.FlagId FROM Countries c2
	WHERE c2.Id = @CountryId;
	-- Contacts
	UPDATE Contacts SET Email = @Email, Mobile = @Mobile 
	WHERE Id = @ContactId;	
	-- Friends
	UPDATE Friends SET FirstName = @FirstName, LastName = @LastName, Birthday = @Birthday, Age = @Age,
	PictureId = @PictureId, CountryId = @CountryId, ContactId = @ContactId
	WHERE Id = @IdPictures;
END
GO;

/* Delete */

CREATE PROCEDURE DeletedFriends
	@Id INT
AS BEGIN
	-- Person
	DELETE f1 FROM Friends f1
	-- Country
	LEFT JOIN  Countries c1
	ON c1.Id = f1.Id
	-- Pictures
	LEFT JOIN Pictures p2
	ON p2.Id = f1.Id
	-- Contacts
	LEFT JOIN Contacts c2
	ON c2.Id = f1.ContactId
	WHERE f1.Id = @Id;
END
GO;

/* ******************** Friendship ********************* */

/* List */

CREATE PROCEDURE ListedFriendship
	-- @PersonId INT,
	-- @FriendId INT
AS BEGIN
	SELECT * FROM Person p1
	LEFT JOIN Friends f1
	ON p1.Id = f1.Id
END
GO;

/* Detail */

CREATE PROCEDURE DetailedFriendship
	@Id INT
AS BEGIN
	SELECT 
		p1.Id,
		p1.FirstName,
		p1.LastName,
		p1.Age, 
		p1.Birthday,
		p1.PictureId,
		p1.CountryId,
		p1.ContactId,
		f1.Id,
		f1.FirstName,
		f1.LastName, 
		f1.Age, 
		f1.Birthday, 
		f1.PictureId, 
		f1.CountryId, 
		f1.ContactId
	FROM Person p1
	LEFT JOIN Friends f1
	ON p1.Id = f1.Id
	WHERE f1.Id = @Id
	ORDER BY p1.Id, f1.Id
END
GO;

/* Create */

CREATE PROCEDURE CreatedFriendship
	@PersonId AS INT,
	@FriendId AS INT
AS BEGIN
	-- Person
	SELECT p1.Id FROM Person p1
	WHERE p1.Id = @PersonId
	-- Friends
	SELECT p1.Id FROM Person p1
	WHERE p1.Id = @PersonId
	-- Friendship
	INSERT INTO Friendship(PersonId, FriendId) 
	VALUES (@PersonId, @FriendId);
END
GO;

/* Update */

CREATE PROCEDURE UpdatedFriendship
	@PersonId AS INT,
	@FriendId AS INT
AS BEGIN
	-- Person
	SELECT p1.Id FROM Person p1
	WHERE p1.Id = @PersonId
	-- Friends
	SELECT p1.Id FROM Person p1
	WHERE p1.Id = @PersonId
	-- Friendship
	INSERT INTO Friendship(PersonId, FriendId) 
	VALUES (@PersonId, @FriendId);
END
GO;

/* Delete */

CREATE PROCEDURE DeletedFriendship
	@Id INT
AS BEGIN
	-- Person
	DELETE p1 FROM Person p1
	LEFT JOIN  Friends f1
	ON p1.Id = f1.Id
	WHERE p1.Id = @Id;
END
GO;


--------------------------------------------	--------------
			  /* Running Procedure Store */
----------------------------------------------------------


/* ******************** Person ********************* */


EXEC ListedPerson;

EXEC CreatedPerson @Symbol	= 'MyPictures' ;



/* ******************** Country ********************* */

EXEC ListedCountry;

EXEC DetailedCountry @Id = 2;

EXEC CreatedCountry @Label = 'Brazil', @FlagId = 1, @Symbol = 'Brazil.jpg', @Path = '/Home/Luiz';

EXEC UpdatedCountry @Id = 1, @Label = 'Brazil', @FlagId = 1, @Symbol = 'Brazil.jpg', @Path = '/Home/Luiz';

EXEC DeletedCountry @Id = 2;