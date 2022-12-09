using System;
using NLog.Web;
using System.IO;
using System.Linq;
using FinalProject.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Display Category and related products");
                    Console.WriteLine("3) Display all Categories and their related products");
                    Console.WriteLine("4) Display Products");
                    Console.WriteLine("5) Display Specific Product");
                    Console.WriteLine("6) Add Category");
                    Console.WriteLine("7) Add Product");
                    Console.WriteLine("8) Edit Category");
                    Console.WriteLine("9) Edit Product");
                    Console.WriteLine("10) Delete Category Record");
                    Console.WriteLine("11) Delete Product Record");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();

                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new Northwind22RCJContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (choice == "2")
                    {
                        var db = new Northwind22RCJContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Product p in category.Products)
                        {
                            if (p.Discontinued == true)
                            {
                                
                            } else if (p.Discontinued == false)
                            {
                            Console.WriteLine(p.ProductName);
                            }
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new Northwind22RCJContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Product p in item.Products)
                            {
                                if(p.Discontinued == false)
                                {
                                Console.WriteLine($"\t{p.ProductName}");
                                }
 
                            }
                        }
                    }
                    else if (choice == "4")
                    {   

                        var db = new Northwind22RCJContext();
                        Console.WriteLine("1) Display All Products");
                        Console.WriteLine("2) Display Discontinued Products");
                        Console.WriteLine("3) Display Active Products");
                        string option = Console.ReadLine();

                        var query = db.Products.OrderBy(p => p.ProductName);
                        foreach( var item in query)
                        {
                            if(option == "1")
                            {
                                Console.WriteLine($"\t{item.ProductName}");
                            }
                            else if(option == "2")
                            {
                                if(item.Discontinued == true)
                                {
                                    Console.WriteLine($"\t{item.ProductName}");
                                }
                            }
                            else if(option == "3")
                            {
                                if(item.Discontinued == false)
                                {
                                    Console.WriteLine($"\t{item.ProductName}");
                                }
                            }
                        }
                    }
                    else if (choice == "5")
                    {}
                    else if (choice == "6")
 {
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        var catName = category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        var catDescription = category.Description = Console.ReadLine();
                        

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new Northwind22RCJContext();
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddCategory(category);
                                logger.Info($"Category: {category.CategoryName} Created");
                                
                            }                        
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "7")
                    {
                        Product product = new Product();
                        Console.WriteLine("Enter Product Name: ");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter Product Supplier ID: ");
                        product.SupplierId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter Product Category ID: ");
                        product.CategoryId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter Quantity Per Unit: ");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter Product Unit Price: ");
                        product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Units In Stock: ");
                        product.UnitsInStock = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units On Order: ");
                        product.UnitsOnOrder = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Recorder Level: ");
                        product.ReorderLevel = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter If Product is Discontinued: ");
                        Console.WriteLine("Enter Truth Or False: ");
                        string dis = Console.ReadLine().ToLower();
                        if(dis == "True")
                        {
                            product.Discontinued = true;
                        }
                        else if(dis == "false")
                        {
                            product.Discontinued = false;
                        }

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if(isValid)
                        {
                            var db = new Northwind22RCJContext();
                            if(db.Products.Any(p => p.ProductName == product.ProductName))
                            {
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] {"ProductName"}));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddProduct(product);
                                logger.Info($"Product: {product.ProductName} created"); 
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "8")
                    {
                        Console.WriteLine("Choose the Category you would like to edit");
                        var db = new Northwind22RCJContext();
                        var category = GetCategory(db);
                        if(category != null)
                        {
                            Category  updatedCategory = InputCategory(db);
                            if(updatedCategory != null)
                            {
                                updatedCategory.CategoryId = category.CategoryId;
                                db.EditCategory(updatedCategory);
                                logger.Info($"Category: {category.CategoryName} updated");
                            }
                        }
                    }
                    else if (choice == "9")
                    {
                        Console.WriteLine("Choose the Product you would like to edit");
                        var db = new Northwind22RCJContext();
                        var product = GetProduct(db);
                        if(product != null)
                        {
                            Product updatedProduct = InputProduct(db);
                            if(updatedProduct != null)
                            {
                                updatedProduct.ProductId = product.ProductId;
                                db.EditProduct(updatedProduct);
                                logger.Info($"Product {product.ProductName} updated");
                            }
                        }
                    }
                    else if (choice == "10")
                    {
                        Console.WriteLine("Choose the Category you would like to delete");
                        var db = new Northwind22RCJContext();
                        var category = GetCategory(db);
                        if (category != null)
                        {
                            db.DeleteCategory(category);
                            logger.Info($"Category {category.CategoryName} deleted");
                        }
                        

                        
                    }
                    else if (choice == "11")
                    {
                        Console.WriteLine("Choose the Product you would like to delete");
                        var db = new Northwind22RCJContext();
                        var product = GetProduct(db);
                        if(product != null)
                        {
                            db.DeleteProduct(product);
                            logger.Info($"Product {product.ProductName} deleted");
                        }                        
                    }
                    logger.Info($"Option {choice} finished");
                    Console.WriteLine();                    

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }


        public static Category GetCategory(Northwind22RCJContext db)
        {
            var categories = db.Categories.OrderBy(p => p.CategoryId);
            foreach(Category c in categories)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{c.CategoryId}: {c.CategoryName}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            if(int.TryParse(Console.ReadLine(), out int CategoryId))
            {
                Category category = db.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
                if(category != null)
                {
                    return category;
                }
            }
            logger.Error("Invalid CategoryId");
            return null;
            
        }

        public static Product GetProduct(Northwind22RCJContext db)
        {
            var products = db.Products.OrderBy(p => p.ProductId);
            foreach(Product p in products)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{p.ProductId}: {p.ProductName}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            if(int.TryParse(Console.ReadLine(), out int ProductId))
            {
                Product product = db.Products.FirstOrDefault(p => p.ProductId == ProductId);
                if(product != null)
                {
                    return product;
                }
            }
            logger.Error("Invalid ProductId");
            return null;
        }

        public static Category InputCategory(Northwind22RCJContext db)
        {
            Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        var catName = category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        var catDescription = category.Description = Console.ReadLine();
                        

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if(isValid)
                        {
                            return category;
                        }
                        else
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                        return null;
                    }


        public static Product InputProduct(Northwind22RCJContext db)
        { Product product = new Product();
                        Console.WriteLine("Enter Product Name: ");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter Product Supplier ID: ");
                        product.SupplierId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter Product Category ID: ");
                        product.CategoryId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter Quantity Per Unit: ");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter Product Unit Price: ");
                        product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Units In Stock: ");
                        product.UnitsInStock = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units On Order: ");
                        product.UnitsOnOrder = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Recorder Level: ");
                        product.ReorderLevel = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter If Product is Discontinued: ");
                        Console.WriteLine("Enter Truth Or False: ");
                        string dis = Console.ReadLine().ToLower();
                        if(dis == "True")
                        {
                            product.Discontinued = true;
                        }
                        else if(dis == "false")
                        {
                            product.Discontinued = false;
                        }

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if(isValid)
                        {
                            return product;
                        }
                        else
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                        return null;
                    }
    }
}