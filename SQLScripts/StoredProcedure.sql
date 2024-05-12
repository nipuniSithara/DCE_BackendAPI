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
    SELECT c.UserId,
           c.Username,
		   c.FirstName,
		   c.LastName,
		   c.Email,
		   c.CreatedOn,
		   c.IsActive AS IsCustomerActive,
           o.OrderId,
           o.OrderStatus,
           o.OrderType,
           o.OrderBy,
           o.OrderedOn,
           o.ShippedOn,
           o.IsActive AS IsOrderActive
    FROM Customer c
    INNER JOIN [Order] o ON c.UserId = o.OrderBy
    WHERE c.UserId = @CustomerId AND o.IsActive = 1;
END
