----------------------------------------------------------
				 /* Creating Database */
----------------------------------------------------------

-- Create

CREATE DATABASE Friendzone;

-- Drop

DROP DATABASE Friendzone;

DROP TABLE [dbo].[Contacts];
DROP TABLE [dbo].[Flags];
DROP TABLE [dbo].[Pictures];
DROP TABLE [dbo].[Countries];
DROP TABLE [dbo].[States];
DROP TABLE [dbo].[Person];
DROP TABLE [dbo].[Friends];
DROP TABLE [dbo].[Friendship];

-- User

use Friendzone;
								

----------------------------------------------------------
				  /* CREATE TABLES */
----------------------------------------------------------

-- Creating Contacts

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Contacts' and type in (N'U'))

CREATE TABLE [dbo].[Contacts](
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Email NVARCHAR(250) NOT NULL,
	Mobile NVARCHAR(250) NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Contacts - Exists this table !!!'
GO

-- Creating Pictures

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Pictures' and type in (N'U'))

CREATE TABLE [dbo].[Pictures](
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

CREATE TABLE [dbo].[Flags](
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] NVARCHAR(255) NOT NULL,
	[Path] NVARCHAR(255) NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Flags - Exists this table !!!'
GO

-- Creating Countries

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Countries' and type in (N'U'))

CREATE TABLE [dbo].[Countries] (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) NOT NULL,
	[FlagId] INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT FK_Flags_Countries_Id FOREIGN KEY (FlagId) REFERENCES [dbo].[Flags](Id)
); 
ELSE
	PRINT 'Countries - Exists this table !!!'
GO

-- Creating States

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'States' and type in (N'U'))

CREATE TABLE [dbo].[States] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) NOT NULL,
	[FlagId] INT NOT NULL,
	[CountryId] INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	-- FOREIGN KEY ([FlagId]) REFERENCES [dbo].[Flags](Id),
	-- FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries](Id)
); 
ELSE
	PRINT 'States - Exists this table !!!'
GO

-- Creating Person

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Person' and type in (N'U'))

CREATE TABLE [dbo].[Person] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FirstName] NVARCHAR(250) NOT NULL,
	[LastName] NVARCHAR(250) NOT NULL,
	[Birthday] DATE NOT NULL,
	[Age] INT NOT NULL,
	[PictureId] INT NOT NULL,
	[CountryId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	-- FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id),
	-- FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id),
	-- FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id)
);
ELSE
	PRINT 'Person - Exists this table !!!'
GO

-- Creating Friends

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Friends' and type in (N'U'))

CREATE TABLE [dbo].[Friends] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FirstName] NVARCHAR(250) NOT NULL,
	[LastName] NVARCHAR(250) NOT NULL,
	[Birthday] DATE NOT NULL,
	[Age] INT NOT NULL,
	[PictureId] INT NOT NULL,
	[CountryId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	-- FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id),
	-- FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id),
	-- FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id)
);

ELSE
	PRINT 'Friends - Exists this table !!!'
GO

-- Creating Friendship

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Friendship' and type in (N'U'))

CREATE TABLE [dbo].[Friendship](
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PersonId] INT  NOT NULL,
	[FriendsId] INT NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	-- PRIMARY KEY([PersonId], [FriendsId]),
	-- FOREIGN KEY (PersonId) REFERENCES [dbo].[Person](Id),
    -- FOREIGN KEY (FriendsId) REFERENCES [dbo].[Friends](Id)
);

ELSE
	PRINT 'Friendship - Exists this table !!!'
GO


----------------------------------------------------------
				  /* ALTER TABLES */
----------------------------------------------------------

-- Creating Contacts


ALTER TABLE [dbo].[Contacts]
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating Pictures

ALTER TABLE [dbo].[Pictures]
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating Flags

ALTER TABLE [dbo].[Flags]
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
	ADD Created_at DATE UNIQUE DEFAULT CURRENT_TIMESTAMP,
    ADD Updated_at DATE DEFAULT CURRENT_TIMESTAMP,
GO

-- Creating Countries

ALTER TABLE [dbo].[Countries]
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	-- Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    -- Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (FlagId) REFERENCES [dbo].[Flags](Id)
GO

-- Creating States

ALTER TABLE [dbo].[States] 
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	ADD Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    ADD Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY ([FlagId]) REFERENCES [dbo].[Flags](Id),
	FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries](Id)
GO

-- Creating Person

ALTER TABLE [dbo].[Person] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	ADD Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    ADD Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id),
	FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id),
	FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id)
GO

-- Creating Friends

ALTER TABLE [dbo].[Friends] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	ADD Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    ADD Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id),
	FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id),
	FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id)
GO

-- Creating Friendship

ALTER TABLE [dbo].[Friendship]
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PersonId] INT  NOT NULL,
	[FriendsId] INT NOT NULL,
	Created_at DATETIME2 UNIQUE DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME2 DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY([PersonId], [FriendsId]),
	ADD CONSTRAINT fk_person_friendship FOREIGN KEY (PersonId) REFERENCES [dbo].[Person](Id) ON DELETE CASCADE,
    ADD CONSTRAINT fk_friends_friendship FOREIGN KEY (FriendsId) REFERENCES [dbo].[Friends](Id) ON DELETE CASCADE,
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

CREATE PROCEDURE [dbo].[ListCountry]
	--	
AS BEGIN
	SELECT 
		-- Country
		C1.Id,
		C1.[Label],	
		C1.FlagId,
		-- Flag
		f1.Id,
		f1.Symbol,
		f1.[Path]
	FROM [dbo].Countries c1
	LEFT JOIN [dbo].Flags f1	
	ON c1.FlagId = f1.Id
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetCountry]
	@IdCountry INT
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
	FROM [dbo].Countries c1
	LEFT JOIN [dbo].Flags f1	
	ON c1.FlagId = f1.Id
	WHERE c1.Id = @IdCountry
END
GO;

/* Create */

CREATE PROCEDURE [dbo].[PostCountry]
	-- Country
	@Label AS NVARCHAR(255),
	@FlagId AS INT,
	-- Flag
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255)
AS BEGIN
	-- Country
	INSERT INTO [dbo].[Countries]([Label], [FlagId])
	VALUES (@Label, @FlagId)

	-- Flag
	INSERT INTO [dbo].[Flags]([Symbol], [Path])
	VALUES (@Symbol, @Path)
END
GO;

/* Update */

CREATE PROCEDURE [dbo].[PutCountry]
	-- Country
	@IdCountry AS INT,
	@Label AS NVARCHAR(255),
	@FlagId AS INT,
	-- Flag
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255)
AS BEGIN
	-- Country
	UPDATE 
		[dbo].[Countries]
	SET	
		[Label] = @Label,
		[FlagId] = @FlagId
	WHERE Id = @IdCountry		
	-- Flag		
	UPDATE 
		[dbo].[Flags]
	SET
		[Symbol] = @Symbol,
		[Path] = @Path
	WHERE Id = @FlagId
END
GO;

/* Delete */

CREATE PROCEDURE [dbo].[DeleteCountry]
	@IdCountry AS INT	
AS BEGIN
	DELETE c1 FROM [dbo].[Countries] c1
	INNER JOIN [dbo].[Flags] f1
	ON c1.FlagId = f1.Id

	WHERE c1.Id = @IdCountry
END	
GO;

/* ********************* States ********************* */

/* List */

CREATE PROCEDURE [dbo].[ListStates]	
	-- @IdStates AS INT
AS BEGIN 
	-- States
	SELECT * FROM [dbo].[States] s1
	-- Flag
	LEFT JOIN [dbo].[Flags] f1
	ON s1.FlagId = f1.Id
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetStates]
	@IdStates AS INT
AS BEGIN
	-- States
	SELECT s1.Id,
		s1.[Label],
		s1.FlagId,
		s1.CountryId, 
		f1.Id,
		f1.[Symbol],
		f1.[Path]		
	FROM [dbo].[States] s1
	-- Flag
	LEFT JOIN [dbo].[Flags] f1
	ON s1.FlagId = f1.Id
	WHERE s1.Id = @IdStates
END
GO;

/* Create */

CREATE PROCEDURE [dbo].[PostStates]
	-- States
	@Label AS NVARCHAR(255),
	@FlagId AS INT,
	-- Flag
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	-- Country
	@CountryId AS INT
AS BEGIN 
	INSERT INTO [dbo].[States]([Label], [FlagId], [CountryId])
	VALUES (@Label, @FlagId, @CountryId)

	INSERT INTO [dbo].[Flags]([Symbol], [Path])
	VALUES (@Symbol, @Path)
END
GO;

/* Update */

CREATE PROCEDURE [dbo].[PutStates]
	-- States
	@IdStates AS INT,
	@Label AS NVARCHAR(255),
	@FlagId AS INT,
	-- Flag
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	-- Country
	@CountryId AS INT	
AS BEGIN 
-- Update States
	UPDATE s1 SET 
		[Label] = @Label,
		FlagId = @FlagId,
		CountryId = @CountryId
	-- States
	FROM [dbo].[States] s1
	-- Flag
	INNER JOIN [dbo].[Flags] f1
	ON s1.FlagId = f1.Id
	-- Country
	INNER JOIN [dbo].[Countries] c1
	ON s1.CountryId = c1.Id
	WHERE s1.Id = @IdStates

-- Update Flag
	UPDATE f1 SET
		[Symbol] = @Symbol,
		[Path] = @Path
	FROM [dbo].[Flags] f1
	INNER JOIN [dbo].[States] s1
	-- ON s1.Id = f1.FlagId
	ON s1.FlagId = f1.Id			-- Here is same above exemple; It's go change only second data tables.
	-- Country
	INNER JOIN [dbo].[Countries] c1
	ON s1.CountryId = c1.Id
	WHERE s1.Id = @IdStates
END
GO;

/* Delete */

CREATE PROCEDURE [dbo].[DeteteStates]
	@Idstates AS INT
AS BEGIN 
	DELETE s1
	-- States
	FROM [dbo].[States] s1
	-- Flag
	INNER JOIN [dbo].[Flags] f1
	ON s1.FlagId = f1.Id
	WHERE s1.Id = @IdStates
END
GO;

/* ********************* Person ********************* */


/* List */

CREATE PROCEDURE [dbo].[ListPerson]
	-- @IdPerson AS INT
AS BEGIN
	-- Person
	SELECT * FROM [dbo].[Person] p1
	-- Pictures
	LEFT JOIN [dbo].[Pictures] p2
	ON p1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN [dbo].[Contacts] c2
	ON p1.ContactId = c2.Id
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetPerson]
	@IdPerson AS INT
AS BEGIN
	SELECT 
		-- Person
		p1.Id,
		p1.FirstName,
		p1.LastName,
		p1.Age,
		p1.Birthday,
		-- Country
		p1.CountryId,
		-- Pictures
		p2.Id,
		p2.[Symbol],
		p2.[Path],
		-- Contacts
		c2.Id,
		c2.Email,
		c2.Mobile	
	FROM Person p1
	-- Pictures
	LEFT JOIN [dbo].[Pictures] p2
	ON p1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN [dbo].[Contacts] c2
	ON p1.ContactId = c2.Id
	WHERE p1.Id = @IdPerson
END
GO;

/* Create */

CREATE PROCEDURE [dbo].[PostPerson]
	-- Person
	@FirstName AS NVARCHAR(250),
	@LastName AS NVARCHAR(250),
	@Birthday AS DATE,
	@Age AS INT,
	-- Contacts
	@ContactId AS INT,
	@Email AS NVARCHAR(250),
	@Mobile AS NVARCHAR(250),
	-- Pictures
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	@PictureId AS INT,
	-- Country
	@CountryId AS INT
AS BEGIN
	-- Pictures
	INSERT INTO [dbo].[Pictures]([Symbol], [Path]) 
	VALUES (@Symbol, @Path);
	-- Contacts
	INSERT INTO [dbo].[Contacts]([Email], [Mobile]) 
	VALUES (@Email, @Mobile);
	-- Person
	INSERT INTO [dbo].[Person]([FirstName], [LastName], [Birthday], [Age], [PictureId], [CountryId], [ContactId])
	VALUES (@FirstName, @LastName, Convert(date, @Birthday), @Age, @PictureId, @CountryId, @ContactId);
END
GO;

/* Update */
-- https://dba.stackexchange.com/questions/239282/update-two-columns-of-two-tables-using-inner-join

CREATE PROCEDURE [dbo].[PutPerson]
	@IdPerson AS INT,
	-- Pictures
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	@PictureId AS INT,
	-- Contacts
	@Email AS NVARCHAR(255),
	@Mobile AS NVARCHAR(255),
	@ContactId AS INT,
	-- Person
	@FirstName AS NVARCHAR(250),
	@LastName AS NVARCHAR(250),
	@Birthday AS DATE,
	@Age AS INT,
	-- Country
	@CountryId AS INT	
AS BEGIN
	-- Pictures
	UPDATE [dbo].[Pictures] SET 
		[Symbol] = @Symbol,
		[Path] = @Path
	WHERE Id = @PictureId;
	
	-- Contacts
	UPDATE [dbo].[Contacts] SET
		Email = @Email,
		Mobile = @Mobile 
	WHERE Id = @ContactId;	
	
	-- Person
	UPDATE [dbo].[Person] SET 
		FirstName = @FirstName,
		LastName = @LastName,
		Birthday = Convert(date, @Birthday),
		Age = @Age,
		PictureId = @PictureId,
		CountryId = @CountryId, 
		ContactId = @ContactId
	WHERE Id = @IdPerson;
END
GO;

/* Delete */

CREATE PROCEDURE [dbo].[DeletePerson]	
	@IdPerson AS INT
AS BEGIN 
	DELETE p1 FROM Person p1
	-- Pictures
	LEFT JOIN [dbo].[Pictures] p2
	ON p1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN [dbo].[Contacts] c2
	ON p1.ContactId = c2.Id
	WHERE p1.Id = @IdPerson
END
GO;

/* ********************* Friends ********************* */

/* List */

CREATE PROCEDURE [dbo].[ListFriends]	
-- @IdPerson AS INT
AS BEGIN
	-- Person
	SELECT * FROM Friends f1
	-- Pictures
	LEFT JOIN [dbo].[Pictures] p2
	ON f1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN [dbo].[Contacts] c2
	ON f1.ContactId = c2.Id
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetFriends]	
	 @IdFriends AS INT
AS BEGIN
	-- Friends
	SELECT * FROM [dbo].[Friends] f1
	-- Pictures
	LEFT JOIN [dbo].[Pictures] p2
	ON f1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN [dbo].[Contacts] c2
	ON f1.ContactId = c2.Id
	WHERE f1.Id = @IdFriends
END
GO;

/* Create */
	
CREATE PROCEDURE [dbo].[PostFriends]
	-- Friends
	@FirstName AS NVARCHAR(250),
	@LastName AS NVARCHAR(250),
	@Birthday AS DATE,
	@Age AS INT,
	-- Contacts
	@ContactId AS INT,
	@Email AS NVARCHAR(250),
	@Mobile AS NVARCHAR(250),
	-- Pictures
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	@PictureId AS INT,
	-- Country
	@CountryId AS INT		
AS BEGIN
	-- Pictures
	INSERT INTO [dbo].[Pictures]([Symbol], [Path]) 
	VALUES (@Symbol, @Path);
	-- Contacts
	INSERT INTO [dbo].[Contacts]([Email], [Mobile]) 
	VALUES (@Email, @Mobile);
	-- Friends
	INSERT INTO [dbo].[Friends]([FirstName], [LastName], [Birthday], [Age], [PictureId], [CountryId], [ContactId])
	VALUES (@FirstName, @LastName, Convert(date, @Birthday), @Age, @PictureId, @CountryId, @ContactId);
END
GO;

/* Update */

CREATE PROCEDURE [dbo].[PutFriends]
	@IdFriends AS INT,
	-- Pictures
	@Symbol AS NVARCHAR(255),
	@Path AS NVARCHAR(255),
	@PictureId AS INT,
	-- Contacts
	@Email AS NVARCHAR(255),
	@Mobile AS NVARCHAR(255),
	@ContactId AS INT,
	-- Friends
	@FirstName AS NVARCHAR(250),
	@LastName AS NVARCHAR(250),
	@Birthday AS DATE,
	@Age AS INT,
	-- Country
	@CountryId AS INT	
AS BEGIN
	-- Pictures
	UPDATE [dbo].[Pictures] SET 
		[Symbol] = @Symbol,
		[Path] = @Path
	WHERE Id = @PictureId;
	
	-- Contacts
	UPDATE [dbo].[Contacts] SET
		Email = @Email,
		Mobile = @Mobile 
	WHERE Id = @ContactId;	
	
	-- Friends
	UPDATE [dbo].[Friends] SET 
		FirstName = @FirstName,
		LastName = @LastName,
		Birthday = Convert(date, @Birthday),
		Age = @Age,
		PictureId = @PictureId,
		CountryId = @CountryId, 
		ContactId = @ContactId
	WHERE Id = @IdFriends;
END
GO;

/* Delete */

CREATE PROCEDURE [dbo].[DeleteFriends]	
	@IdFriends AS INT
AS BEGIN 
	DELETE f1 FROM  [dbo].[Friends] f1
	-- Pictures
	LEFT JOIN  [dbo].[Pictures] p2
	ON f1.PictureId = p2.Id
	-- Contacts
	LEFT JOIN  [dbo].[Contacts] c2
	ON f1.ContactId = c2.Id
	WHERE f1.Id = @IdFriends		
END
GO;

/* ******************** Friendship ********************* */

/* List */

CREATE PROCEDURE [dbo].[ListFriendship]	
	-- @PersonId INT,
	-- @FriendId INT
AS BEGIN
	-- Person
	SELECT * FROM [dbo].[Friendship] f0
	-- Person
	LEFT JOIN [dbo].[Person] p1
	ON f0.PersonId = p1.Id
	--Friends
	LEFT JOIN  [dbo].[Friends] f1
	ON f0.FriendsId = f1.Id
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetFriendship]
	@IdFriendship AS INT
AS BEGIN
	SELECT
		-- Person
		p1.Id,
		p1.FirstName,
		p1.LastName,
		p1.Age, 
		p1.Birthday,
		p1.PictureId,
		p1.CountryId,
		p1.ContactId,
		-- Friends
		f1.Id,
		f1.FirstName,
		f1.LastName, 
		f1.Age, 
		f1.Birthday, 
		f1.PictureId, 
		f1.CountryId, 
		f1.ContactId
		-- Frindship
	FROM [dbo].[Friendship] f0
	-- Person
	LEFT JOIN [dbo].[Person] p1
	ON f0.PersonId = p1.Id
	--Friends
	LEFT JOIN  [dbo].[Friends] f1
	ON f0.FriendsId = f1.Id
END
GO;

/* Create */

CREATE PROCEDURE [dbo].[PostFriendship]
	-- Person
	@PersonId AS INT,
	-- Friends
	@FriendId AS INT
AS BEGIN
	-- Friendship
	INSERT INTO [dbo].Friendship(PersonId, FriendsId) 
	VALUES (@PersonId, @FriendId);
END
GO;

/* Update */

CREATE PROCEDURE [dbo].[PutFriendship]
	-- Person
	@PersonId AS INT,
	-- Friends
	@FriendId AS INT
AS BEGIN
	UPDATE Friendship SET
		PersonId = @PersonId,
		FriendsId = @FriendId
	WHERE PersonId = @PersonId
END
GO;
/* Delete */

CREATE PROCEDURE [dbo].[DeleteFriendship]
	-- Person
	@IdFriendship AS INT
AS BEGIN
	DELETE f1
	FROM [dbo].[Friendship] f1
	WHERE f1.PersonId = @IdFriendship
END
GO;



----------------------------------------------------------
			  /* Running Procedure Store */
----------------------------------------------------------


/* ********************* Country ******************** */

/* List */

EXEC [dbo].[ListCountry];

/* Details */

EXEC [dbo].[GetCountry] @IdCountry = 2;

/* Create */

EXEC [dbo].[PostCountry] @Label = 'Espanha', @FlagId = 1, @Symbol = 'Espanha.png', @Path = '../Pictures/Espanha.png';

/* Update */

EXEC [dbo].[PutCountry] @Idcountry = 2, @Label = 'Brazil', @FlagId = 1, @Symbol = 'Brasil.png', @Path = '../Pictures/Brasil.png';

/* Delete */

EXEC [dbo].[DeleteCountry] @IdCountry = 1;


/* ********************* States ********************* */


/* List */

EXEC [dbo].[ListStates];

/* Details */

EXEC [dbo].[GetStates] @IdStates = 1;

/* Create */
insert into States (Label, FlagId, CountryId) VALUES ('Rio de Janeiro', 1, 1);
EXEC [dbo].[PostStates] @Label = 'São Paulo', @FlagId = 1, @Symbol = 'sp.png', @Path = '../Pictures/sp.png', @CountryId = 1;

/* Update */

EXEC [dbo].[PutStates] @IdStates = 1, @Label = 'Brazil', @FlagId = 2, @Symbol = 'Brazil.png', @Path = '../Pictures/Brazil.png',  @CountryId = 2;

/* Delete */

EXEC [dbo].[DeteteStates] @IdStates = 1;


/* ********************* Person ********************* */

/* List */

EXEC [dbo].[ListPerson];

/* Details */

EXEC [dbo].[GetPerson] @IdPerson = 1;

/* Create */

EXEC [dbo].[PostPerson] @Symbol = 'MyPicturePerson', @Path = '../Pictures/my_picture_person.png',@PictureId = 1, 
	@Email = 'luiz@siqueira.psk', @Mobile = '21975918265', @ContactId = 1, 
	@Firstname = 'Luiz', @Lastname = 'Siqueira', @Age = '31', @Birthday = '1990-01-28',  @CountryId = 2;  

/* Update */

EXEC [dbo].[PutPerson] @IdPerson = 1, @Symbol = 'MyPicturePerson2', @Path = '../Pictures/my_picture_person2.png',@PictureId = 2, 
	@Email = 'luiz@siqueiras.psk', @Mobile = '2197591-8265', @ContactId = 2, 
	@Firstname = 'Luiz', @Lastname = 'Siqueira', @Age = '32', @Birthday = '1990-01-28',  @CountryId = 2;  

/* Delete */

EXEC [dbo].[DeletePerson] @IdPerson = 1;


/* ********************* Friends ******************** */


/* List */

EXEC [dbo].[ListFriends];

/* Details */

EXEC [dbo].[GetFriends] @IdFriends = 1;

/* Create */

EXEC [dbo].[PostFriends] @Symbol = 'MyPictureFriends', @Path = '../Pictures/my_picture_Friends.png',@PictureId = 8, 
	@Email = 'paulo@mendes.psk', @Mobile = '21975918265', @ContactId = 8, 
	@Firstname = 'Paulo', @Lastname = 'Mendes', @Age = '30', @Birthday = '1990-01-28',  @CountryId = 8;  

/* Update */

EXEC [dbo].[PutFriends] @IdFriends = 1, @Symbol = 'MyPictureFriends', @Path = '../Pictures/my_picture_Friends.png',@PictureId = 1, 
	@Email = 'paulo@mendes.psk', @Mobile = '2197591-8265', @ContactId = 1, 
	@Firstname = 'Paulo', @Lastname = 'Mendes', @Age = '30', @Birthday = '1990-01-28',  @CountryId = 1;  

/* Delete */

EXEC [dbo].[DeleteFriends] @IdFriends = 1;


/* ******************** Friendship ****************** */


/* List */

EXEC [dbo].[ListFriendship];

/* Details */

EXEC [dbo].[GetFriendship] @IdFriendship = 1;

/* Create */

EXEC [dbo].[PostFriendship] @PersonId = 1, @FriendId = 1;  

/* Update */

EXEC [dbo].[PutFriendship] @PersonId = 1, @FriendId = 2;  

/* Delete */

EXEC [dbo].[DeleteFriendship] @IdFriendship = 1;