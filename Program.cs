using System;
using NLog.Web;
using System.IO;
using System.Linq;
using NorthWind_Console.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NorthWind_Console
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
                    Console.WriteLine("1) Add Product");
                    Console.WriteLine("2) Edit Product");
                    Console.WriteLine("3) Display all Products");
                    Console.WriteLine("4) Display a Specific Product");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();

                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    { }
                    else if (choice == "2")
                    { }
                    else if (choice == "3")
                    { }
                    else if (choice == "4")
                    { }
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