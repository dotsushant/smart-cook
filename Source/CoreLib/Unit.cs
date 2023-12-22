using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using CrazyAboutPi.DataAccess;

namespace CrazyAboutPi.CoreLib
{
    public class Unit
    {
        internal static DataAccess.DALTableAdapters.UnitsTableAdapter m_sTA = new DataAccess.DALTableAdapters.UnitsTableAdapter();

        public static Unit Get(long id)
        {
            Unit result = null;

            lock (m_sTA)
            {
                var resultTable = m_sTA.GetByID(id);

                if (resultTable.Rows.Count > 0)
                {
                    result = new Unit(resultTable[0]); // First match is all we need
                }
            }

            return result;
        }

        public static bool Add(string description)
        {
            bool success = false;

            lock (m_sTA)
            {
                try
                {
                    success = m_sTA.Add(description) > 0;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }

            return success;
        }

        public Unit()
        {
            // NOP
        }

        public Unit(DataAccess.DAL.UnitsRow row)
        {
            ID = row.ID;
            Name = row.Name;
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
    }
}