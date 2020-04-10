CREATE TABLE [dbo].[ItemList] (
    [ItemListID]    INT            NOT NULL,
    [ItemID]        INT            NOT NULL,
    [ItemListCode]  NVARCHAR (100) NOT NULL,
    [ItemListName]  NVARCHAR (100) NULL,
    [ItemListOrder] INT            NULL,
    [IsEffective]   BIT            NULL,
    [Remark]        NVARCHAR (500) NULL
);

