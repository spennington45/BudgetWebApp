INSERT INTO [User] (UserId, FirstName, LastName)
VALUES (1, 'Sam', 'Inner');

INSERT INTO Category (CategoryId, CategoryName)
VALUES 
(1, 'Food'),
(2, 'Travel'),
(3, 'Rent'),
(4, 'Utilities'),
(5, 'Entertainment'),
(6, 'Salary'),
(7, 'Freelance');

INSERT INTO SourceType (SourceTypeId, SourceName)
VALUES 
(1, 'Credit Card'),
(2, 'Bank Account'),
(3, 'Cash'),
(4, 'Income'); -- Used for income line items

INSERT INTO Budget (BudgetId, Date, UserId)
VALUES
(1, '2024-07-01', 1),
(2, '2024-08-01', 1),
(3, '2024-09-01', 1),
(4, '2024-10-01', 1),
(5, '2024-11-01', 1),
(6, '2024-12-01', 1),
(7, '2025-01-01', 1),
(8, '2025-02-01', 1),
(9, '2025-03-01', 1),
(10, '2025-04-01', 1),
(11, '2025-05-01', 1),
(12, '2025-06-01', 1);

-- BudgetId 1 (July)
INSERT INTO BudgetLineItem (BugetLineItemId, BudgetId, CategoryId, Label, Value, SourceTypeId) VALUES
(1, 1, 6, 'Salary', 4200, 4),
(2, 1, 7, 'Freelance', 1800, 4),
(3, 1, 1, 'Food Expense', -500, 1),
(4, 1, 2, 'Travel Expense', -750, 2),
(5, 1, 3, 'Rent Expense', -1200, 3),
(6, 1, 4, 'Utilities Expense', -300, 1),
(7, 1, 5, 'Entertainment Expense', -400, 2);

-- BudgetId 2 (August)
INSERT INTO BudgetLineItem VALUES
(8, 2, 6, 'Salary', 4300, 4),
(9, 2, 1, 'Food Expense', -550, 1),
(10, 2, 2, 'Travel Expense', -600, 2),
(11, 2, 3, 'Rent Expense', -1200, 3),
(12, 2, 4, 'Utilities Expense', -310, 1),
(13, 2, 5, 'Entertainment Expense', -450, 2);

-- BudgetId 3 (September)
INSERT INTO BudgetLineItem VALUES
(14, 3, 6, 'Salary', 4100, 4),
(15, 3, 7, 'Freelance', 2200, 4),
(16, 3, 1, 'Food Expense', -480, 1),
(17, 3, 2, 'Travel Expense', -700, 2),
(18, 3, 3, 'Rent Expense', -1200, 3),
(19, 3, 4, 'Utilities Expense', -290, 1),
(20, 3, 5, 'Entertainment Expense', -500, 2);

-- BudgetId 4 (October)
INSERT INTO BudgetLineItem VALUES
(21, 4, 6, 'Salary', 4400, 4),
(22, 4, 1, 'Food Expense', -530, 1),
(23, 4, 2, 'Travel Expense', -650, 2),
(24, 4, 3, 'Rent Expense', -1200, 3),
(25, 4, 4, 'Utilities Expense', -320, 1),
(26, 4, 5, 'Entertainment Expense', -470, 2);

-- BudgetId 5 (November)
INSERT INTO BudgetLineItem VALUES
(27, 5, 6, 'Salary', 4250, 4),
(28, 5, 7, 'Freelance', 2000, 4),
(29, 5, 1, 'Food Expense', -510, 1),
(30, 5, 2, 'Travel Expense', -720, 2),
(31, 5, 3, 'Rent Expense', -1200, 3),
(32, 5, 4, 'Utilities Expense', -310, 1),
(33, 5, 5, 'Entertainment Expense', -430, 2);

-- BudgetId 6 (December)
INSERT INTO BudgetLineItem VALUES
(34, 6, 6, 'Salary', 4500, 4),
(35, 6, 1, 'Food Expense', -560, 1),
(36, 6, 2, 'Travel Expense', -800, 2),
(37, 6, 3, 'Rent Expense', -1200, 3),
(38, 6, 4, 'Utilities Expense', -330, 1),
(39, 6, 5, 'Entertainment Expense', -490, 2);

-- BudgetId 7 (January)
INSERT INTO BudgetLineItem VALUES
(40, 7, 6, 'Salary', 4300, 4),
(41, 7, 7, 'Freelance', 2100, 4),
(42, 7, 1, 'Food Expense', -520, 1),
(43, 7, 2, 'Travel Expense', -680, 2),
(44, 7, 3, 'Rent Expense', -1200, 3),
(45, 7, 4, 'Utilities Expense', -300, 1),
(46, 7, 5, 'Entertainment Expense', -460, 2);

-- BudgetId 8 (February)
INSERT INTO BudgetLineItem VALUES
(47, 8, 6, 'Salary', 4400, 4),
(48, 8, 1, 'Food Expense', -540, 1),
(49, 8, 2, 'Travel Expense', -710, 2),
(50, 8, 3, 'Rent Expense', -1200, 3),
(51, 8, 4, 'Utilities Expense', -310, 1),
(52, 8, 5, 'Entertainment Expense', -480, 2);

-- BudgetId 9 (March)
INSERT INTO BudgetLineItem VALUES
(53, 9, 6, 'Salary', 4350, 4),
(54, 9, 7, 'Freelance', 1900, 4),
(55, 9, 1, 'Food Expense', -500, 1),
(56, 9, 2, 'Travel Expense', -750, 2),
(57, 9, 3, 'Rent Expense', -1200, 3),
(58, 9, 4, 'Utilities Expense', -320, 1),
(59, 9, 5, 'Entertainment Expense', -470, 2);

-- BudgetId 10 (April)
INSERT INTO BudgetLineItem VALUES
(60, 10, 6, 'Salary', 4450, 4),
(61, 10, 1, 'Food Expense', -510, 1),
(62, 10, 2, 'Travel Expense', -690, 2),
(63, 10, 3, 'Rent Expense', -1200, 3),
(64, 10, 4, 'Utilities Expense', -310, 1),
(65, 10, 5, 'Entertainment Expense', -460, 2);

-- BudgetId 11 (May)
INSERT INTO BudgetLineItem VALUES
(66, 11, 6, 'Salary', 4300, 4),
(67, 11, 7, 'Freelance', 2300, 4),
(68, 11, 1, 'Food Expense', -520, 1),
(69, 11, 2, 'Travel Expense', -740, 2),
(70, 11, 3, 'Rent Expense', -1200, 3),
(71, 11, 4, 'Utilities Expense', -300, 1),
(72, 11, 5, 'Entertainment Expense', -450, 2);

-- BudgetId 12 (June)
INSERT INTO BudgetLineItem VALUES
(73, 12, 6, 'Salary', 4550, 4),
(74, 12, 1, 'Food Expense', -530, 1),
(75, 12, 2, 'Travel Expense', -780, 2),
(76, 12, 3, 'Rent Expense', -1200, 3),
(77, 12, 4, 'Utilities Expense', -310, 1),
(78, 12, 5, 'Entertainment Expense', -490, 2);

INSERT INTO BudgetTotal (UserId, TotalValue)
VALUES (1, 15770.00);

INSERT INTO RecurringExpense (RecurringExpensesId, Label, CategoryId, SourceTypeId, UserId)
VALUES
(1, 'Monthly Rent', 3, 2, 1),
(2, 'Electric Bill', 4, 1, 1),
(3, 'Water Bill', 4, 2, 1),
(4, 'Netflix Subscription', 5, 1, 1),
(5, 'Spotify Subscription', 5, 1, 1),
(6, 'Grocery Delivery Membership', 1, 2, 1),
(7, 'Gym Membership', 5, 3, 1),
(8, 'Mobile Phone Plan', 4, 2, 1);