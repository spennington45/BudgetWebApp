using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Helpers
{
    public static class TestDataHelper
    {
        public static IEnumerable<Budget> GetTestData()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Test",
                ExternalId = "Test",
                Email = "Test",
            };

            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Food" },
                new Category { CategoryId = 2, CategoryName = "Travel" },
                new Category { CategoryId = 3, CategoryName = "Rent" },
                new Category { CategoryId = 4, CategoryName = "Utilities" },
                new Category { CategoryId = 5, CategoryName = "Entertainment" },
                new Category { CategoryId = 6, CategoryName = "Salary" },
                new Category { CategoryId = 7, CategoryName = "Freelance" }
            };

            var sources = new List<SourceType>
            {
                new SourceType { SourceTypeId = 1, SourceName = "Credit Card" },
                new SourceType { SourceTypeId = 2, SourceName = "Bank Account" },
                new SourceType { SourceTypeId = 3, SourceName = "Cash" }
            };

            var budgets = new List<Budget>();
            int budgetIdCounter = 1;
            int lineItemIdCounter = 1;

            var startDate = new DateTime(DateTime.Now.Year - 1, 7, 1);
            var random = new Random();

            for (int i = 0; i < 12; i++)
            {
                var budgetDate = startDate.AddMonths(i);
                var budgetLineItems = new List<BudgetLineItem>();

                var incomeCategories = new[] { categories[5], categories[6] };
                foreach (var cat in incomeCategories)
                {
                    budgetLineItems.Add(new BudgetLineItem
                    {
                        BudgetId = budgetIdCounter,
                        BudgetLineItemId = lineItemIdCounter++,
                        CategoryId = cat.CategoryId,
                        Label = cat.CategoryName,
                        Value = random.Next(2000, 5000),
                        Category = cat,
                        SourceType = new SourceType { SourceTypeId = 4, SourceName = "Income" }
                    });
                }

                var expenseCategories = new[] { categories[0], categories[1], categories[2], categories[3], categories[4] };
                int expenseCount = random.Next(3, 8);

                for (int j = 0; j < expenseCount; j++)
                {
                    var cat = expenseCategories[random.Next(expenseCategories.Length)];
                    budgetLineItems.Add(new BudgetLineItem
                    {
                        BudgetId = budgetIdCounter,
                        BudgetLineItemId = lineItemIdCounter++,
                        CategoryId = cat.CategoryId,
                        Label = $"{cat.CategoryName} Expense {j + 1}",
                        Value = -random.Next(50, 1500),
                        Category = cat,
                        SourceType = sources[random.Next(sources.Count)]
                    });
                }

                budgets.Add(new Budget
                {
                    BudgetId = budgetIdCounter++,
                    Date = budgetDate,
                    UserId = user.UserId,
                    User = user,
                    BudgetLineItems = budgetLineItems
                });
            }

            return budgets;
        }
    }
}