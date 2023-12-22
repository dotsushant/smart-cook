using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class LineItem
    {
        internal static DataAccess.DALTableAdapters.LineItemsTableAdapter m_sTA = new DataAccess.DALTableAdapters.LineItemsTableAdapter();

        public static LineItem Get(long id)
        {
            LineItem result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByID(id);

                if (resultTable.Rows.Count > 0)
                {
                    result = new LineItem(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public LineItem()
        {
            // NOP
        }

        public LineItem(DataAccess.DAL.LineItemsRow row)
        {
            ID = row.ID;
            UnitID = row.UnitID;
            Amount = row.Amount;
            Remarks = row.Remarks;
            RecipeID = row.RecipeID;
            IngredientID = row.IngredientID;
        }

        public long ID
        {
            get;
            set;
        }

        public long RecipeID
        {
            get;
            set;
        }

        public long IngredientID
        {
            get;
            set;
        }

        public decimal Amount
        {
            get;
            set;
        }

        public long UnitID
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public Unit Unit
        {
            get
            {
                return Unit.Get(UnitID);
            }
        }

        public Ingredient Ingredient
        {
            get
            {
                return Ingredient.Get(IngredientID);
            }
        }
    }
}