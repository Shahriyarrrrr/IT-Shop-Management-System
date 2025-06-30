-- Step 1: Declare the variable
DECLARE @CartID INT;

-- Step 2: Insert into the Cart table (assume CustomerID = 1 for now)
INSERT INTO Cart (CustomerID)
VALUES (1);  -- Replace with real CustomerID

-- Step 3: Get the newly generated CartID
SET @CartID = SCOPE_IDENTITY();

-- Step 4: Insert into CartItem using valid ProductID and Quantity
INSERT INTO CartItem (CartID, ProductID, Quantity)
VALUES (@CartID, 1, 1);  -- Use a valid ProductID from your Product table

SELECT * FROM CART;

