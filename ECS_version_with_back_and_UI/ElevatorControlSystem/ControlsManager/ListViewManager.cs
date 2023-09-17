using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElevatorControlSystem
{
    public class ListViewManager<T> where T : class
    {
        private List<T> collection;

        public ListViewManager(List<T> collection)
        {
            this.collection = collection;
        }

        public void ConfigureListViewColumns(ListView listView, string tableName)
        {
            listView.View = new GridView();

            switch (tableName)
            {
                case "Lab1":
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Номер накладной", DisplayMemberBinding = new System.Windows.Data.Binding("numInvoice") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Дата прихода", DisplayMemberBinding = new System.Windows.Data.Binding("arrivalDate") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Номер ТС", DisplayMemberBinding = new System.Windows.Data.Binding("numTC") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Поставщик", DisplayMemberBinding = new System.Windows.Data.Binding("supplier") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Наименование продукции", DisplayMemberBinding = new System.Windows.Data.Binding("productTitle") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Физ. Вес", DisplayMemberBinding = new System.Windows.Data.Binding("physicalWeight") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Создатель", DisplayMemberBinding = new System.Windows.Data.Binding("CreatedBy") });
                    break;

                case "Prod1":
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Id", DisplayMemberBinding = new System.Windows.Data.Binding("Id") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Test1", DisplayMemberBinding = new System.Windows.Data.Binding("otherColumn1") });
                    ((GridView)listView.View).Columns.Add(new GridViewColumn { Header = "Test2", DisplayMemberBinding = new System.Windows.Data.Binding("otherColumn2") });
                    break;

                default:
                    break;
            }
        }

        public List<T> SearchListView<TItem>(ListView listView, string argument1, string argument2, string argument3, string firstProperty, string secondProperty, string thirdProperty)
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

            RefreshListView(listView, results);
            return results;
        }

        private void RefreshListView(ListView listView, List<T> items)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = items;
        }
    }
}
