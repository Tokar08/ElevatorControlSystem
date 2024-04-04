using ElevatorControlSystem.ControlsManager;
using ElevatorControlSystem.Repozitories;
using ElevatorControlSystem.RepozitoryInterfaces;
using GrainElevatorCS_ef;
using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElevatorControlSystem
{
    public static class ListViewManager
    {
        public static async Task FilterAndRefreshListView<T>(ListView listView, Dictionary<string, int> dictionary1, Dictionary<string, int> dictionary2, TextBox filterTextBox1, TextBox filterTextBox2, DateTime? date = null)
        {
            try
            {
                bool isValidInput1 = CheckManager.IsNotNullOrWhiteSpace(filterTextBox1.Text);
                bool isValidInput2 = CheckManager.IsNotNullOrWhiteSpace(filterTextBox2.Text);

                int? id1 = null;
                int? id2 = null;

                if (isValidInput1 && dictionary1.TryGetValue(filterTextBox1.Text, out var tempId1))
                {
                    id1 = tempId1;
                }

                if (isValidInput2 && dictionary2.TryGetValue(filterTextBox2.Text, out var tempId2))
                {
                    id2 = tempId2;
                }

                if (!date.HasValue)
                {
                    NotificationManager.ShowInfoMessageBox("Пожалуйста, выберите дату. В этой фильтрации она неотъемлемый элемент.");
                    return;
                }

                using var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions());
                var laboratoryCardRepContext = new LaboratoryCardRepository(db);

                var laboratoryCards = await laboratoryCardRepContext.GetAllAsync();
                IEnumerable<LaboratoryCard> laboratories = db.LaboratoryCards.Include(lc => lc.IdNavigation).ToList();

                var filteredEntities = laboratories
                    .Where(lc =>
                        (!id1.HasValue || lc.IdNavigation.SupplierId == id1) &&
                        (!id2.HasValue || lc.IdNavigation.ProductTitleId == id2) &&
                        (lc.IdNavigation.ArrivalDate.Date == date.Value.Date))
                    .ToList();

                if (filteredEntities.Any())
                {
                    listView.ItemsSource = filteredEntities;
                }
                else
                {
                    NotificationManager.ShowInfoMessageBox("Ничего не найдено по указанным параметрам.");
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }


        public static async Task FilterAndRefreshListView<T>(ListView listView, IBaseRepository<T> repository, Dictionary<string, int> dictionary1, Dictionary<string, int> dictionary2, TextBox filterTextBox1, TextBox filterTextBox2)
        {
            try
            {
                bool isValidInput1 = CheckManager.IsNotNullOrWhiteSpace(filterTextBox1.Text);
                bool isValidInput2 = CheckManager.IsNotNullOrWhiteSpace(filterTextBox2.Text);

                if (isValidInput1 || isValidInput2)
                {
                    int? id1 = null;
                    int? id2 = null;

                    if (isValidInput1 && dictionary1.TryGetValue(filterTextBox1.Text, out var tempId1))
                    {
                        id1 = tempId1;
                    }

                    if (isValidInput2 && dictionary2.TryGetValue(filterTextBox2.Text, out var tempId2))
                    {
                        id2 = tempId2;
                    }

                    if ((isValidInput1 && !id1.HasValue) || (isValidInput2 && !id2.HasValue))
                    {
                        StringBuilder errorMessage = new StringBuilder("Введенные Вами значение не существует:\n");

                        if (isValidInput1 && !id1.HasValue)
                        {
                            errorMessage.AppendLine($" • {filterTextBox1.Text}");
                        }

                        if (isValidInput2 && !id2.HasValue)
                        {
                            errorMessage.AppendLine($" • {filterTextBox2.Text}");
                        }

                        NotificationManager.ShowErrorMessageBox(errorMessage.ToString());
                        return;
                    }

                    var items = await repository.GetAllAsync();

                    var filteredEntities = items.Where(entity =>
                        (!id1.HasValue || GetPropertyValue<int>(entity, "SupplierId") == id1) &&
                        (!id2.HasValue || GetPropertyValue<int>(entity, "ProductTitleId") == id2)
                    ).ToList();

                    if (filteredEntities.Count > 0)
                    {
                        listView.ItemsSource = filteredEntities;
                    }
                    else
                    {
                        NotificationManager.ShowInfoMessageBox("Ничего не найдено по указанным параметрам.");
                    }
                }
                else
                {
                    listView.ItemsSource = await repository.GetAllAsync();
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }

     

        public static async Task RefreshListView<T>(ListView listView, IBaseRepository<T> repository)
        {
          listView.ItemsSource = await repository.GetAllAsync();
        }

        public static void RefreshListView(ListView listView)
        {
            var itemsSource = listView.ItemsSource;
            listView.ItemsSource = null;
            listView.ItemsSource = itemsSource;
        }

        public static async Task ClearListViewAndFields<T>(ListView listView,TextBox filterTextBox1,TextBox filterTextBox2,Button calendarButton)
        {
            try
            {
                filterTextBox1.Clear();
                filterTextBox2.Clear();
                calendarButton.Content = "Выбрать дату";

                using var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions());
                LaboratoryCardRepository laboratoryCardRepository = new(db);
                List<LaboratoryCard> laboratories = await laboratoryCardRepository.GetAllAsync();
                listView.ItemsSource = laboratories;
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }


        private static T GetPropertyValue<T>(object obj, string propertyName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            return property != null ? (T)property.GetValue(obj) : default;
        }
    }
}