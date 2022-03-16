 ----------------------------------------------------------
				 /* Creating Database */
----------------------------------------------------------

-- Create

CREATE DATABASE Friendzone;

-- Drop

DROP DATABASE Friendzone;


DROP TABLE [dbo].[Friendship];
DROP TABLE [dbo].[Person];
DROP TABLE [dbo].[Friends];
DROP TABLE [dbo].[Pictures];
DROP TABLE [dbo].[Contacts];
DROP TABLE [dbo].[States];
DROP TABLE [dbo].[Countries];
DROP TABLE [dbo].[Flags];

-- User

use Friendzone;
								

----------------------------------------------------------
				  /* CREATE TABLES */
----------------------------------------------------------

-- Creating Contacts

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Contacts' and type in (N'U'))

CREATE TABLE [dbo].[Contacts](
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Email NVARCHAR(250) NOT NULL,
	Mobile NVARCHAR(250) NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Contacts - Exists this table !!!'
GO

-- Creating Pictures

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Pictures' and type in (N'U'))

CREATE TABLE [dbo].[Pictures](
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] NVARCHAR(255) NULL,
	[Path] NVARCHAR(255) NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Pictures - Exists this table !!!'
GO

-- Creating Flags

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Flags' and type in (N'U'))

CREATE TABLE [dbo].[Flags](
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] NVARCHAR(255) NULL,
	[Path] NVARCHAR(255) NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
);
ELSE
	PRINT 'Flags - Exists this table !!!'
GO

-- Creating Countries

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Countries' and type in (N'U'))

CREATE TABLE [dbo].[Countries] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) NOT NULL,
	[FlagId] INT NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (FlagId) REFERENCES [dbo].[Flags](Id) ON UPDATE CASCADE ON DELETE CASCADE
); 
ELSE
	PRINT 'Countries - Exists this table !!!'
GO

-- Creating States

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'States' and type in (N'U'))

CREATE TABLE [dbo].[States] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Label] NVARCHAR(250) NOT NULL,
	[FlagId] INT NULL,
	[CountryId] INT NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY ([FlagId]) REFERENCES [dbo].[Flags](Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
	FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries](Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
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
	[PictureId] INT NULL,
	[CountryId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id) ON UPDATE CASCADE ON DELETE CASCADE
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
	[PictureId] INT NULL,
	[CountryId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (PictureId) REFERENCES [dbo].[Pictures](Id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (ContactId) REFERENCES [dbo].[Contacts](Id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (CountryId) REFERENCES [dbo].[Countries](Id) ON UPDATE CASCADE ON DELETE CASCADE
);

ELSE
	PRINT 'Friends - Exists this table !!!'
GO

-- Creating Friendship

IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME=N'Friendship' and type in (N'U'))

CREATE TABLE [dbo].[Friendship](
	[PersonId] INT  NOT NULL,
	[FriendsId] INT NOT NULL,
	Created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PersonId) REFERENCES [dbo].[Person](Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
    FOREIGN KEY (FriendsId) REFERENCES [dbo].[Friends](Id) ON UPDATE NO ACTION ON DELETE NO ACTION
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
GO

-- Creating Countries

ALTER TABLE [dbo].[Countries]
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating States

ALTER TABLE [dbo].[States] 
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating Person

ALTER TABLE [dbo].[Person] 
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating Friends

ALTER TABLE [dbo].[Friends] 
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
GO

-- Creating Friendship

ALTER TABLE [dbo].[Friendship]
	ADD Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL;
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
	-- Flag
	INSERT INTO [dbo].[Flags]([Symbol], [Path])
	VALUES (@Symbol, @Path)

	-- Country
	INSERT INTO [dbo].[Countries]([Label], [FlagId]) 
	VALUES (@Label, @FlagId)
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
	-- Flag
	INSERT INTO [dbo].[Flags]([Symbol], [Path])
	VALUES (@Symbol, @Path)
	-- States
	INSERT INTO [dbo].[States]([Label], [FlagId], [CountryId])
	VALUES (@Label, @FlagId, @CountryId)
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
	UPDATE
		[dbo].[States] 
	SET 
		[Label] = @Label,
		FlagId = @FlagId,
		CountryId = @CountryId
	WHERE Id = @IdStates

-- Update Flag
	UPDATE 
		[dbo].[Flags]
	SET
		[Symbol] = @Symbol,
		[Path] = @Path
	WHERE Id = @IdStates
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
	-- Friendship
	SELECT * FROM [dbo].[Friendship] f
END
GO;

/* Detail */

CREATE PROCEDURE [dbo].[GetFriendship]
	@IdPerson AS INT
AS BEGIN
	-- Friendship
	SELECT 
		f.PersonId,
		f.FriendsId 
	FROM [dbo].[Friendship] f
	WHERE f.PersonId = @IdPerson
END
GO;

/* Create */

CREATE PROCEDURE [dbo].[PostFriendship]
	-- Person
	@IdPerson AS INT,
	-- Friends
	@IdFriend AS INT
AS BEGIN
	-- Friendship
	INSERT INTO [dbo].Friendship(PersonId, FriendsId) 
	VALUES (@IdPerson, @IdFriend);
END
GO;

/* Update */

CREATE PROCEDURE [dbo].[PutFriendship]
	-- Person
	@IdPerson AS INT,
	-- Friends
	@IdFriend AS INT
AS BEGIN
	UPDATE
		[dbo].[Friendship]
	SET
		PersonId = @IdPerson,
		FriendsId = @IdFriend
	WHERE PersonId = @IdFriend
END
GO;
/* Delete */

CREATE PROCEDURE [dbo].[DeleteFriendship]
	-- Person
	@IdPerson AS INT
AS BEGIN
	DELETE f1
	FROM [dbo].[Friendship] f1
	WHERE f1.PersonId = @IdPerson
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

EXEC [dbo].[PostCountry] @Label = 'Brasil', @FlagId = 1, @Symbol = 'br.png', @Path = '../pictures/br.png';

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

EXEC [dbo].[PostStates] @Label = 'Rio de Janeiro', @FlagId = 1, @Symbol = 'rj.png', @Path = '../Pictures/rj.png', @CountryId = 1;

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
	@Firstname = 'Luiz', @Lastname = 'Siqueira', @Age = '31', @Birthday = '1990-01-28',  @CountryId = 1; 

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

EXEC [dbo].[PostFriends] @Symbol = 'MyPictureFriends', @Path = '../Pictures/my_picture_Friends.png',@PictureId = 1, 
	@Email = 'paulo@mendes.psk', @Mobile = '21975918265', @ContactId = 1, 
	@Firstname = 'Paulo', @Lastname = 'Mendes', @Age = '30', @Birthday = '1990-01-28',  @CountryId = 1;  

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

EXEC [dbo].[GetFriendship] @IdPerson = 1;

/* Create */

EXEC [dbo].[PostFriendship] @IdPerson = 2, @IdFriend = 2;  

/* Update */

EXEC [dbo].[PutFriendship] @IdPerson = 1, @IdFriend = 1;  

/* Delete */

EXEC [dbo].[DeleteFriendship] @IdPerson = 1;