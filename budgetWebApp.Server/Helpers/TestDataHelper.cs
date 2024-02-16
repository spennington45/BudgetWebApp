using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Helpers
{
    public static class TestDataHelper
    {
        public static IEnumerable<Budget> GetTestData()
        {
            return new List<Budget>
            {
                new Budget
                {
                    BudgetId = 1,
                    Date = DateTime.Now,
                    UserId = 1,
                    User = new User
                    {
                        UserId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                    },
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem
                        {
                            BudgetId = 1,
                            BugetLineItemId = 1,
                            CategoryId = 1,
                            Value = 100,
                            Label = "Test",
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 1,
                            BugetLineItemId = 3,
                            CategoryId = 1,
                            Label = "Test",
                            Value = -150,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        }
                    }
                },
                new Budget
                {
                    BudgetId = 2,
                    Date = DateTime.Now.AddMonths(-1),
                    UserId = 1,
                    User = new User
                    {
                        UserId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                    },
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 2,
                            CategoryId = 1,
                            Label = "Test",
                            Value = 100,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 4,
                            CategoryId = 1,
                            Label = "Test",
                            Value = 150,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 5,
                            CategoryId = 1,
                            Label = "Test",
                            Value = -75,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                    }
                },
                new Budget
                {
                    BudgetId = 3,
                    Date = DateTime.Now.AddYears(-1),
                    UserId = 1,
                    User = new User
                    {
                        UserId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                    },
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem
                        {
                            BudgetId = 3,
                            BugetLineItemId = 6,
                            CategoryId = 1,
                            Label = "Test",
                            Value = 100,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 3,
                            BugetLineItemId = 7,
                            CategoryId = 1,
                            Label = "Test",
                            Value = 150,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 3,
                            BugetLineItemId = 8,
                            CategoryId = 1,
                            Label = "Test",
                            Value = -75,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            },
                            SourceType = new SourceType
                            {
                                SourceTypeId = 1,
                                SourceName = "Test",
                            }
                        },
                    }
                }
            };
        }
    }
}
