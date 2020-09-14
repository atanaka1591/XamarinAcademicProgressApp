using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971Project
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
