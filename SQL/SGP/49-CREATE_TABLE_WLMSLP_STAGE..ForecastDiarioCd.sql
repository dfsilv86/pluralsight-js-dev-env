
CREATE TABLE WLMSLP_STAGE..ForecastDiarioCd(
	[IdForecastDiarioCd] [bigint] IDENTITY(1,1) NOT NULL,
	[IdLoja] [int] NULL,
	[IdCd] [int] NULL,
	[IdItemDetalhe] [bigint] NULL,
	[CdSemanaWalmartForecast] [int] NULL,
	[Semana] [tinyint] NULL,
	[DiaSemana] [tinyint] NULL,
	[DiaMes] [smallint] NULL,
	[VlForecastSemana] [float] NULL,
	[Alloc] [varchar](max) NULL,
	[PercentualDiario] [int] NULL,
	[ValorForecast] [float] NULL,
	[DtInicioForecast] [datetime] NULL,
	[DataForecast] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdForecastDiarioCd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE WLMSLP_STAGE..ForecastDiarioCd ADD  DEFAULT ((0)) FOR [Semana]
GO


