BEGIN TRY
BEGIN TRANSACTION;

    DECLARE @UserId INT;
    DECLARE @BudgetId_July INT, @BudgetId_August INT, @BudgetId_Sept INT, @BudgetId_Oct INT;
    DECLARE @BudgetId_Nov INT, @BudgetId_Dec INT, @BudgetId_Jan INT, @BudgetId_Feb INT;
    DECLARE @BudgetId_Mar INT, @BudgetId_Apr INT, @BudgetId_May INT, @BudgetId_June INT;

    PRINT 'Add Users';
    INSERT INTO [User] (ExternalId, Name, Email)
    VALUES ('test-external-id-123', 'Sam Inner', 'sam.inner@gmail.com');

    SET @UserId = SCOPE_IDENTITY();  

    PRINT 'Add Categories';
    INSERT INTO Category (CategoryName)
    VALUES 
    ('Food'),
    ('Travel'),
    ('Rent'),
    ('Utilities'),
    ('Entertainment'),
    ('Salary'),
    ('Freelance');

    PRINT 'Add Source Types';
    INSERT INTO SourceType (SourceName)
    VALUES 
    ('Credit Card'),
    ('Bank Account'),
    ('Cash'),
    ('Income');

    PRINT 'Add Budgets';
    INSERT INTO Budget (Date, UserId) VALUES ('2024-07-01', @UserId);
    SET @BudgetId_July = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2024-08-01', @UserId);
    SET @BudgetId_August = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2024-09-01', @UserId);
    SET @BudgetId_Sept = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2024-10-01', @UserId);
    SET @BudgetId_Oct = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2024-11-01', @UserId);
    SET @BudgetId_Nov = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2024-12-01', @UserId);
    SET @BudgetId_Dec = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-01-01', @UserId);
    SET @BudgetId_Jan = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-02-01', @UserId);
    SET @BudgetId_Feb = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-03-01', @UserId);
    SET @BudgetId_Mar = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-04-01', @UserId);
    SET @BudgetId_Apr = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-05-01', @UserId);
    SET @BudgetId_May = SCOPE_IDENTITY();

    INSERT INTO Budget (Date, UserId) VALUES ('2025-06-01', @UserId);
    SET @BudgetId_June = SCOPE_IDENTITY();

    PRINT 'Add Budget Totals';
    INSERT INTO BudgetTotal (UserId, TotalValue)
    VALUES (@UserId, 15770.00);

    PRINT 'Add Recurring Expenses';
    INSERT INTO RecurringExpense (Label, CategoryId, SourceTypeId, UserId, [Value])
    VALUES
    ('Monthly Rent', 3, 2, @UserId, 1500.00),
    ('Electric Bill', 4, 1, @UserId, 75.25),
    ('Water Bill', 4, 2, @UserId, 36.84),
    ('Netflix Subscription', 5, 1, @UserId, 15.99),
    ('Spotify Subscription', 5, 1, @UserId, 9.99),
    ('Grocery Delivery Membership', 1, 2, @UserId, 15.49),
    ('Gym Membership', 5, 3, @UserId, 25.00),
    ('Mobile Phone Plan', 4, 2, @UserId, 125.48);

    PRINT 'Add Budget Line Items';
    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_July, 6, 'Salary', 4200, 4),
    (@BudgetId_July, 7, 'Freelance', 1800, 4),
    (@BudgetId_July, 1, 'Food Expense', -500, 1),
    (@BudgetId_July, 2, 'Travel Expense', -750, 2),
    (@BudgetId_July, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_July, 4, 'Utilities Expense', -300, 1),
    (@BudgetId_July, 5, 'Entertainment Expense', -400, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_August, 6, 'Salary', 4300, 4),
    (@BudgetId_August, 1, 'Food Expense', -550, 1),
    (@BudgetId_August, 2, 'Travel Expense', -600, 2),
    (@BudgetId_August, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_August, 4, 'Utilities Expense', -310, 1),
    (@BudgetId_August, 5, 'Entertainment Expense', -450, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Sept, 6, 'Salary', 4100, 4),
    (@BudgetId_Sept, 7, 'Freelance', 2200, 4),
    (@BudgetId_Sept, 1, 'Food Expense', -480, 1),
    (@BudgetId_Sept, 2, 'Travel Expense', -700, 2),
    (@BudgetId_Sept, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Sept, 4, 'Utilities Expense', -290, 1),
    (@BudgetId_Sept, 5, 'Entertainment Expense', -500, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Oct, 6, 'Salary', 4400, 4),
    (@BudgetId_Oct, 1, 'Food Expense', -530, 1),
    (@BudgetId_Oct, 2, 'Travel Expense', -650, 2),
    (@BudgetId_Oct, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Oct, 4, 'Utilities Expense', -320, 1),
    (@BudgetId_Oct, 5, 'Entertainment Expense', -470, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Nov, 6, 'Salary', 4250, 4),
    (@BudgetId_Nov, 7, 'Freelance', 2000, 4),
    (@BudgetId_Nov, 1, 'Food Expense', -510, 1),
    (@BudgetId_Nov, 2, 'Travel Expense', -720, 2),
    (@BudgetId_Nov, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Nov, 4, 'Utilities Expense', -310, 1),
    (@BudgetId_Nov, 5, 'Entertainment Expense', -430, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Dec, 6, 'Salary', 4500, 4),
    (@BudgetId_Dec, 1, 'Food Expense', -560, 1),
    (@BudgetId_Dec, 2, 'Travel Expense', -800, 2),
    (@BudgetId_Dec, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Dec, 4, 'Utilities Expense', -330, 1),
    (@BudgetId_Dec, 5, 'Entertainment Expense', -490, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Jan, 6, 'Salary', 4300, 4),
    (@BudgetId_Jan, 7, 'Freelance', 2100, 4),
    (@BudgetId_Jan, 1, 'Food Expense', -520, 1),
    (@BudgetId_Jan, 2, 'Travel Expense', -680, 2),
    (@BudgetId_Jan, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Jan, 4, 'Utilities Expense', -300, 1),
    (@BudgetId_Jan, 5, 'Entertainment Expense', -460, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Feb, 6, 'Salary', 4400, 4),
    (@BudgetId_Feb, 1, 'Food Expense', -540, 1),
    (@BudgetId_Feb, 2, 'Travel Expense', -710, 2),
    (@BudgetId_Feb, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Feb, 4, 'Utilities Expense', -310, 1),
    (@BudgetId_Feb, 5, 'Entertainment Expense', -480, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Mar, 6, 'Salary', 4350, 4),
    (@BudgetId_Mar, 7, 'Freelance', 1900, 4),
    (@BudgetId_Mar, 1, 'Food Expense', -500, 1),
    (@BudgetId_Mar, 2, 'Travel Expense', -750, 2),
    (@BudgetId_Mar, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Mar, 4, 'Utilities Expense', -320, 1),
    (@BudgetId_Mar, 5, 'Entertainment Expense', -470, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_Apr, 6, 'Salary', 4450, 4),
    (@BudgetId_Apr, 1, 'Food Expense', -510, 1),
    (@BudgetId_Apr, 2, 'Travel Expense', -690, 2),
    (@BudgetId_Apr, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_Apr, 4, 'Utilities Expense', -310, 1),
    (@BudgetId_Apr, 5, 'Entertainment Expense', -460, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_May, 6, 'Salary', 4300, 4),
    (@BudgetId_May, 7, 'Freelance', 2300, 4),
    (@BudgetId_May, 1, 'Food Expense', -520, 1),
    (@BudgetId_May, 2, 'Travel Expense', -740, 2),
    (@BudgetId_May, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_May, 4, 'Utilities Expense', -300, 1),
    (@BudgetId_May, 5, 'Entertainment Expense', -450, 2);

    INSERT INTO BudgetLineItem (BudgetId, CategoryId, Label, [Value], SourceTypeId) VALUES
    (@BudgetId_June, 6, 'Salary', 4550, 4),
    (@BudgetId_June, 1, 'Food Expense', -530, 1),
    (@BudgetId_June, 2, 'Travel Expense', -780, 2),
    (@BudgetId_June, 3, 'Rent Expense', -1200, 3),
    (@BudgetId_June, 4, 'Utilities Expense', -310, 1),
    (@BudgetId_June, 5, 'Entertainment Expense', -490, 2);

    COMMIT TRANSACTION;
    PRINT 'Commited';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred: ' + ERROR_MESSAGE();
END CATCH;
