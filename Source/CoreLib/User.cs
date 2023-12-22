using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class User
    {
        static Dictionary<long, Guid> m_sGuidMap = new Dictionary<long, Guid>();
        internal static DataAccess.DALTableAdapters.UsersTableAdapter m_sTA = new DataAccess.DALTableAdapters.UsersTableAdapter();
        internal static DataAccess.DALTableAdapters.IngredientStatusTableAdapter m_sIngredientStatusTA = new DataAccess.DALTableAdapters.IngredientStatusTableAdapter();

        private void UpdateMap(User user)
        {
            if (!m_sGuidMap.ContainsKey(user.ID))
            {
                m_sGuidMap.Add(user.ID, System.Guid.NewGuid());
            }
        }

        public static User Get(long id)
        {
            User result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByID(id);

                if (resultTable.Rows.Count > 0)
                {
                    result = new User(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public static User Get(string email)
        {
            User result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByEmail(email);

                if (resultTable.Rows.Count > 0)
                {
                    result = new User(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public static User Get(Guid guid)
        {
            User user = null;
            
            if(m_sGuidMap.ContainsValue(guid))
            {
                user = Get(m_sGuidMap.First(u => u.Value == guid).Key);
            }

            return user;
        }

        public static bool Add(string firstName, string lastName, string email)
        {
            bool success = false;

            lock (m_sTA)
            {
                try
                {
                    success = m_sTA.Add(firstName, lastName, email) > 0;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return success;
        }

        public User()
        {
            // NOP
        }

        public User(DataAccess.DAL.UsersRow row)
        {
            ID = row.ID;
            Email = row.Email;
            FirstName = row.FirstName;
            LastName = row.LastName;
            UpdateMap(this); // Store guid
        }

        public long ID
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public Guid? Guid // This is just an extension for now
        {
            get
            {
                return m_sGuidMap.ContainsKey(ID) ? m_sGuidMap[ID] : (Guid?) null;
            }
        }

        public IEnumerable<Recipe> Recipes
        {
            get
            {
                lock (Recipe.m_sTA)
                {
                    foreach (DataAccess.DAL.RecipesRow row in Recipe.m_sTA.GetAllByUserID(ID))
                    {
                        yield return new Recipe(row);
                    }
                }
            }
        }

        public IEnumerable<Recipe> Suggestions
        {
            get
            {
                return new List<Recipe>(); // return empty for now
            }
        }

        public bool UpdateIngredientAvailability(Ingredient ingredient, bool available)
        {
            bool success = false;
            int availability = available ? 1 : 0;

            lock (m_sIngredientStatusTA)
            {
                if (m_sIngredientStatusTA.Get(ID, ingredient.ID).Count == 0)
                {
                    success = m_sIngredientStatusTA.Add(ID, ingredient.ID, availability) > 0;
                }
                else
                {
                    success = m_sIngredientStatusTA.Refresh(availability, ID, ingredient.ID) > 0;
                }
            }

            return success;
        }

        public Recipe AddRecipe(string name, long cookingTime, long servings, string imageLink = "#")
        {
            Recipe recipe = null;

            lock (Recipe.m_sTA)
            {
                try
                {
                    if (Recipe.m_sTA.Add(ID, name, cookingTime, servings, imageLink) > 0)
                    {
                        recipe = new Recipe(Recipe.m_sTA.GetAllByUserID(ID).LastOrDefault());
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return recipe;
        }
    }
}