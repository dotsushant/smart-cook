using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using CrazyAboutPi.CoreLib;
using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.Testing
{
    class Program
    {
        static Random m_Random = new Random((int)DateTime.Now.Ticks);

        static void Main(string[] args)
        {
            //string fileName = @"C:\Users\ksushant\BitBucket\WhatsCook\Source\Testing\Ingredients.txt";

            //foreach (var fileLine in File.ReadAllLines(fileName))
            //{
            //    Ingredient.Add(fileLine);
            //}


            //foreach (var ingredient in Ingredient.GetAll())
            //{
            //    Ingredient.Update(ingredient.ID, false);
            //}

            CreateTestData();
        }

        static void Print(object message)
        {
            Console.WriteLine(message);
        }

        static void CreateTestData()
        {
        }
    }
}