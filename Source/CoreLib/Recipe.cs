using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class Recipe
    {
        internal static DataAccess.DALTableAdapters.RecipesTableAdapter m_sTA = new DataAccess.DALTableAdapters.RecipesTableAdapter();

        public static Recipe Get(long id)
        {
            Recipe result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByID(id);

                if (resultTable.Rows.Count > 0)
                {
                    result = new Recipe(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public static IEnumerable<Recipe> GetAll()
        {
            lock (m_sTA)
            {
                foreach (DataAccess.DAL.RecipesRow row in m_sTA.GetAll())
                {
                    yield return new Recipe(row);
                }
            }
        }

        public Recipe()
        {
            // NOP
        }

        public Recipe(DataAccess.DAL.RecipesRow row)
        {
            ID = row.ID;
            Name = row.Name;
            UserID = row.UserID;
            Servings = row.Servings;
            CookingTime = row.CookingTime;
        }

        public long ID
        {
            get;
            set;
        }

        public long UserID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public long CookingTime
        {
            get;
            set;
        }

        public long Servings
        {
            get;
            set;
        }

        public string ImageLink
        {
            get;
            set;
        }

        public IEnumerable<LineItem> LineItems
        {
            get
            {
                lock (LineItem.m_sTA)
                {
                    foreach (DataAccess.DAL.LineItemsRow row in LineItem.m_sTA.GetAllByRecipeID(ID))
                    {
                        yield return new LineItem(row);
                    }
                }
            }
        }

        public IEnumerable<Direction> Directions
        {
            get
            {
                lock (Direction.m_sTA)
                {
                    foreach (DataAccess.DAL.DirectionsRow row in Direction.m_sTA.GetAllByRecipeID(ID))
                    {
                        yield return new Direction(row);
                    }
                }
            }
        }

        public bool AddDirection(string description)
        {
            bool success = false;

            lock (Direction.m_sTA)
            {
                try
                {
                    success = Direction.m_sTA.Add(ID, description) > 0;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return success;
        }

        public bool AddLineItem(Ingredient ingredient, decimal amount, Unit unit, string remarks)
        {
            bool success = false;

            lock (LineItem.m_sTA)
            {
                try
                {
                    success = LineItem.m_sTA.Add(ID, ingredient.ID, amount, unit.ID, remarks) > 0;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return success;
        }
    }
}