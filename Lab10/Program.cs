using Lab10.Data;
using Lab10.Models;

namespace Lab10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NorthContext())
            {
                string sortOrderPreference = GetUserSortOrderPreference();

                var listOfCustomers = context.Customers
                    .Select(l => new
                    {
                        l.CustomerId,
                        l.CompanyName,
                        l.Country,
                        l.Region,
                        l.Phone,
                        OrderCount = l.Orders.Count(),

                        l.ContactName,
                        l.ContactTitle,
                        l.Address,
                        l.City,
                        l.PostalCode,
                        l.Fax

                    });

                if (sortOrderPreference == "S")
                {
                    listOfCustomers = listOfCustomers.OrderBy(l => l.CompanyName);
                }else
                {
                    listOfCustomers = listOfCustomers.OrderByDescending(l => l.CompanyName);
                }

                var customerList = listOfCustomers.ToList();

                int listIndex = 1;
                foreach (var customer in customerList)
                {
                    Console.WriteLine($"{listIndex}: Kundnamn: {customer.CompanyName}, Land: {customer.Country}, " +
                        $"Region: {customer.Region}, Telenr: {customer.Phone}, Total antal ordrar: {customer.OrderCount}.");
                    listIndex++;
                }

                Console.WriteLine("\nTryck in numret på den kund du vill ha mer information om, samt orderinformation.");
                Console.Write("Input: ");

                int selectedIndex;
                while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > customerList.Count)
                {
                    Console.WriteLine("Ogiltigt val. Var god och skriv ett nummer som är listad för kunden.");
                }

                Console.Clear();
                var selectedCustomer = customerList[selectedIndex - 1];
                Console.WriteLine($"\nDu har valt: {selectedCustomer.CompanyName}\n");
                Console.WriteLine($"Tryck 'valfri tangent' för komplett information från databasen.\n");
                Console.ReadKey();
                Console.WriteLine("----------------         Kunddata         ------------------\n");
                Console.WriteLine($"Företagsnamn: \t{selectedCustomer.CompanyName}");
                Console.WriteLine($"Kontakt: \t{selectedCustomer.ContactName}");
                Console.WriteLine($"Kontakt Titel: \t{selectedCustomer.ContactTitle}");
                Console.WriteLine($"Adress: \t{selectedCustomer.Address}");
                Console.WriteLine($"Stad: \t\t{selectedCustomer.City}");
                Console.WriteLine($"Region: \t{selectedCustomer.Region}");
                Console.WriteLine($"Postkod: \t{selectedCustomer.PostalCode}");
                Console.WriteLine($"Land: \t\t{selectedCustomer.Country}");
                Console.WriteLine($"Telefonnummer: \t{selectedCustomer.Phone}");
                Console.WriteLine($"Faxnummer: \t{selectedCustomer.Fax}");
                Console.WriteLine($"\n-------- All kundinformation har blivit presenterad --------");

                var ordersList = context.Orders
                    .Where(o => o.CustomerId == selectedCustomer.CustomerId)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.ShipAddress,
                        o.ShipCity,
                        o.ShipCountry,
                    });

                var ordersPresented = ordersList.ToList();
                if (ordersPresented.Any())
                {
                    Console.WriteLine("\nOrdrar med information kommer visas.(Tryck valfri tangent).\n");
                    Console.ReadKey();
                    Console.WriteLine("\n-----Ordrar för vald kund------");
                    foreach (var order in ordersPresented)
                    {
                        Console.WriteLine("\n-----------------------------------");
                        Console.WriteLine($"Order ID: \t{order.OrderId}");
                        Console.WriteLine($"Order Adress: \t{order.ShipAddress}");
                        Console.WriteLine($"Order Stad: \t{order.ShipCity}");
                        Console.WriteLine($"Order Land: \t{order.ShipCountry}");
                        Console.WriteLine("-----------------------------------");
                    }
                }else
                {
                    Console.WriteLine("Den här kunden har inga ordrar för nuvarande.");
                }

                Console.ReadKey();
                Console.Clear();

                AddNewCustomer();

                Console.WriteLine("\nVar god fyll i följande information för att skapa ny användare.\n");
                Console.Write("Företagsnamn: ");
                string newCompanyName = Console.ReadLine();
                Console.Write("Kontaktnamn: ");
                string newContactName = Console.ReadLine();
                Console.Write("Kontakttitel: ");
                string newContactTitle = Console.ReadLine();
                Console.Write("Adress: ");
                string newAdress = Console.ReadLine();
                Console.Write("Stad: ");
                string newCity = Console.ReadLine();
                Console.Write("Region: ");
                string newRegion = Console.ReadLine();
                Console.Write("Postkod: ");
                string newPostalCode = Console.ReadLine();
                Console.Write("Land: ");
                string newCountry = Console.ReadLine();
                Console.Write("Telenr: ");
                string newPhone = Console.ReadLine();
                Console.Write("Fax: ");
                string newFax = Console.ReadLine();

                Customer customer1 = new Customer()
                {
                    CustomerId = GenerateCustomerId(),
                    CompanyName = newCompanyName,
                    ContactName = newContactName,
                    ContactTitle = newContactTitle,
                    Address = newAdress,
                    City = newCity,
                    Region = newRegion,
                    PostalCode = newPostalCode,
                    Country = newCountry,
                    Phone = newPhone,
                    Fax = newFax
                };

                Console.WriteLine("\nSystem: Genererad ID kontrolleras om det redan finns.");
                Console.ReadKey();
                bool checkIfIdExists = context.Customers.Any(c => c.CustomerId == customer1.CustomerId);
                while (checkIfIdExists)
                {
                    Console.WriteLine("System: Den genererade ID'n fanns redan.\n\tNytt ID Genereras.");
                    Console.ReadKey();
                    customer1.CustomerId = GenerateCustomerId();
                    checkIfIdExists = context.Customers.Any(c => c.CustomerId == customer1.CustomerId);
                }

                context.Customers.Add(customer1);
                context.SaveChanges();

                Console.WriteLine("\nSystem: Lyckad skapning av ny kund.");
                Console.WriteLine($"\nDin databas har blivit uppdaterad med en ny kund:\n\tNy KundID: {customer1.CustomerId}");

            }
        }

        public static string GenerateCustomerId()
        {
            var charsAllowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var randomScrambler = new Random();
            var generatedId = new char[5];

            for (int maxAmountOfChars = 0; maxAmountOfChars < generatedId.Length; maxAmountOfChars++)
            {
                generatedId[maxAmountOfChars] = charsAllowed[randomScrambler.Next(charsAllowed.Length)];
            }

            return new String(generatedId);
        }

        static string AddNewCustomer()
        {
            Console.WriteLine("Nu kommer du få tillfälle att lägga till en ny kund.");
            Console.ReadKey();

            Console.WriteLine("Skriv (N) för att skapa ny användare.");
            Console.Write("Input: ");
            string userInputForNewCustomer = Console.ReadLine().ToUpper();

            while (userInputForNewCustomer != "N")
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
                Console.Write("Input: ");
                userInputForNewCustomer = Console.ReadLine().ToUpper();
            }
            return userInputForNewCustomer;
        }

        static string GetUserSortOrderPreference()
        {
            Console.WriteLine("\nNorthwind Databas AccessPoint för kundlista och information:");
            Console.WriteLine("\nVill du sortera kundlistan i stigande (S) eller fallande ordning (F)? (Ej capkänsligt) (Listan är sorterad på kundnamn).\n");
            Console.Write("Input: ");
            string sortOrder = Console.ReadLine().ToUpper();

            while (sortOrder != "S" && sortOrder != "F")
            {
                Console.WriteLine("\nOgiltigt val:\n var god skriv S eller F.\n");
                Console.Write("Input: ");
                sortOrder = Console.ReadLine().ToUpper();
            } return sortOrder;
        }
    }
}