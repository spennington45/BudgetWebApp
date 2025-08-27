CREATE TABLE [User] (
    UserId BIGINT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50)
);

CREATE TABLE Category (
    CategoryId BIGINT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(150)
);

CREATE TABLE SourceType (
    SourceTypeId BIGINT IDENTITY(1,1) PRIMARY KEY,
    SourceName VARCHAR(250)
);

CREATE TABLE Budget (
    BudgetId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Date DATETIME,
    UserId BIGINT NOT NULL,
    CONSTRAINT FK_Budget_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId)
        ON DELETE NO ACTION
);

CREATE TABLE BudgetTotal (
    BudgetTotalId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId BIGINT NOT NULL,
    TotalValue DECIMAL(18,2) NOT NULL DEFAULT 0,

    CONSTRAINT FK_BudgetTotal_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId)
        ON DELETE NO ACTION
);

CREATE TABLE BudgetLineItem (
    BugetLineItemId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Label VARCHAR(250),
    BudgetId BIGINT NOT NULL,
    CategoryId BIGINT NOT NULL,
    SourceTypeId BIGINT,
    CONSTRAINT FK_BugetLineItem_Budget FOREIGN KEY (BudgetId)
        REFERENCES Budget(BudgetId)
        ON DELETE NO ACTION,
    CONSTRAINT FK_BugetLineItem_Category FOREIGN KEY (CategoryId)
        REFERENCES Category(CategoryId)
        ON DELETE NO ACTION,
    CONSTRAINT FK_BudgetLineItem_SourceType FOREIGN KEY (SourceTypeId)
        REFERENCES SourceType(SourceTypeId)
        ON DELETE NO ACTION
);

CREATE TABLE RecurringExpense (
    RecurringExpensesId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Label VARCHAR(250),
    CategoryId BIGINT NOT NULL,
    SourceTypeId BIGINT,
    Value DECIMAL(18,2) NOT NULL DEFAULT 0,
    UserId BIGINT NOT NULL,
    CONSTRAINT FK_RecurringExpenses_Category FOREIGN KEY (CategoryId)
        REFERENCES Category(CategoryId)
        ON DELETE NO ACTION,
    CONSTRAINT FK_RecurringExpenses_SourceType FOREIGN KEY (SourceTypeId)
        REFERENCES SourceType(SourceTypeId)
        ON DELETE NO ACTION,
    CONSTRAINT FK_RecurringExpenses_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId)
        ON DELETE NO ACTION
);
