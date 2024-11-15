/****** SECTOR ******/
CREATE TABLE [dbo].[Sector](
	[Id] [int] NOT NULL,
	[Codigo] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Sector] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** PUESTO ******/
CREATE TABLE [dbo].[Puesto](
	[Id] [int] NOT NULL,
	[Codigo] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Puesto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** NACIONALIDAD ******/
CREATE TABLE [dbo].[Nacionalidad](
	[Id] [int] NOT NULL,
	[Codigo] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](250) NULL,
 CONSTRAINT [PK_Nacionalidad] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** ESTADOPROCESOSELECCION ******/
CREATE TABLE [dbo].[EstadoProcesoSeleccion](
	[Id] [int] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_EstadoProcesoSeleccion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** ESTADOCANDIDATURA ******/
CREATE TABLE [dbo].[EstadoCandidatura](
	[Id] [int] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO


/****** CANDIDATO ******/
CREATE TABLE [dbo].[Candidato](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nif] [nchar](10) NOT NULL,
	[Apellidos] [nvarchar](250) NOT NULL,
	[Nombre] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[FechaNacimiento] [datetime] NULL,
	[IdNacionalidad] [int] NULL,
	[FechaValidezPermisoTrabajo] [datetime] NULL,
 CONSTRAINT [PK_Candidato] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** PROCESOSELECCION ******/
CREATE TABLE [dbo].[ProcesoSeleccion](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
	[FechaIncorporacion] [datetime] NOT NULL,
	[FechaFinalizacion] [datetime] NULL,
	[IdEstado] [int] NULL,	
	[IdPuesto] [int] NOT NULL,
	[Vacantes] [int] NOT NULL,
	[CreadoEl] [datetime] NULL,
	[ModificadoEl] [datetime] NULL,
 CONSTRAINT [PK_ProcesoSeleccion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProcesoSeleccion]  WITH CHECK ADD  CONSTRAINT [FK_ProcesoSeleccion_EstadoProcesoSeleccion] FOREIGN KEY([IdEstado])
REFERENCES [dbo].[EstadoProcesoSeleccion] ([Id])
GO

ALTER TABLE [dbo].[ProcesoSeleccion] CHECK CONSTRAINT [FK_ProcesoSeleccion_EstadoProcesoSeleccion]
GO

ALTER TABLE [dbo].[ProcesoSeleccion]  WITH CHECK ADD  CONSTRAINT [FK_ProcesoSeleccion_Puesto] FOREIGN KEY([IdPuesto])
REFERENCES [dbo].[Puesto] ([Id])
GO

ALTER TABLE [dbo].[ProcesoSeleccion] CHECK CONSTRAINT [FK_ProcesoSeleccion_Puesto]
GO



/****** CANDIDATURA ******/
CREATE TABLE [dbo].[Candidatura](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCandidato] [bigint] NOT NULL,
	[IdProcesoSeleccion] [bigint] NOT NULL,
	[IdEstado] [int] NOT NULL,
	[Observaciones] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Candidatura] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Candidatura]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_Candidato] FOREIGN KEY([IdCandidato])
REFERENCES [dbo].[Candidato] ([Id])
GO

ALTER TABLE [dbo].[Candidatura] CHECK CONSTRAINT [FK_Candidatura_Candidato]
GO


/****** EXPERIENCIA LABORAL ******/
CREATE TABLE [dbo].[ExperienciaLaboral](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCandidato] [bigint] NOT NULL,
	[IdPuesto] [int] NOT NULL,
	[IdSector] [int] NOT NULL,
	[Empresa] [nvarchar](150) NOT NULL,
	[Funciones] [nvarchar](500) NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NULL,
 CONSTRAINT [PK_ExperienciaLaboral] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExperienciaLaboral]  WITH CHECK ADD  CONSTRAINT [FK_ExperienciaLaboral_Candidato] FOREIGN KEY([IdSector])
REFERENCES [dbo].[Sector] ([Id])
GO

ALTER TABLE [dbo].[ExperienciaLaboral] CHECK CONSTRAINT [FK_ExperienciaLaboral_Candidato]
GO

ALTER TABLE [dbo].[ExperienciaLaboral]  WITH CHECK ADD  CONSTRAINT [FK_ExperienciaLaboral_Puesto] FOREIGN KEY([IdPuesto])
REFERENCES [dbo].[Puesto] ([Id])
GO

ALTER TABLE [dbo].[ExperienciaLaboral] CHECK CONSTRAINT [FK_ExperienciaLaboral_Puesto]
GO

ALTER TABLE [dbo].[Candidatura]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_ProcesoSeleccion] FOREIGN KEY([IdProcesoSeleccion])
REFERENCES [dbo].[ProcesoSeleccion] ([Id])
GO

ALTER TABLE [dbo].[Candidatura] CHECK CONSTRAINT [FK_Candidatura_ProcesoSeleccion]
GO
