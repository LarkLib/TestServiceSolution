CREATE TABLE [dbo].[ItemMain] (
    [ItemID]      INT            NOT NULL,
    [ParentID]    INT            NULL,
    [ItemCode]    NVARCHAR (100) NULL,
    [ItemName]    NVARCHAR (100) NULL,
    [ItemOrder]   INT            NULL,
    [IsEffective] BIT            NULL,
    [Remark]      NVARCHAR (500) NULL
);

