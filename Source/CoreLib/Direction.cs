using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class Direction
    {
        internal static DataAccess.DALTableAdapters.DirectionsTableAdapter m_sTA = new DataAccess.DALTableAdapters.DirectionsTableAdapter();

        public Direction()
        {
            // NOP
        }

        public Direction(DataAccess.DAL.DirectionsRow row)
        {
            ID = row.ID;
            RecipeID = row.RecipeID;
            Description = row.Description;
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

        public string Description
        {
            get;
            set;
        }
    }
}