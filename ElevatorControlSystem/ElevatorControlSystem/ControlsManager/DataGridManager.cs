using System.Windows.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Data;
using ElevatorControlSystem.Repozitories;
using GrainElevatorCS_ef.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrainElevatorCS_ef;
using ElevatorControlSystem.ControlsManager;
using ElevatorControlSystem.Models;
using ElevatorControlSystem.RepozitoryInterfaces;
using System.Windows;
using Calendar = System.Windows.Controls.Calendar;
using System.Windows.Media;
using System.Text.RegularExpressions;

public class DataGridFilterCriteria
{
    public string FirstArgumentFilter { get; set; }
    public string SecondArgumentFilter { get; set; }
    public DateTime? DateFilter { get; set; }
}

public class DataGridManager
{
    private DataGridFilterCriteria currentFilterCriteria;
    private DataGridFilterCriteria defaultFilterCriteria;
    private Db db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions());
    public DataGridManager()
    {
        defaultFilterCriteria = new DataGridFilterCriteria
        {
            FirstArgumentFilter = string.Empty,
            SecondArgumentFilter = string.Empty,
            DateFilter = null
        };

    }

    public async Task ConfigureDataGridColumns(DataGrid dataGrid, string tableName, DataGridFilterCriteria filterCriteria = null)
    {
        currentFilterCriteria = filterCriteria;
        await ApplyFilterAsync(dataGrid, tableName);
        dataGrid.CellEditEnding += DataGrid_CellEditEnding;
    }



    private async Task ApplyFilterAsync(DataGrid dataGrid, string tableName)
    {
        var inputInvoices = new InputInvoiceRepository(db);
        var laboratories = new LaboratoryCardRepository(db);
        var registerRepository = new RegisterRepository(db);
        var complectionReportRepository = new CompletionReportRepository(db);
        var priceListRepository = new PriceListRepository(db);
        var outputIncoicesRepository = new OutputInvoiceRepository(db);
        var usersRepository = new UserRepository(db);

        var categoryRepository = new CategoryRepository(db);
        var depoItemsRepository = new DepotItemRepository(db);
        var priceByOperation = new PriceByOperationRepository(db);
        var productionBatch = new ProductionBatchRepository(db);
        var productTitle = new ProductTitleRepository(db);
        var supllier = new SupplierRepository(db);
        var technologicalOperation = new TechnologicalOperationRepository(db);

        dataGrid.Columns.Clear();

        switch (tableName)
        {

            case "Входные накладные":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер накладной:", Binding = new Binding("InvNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода:", Binding = new Binding("ArrivalDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер ТС:", Binding = new Binding("VehicleNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("ProductTitle.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Физический вес, кг:", Binding = new Binding("PhysicalWeight") });

                var inputInvoicesData = await inputInvoices.GetAllAsync();
                var filteredInputInvoices = await ApplyFilterAsync(inputInvoicesData, currentFilterCriteria);
                dataGrid.ItemsSource = filteredInputInvoices.Any() ? filteredInputInvoices : inputInvoicesData;

                break;

            case "Лабораторные карточки":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "№ Лаб. карточки:", Binding = new Binding("LabCardNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода:", Binding = new Binding("IdNavigation.ArrivalDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("IdNavigation.Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("IdNavigation.ProductTitle.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Физический вес, кг:", Binding = new Binding("IdNavigation.PhysicalWeight") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Сорная примесь, %:", Binding = new Binding("Weediness") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Влажность, %:", Binding = new Binding("Moisture") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Зерновая примесь, %:", Binding = new Binding("GrainImpurity") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "В производство:", Binding = new Binding("IsProduction") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Особые отметки:", Binding = new Binding("SpecialNotes") });

                List<LaboratoryCard> laboratoryCards  = await laboratories.GetAllAsync();
                var filteredLaboratories = await ApplyFilterAsync(laboratoryCards, currentFilterCriteria);
                dataGrid.ItemsSource = filteredLaboratories.Any() ? filteredLaboratories : laboratoryCards;

                break;


            case "Реестры":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "№ Реестра:", Binding = new Binding("RegisterNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода:", Binding = new Binding("ArrivalDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("ProductTitle.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Физический вес, кг:", Binding = new Binding("PhysicalWeightReg") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Усушка, кг:", Binding = new Binding("ShrinkageReg") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Сорная убыль, кг:", Binding = new Binding("WasteReg") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Зачетный вес, кг:", Binding = new Binding("AccWeightReg") });
  
                var registerData = await registerRepository.GetAllAsync();
                var filteredRegister = await ApplyFilterAsync<Register>(registerData, currentFilterCriteria);
                dataGrid.ItemsSource = filteredRegister.Any() ? filteredRegister : registerData;

                break;

            case "Акт доработки":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "№ Акта доработки:", Binding = new Binding("ReportNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата составления:", Binding = new Binding("ReportDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("ProductTitle.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Количество сушки, т * %:", Binding = new Binding("QuantityesDrying") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Физический вес, т:", Binding = new Binding("PhysicalWeightReport") { StringFormat = "0.00" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Финансовый расчет:", Binding = new Binding("IsFinalized") });

                var complectionReportData = await complectionReportRepository.GetAllAsync();
                var filterdComplactionReport = await ApplyFilterAsync<CompletionReport>(complectionReportData, currentFilterCriteria);
                dataGrid.ItemsSource = filterdComplactionReport.Any() ? filterdComplactionReport : complectionReportData;

                break;

            case "Прайс-лист":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование продукции:", Binding = new Binding("ProductTitle") });

                var priceListData = await priceListRepository.GetAllAsync();
                var filterdPriceList = await ApplyFilterAsync<PriceList>(priceListData, currentFilterCriteria);
                dataGrid.ItemsSource = filterdPriceList.Any() ? filterdPriceList : priceListData;

                break;

            case "Расходные накладные":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "№ Расходной накладной:", Binding = new Binding("OutInvNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата отгрузки:", Binding = new Binding("ShipmentDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Номер ТС:", Binding = new Binding("VehicleNumber") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("ProductTitle.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Категория продукции:", Binding = new Binding("Category") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Вес продукции, кг:", Binding = new Binding("ProductWeight") });

                var outputInvoicesData = await outputIncoicesRepository.GetAllAsync();
                var filteredOutputInvoice = await ApplyFilterAsync<OutputInvoice>(outputInvoicesData, currentFilterCriteria);
                dataGrid.ItemsSource = filteredOutputInvoice.Any() ? filteredOutputInvoice : outputInvoicesData;

                break;

            case "Пользователи":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Имя:", Binding = new Binding("FirstName") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия:", Binding = new Binding("LastName") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения:", Binding = new Binding("BirthDate") { StringFormat = "dd.MM.yyyy" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Email:", Binding = new Binding("Email") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Телефон:", Binding = new Binding("Phone") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Пол:", Binding = new Binding("Gender") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Город:", Binding = new Binding("City") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Страна:", Binding = new Binding("Country") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Производственное подразделение:", Binding = new Binding("Role") });

                var usersData = await usersRepository.GetAllAsync();
                var filteredUsers = await ApplyFilterAsync<User>(usersData, currentFilterCriteria);
                dataGrid.ItemsSource = filteredUsers.Any() ? filteredUsers : usersData;

                break;


            case "Категории складских единиц":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории:", Binding = new Binding("CategoryTitle") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Значение категории:", Binding = new Binding("CategoryValue") });

                var categoryRepositoryData = await categoryRepository.GetAllAsync();
                var filteredCategoryRepository = await ApplyFilterAsync(categoryRepositoryData, currentFilterCriteria);
                dataGrid.ItemsSource = filteredCategoryRepository.Any() ? filteredCategoryRepository : categoryRepositoryData;

                break;


            case "Складские единицы":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Supplier.Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Продукция:", Binding = new Binding("ProductTitle.Title") });

                var depoItemsData = await depoItemsRepository.GetAllAsync();
                var filteredDepoItems = await ApplyFilterAsync(depoItemsData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredDepoItems.Any() ? filteredDepoItems : depoItemsData;

                break;


            case "Произ. партия":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Базовая сорная примесь, %:", Binding = new Binding("WeedinessBase") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Базовая влажность, %:", Binding = new Binding("MoistureBase") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Отход, кг:", Binding = new Binding("Waste") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Усушка, кг:", Binding = new Binding("Shrinkage") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Зачетный вес, кг:", Binding = new Binding("AccountWeight") });
               
                var productionBatchData = await productionBatch.GetAllAsync();
                var filteredproductionBatch = await ApplyFilterAsync(productionBatchData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredproductionBatch.Any() ? filteredproductionBatch : productionBatchData;

                break;

            case "Продукция":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование:", Binding = new Binding("Title") });

                var productTitleData = await productTitle.GetAllAsync();
                var filteredProductTitle = await ApplyFilterAsync(productTitleData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredProductTitle.Any() ? filteredProductTitle : productTitleData;

                break;

            case "Поставщики":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Поставщик:", Binding = new Binding("Title") });


                var suppliersData = await supllier.GetAllAsync();
                var filteredSuppliers = await ApplyFilterAsync(suppliersData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredSuppliers.Any() ? filteredSuppliers : suppliersData;


                break;

            case "Цены операций":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Название операции:", Binding = new Binding("OperationTitle") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Цена операции, грн/т:", Binding = new Binding("OperationPrice") });
     

                var priceByOperationData = await priceByOperation.GetAllAsync();
                var filteredPriceByOperation = await ApplyFilterAsync(priceByOperationData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredPriceByOperation.Any() ? filteredPriceByOperation : priceByOperationData;


                break;

            case "Тех. оперции":
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование операции:", Binding = new Binding("Title") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Количество продукции, т:", Binding = new Binding("Amount") { StringFormat = "0.00" } });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Цена операции, грн/т:", Binding = new Binding("Price") });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Общая стоимость, грн:", Binding = new Binding("TotalCost") });
    
                var technologicalOperationData = await technologicalOperation.GetAllAsync();
                var filteredTechnologicalOperation = await ApplyFilterAsync(technologicalOperationData, currentFilterCriteria);

                dataGrid.ItemsSource = filteredTechnologicalOperation.Any() ? filteredTechnologicalOperation : technologicalOperationData;



                break;

            default:
                break;


        }


    }
    private async Task SaveChangesForItemsAsync<T>(IEnumerable<T> items, Func<Db, IBaseRepository<T>> repositoryFactory)
    {
        try
        {
            var errorOccurred = false;
            var tasks = new List<Task>();

            foreach (var item in items)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
                        {
                            var repository = repositoryFactory(db);
                            await repository.UpdateAsync(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorOccurred = true;
                        NotificationManager.ShowErrorMessageBox($"Ошибка обновления элемента: {ex.InnerException?.Message ?? ex.Message}");

                    }
                }));
            }

            await Task.WhenAll(tasks);

            if (!errorOccurred)
            {
                NotificationManager.ShowSuccessMessageBox("Изменения успешно сохранены!");
            }
        }
        catch (Exception ex)
        {
            NotificationManager.ShowErrorMessageBox($"Ошибка обработки элементов: {ex.Message}");
        }
    }

    public async Task SaveChangesAsync(DataGrid dataGrid, string tableName)
    {
        switch (tableName)
        {
            case "Входные накладные":
                await SaveChangesForItemsAsync((IEnumerable<InputInvoice>)dataGrid.ItemsSource, db => new InputInvoiceRepository(db));
                break;

            case "Лабораторные карточки":
                await SaveChangesForItemsAsync((IEnumerable<LaboratoryCard>)dataGrid.ItemsSource, db => new LaboratoryCardRepository(db));
                break;
            case "Реестры":
                await SaveChangesForItemsAsync((IEnumerable<Register>)dataGrid.ItemsSource, db => new RegisterRepository(db));
                break;


            case "Акт доработки":
                await SaveChangesForItemsAsync((IEnumerable<CompletionReport>)dataGrid.ItemsSource, db => new CompletionReportRepository(db));
                break;

            case "Прайс-лист":
                await SaveChangesForItemsAsync((IEnumerable<PriceList>)dataGrid.ItemsSource, db => new PriceListRepository(db));
                break;

            case "Расходные накладные":
                await SaveChangesForItemsAsync((IEnumerable<OutputInvoice>)dataGrid.ItemsSource, db => new OutputInvoiceRepository(db));

                break;

            case "Пользователи":
                await SaveChangesForItemsAsync((IEnumerable<User>)dataGrid.ItemsSource, db => new UserRepository(db));
                break;


            case "Категории складских единиц":
                await SaveChangesForItemsAsync((IEnumerable<DepotItemCategory>)dataGrid.ItemsSource, db => new CategoryRepository(db));
                break;


            case "Складские единицы":
                await SaveChangesForItemsAsync((IEnumerable<DepotItem>)dataGrid.ItemsSource, db => new DepotItemRepository(db));
                break;


            case "Произ. партия":
                await SaveChangesForItemsAsync((IEnumerable<ProductionBatch>)dataGrid.ItemsSource, db => new ProductionBatchRepository(db));
                break;

            case "Продукция":
                await SaveChangesForItemsAsync((IEnumerable<ProductTitle>)dataGrid.ItemsSource, db => new ProductTitleRepository(db));
                break;

            case "Поставщики":
                await SaveChangesForItemsAsync((IEnumerable<Supplier>)dataGrid.ItemsSource, db => new SupplierRepository(db));
                break;

            case "Цены операций":
                await SaveChangesForItemsAsync((IEnumerable<PriceByOperation>)dataGrid.ItemsSource, db => new PriceByOperationRepository(db));
                break;

            case "Тех. оперции":
                await SaveChangesForItemsAsync((IEnumerable<TechnologicalOperation>)dataGrid.ItemsSource, db => new TechnologicalOperationRepository(db));
                break;

            default:
                break;
        }
    }


    private async Task<IEnumerable<T>> ApplyFilterAsync<T>(IEnumerable<T> data, DataGridFilterCriteria criteria)
    {
        await Task.Delay(0);

        var filteredData = data;

        if (criteria != null)
        {
            if (!string.IsNullOrEmpty(criteria.FirstArgumentFilter))
            {
                filteredData = filteredData.Where(item => CheckItemProperties(item, criteria.FirstArgumentFilter));
            }

            if (!string.IsNullOrEmpty(criteria.SecondArgumentFilter))
            {
                filteredData = filteredData.Where(item => CheckItemProperties(item, criteria.SecondArgumentFilter));
            }

            if (criteria.DateFilter != null)
            {
                filteredData = filteredData.Where(item => CheckDateProperty(item, criteria.DateFilter.Value));
            }
        }

        var result = filteredData.ToList();

        if (!result.Any())
        {
            NotificationManager.ShowInfoMessageBox("Ничего не найдено по Вашему запросу.\nПопробуйте еще раз!");
        }

        return result;
    }

    private bool CheckItemProperties<T>(T item, string filter, HashSet<string> visitedProperties = null)
    {
        if (item == null)
        {
            return false;
        }

        visitedProperties ??= new HashSet<string>();

        var properties = item.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (visitedProperties.Contains(property.Name))
            {
                continue;
            }

            visitedProperties.Add(property.Name);

            if (property.Name.Equals("Id"))
            {
                continue;
            }

            var propertyValue = property.GetValue(item);

            if (propertyValue == null)
            {
                continue;
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                if (CheckItemProperties(propertyValue, filter, visitedProperties))
                {
                    return true;
                }
            }
            else
            {
                var stringValue = propertyValue.ToString().Trim();
                if (!string.IsNullOrEmpty(stringValue) && Regex.IsMatch(stringValue, $@"\b{Regex.Escape(filter)}\b", RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }



    private bool CheckDateProperty<T>(T item, DateTime filterDate)
    {
        var properties = item.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                var dateValue = property.GetValue(item, null);
                if (dateValue is DateTime dt && dt.Date == filterDate.Date)
                {
                    return true;
                }
            }
            else if (property.PropertyType.IsClass)
            {
                var nestedPropertyValue = property.GetValue(item, null);

                if (nestedPropertyValue != null)
                {
                    var nestedProperties = nestedPropertyValue.GetType().GetProperties();

                    foreach (var nestedProperty in nestedProperties)
                    {
                        if (nestedProperty.PropertyType == typeof(DateTime))
                        {
                            var nestedDateValue = nestedProperty.GetValue(nestedPropertyValue, null);
                            if (nestedDateValue is DateTime nestedDt && nestedDt.Date == filterDate.Date)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
    private void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : DependencyObject
    {
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            if (child is T)
            {
                visualCollection.Add(child as T);
            }
            else
            {
                GetVisualChildCollection(child, visualCollection);
            }
        }
    }
    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.EditAction == DataGridEditAction.Commit && Validation.GetHasError(e.EditingElement))
        {
            e.Cancel = true;

            string errorMessage = Validation.GetErrors(e.EditingElement).OfType<ValidationError>().FirstOrDefault()?.ErrorContent.ToString();

            NotificationManager.ShowErrorMessageBox(errorMessage);

            if (e.EditingElement is FrameworkElement editedCell)
            {
                editedCell.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            }
        }
    }

    public void Reset(TextBox tbForFirstArgument, TextBox tbForSecondArgument, Button dateButton, Calendar datePicker, DataGrid dataGrid, string tableName)
    {
        tbForFirstArgument.Text = string.Empty;
        tbForSecondArgument.Text = string.Empty;
        datePicker.SelectedDate = null;

        currentFilterCriteria = defaultFilterCriteria;
        ApplyFilterAsync(dataGrid, tableName);

        dateButton.Content = "Выберите дату";
    }



    public async Task RefreshDataGridAsync(DataGrid dataGrid, string tableName)
    {
        await ConfigureDataGridColumns(dataGrid, tableName, currentFilterCriteria);
    }

  
}
