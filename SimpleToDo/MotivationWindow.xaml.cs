using SimpleToDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleToDo
{
    /// <summary>
    /// Interaction logic for MotivationWindow.xaml
    /// </summary>
    public partial class MotivationWindow : Window
    {
        private readonly AppDbContext _db = new AppDbContext();
        private int _selectedMotivationId = 0;
        private bool _deleteColumnAdded = false;

        private void ResetAll()
        {
            motivationTextBox.Clear();
            _selectedMotivationId = 0;
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
            motivTable.Columns.Add(deleteColumn);
        }

        private void LoadMotivations()
        {
            motivTable.Columns.Clear();

            List<Motivation> motivations = _db.Motivations.ToList();
            motivTable.ItemsSource = motivations;

            if (_deleteColumnAdded == true)
                AddDeleteButton();
        }

        private void StoreMotivation(string inputText)
        {
            var newMotivation = new Motivation
            {
                Text = inputText
            };

            _db.Motivations.Add(newMotivation);
            _db.SaveChanges();
        }

        private void CreateMotivation(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                MessageBox.Show("Please enter a motivational text.");
                return;
            }
            else
            {
                StoreMotivation(inputText);
                MessageBox.Show("Motivation added!");

                ResetAll();
                LoadMotivations();
            }
        }

        private void UpdateMotivation(int Id, string inputText)
        {
            var existing = _db.Motivations.FirstOrDefault(m => m.Id == Id);

            if (existing != null)
            {
                existing.Text = inputText;
                _db.Motivations.Update(existing);
                _db.SaveChanges();
            }
        }

        private void EditMotivation(int Id, string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                MessageBox.Show("Please enter a motivational text.");
                return;
            }
            else if (Id == 0)
            {
                MessageBox.Show("Please select a motivational text.");
                return;
            }
            else
            {
                UpdateMotivation(Id, inputText);
                MessageBox.Show("Motivation updated!");

                ResetAll();
                LoadMotivations();
            }
        }

        private void DeleteMotivation(int Id)
        {
            if (Id == 0)
            {
                MessageBox.Show("Select a motivation to delete.");
                return;
            }

            var toDelete = _db.Motivations.FirstOrDefault(m => m.Id == Id);
            if (toDelete != null)
            {
                _db.Motivations.Remove(toDelete);
                _db.SaveChanges();
                LoadMotivations();
                ResetAll();
            }
        }

        /// LINE BREAK LINE BREAK LINE BREAK LINE BREAK

        public MotivationWindow()
        {
            InitializeComponent();
            LoadMotivations();
            motivTable.Loaded += motivTable_Loaded;
        }

        private void motivTable_Loaded(object sender, RoutedEventArgs e)
        {
            AddDeleteButton();
            _deleteColumnAdded = true;
        }

        private void motivTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Motivation)motivTable.SelectedItem;
            if (selected != null)
            {
                _selectedMotivationId = selected.Id;
                motivationTextBox.Text = selected.Text;
                saveButton.Content = "Update It";
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMotivationId == 0)
                CreateMotivation(motivationTextBox.Text.Trim());
            else
            {
                EditMotivation(_selectedMotivationId, motivationTextBox.Text.Trim());
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Motivation motivation)
            {
                var result = MessageBox.Show($"Delete \"{motivation.Text}\"?", "Confirm Delete", MessageBoxButton.YesNo); ;
                if (result == MessageBoxResult.No)
                    return;

                DeleteMotivation(motivation.Id);
            }

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }
    }
}
