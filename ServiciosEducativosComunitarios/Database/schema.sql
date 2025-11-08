CREATE TABLE [dbo].[Locality](
	[id] INT IDENTITY(1,1) NOT NULL,
	[code] NVARCHAR(127) NULL,
	[municipio] INT NULL,
	[comunidad] NVARCHAR(1023) NULL,
	[ambito] INT NULL,
	[latitud] NVARCHAR(1023) NULL,
	[longitud] NVARCHAR(1023) NULL,
	[poblacion] INT NULL,
	PRIMARY KEY CLUSTERED([id] ASC)
);

CREATE TABLE [dbo].[Service](
	[id] INT IDENTITY(1,1) NOT NULL,
	[code] NVARCHAR(127) NULL,
	[localityId] INT NULL,
	[period] INT NULL,
	[program] INT NULL,
	[status] INT NULL,
	PRIMARY KEY CLUSTERED([id] ASC)
);
