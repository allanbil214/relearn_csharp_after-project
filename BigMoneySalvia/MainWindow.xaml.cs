using BigMoneySalvia.Functions;
using BigMoneySalvia.Models;
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
using System.Windows.Threading;

namespace BigMoneySalvia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLShenanigans shenanigans = new SQLShenanigans();
        public int expenseId = 0;
        public bool isEdit = false;

        public void ResetAll()
        {
            filterDatePicker.SelectedDate = DateTime.Now;
            searchCategoryComboBox.SelectedIndex = 2;
            searchTextBox.Clear();

            expenseDatePicker.SelectedDate = DateTime.Now;
            exspenseAmountTextBox.Clear();
            descTextBox.Clear();
            filterDateCheck.IsChecked = false;
            categoryComboBox.SelectedIndex = 0;

            saveButton.Content = "Add";
            isEdit = false;

            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void AddDeleteButtonIfNotExists()
        {
            bool deleteColumnExists = expenseDataGrid.Columns.Any(c => c.Header?.ToString() == "Delete");

            if (!deleteColumnExists)
            {
                DataGridTemplateColumn deleteColumn = new DataGridTemplateColumn
                {
                    Header = "Delete",
                    Width = new DataGridLength(80) // Fixed width for delete button
                };

                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonFactory.SetValue(Button.ContentProperty, "Delete");
                buttonFactory.SetValue(Button.BackgroundProperty, Brushes.Red);
                buttonFactory.SetValue(Button.ForegroundProperty, Brushes.White);
                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(deleteButton_Click));

                DataTemplate buttonTemplate = new DataTemplate();
                buttonTemplate.VisualTree = buttonFactory;
                deleteColumn.CellTemplate = buttonTemplate;

                expenseDataGrid.Columns.Add(deleteColumn);
            }
        }

        private bool CheckFloat(string value, out float amount)
        {
            if (!float.TryParse(value, out amount))
            {
                MessageBox.Show("Amount harus berupa angka!");
                return false;
            }
            return true;
        }

        private void MoveDeleteButtonToRight()
        {
            var deleteColumn = expenseDataGrid.Columns.FirstOrDefault(c => c.Header?.ToString() == "Delete");
            if (deleteColumn != null && expenseDataGrid.Columns.Count > 1)
            {
                deleteColumn.DisplayIndex = expenseDataGrid.Columns.Count - 1;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            filterDatePicker.SelectedDate = DateTime.Now;
            expenseDatePicker.SelectedDate = DateTime.Now;
            
            shenanigans.LoadCategory(this);
            shenanigans.LoadColumnNames(this);
            shenanigans.LoadExpense(this);

            AddDeleteButtonIfNotExists();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (expenseDataGrid.Columns.Count > 0)
                {
                    MoveDeleteButtonToRight();
                }
            }), DispatcherPriority.Loaded);
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            shenanigans.LoadExpense(this);
            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void searchCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            shenanigans.LoadExpense(this);
            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void filterDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            shenanigans.LoadExpense(this);
            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void expenseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            shenanigans.GetRowData();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            shenanigans.LoadExpense(this);
            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void filterDateCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            shenanigans.LoadExpense(this);
            AddDeleteButtonIfNotExists();
            MoveDeleteButtonToRight();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ResetAll();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext != null)
            {
                var data = btn.DataContext;
                var idProp = data.GetType().GetProperty("Id");

                if (idProp != null)
                {
                    int id = (int)idProp.GetValue(data);
                    var result = MessageBox.Show($"Delete this expense (ID: {id})?", "Confirm Delete", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        shenanigans.DeleteExpense(id);
                        ResetAll();
                    }
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit == false)
            {
                if (!CheckFloat(exspenseAmountTextBox.Text, out float amount)) return;

                if (expenseDatePicker.SelectedDate == null || categoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Tanggal dan kategori wajib diisi.");
                    return;
                }

                DateOnly expenseDate = DateOnly.FromDateTime(expenseDatePicker.SelectedDate.Value);
                shenanigans.CreateExpense(expenseDate, amount, descTextBox.Text, (int)categoryComboBox.SelectedValue);
                ResetAll();
            }
            else
            {
                if (!CheckFloat(exspenseAmountTextBox.Text, out float amount)) return;

                if (expenseDatePicker.SelectedDate == null || categoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Tanggal dan kategori wajib diisi.");
                    return;
                }

                DateOnly expenseDate = DateOnly.FromDateTime(expenseDatePicker.SelectedDate.Value);
                shenanigans.EditExpense(expenseDate, amount, descTextBox.Text, (int)categoryComboBox.SelectedValue, expenseId);
                ResetAll();
            }
        }
    }
}