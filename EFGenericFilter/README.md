## Run
Изменить строку подключения к бд в файле `EFGenericFilter/appsettings.json`
```shell
dotnet run
```
## Attributes
### IgnoreFiltering
Атрибут нужен для того чтобы игнорировать некоторые ненужные для фильтрации параметры
```c#
class FilteringParams
{
    // свойство не нужное для фильтрации
    [IgnoreFiltering]
    int PageNumber { get; set; }
    // свойство не нужное для фильтрации
    [IgnoreFiltering]
    int PageSize { get; set; }
    
    string Name { get; set; }
}
```
### CompareWith
Добавьте этот атрибут если название параметра фильтрации не совпадает с названием свойства модели таблицы
```c#
class FilteringParams 
{
    // свойство которое не нуждается в атрибуте CompareWith 
    int Id { get; set; }
    // свойство которому необходимо добавить атрибут CompareWith
    [CompareWith(nameof(Model.Property))]
    string Prop { get; set; }
}

class Model 
{
    int Id { get; set; }
    string Property { get; set; }
}
```
### OperationType
Тип операции проводимое между параметром фильтрации и свойством модели, если не указывать тип операции, по умолчанию будет проводится операции Equal
#### Operations 
```c#
enum Operations 
{
    Equal, // равно
    Contains, // содержит, только для тех типов которые поддерживают операцию содержания
    LessThanOrEqual, // менье либо равно
    GreaterThanOrEqual, // больше либо равно
    LessThan, // меньше
    GreaterThan, // больше
    NotEqual // не равно
}

class FilteringParams
{
    int Id { get; set; }
    [OperationType(Operations.Contains)]
    string Name { get; set; }
}
```


### Usage
```c#
public class FilteringParams 
{
    int Id { get; set; }
    string Name { get; set; }
}

public class ModelRepository 
{
    private readonly DataContext _context;

    public class ModelRepository(DataContext context) 
    {
        _context = context;
    }

    // Обычный запрос
    public List<Model> GetFilteredModels(FilteringParams parameters) 
    {
        return _context.Models.Where(m=> m.Name.Contains(parameters.Name) && m.Id.Equals(parameters.Id)).ToList();
    }
    
    // Запрос с generic фильтрацией
    public List<Model> GetGenericFilteredModels(FilteringParams parameters) 
    {
        return _context.Models.Filter(parameters).ToList();
    }
}
```