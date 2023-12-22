using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class Ingredient
    {
        static DataAccess.DALTableAdapters.IngredientsTableAdapter m_sTA = new DataAccess.DALTableAdapters.IngredientsTableAdapter();

        public static Ingredient Get(long id)
        {
            Ingredient result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByID(id);

                if (resultTable.Rows.Count > 0)
                {
                    result = new Ingredient(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public static IEnumerable<Ingredient> GetAll()
        {
            lock (m_sTA)
            {
                foreach (DataAccess.DAL.IngredientsRow row in m_sTA.GetAll())
                {
                    yield return new Ingredient(row);
                }
            }
        }

        public static bool Add(string name, string description, string imageLink="#")
        {
            bool success = false;

            lock (m_sTA)
            {
                try
                {
                    success = m_sTA.Add(name, description, imageLink) > 0;
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return success;
        }

        public Ingredient()
        {
            // NOP
        }

        public Ingredient(DataAccess.DAL.IngredientsRow row)
        {
            ID = row.ID;
            Name = row.Name;
            ImageLink = row.ImageLink;
            Description = row.Description;
        }

        public long ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ImageLink
        {
            get;
            set;
        }
    }
}