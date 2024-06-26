USE [DCE_DB]
GO
/****** Object:  StoredProcedure [dbo].[GetActiveOrdersByCustomer]    Script Date: 5/12/2024 10:33:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetActiveOrdersByCustomer]
(
 @CustomerId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT o.OrderId, o.ProductId, o.OrderStatus, o.OrderType, o.OrderBy, o.OrderedOn, o.ShippedOn, o.IsActive AS IsOrderActive,
		   p.ProductName, p.SupplierId, p.UnitPrice, p.CreatedOn AS ProductCreatedDate, p.IsActive AS IsProductActive,
		   s.SupplierName, s.CreatedOn AS SupplierCreatedDate, s.IsActive AS IsSupplierActive 
    FROM [Order] o 
	INNER JOIN Product p ON o.ProductId = p.ProductId
	INNER JOIN Supplier s ON p.SupplierId = s.SupplierId
    WHERE o.OrderBy = @CustomerId AND o.IsActive = 1;
END
