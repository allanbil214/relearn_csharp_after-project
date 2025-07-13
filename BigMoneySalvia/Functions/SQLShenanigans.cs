using BigMoneySalvia.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BigMoneySalvia.Functions
{

    internal class SQLShenanigans
    {
        private readonly AppDbContext _db = new AppDbContext();
        private MainWindow _mainWindow;

        public void LoadCategory(MainWindow mainWindow) 
        { 
            _mainWindow = mainWindow;

            _mainWindow.categoryComboBox.ItemsSource = _db.Categories.ToList();
            _mainWindow.categoryComboBox.DisplayMemberPath = "CategoryName";
            _mainWindow.categoryComboBox.SelectedValuePath = "Id";
            _mainWindow.categoryComboBox.SelectedIndex = 0;
        }

        public void LoadExpense(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            string searchText = _mainWindow.searchTextBox.Text?.Trim().ToLower() ?? "";
            int searchCategory = _mainWindow.searchCategoryComboBox.SelectedIndex;
            DateTime? searchDate = _mainWindow.filterDatePicker.SelectedDate;

            var allExpense = _db.Expenses.Include(t => t.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                switch (searchCategory)
                {
                    case 0: 
                        if (int.TryParse(searchText, out int id))
                            allExpense = allExpense.Where(t => t.Id == id);
                        break;

                    case 1: 
                        if (double.TryParse(searchText, out double amount))
                            allExpense = allExpense.Where(t => t.ExpensesAmount == amount);
                        break;

                    case 2:
                        allExpense = allExpense.Where(t => EF.Functions.Like(t.Description, $"%{searchText}%"));
                        break;

                    case 3:
                        allExpense = allExpense.Where(t => EF.Functions.Like(t.Category.CategoryName, $"%{searchText}%"));
                        break;
                }
            }

            if (searchDate.HasValue && _mainWindow.filterDateCheck.IsChecked == false)
            {
                DateOnly dateOnly = DateOnly.FromDateTime(searchDate.Value.Date);
                allExpense = allExpense.Where(t => t.ExpenseDate.Equals(dateOnly));
            }

            _mainWindow.sumAmountLabel.Content = $"Sum Amount: Rp{allExpense.Sum(s => s.ExpensesAmount):N2}";


            var expenseProjection = allExpense.Select(e => new
            {
                e.Id,
                ExpensesAmount = $"Rp{e.ExpensesAmount:N2}",
                e.Description,
                e.ExpenseDate,
                CategoryName = e.Category.CategoryName
            });

            _mainWindow.expenseDataGrid.ItemsSource = expenseProjection.ToList();

        }

        public void GetRowData()
        {
            var selectedRow = _mainWindow.expenseDataGrid.SelectedItem;
            if (selectedRow != null)
            {
                var idProp = selectedRow.GetType().GetProperty("Id");
                if (idProp != null)
                {
                    int id = (int)idProp.GetValue(selectedRow);
                    var selectedExpense = _db.Expenses.Include(e => e.Category).FirstOrDefault(e => e.Id == id);

                    if (selectedExpense != null)
                    {
                        _mainWindow.expenseId = selectedExpense.Id;
                        _mainWindow.expenseDatePicker.SelectedDate = selectedExpense.ExpenseDate.ToDateTime(TimeOnly.MinValue);
                        _mainWindow.exspenseAmountTextBox.Text = selectedExpense.ExpensesAmount.ToString();
                        _mainWindow.descTextBox.Text = selectedExpense.Description;
                        _mainWindow.categoryComboBox.SelectedValue = selectedExpense.CategoryId;

                        _mainWindow.saveButton.Content = "Edit";
                        _mainWindow.isEdit = true;
                    }
                }
            }
        }

        public void LoadColumnNames(MainWindow mainWindow)
        {
            var excluded = new[] { "CategoryId", "ExpenseDate" };

            var columnNames = typeof(Expense)
                .GetProperties()
                .Where(p => !excluded.Contains(p.Name))
                .Select(p => p.Name)
                .ToList();


            _mainWindow.searchCategoryComboBox.ItemsSource = columnNames;
            _mainWindow.searchCategoryComboBox.SelectedIndex = 2;
        }

        private bool CheckForm(float amount, string desc)
        {
            if (string.IsNullOrEmpty(desc) || amount <= 0)
            {
                MessageBox.Show("Please fill all the textbox");
                return false;
            }

            return true;
        }


        private bool CheckExist(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Please select an expense");
                return false;
            }

            return true;
        }


        public void StoreExpense(DateOnly date, float amount, string desc, int category)
        {
            var expense = new Expense
            {
                ExpenseDate = date,
                ExpensesAmount = amount,
                Description = desc,
                CategoryId = category
            };

            _db.Expenses.Add(expense);
            _db.SaveChanges();
        }

        public void CreateExpense(DateOnly date, float amount, string desc, int category)
        {
            if (!CheckForm(amount, desc))
                return;

            StoreExpense(date, amount, desc, category);
            MessageBox.Show("Expense Added");
        }


        public void UpdateExpense(DateOnly date, float amount, string desc, int category, int id)
        {
            var expense = _db.Expenses.FirstOrDefault(p => p.Id == id);

            if (expense != null)
            {
                expense.ExpenseDate = date;
                expense.ExpensesAmount = amount;    
                expense.Description = desc;
                expense.CategoryId = category;
                _db.Expenses.Update(expense);
                _db.SaveChanges();
            }
        }

        public void EditExpense(DateOnly date, float amount, string desc, int category, int id)
        {
            if (!CheckForm(amount, desc) || !CheckExist(id))
                return;

            UpdateExpense(date, amount, desc, category, id);
            MessageBox.Show("Expense Updated");
        }


        public void DeleteExpense(int id)
        {
            if (!CheckExist(id))
                return;

            var expense = _db.Expenses.FirstOrDefault(d => d.Id == id);
            if (expense != null)
            {
                _db.Expenses.Remove(expense);
                _db.SaveChanges();
            }
        }

    }
}
