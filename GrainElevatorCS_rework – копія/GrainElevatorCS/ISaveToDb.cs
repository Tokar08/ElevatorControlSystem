using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    public interface ISaveToDb
    {
        Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects);

    }
}
