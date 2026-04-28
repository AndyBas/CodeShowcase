using System.Runtime.CompilerServices;

class Program
{
    #region APPLICATION ENTRY POINT
    static void Main(string[] args)
    {
        using (var unitOfWork = new UnitOfWork(new AppDbContext()))
        {
            try
            {
                bool exit = false;
                while (!exit)
                {
                    DisplayMenu();

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            AddToDatabase(unitOfWork);
                            break;

                        case ConsoleKey.D2:
                            GetFromDatabase(unitOfWork);
                            break;

                        case ConsoleKey.D3:
                            UpdateToDatabase(unitOfWork);
                            break;
                        case ConsoleKey.D4:
                            DeleteFromDatabase(unitOfWork);
                            break;

                        case ConsoleKey.D5:
                            GetRelatives(unitOfWork);
                            break;

                        case ConsoleKey.D6:
                            GetOrderedPeople(unitOfWork);
                            break;

                        case ConsoleKey.D7:
                            GetProductsByCategory(unitOfWork);
                            break;

                        case ConsoleKey.D8:
                            GetProductsByPriceRange(unitOfWork);
                            break;

                        case ConsoleKey.D9:
                            exit = true;
                            break;

                    }
                }
            }
            catch(Exception e)
            {
                unitOfWork.Rollback();
                Console.WriteLine("An issue has been encountered with the bdd: " + e.Message);
            }
        
        }
    }
    #endregion
    
    #region MENU
    static void DisplayMenu()
    {
        Console.WriteLine("Welcome to the CRUD training, What do you want to do?");
        Console.WriteLine("1. Add some data to the database");
        Console.WriteLine("2. Get some data from the database");
        Console.WriteLine("3. Update some data in the database");
        Console.WriteLine("4. Delete some data from the database");
        Console.WriteLine("5. Get relatives of a person");
        Console.WriteLine("6. Order people by age");
        Console.WriteLine("7. Get products by category");
        Console.WriteLine("8. Get products by price range");
        Console.WriteLine("9. Exit");
    }
    #endregion

    #region QUERY OPERATIONS
    private static void GetProductsByPriceRange(UnitOfWork unitOfWork)
    {
        decimal minPrice, maxPrice;
        Console.WriteLine("What is the minimum price ?");
        string? minPriceStr = Console.ReadLine();
        bool hasParsedMinPrice = decimal.TryParse(minPriceStr, out minPrice);
        Console.WriteLine("What is the maximum price ?");
        string? maxPriceStr = Console.ReadLine();
        bool hasParsedMaxPrice = decimal.TryParse(maxPriceStr, out maxPrice);
        if(!hasParsedMinPrice || !hasParsedMaxPrice)
        {
            Console.WriteLine("One of the price you entered is not a valid number");
            return;
        }
        IEnumerable<Product> products = unitOfWork.ProductRepository.GetProductsByPriceRange(minPrice, maxPrice);
        if(!products.Any())
        {
            Console.WriteLine($"No product found between {minPrice}€ and {maxPrice}€");
            return;
        }
        Console.WriteLine($"Products between {minPrice}€ and {maxPrice}€:");
        foreach(Product product in products)
        {
            Console.WriteLine($"- {product.Name} which costs {product.Price}€ and is in the category {product.Category}");
        }
    }

    private static void GetProductsByCategory(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What is the category you want to get ?");
        string? category = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("The category you entered is not valid");
            return;
        }
        IEnumerable<Product> products = unitOfWork.ProductRepository.GetProductsByCategory(category);
        if(!products.Any())
        {
            Console.WriteLine($"No product found in the category {category}");
            return;
        }
        Console.WriteLine($"Products in the category {category}:");
        foreach(Product product in products)
        {
            Console.WriteLine($"- {product.Name} which costs {product.Price}€ and is in the category {product.Category}");
        }
    }

    private static void GetRelatives(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What is the id of the person you want to get the relatives from ?");
        string? id = Console.ReadLine();
        bool hasParsedId = int.TryParse(id, out int idInt);
        if(!hasParsedId)
        {
            Console.WriteLine("The id you entered is not a valid number");
            return;
        }
        Person? person = unitOfWork.PersonRepository.Get(idInt);
        if(person == null)
        {
            Console.WriteLine("The person you entered is not valid");
            return;
        }
        IEnumerable<Person> relatives = unitOfWork.PersonRepository.GetRelatives(person);

        if(!relatives.Any())
        {
            Console.WriteLine($"No relative found for {person.Name} {person.Surname}");
            return;
        }
        Console.WriteLine($"Relatives of {person.Name} {person.Surname}:");
        foreach(Person relative in relatives)
        {
            Console.WriteLine($"- {relative.Name} {relative.Surname} who is {relative.Age} years old and has the mail {relative.Email}");
        }

    }

    private static void GetOrderedPeople(UnitOfWork unitOfWork)
    {
        IEnumerable<Person> people = unitOfWork.PersonRepository.OrderByAge();
        if(!people.Any())
        {
            Console.WriteLine("No person found in the database");
            return;
        }
        Console.WriteLine("People ordered by age:");
        foreach(Person person in people)
        {
            Console.WriteLine($"- {person.Name} {person.Surname} who is {person.Age} years old and has the mail {person.Email}");
        }
    }
    #endregion

    #region CREATE OPERATIONS
    static void AddToDatabase(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What do you want to add ? (1: Person / 2: Product)");

        ConsoleKeyInfo consoleKey = Console.ReadKey();

        switch (consoleKey.Key)
        {
            case ConsoleKey.D1:
                AddPerson(unitOfWork);
                break;

            case ConsoleKey.D2:
                AddProduct(unitOfWork);
                break;
        }
    }

    private static void AddProduct(UnitOfWork unitOfWork)
    {
        Console.Write("What is the <b>name</b> of the product? ");
        string? name = Console.ReadLine();

        Console.Write("\nWhat is the <b>category</b> of the product? ");
        string? category = Console.ReadLine();

        Console.Write("\nWhat is the <b>price</b> of the product? ");
        string? price = Console.ReadLine();
        bool hasParsedPrice = decimal.TryParse(price, out decimal priceDecimal);
        if(string.IsNullOrWhiteSpace(name)
        || string.IsNullOrWhiteSpace(category)
        || !hasParsedPrice)
        {
            Console.WriteLine("One of the value hasn't been filled");
            return;
        }

        Product product = new Product(name, category, priceDecimal);
        unitOfWork.ProductRepository.Add(product);
        unitOfWork.Commit();
    }

    private static void AddPerson(UnitOfWork unitOfWork)
    {
        Console.Write("What is the <b>name</b> of the person? ");
        string? name = Console.ReadLine();
        
        Console.Write("\nWhat is the <b>surname</b> of the person? ");
        string? surname = Console.ReadLine();

        Console.Write("\nWhat is the <b>mail</b> of the person? ");
        string? mail = Console.ReadLine();

        Console.Write("\nWhat is the <b>age</b> of the person? ");
        string? age = Console.ReadLine();
        bool hasParsedAge = int.TryParse(age, out int ageInt);
        if(string.IsNullOrWhiteSpace(name)
        || string.IsNullOrWhiteSpace(surname)
        || string.IsNullOrWhiteSpace(mail)
        || !hasParsedAge)
        {
            Console.WriteLine("One of the value hasn't been filled");
            return;
        }

        Person person = new Person(name, surname, mail, ageInt);
        unitOfWork.PersonRepository.Add(person);
        unitOfWork.Commit();
    }
    #endregion

    #region READ OPERATIONS
    static void GetFromDatabase(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What do you want to get ? (1: Person / 2: Product)");
        ConsoleKeyInfo consoleKey = Console.ReadKey();
        switch (consoleKey.Key)
        {
            case ConsoleKey.D1:
                GetPerson(unitOfWork);
                break;

            case ConsoleKey.D2:
                GetProduct(unitOfWork);
                break;
        }
    }

    private static Product? GetProduct(UnitOfWork unitOfWork, int idInt = -1)
    {
        if(idInt < 0)
        {
            Console.WriteLine("What is the id of the product you want to get ?");
            string? id = Console.ReadLine();

            bool hasParsedId = int.TryParse(id, out idInt);
            if(!hasParsedId)
            {
                Console.WriteLine("The id you entered is not a valid number");
                return null;
            }
        }
        Product? product = unitOfWork.ProductRepository.Get(idInt);
        if(product == null)        {
            Console.WriteLine("No product found with the id " + idInt);
            return null;
        }
        Console.WriteLine($"The product with the id {idInt} is {product.Name} which costs {product.Price}€ and is in the category {product.Category}");
        return product;
    }

    private static Person? GetPerson(UnitOfWork unitOfWork, int idInt = -1)
    {
        if(idInt < 0)
        {
            Console.WriteLine("What is the id of the person you want to get ?");
            string? id = Console.ReadLine();
            bool hasParsedId = int.TryParse(id, out idInt);
            if(!hasParsedId)
            {
                Console.WriteLine("The id you entered is not a valid number");
                return null;
            }
        }

        Person? person = unitOfWork.PersonRepository.Get(idInt);
        if(person == null)        {
            Console.WriteLine("No person found with the id " + idInt);
            return null;
        }
        Console.WriteLine($"The person with the id {idInt} is {person.Name} {person.Surname} who is {person.Age} years old and has the mail {person.Email}");
        return person;
    }
    #endregion

    #region UPDATE OPERATIONS
    static void UpdateToDatabase(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What do you want to update ? (1: Person / 2: Product)");
        ConsoleKeyInfo consoleKey = Console.ReadKey();
        switch (consoleKey.Key)
        {
            case ConsoleKey.D1:
                UpdatePerson(unitOfWork);
                break;

            case ConsoleKey.D2:
                UpdateProduct(unitOfWork);
                break;
        }
    }

    private static void UpdateProduct(UnitOfWork unitOfWork)
    {
        Console.WriteLine("\nWhat is the id of the product you want to update ?");
        string? id = Console.ReadLine();
        bool hasParsedId = int.TryParse(id, out int idInt);
        if(!hasParsedId)
        {
            Console.WriteLine("The id you entered is not a valid number");
            return;
        }
        Product? productToUpdate = GetProduct(unitOfWork, idInt);
        if(productToUpdate == null)        
        {
            return;
        }

        Console.WriteLine("\nWhat is the new name of the product ? (leave empty to keep the same)");
        string? name = Console.ReadLine();
        Console.WriteLine("\nWhat is the new category of the product ? (leave empty to keep the same)");
        string? category = Console.ReadLine();
        Console.WriteLine("\nWhat is the new price of the product ? (leave empty to keep the same)");
        string? price = Console.ReadLine();
        bool hasParsedPrice = decimal.TryParse(price, out decimal priceDecimal);

        if(!string.IsNullOrWhiteSpace(name))
        {
            productToUpdate.Name = name;
        }
        if(!string.IsNullOrWhiteSpace(category))
        {
            productToUpdate.Category = category;
        }
        if(!string.IsNullOrWhiteSpace(price) && hasParsedPrice)
        {
            productToUpdate.Price = priceDecimal;
        }

        unitOfWork.Commit();
        Console.WriteLine("Product updated successfully.");
    }



    private static void UpdatePerson(UnitOfWork unitOfWork)
    {
        Console.WriteLine("\nWhat is the id of the person you want to update ?");
        string? id = Console.ReadLine();
        bool hasParsedId = int.TryParse(id, out int idInt);
        if(!hasParsedId)
        {
            Console.WriteLine("The id you entered is not a valid number");
            return;
        }
        Person? personToUpdate = GetPerson(unitOfWork, idInt);
        if(personToUpdate == null)        
        {
            return;
        }

        Console.WriteLine("\nWhat is the new name of the person ? (leave empty to keep the same)");
        string? name = Console.ReadLine();
        Console.WriteLine("\nWhat is the new surname of the person ? (leave empty to keep the same)");
        string? surname = Console.ReadLine();
        Console.WriteLine("\nWhat is the new mail of the person ? (leave empty to keep the same)");
        string? mail = Console.ReadLine();
        Console.WriteLine("\nWhat is the new age of the person ? (leave empty to keep the same)");
        string? age = Console.ReadLine();
        bool hasParsedAge = int.TryParse(age, out int ageInt);

        if(!string.IsNullOrWhiteSpace(name))
        {
            personToUpdate.Name = name;
        }
        if(!string.IsNullOrWhiteSpace(surname))
        {
            personToUpdate.Surname = surname;
        }
        if(!string.IsNullOrWhiteSpace(mail))
        {
            personToUpdate.Email = mail;
        }
        if(!string.IsNullOrWhiteSpace(age) && hasParsedAge)
        {
            personToUpdate.Age = ageInt;
        }
        unitOfWork.Commit();
        Console.WriteLine("Person updated successfully.");
    }
    #endregion

    #region DELETE OPERATIONS
    static void DeleteFromDatabase(UnitOfWork unitOfWork)
    {
        Console.WriteLine("What do you want to delete ? (1: Person / 2: Product)");
        ConsoleKeyInfo consoleKey = Console.ReadKey();
        switch (consoleKey.Key)
        {
            case ConsoleKey.D1:
                DeletePerson(unitOfWork);
                break;

            case ConsoleKey.D2:
                DeleteProduct(unitOfWork);
                break;
        }
    }

    private static void DeleteProduct(UnitOfWork unitOfWork)
    {
        Console.WriteLine("\nWhat is the id of the product you want to delete ?");
        string? id = Console.ReadLine();
        bool hasParsedId = int.TryParse(id, out int idInt);
        if(!hasParsedId)
        {
            Console.WriteLine("The id you entered is not a valid number");
            return;
        }
        Product? productToDelete = GetProduct(unitOfWork, idInt);
        if(productToDelete == null)        
        {
            return;
        }

        unitOfWork.ProductRepository.Remove(productToDelete);
        unitOfWork.Commit();
        Console.WriteLine("Product deleted successfully.");
    }

    private static void DeletePerson(UnitOfWork unitOfWork)
    {
        Console.WriteLine("\nWhat is the id of the person you want to delete ?");
        string? id = Console.ReadLine();
        bool hasParsedId = int.TryParse(id, out int idInt);
        if(!hasParsedId)
        {
            Console.WriteLine("The id you entered is not a valid number");
            return;
        }
        Person? personToDelete = GetPerson(unitOfWork, idInt);
        if(personToDelete == null)
        {
            return;
        }

        unitOfWork.PersonRepository.Remove(personToDelete);
        unitOfWork.Commit();
        Console.WriteLine("Person deleted successfully.");
    }
    #endregion
}
