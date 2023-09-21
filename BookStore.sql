use BookStore
--Bang AdminAccount
create table [dbo].[AdminAccount] (
	[AdminID] INT IDENTITY(1,1) NOT NULL,
	[Email] NVARCHAR(50),
	[Password] NVARCHAR(50),
	PRIMARY KEY CLUSTERED ([AdminID] ASC)
)
--Bang Category
create table [dbo].[Category] (
	[CategoryID] INT IDENTITY(1,1) NOT NULL,
	[CategoryName] NVARCHAR(250),
	PRIMARY KEY CLUSTERED ([CategoryID] ASC)
)
--Bang Customer
create table [dbo].[Customer] (
	[ID] INT IDENTITY(1,1) NOT NULL,
	[NameCustomer] NVARCHAR(250),
	[Email] NVARCHAR(250),
	[Password] NVARCHAR(250),
	PRIMARY KEY CLUSTERED ([ID] ASC)
) 
--Bang Order
create table [dbo].[Order] (
	[IDOrder] INT IDENTITY(1,1) NOT NULL,
	[Address] NVARCHAR(250),
	[DateOrder] datetime NULL,
	[Status] INT NULL,
	[IDCus] INT NULL,
	PRIMARY KEY CLUSTERED ([IDOrder] ASC)
) 
--Bang OrderDetail
create table [dbo].[OrderDetail] (
	[ID] INT IDENTITY(1,1) NOT NULL,
	[IDOrder] INT NULL,
	[ProductID] INT NULL,
	[Quantity] INT NULL,
	[UnitPrice] FLOAT NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC)
) 
--Bang Product
create table [dbo].[Product] (
	[ProductID] INT IDENTITY(1,1) NOT NULL,
	[ProductName] NVARCHAR(250),
	[Price] float NULL,
	[Introduce] nvarchar(max),
	[Author] nvarchar(200),
	[ImageProd] nvarchar(max),
	[CategoryID] int null,
	PRIMARY KEY CLUSTERED ([ProductID] ASC)
) 
alter table [dbo].[Order] add constraint FK_Order_Cus foreign key ([IDCus]) references [dbo].[Customer] ([ID])
alter table [dbo].[OrderDetail] add constraint FK_OrderDe_Order foreign key ([IDOrder]) references [dbo].[Order] ([IDOrder])
alter table [dbo].[OrderDetail] add constraint FK_OrderDe_Prod foreign key ([ProductID]) references [dbo].[Product] ([ProductID])
alter table [dbo].[Product] add constraint FK_Prod_Cate foreign key ([CategoryID]) references [dbo].[Category] ([CategoryID])