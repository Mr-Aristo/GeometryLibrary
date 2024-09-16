# GeometryLibrary - Документация

## Обзор проекта

Этот проект представляет собой библиотеку на языке C# для вычисления площади геометрических фигур. В проекте используются `Serilog` для логирования, `StackExchange.Redis` для кэширования результатов вычислений, а также применяются интерфейсы для создания обобщенного подхода к вычислению площади фигур.

### Используемые технологии

- **C# и .NET**: Основной язык программирования и платформа для реализации библиотеки.
- **Serilog**: Библиотека для логирования, обеспечивающая гибкость и расширяемость записи логов.
- **StackExchange.Redis**: Библиотека для взаимодействия с Redis, используемая для кэширования данных.

## Описание классов и интерфейсов

### Интерфейсы

#### IShape

Интерфейс `IShape` определяет базовое свойство `Name`, которое должно быть реализовано каждым классом фигуры. Это свойство возвращает название фигуры.

#### IAreaCalculable

Интерфейс `IAreaCalculable` определяет метод `CalculateArea`, который должен реализовывать любой класс фигуры для вычисления ее площади.

#### IShapeAreaCalculator<T>

Интерфейс `IShapeAreaCalculator<T>` определяет метод `CalculateAreaAsync`, который принимает объект типа `T`, реализующий интерфейс `IAreaCalculable`, и асинхронно возвращает вычисленную площадь фигуры.

### Абстрактный класс

#### Shape

Абстрактный класс `Shape` реализует интерфейс `IShape` и требует от наследников реализации свойства `Name`.

### Реализация фигур

#### Circle

Класс `Circle` представляет собой реализацию круга. Он наследует абстрактный класс `Shape` и реализует интерфейс `IAreaCalculable`. Ключевое свойство класса - `Radius`, которое определяет радиус круга. Метод `CalculateArea` вычисляет площадь круга на основе его радиуса.

#### Triangle

Класс `Triangle` представляет собой реализацию треугольника. Он также наследует абстрактный класс `Shape` и реализует интерфейс `IAreaCalculable`. В классе определены три свойства (`SideA`, `SideB`, `SideC`), которые соответствуют сторонам треугольника. Метод `CalculateArea` вычисляет площадь треугольника, используя формулу Герона. Кроме того, метод `IsRightAngled` проверяет, является ли треугольник прямоугольным.

### Класс ShapeAreaCalculator<T>

Класс `ShapeAreaCalculator<T>` представляет собой реализацию интерфейса `IShapeAreaCalculator<T>`. В конструкторе принимаются объекты `ILogger` для логирования и `IDatabase` для работы с Redis. Метод `CalculateAreaAsync` сначала проверяет наличие кэша для фигуры, используя Redis, и, если данные найдены, возвращает их. В противном случае, метод вычисляет площадь, сохраняет результат в кэше и затем возвращает его.

### SQL
```sql
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100)
);

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY,
    CategoryName NVARCHAR(100)
);

CREATE TABLE ProductCategories (
    ProductID INT,
    CategoryID INT,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

SELECT 
    p.ProductName,
    c.CategoryName
FROM 
    Products p
LEFT JOIN 
    ProductCategories pc ON p.ProductID = pc.ProductID
LEFT JOIN 
    Categories c ON pc.CategoryID = c.CategoryID;




