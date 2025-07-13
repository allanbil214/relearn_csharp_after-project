using Microsoft.EntityFrameworkCore;
using SimpleToDo.Models;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _db = new AppDbContext(); // to get the EF db from AppDbContext.cs
        private int _selectedTodoId = 0;
        private bool _deleteColumnAdded = false;

        private string GetMotivation()
        {
            string msg = string.Empty;

            List<Motivation> allMotivations = _db.Motivations.ToList(); // to get the EF db from Motivation.cs

            if (allMotivations.Count == 0)
            {
                return msg = "No Motivation today.";
            }

            Random random = new Random();
            int index = random.Next(allMotivations.Count);
            msg = allMotivations[index].Text;

            return msg;
        }

        private void LoadTodos()
        {
            searchTextbox.Text = "";
            filterComboBox.SelectedValue = -1;

            SearchTodos();
        }

        private void LoadStatus()
        {
            statusComboBox.ItemsSource = _db.Statuses.ToList();
            statusComboBox.DisplayMemberPath = "StatusName";
            statusComboBox.SelectedValuePath = "Id";
            statusComboBox.SelectedIndex = 0;
        }

        private void SearchTodos()
        {
            string searchText = searchTextbox.Text?.Trim().ToLower() ?? "";
            int? filterStatusId = null;

            if (filterComboBox.SelectedValue != null && (int)filterComboBox.SelectedValue != -1)
            {
                filterStatusId = (int)filterComboBox.SelectedValue;
            }

            var allTodos = _db.Todos.Include(t => t.Status).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                allTodos = allTodos.Where(t => t.Title.ToLower().Contains(searchText));
            }

            if (filterStatusId.HasValue)
            {
                allTodos = allTodos.Where(t => t.StatusId == filterStatusId.Value);
            }

            todoTable.Columns.Clear();
            todoTable.ItemsSource = allTodos.ToList();

            if (_deleteColumnAdded == true)
                AddDeleteButton();
        }

        private void LoadFilterStatus()
        {
            var allStatuses = _db.Statuses.ToList();

            var filterStatuses = new List<dynamic>();
            filterStatuses.Add(new { Id = -1, StatusName = "All" });

            foreach (var status in allStatuses)
            {
                filterStatuses.Add(new { Id = status.Id, StatusName = status.StatusName });
            }

            filterComboBox.ItemsSource = filterStatuses;
            filterComboBox.DisplayMemberPath = "StatusName";
            filterComboBox.SelectedValuePath = "Id";
            filterComboBox.SelectedIndex = 0;
        }

        private void ResetAll()
        {
            taskTextbox.Clear();
            descTextbox.Clear();
            _selectedTodoId = 0;
            saveButton.Content = "Add It";
        }

        private void AddDeleteButton()
        {
            DataGridTemplateColumn deleteColumn = new DataGridTemplateColumn
            {
                Header = "Delete"
            };

            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(Button.ContentProperty, "Delete");
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));

            DataTemplate buttonTemplate = new DataTemplate();
            buttonTemplate.VisualTree = buttonFactory;

            deleteColumn.CellTemplate = buttonTemplate;
            todoTable.Columns.Add(deleteColumn);
        }

        private void StoreTodo(string inputText, string inputDesc, int statusId)
        {
            var newTodos = new Todo
            {
                Title = inputText,
                Description = inputDesc,
                StatusId = statusId
            };

            _db.Todos.Add(newTodos);
            _db.SaveChanges();
        }

        private void CreateTodo(string inputText, string inputDesc, int statusId)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(inputDesc))
            {
                MessageBox.Show("Please fill all the textbox.");
                return;
            }
            else
            {
                StoreTodo(inputText, inputDesc, statusId);
                MessageBox.Show("Todo added!");

                ResetAll();
                LoadTodos();
            }
        }

        private void UpdateTodo(int Id, string inputText, string inputDesc, int statusId)
        {
            var existing = _db.Todos.FirstOrDefault(m => m.Id == Id);

            if (existing != null)
            {
                existing.Title = inputText;
                existing.Description = inputDesc;
                existing.StatusId = statusId;   
                _db.Todos.Update(existing);
                _db.SaveChanges();
            }
        }

        private void EditTodo(int Id, string inputText, string inputDesc, int statusId)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(inputDesc))
            {
                MessageBox.Show("Please fill all the textbox.");
                return;
            }
            else if (Id == 0)
            {
                MessageBox.Show("Please select a todo.");
                return;
            }
            else
            {
                UpdateTodo(Id, inputText, inputDesc, statusId);
                MessageBox.Show("Todo updated!");

                ResetAll();
                LoadTodos();
            }
        }

        private void DeleteTodo(int Id)
        {
            if (Id == 0)
            {
                MessageBox.Show("Select a motivation to delete.");
                return;
            }

            var toDelete = _db.Todos.FirstOrDefault(m => m.Id == Id);
            if (toDelete != null)
            {
                _db.Todos.Remove(toDelete);
                _db.SaveChanges();
                LoadTodos();
                ResetAll();
            }
        }


        /// LINE BREAK LINE BREAK LINE BREAK LINE BREAK

        public MainWindow()
        {
            InitializeComponent();

            LoadTodos();
            LoadStatus();
            LoadFilterStatus();
            motivationButton.Content = GetMotivation();
            todoTable.Loaded += todoTable_Loaded;
        }

        private void todoTable_Loaded(object sender, RoutedEventArgs e)
        {
            AddDeleteButton();
            _deleteColumnAdded = true;
        }
        private void todoTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Todo)todoTable.SelectedItem;
            if (selected != null)
            {
                _selectedTodoId = selected.Id;
                taskTextbox.Text = selected.Title;
                descTextbox.Text = selected.Description;
                statusComboBox.SelectedValue = selected.StatusId;
                saveButton.Content = "Update It";
            }
        }

        private void motivationButton_Click(object sender, RoutedEventArgs e)
        {
            MotivationWindow motivationWindow = new MotivationWindow();
            motivationWindow.Owner = this;
            motivationWindow.Show();
        }

        private void todoTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "StatusId")
            {
                e.Cancel = true; 
            }
            else if (e.PropertyName == "Status")
            {
                e.Column.Header = "Status";
                if (e.Column is DataGridBoundColumn boundColumn)
                {
                    boundColumn.Binding = new Binding("Status.StatusName");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Todo todo)
            {
                var result = MessageBox.Show($"Delete \"{todo.Title}\"?", "Confirm Delete", MessageBoxButton.YesNo); ;
                if (result == MessageBoxResult.No)
                    return;

                DeleteTodo(todo.Id);
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTodoId == 0)
                CreateTodo(taskTextbox.Text.Trim(), descTextbox.Text.Trim(), (int)statusComboBox.SelectedValue); 
            else
            {
                EditTodo(_selectedTodoId, taskTextbox.Text.Trim(), descTextbox.Text.Trim(), (int)statusComboBox.SelectedValue);
            }
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(statusComboBox.SelectedValue?.ToString());
        }

        private void searchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchTodos();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filterComboBox.SelectedIndex != -1)
            {
                SearchTodos();
            }
        }
    }

}