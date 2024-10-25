

using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    };

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ShoppingCart
    {
        static void printwithDelay(string text, int delay = 20)
        {

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
        static void progress()
        {
            char hash = '#';
            Console.Write("Progress[");
            int delay = 30;
            for (int i = 0; i < 25; i++)
            {
                if (i == 10)
                {
                    delay = 45;
                }
                if (i == 17)
                {
                    delay = 15;
                }
                Console.Write(hash);
                Thread.Sleep(delay);
            }
            Console.Write("]\nProgress Completed! \n");
            Thread.Sleep(1200);
            Console.WriteLine();

        }




        private List<Product> items = new List<Product>();
        public User currentUser;

        private List<User> users = new List<User>();

        public bool Register(string username, string password)
        {

            string filePath = @"../../../users.txt";
            string userRecord = $"{username}:{password}";
            foreach (var line in File.ReadLines(filePath))
            {
                string[] parts = line.Split(':');
                if (parts[0] == username)
                {
                    printwithDelay("Username Already! exists \nTry using some other ...");
                    printwithDelay("Set username: ");
                    username = Console.ReadLine();
                    printwithDelay("Set Password: ");
                    password = Console.ReadLine();
                    Register(username, password);
                    return true;
                }
            }
            progress();

            File.AppendAllLines(filePath, new[] { userRecord });
            Console.Clear();
            printwithDelay("User registered successfully.");
            return true;





        }

        public bool Login(string username, string password)
        {
            int tries = 0;
            string filePath = @"../../../users.txt";
            string check_up = $"{username}:{password}";
            foreach (var line in File.ReadLines(filePath))
            {
                tries++;
                if (check_up == line)
                {
                    progress();
                    currentUser = new User
                    {
                        Username = username,
                        Password = password
                    };
                    Console.Clear();
                    printwithDelay("===> WELCOME! BACK " + username + "\nLogin Successful\n");
                    return true;

                }
                else
                {
                    printwithDelay("Login failed! \nInvalid username or password\n");
                    printwithDelay("Enter username: ");
                    username = Console.ReadLine();
                    printwithDelay("Enter Password: ");
                    password = Console.ReadLine();
                    Login(username, password);
                    if (tries >= 3)
                    {
                        printwithDelay("Try Register \n");
                        username = Console.ReadLine();
                        printwithDelay("Enter Password: ");
                        password = Console.ReadLine();
                        Register(username, password);
                        progress();
                        Console.Clear();
                        printwithDelay("Enter username: ");
                        username = Console.ReadLine();
                        printwithDelay("Enter Password: ");
                        password = Console.ReadLine();
                        Login(username, password);




                    }
                    return false;


                }
            }
            return true;



        }


        public void Logout()
        {
            currentUser = null;
            progress();
            printwithDelay("You have logged out.");
        }

        public void AddProduct(Product product)
        {
            if (currentUser.Username == null)
            {
                printwithDelay("Please log in to add products to the cart.");
                return;
            }
            items.Add(product);
            printwithDelay($"ProductID: {product.Id} \n{product.Quantity} {product.Name}(s) added to the cart.");
        }

        public void RemoveProduct(int productId)
        {
            if (currentUser == null)
            {
                Console.WriteLine("Please log in to remove products from the cart.");
                return;
            }

            var product = items.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                items.Remove(product);
                Console.WriteLine($"{product.Name} removed from the cart.");
            }
            else
            {
                Console.WriteLine($"Product ID {productId} not found in the cart.");
            }
        }

        public void ViewCart()
        {
            if (currentUser == null)
            {
                printwithDelay("Please log in to view the cart.");
                Thread.Sleep(4000);
                return;
            }

            string filePath = $"../../../{currentUser.Username}_cart.txt";

            if (!File.Exists(filePath))
            {
                printwithDelay("Your cart is empty or does not exist.");
                Thread.Sleep(4000);
                return;
            }

            printwithDelay($"Shopping Cart for {currentUser.Username}:");

            string[] cartItems = File.ReadAllLines(filePath);
            if (cartItems.Length == 0)
            {
                printwithDelay("Your cart is empty.");
                Thread.Sleep(4000);
                return;
            }

            foreach (var line in cartItems)
            {
                printwithDelay(line);
            }
            Thread.Sleep(5000);
        }


        public decimal CalculateTotal()
        {
            if (currentUser == null)
            {
                printwithDelay("Please log in to calculate the total.");
                return 0;
            }

            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Price;

            }



            if (currentUser == null)
            {
                Console.WriteLine("Please log in to make a purchase.");
                return 0;
            }

            if (items.Count == 0)
            {
                Console.WriteLine("Your cart is empty. Add products to your cart before purchasing.");
                return 0;
            }
            Console.WriteLine("Price Calculated: " + total);
            string filePath = $"../../../{currentUser.Username}_cart.txt";
            StreamWriter sw = new StreamWriter(filePath, true);

            sw.WriteLine($"Purchase for {currentUser.Username} on {DateTime.Now}");
            foreach (var item in items)
            {
                sw.WriteLine($"{item.Id}: {item.Quantity} x {item.Name} - {item.Price}");

            }
            sw.WriteLine($"Total: {total}:Rupees");
            sw.WriteLine("------------------------------------------------");

            sw.Close();
            Console.WriteLine("Purchase completed successfully. Thank you for your order!");
            items.Clear();
            progress();




            // Console.WriteLine("Price Calculated: " + total);
            return total;
        }

        public void Purchase()
        {
            CalculateTotal();
            //if (currentUser == null)
            //{
            //    Console.WriteLine("Please log in to make a purchase.");
            //    return;
            //}

            //if (items.Count == 0)
            //{
            //    Console.WriteLine("Your cart is empty. Add products to your cart before purchasing.");
            //    return;
            //}
            //Console.WriteLine("Price Calculated: "+CalculateTotal());
            //string filePath = $"../../../{currentUser.Username}_cart.txt";
            //StreamWriter sw = new StreamWriter(filePath, true);

            //sw.WriteLine($"Purchase for {currentUser.Username} on {DateTime.Now}");
            //foreach (var item in items)
            //{
            //    sw.WriteLine($"{item.Id}: {item.Quantity} x {item.Name} - {item.Price * item.Quantity:C}");

            //}
            //sw.WriteLine($"Total: {CalculateTotal():Rupees}");
            //sw.WriteLine("------------------------------------------------");

            //sw.Close();
            //Console.WriteLine("Purchase completed successfully. Thank you for your order!");
            //items.Clear();
            //progress();
        }

        private List<Product> GetVegetableProducts()
        {
            return new List<Product>
    {
        new Product { Id = 1, Name = "Carrot", Price = 1.0m },
        new Product { Id = 2, Name = "Broccoli", Price = 1.5m },
        new Product { Id = 3, Name = "Lettuce", Price = 0.9m }
    };
        }

        private List<Product> GetFruitProducts()
        {
            return new List<Product>
    {
        new Product { Id = 4, Name = "Apple", Price = 1.2m },
        new Product { Id = 5, Name = "Orange", Price = 1.0m },
        new Product { Id = 6, Name = "Banana", Price = 0.8m }
    };
        }

        private List<Product> GetMeatProducts()
        {
            return new List<Product>
    {
        new Product { Id = 7, Name = "Chicken", Price = 5.0m },
        new Product { Id = 8, Name = "Beef", Price = 7.0m },
        new Product { Id = 9, Name = "Fish", Price = 6.0m }
    };
        }

        private List<Product> GetElectronicsProducts()
        {
            return new List<Product>
    {
        new Product { Id = 10, Name = "Laptop", Price = 999.99m },
        new Product { Id = 11, Name = "Smartphone", Price = 699.99m },
        new Product { Id = 12, Name = "Headphones", Price = 199.99m }
    };
        }








        public void ShowProductOptions()
        {
            while (true)
            {
                Console.WriteLine("Select product category to add to cart:");
                Console.WriteLine("1. Vegetables");
                Console.WriteLine("2. Fruits");
                Console.WriteLine("3. Meat");
                Console.WriteLine("4. Electronics");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddSelectedProduct("Vegetable", GetVegetableProducts());
                        break;
                    case "2":
                        AddSelectedProduct("Fruit", GetFruitProducts());
                        break;
                    case "3":
                        AddSelectedProduct("Meat", GetMeatProducts());
                        break;
                    case "4":
                        AddSelectedProduct("Electronics", GetElectronicsProducts());
                        break;
                    case "5":
                        Console.WriteLine("Exiting product selection...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }



        }

        private void AddSelectedProduct(string category, List<Product> products)
        {
            while (true)
            {
                Console.WriteLine($"\nAvailable {category}s:");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id}. {product.Name} - ${product.Price}");
                }

                Console.WriteLine($"Enter the product ID of the {category} to add to cart (or 0 to exit):");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int productId))
                {
                    if (productId == 0)
                    {
                        Console.WriteLine("Exiting product selection...");
                        return;
                    }

                    var selectedProduct = products.FirstOrDefault(p => p.Id == productId);
                    if (selectedProduct != null)
                    {
                        AddProduct(selectedProduct);
                    }
                    else
                    {
                        Console.WriteLine("Invalid product ID. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
    };
    public class Program
    {

        static void printwithDelay(string text, int delay = 20)
        {

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            Console.Clear();
            printwithDelay("\t\t\t|==================================================|");
            printwithDelay("\t\t\t|          WELCOME TO THE LOGIN SYSTEM             |");
            printwithDelay("\t\t\t|==================================================|");
            printwithDelay("\t\t\t|             Please choose an option              |");
            printwithDelay("\t\t\t|--------------------------------------------------|");
            printwithDelay("\t\t\t|   1. Register as New User                        |", 5);
            printwithDelay("\t\t\t|   2. Login                                       |", 5);
            printwithDelay("\t\t\t|   3. Exit                                        |", 5);
            printwithDelay("\t\t\t|__________________________________________________|", 5);
        }
        public static void after_login(ShoppingCart curren_cart)
        {

            Console.Clear();

            printwithDelay("\t\t\t|==================================================|");
            printwithDelay("\t\t\t|  WELCOME TO THE SHOPPING CART MANAGEMENT SYSTEM  |");
            printwithDelay("\t\t\t|==================================================|");
            printwithDelay("\t\t\t|             Please choose an option              |");
            printwithDelay("\t\t\t|--------------------------------------------------|");
            printwithDelay("\t\t\t|   1. Show Available Items                        |", 5);
            printwithDelay("\t\t\t|   2. Show Previous Cart                          |", 5);
            printwithDelay("\t\t\t|   3. Remove Product from Cart                    |", 5);
            printwithDelay("\t\t\t|   4. Purchase Current Cart                       |", 5);
            printwithDelay("\t\t\t|   5. Exit                                        |", 5);
            printwithDelay("\t\t\t|__________________________________________________|", 5);
            Console.WriteLine("\t\t\tEnter: ");
            string option = Console.ReadLine();
            if (option == null)
            {
                after_login(curren_cart);
            }
            else if (option == "1")
            {

                curren_cart.ShowProductOptions();
                after_login(curren_cart);

            }
            else if (option == "2")
            {
                curren_cart.ViewCart();
                after_login(curren_cart);



            }
            else if (option == "3")
            {
                Console.WriteLine("Enter Product ID: ");

                string id = Console.ReadLine();
                int p_id = Int32.Parse(id);
                curren_cart.RemoveProduct(p_id);
                after_login(curren_cart);


            }
            else if (option == "4")
            {

                curren_cart.Purchase();
                after_login(curren_cart);
            }
            else if (option == "5")
            {

                return;


            }
            else
            {
                after_login(curren_cart);
            }

        }
        public static void Config_menu(ShoppingCart cart)
        {
            Console.Clear();
            ShowMenu();
            Console.WriteLine("Enter: ");
            string choice = Console.ReadLine();


            if (choice == "1")
            {

                printwithDelay("Enter a username for registration:");
                string newUsername = Console.ReadLine();

                printwithDelay("Enter a password for registration:");
                string newPassword = Console.ReadLine();

                if (cart.Register(newUsername, newPassword))
                {
                    printwithDelay("Registration complete. Please log in.");
                }
                Console.Clear();
                printwithDelay("Enter your username to log in:");
                string username = Console.ReadLine();
                printwithDelay("Enter your password:");
                string password = Console.ReadLine();
                if (cart.Login(username, password))
                {

                    after_login(cart);
                    cart.Logout();
                    Config_menu(cart);



                }




            }
            if (choice == "2")
            {
                printwithDelay("Enter your username to log in: ");
                string username = Console.ReadLine();
                printwithDelay("Enter your password:");
                string password = Console.ReadLine();

                if (cart.Login(username, password))
                {

                    after_login(cart);


                    cart.Logout();
                    Config_menu(cart);
                }
            }
            else if (choice == "3")
            {
                printwithDelay("Thanks for using \nProgram Exiting ...\n");
                return;
            }


            else
            {
                Config_menu(cart);
            }

        }

        public static void Main(String[] args)
        {
            ShoppingCart cart = new ShoppingCart();
            Config_menu(cart);




        }
    }

}









