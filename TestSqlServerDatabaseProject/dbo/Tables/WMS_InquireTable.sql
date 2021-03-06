﻿CREATE TABLE [dbo].[WMS_InquireTable] (
    [sn]                   NVARCHAR (255) NOT NULL,
    [source]               NVARCHAR (255) NULL,
    [track_no]             INT            NULL,
    [dataset_counter]      INT            NULL,
    [order_no]             NVARCHAR (255) NULL,
    [sheet_length]         INT            NULL,
    [sheet_width]          INT            NULL,
    [partial_stack_sheets] INT            NULL,
    [total_width]          INT            NULL,
    [overall_length_stack] INT            NULL,
    [overall_height_stack] INT            NULL,
    [stackpattern]         NVARCHAR (255) NULL,
    [stackdirect]          NVARCHAR (255) NULL,
    [warehouseshift]       NVARCHAR (255) NULL,
    [warehousestaff]       NVARCHAR (255) NULL,
    [warehousedate]        DATETIME       NULL,
    [qrcode]               NVARCHAR (255) NULL,
    [packno]               NVARCHAR (255) NULL,
    [packheight]           INT            NULL,
    [orderno]              NVARCHAR (255) NULL,
    [productno]            NVARCHAR (255) NULL,
    [productname]          NVARCHAR (255) NULL,
    [customerpno]          NVARCHAR (255) NULL,
    [customerno]           NVARCHAR (255) NULL,
    [customernames]        NVARCHAR (255) NULL,
    [customernamea]        NVARCHAR (255) NULL,
    [customerunit]         NVARCHAR (255) NULL,
    CONSTRAINT [PK_WMS_Inquire] PRIMARY KEY CLUSTERED ([sn] ASC)
);

