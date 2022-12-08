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
                                if(p.Discontinued == true)
                                {

                                } else if(p.Discontinued == false)
                                {
                                Console.WriteLine($"\t{p.ProductName}");
                                }
                                
                            }
                        }
                    }
                    else if (choice == "4")
                    {}
                    else if (choice == "5")
                    {}
                    else if (choice == "6")
                    {}
                    else if (choice == "7")
                    {}
                    else if (choice == "8")
                    {}
                    else if (choice == "9")
                    {}
                    else if (choice == "10")
                    {}
                    else if (choice == "11")
                    {}
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
    }
}