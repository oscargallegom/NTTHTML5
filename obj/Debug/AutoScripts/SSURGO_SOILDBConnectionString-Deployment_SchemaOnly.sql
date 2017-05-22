SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ALSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AZSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CTSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DCSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FLSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DESOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NJSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IDSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KSSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KYSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MESOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MISOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MNSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NDSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NMSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NVSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OKSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RISOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SDSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TNSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TXSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UTSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VTSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WISOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WYSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ILSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MDSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NYSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VASOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WVSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ORSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OHSOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [float] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [float] NULL,
	[SLOPEH] [float] NULL,
	[COMPPCT] [float] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NESOILS](
	[TSID] [int] NULL,
	[TSSSACode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TSCountyCode] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUID] [int] NULL,
	[SLID] [float] NULL,
	[SERIES] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UDEP] [float] NULL,
	[LDEP] [int] NULL,
	[SAND] [float] NULL,
	[SILT] [float] NULL,
	[CLAY] [float] NULL,
	[TEXTR] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BD] [float] NULL,
	[OM] [float] NULL,
	[CEC] [float] NULL,
	[PH] [float] NULL,
	[PAW] [float] NULL,
	[WC13] [float] NULL,
	[WC15] [float] NULL,
	[CF] [float] NULL,
	[SALIN] [float] NULL,
	[HORIZDESC1] [int] NULL,
	[HORIZGEN] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HORIZDESC2] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NO3N] [float] NULL,
	[P205] [float] NULL,
	[WCSTART] [float] NULL,
	[SERIESNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MUNAME] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SLOPEL] [int] NULL,
	[SLOPEH] [int] NULL,
	[COMPPCT] [int] NULL,
	[TFACTOR] [float] NULL,
	[KSAT] [float] NULL,
	[ALBEDO] [float] NULL,
	[COARSE] [float] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_test](
	[County_name] [nchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
