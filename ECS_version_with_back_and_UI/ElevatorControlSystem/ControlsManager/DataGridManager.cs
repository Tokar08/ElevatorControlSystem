using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;

public class DataGridManager<T> where T : class
{
    private List<T> collection;

    public DataGridManager(List<T> collection)
    {
        this.collection = collection;
    }

    public void ConfigureDataGridColumns(DataGrid dataGrid,string tableName)
    {
        dataGrid.Columns.Clear();

        switch (tableName)
        {
            case "Lab1":
              
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер накладной:", Binding = new Binding("numInvoice") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода:", Binding = new Binding("arrivalDate") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер ТС:", Binding = new Binding("numTC") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("supplier") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование продукции:", Binding = new Binding("productTitle") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Физ. Вес:", Binding = new Binding("physicalWeight") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Создатель:", Binding = new Binding("CreatedBy") });

                break;

            case "Prod1":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Id:", Binding = new Binding("Id") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Test1:", Binding = new Binding("otherColumn1") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Test2:", Binding = new Binding("otherColumn2") });
                break;

          
            default:
                break;
        }
    }

    public List<T> SearchDataGrid<TItem>(DataGrid dataGrid, string argument1, string argument2, string argument3, string firstProperty, string secondProperty, string thirdProperty)
    {
        bool shouldFilter = !(string.IsNullOrWhiteSpace(argument1) && string.IsNullOrWhiteSpace(argument2) && string.IsNullOrWhiteSpace(argument3));

        List<T> results = new List<T>();

        foreach (T item in collection)
        {
            PropertyInfo property1Info = typeof(T).GetProperty(firstProperty);
            PropertyInfo property2Info = typeof(T).GetProperty(secondProperty);
            PropertyInfo property3Info = typeof(T).GetProperty(thirdProperty);

            object value1 = property1Info.GetValue(item);
            object value2 = property2Info.GetValue(item);
            object value3 = property3Info.GetValue(item);

            bool isMatch = true;

            if (shouldFilter)
            {
                if (!string.IsNullOrWhiteSpace(argument1) && value1.ToString() != argument1)
                {
                    isMatch = false;
                }
                if (!string.IsNullOrWhiteSpace(argument2) && value2.ToString() != argument2)
                {
                    isMatch = false;
                }
                if (!string.IsNullOrWhiteSpace(argument3))
                {
                    if (value3 is DateTime dateTime)
                    {
                        if (!DateTime.TryParseExact(argument3, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetDate) || dateTime.Date != targetDate.Date)
                        {
                            isMatch = false;
                        }
                    }
                    else if (value3.ToString() != argument3)
                    {
                        isMatch = false;
                    }
                }
            }

            if (isMatch)
            {
                results.Add(item);
            }
        }

        RefreshDataGrid(dataGrid, results);
        return results;
    }




    public bool IsDateMatch(T item, Func<T, object> propertyAccessor, string argument)
    {
        object propertyValue = propertyAccessor(item);

        if (propertyValue is DateTime dateTime)
        {
            if (DateTime.TryParseExact(argument, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetDate))
            {
                return dateTime.Date == targetDate.Date;
            }
        }

        return false;
    }


    private bool IsRowSelected(DataGrid dataGrid, T item)
    {
        DataGridCell cell = GetCell(dataGrid, item, 0);

        if (cell != null)
        {
            CheckBox checkBox = FindVisualChild<CheckBox>(cell);
            return checkBox != null && checkBox.IsChecked == true;
        }

        return false;
    }

    public void RefreshDataGrid<TItem>(DataGrid dataGrid, List<TItem> items)
    {
        dataGrid.ItemsSource = items;
    }
    public DataGridCell GetCell(DataGrid data, object item, int column)
    {
        DataGridRow row = data.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        if (row != null)
        {
            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter != null)
            {
                return presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
            }
        }
        return null;
    }

    public T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T t)
            {
                return t;
            }
            else
            {
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }
        return null;
    }

    //Примерка как это будет рабоатать с кодом 
    //private List<Item> GetDataFromDatabase(string tableName)
    //{
    //    try
    //    {
    //        using (SqlConnection connection = new SqlConnection(connectionString))
    //        {
    //            connection.Open();

    //            string query = $"SELECT * FROM {tableName}";
    //            SqlCommand command = new SqlCommand(query, connection);

    //            List<Item> items = new List<Item>();

    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Item item = new Item
    //                    {
    //                      // TODO
    //                    };
    //                    items.Add(item);
    //                }
    //            }

    //            return items;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show($"Ошибка при получении данных из БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    //        return null;
    //    }
    //}
}
