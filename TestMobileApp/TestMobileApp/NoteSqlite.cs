using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestMobileApp
{
    public class NoteSqlite
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
