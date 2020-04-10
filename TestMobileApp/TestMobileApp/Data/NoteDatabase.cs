using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestMobileApp
{
    public class NoteDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public NoteDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<NoteSqlite>().Wait();
        }

        public Task<List<NoteSqlite>> GetNotesAsync()
        {
            return _database.Table<NoteSqlite>().ToListAsync();
        }

        public Task<NoteSqlite> GetNoteAsync(int id)
        {
            return _database.Table<NoteSqlite>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(NoteSqlite note)
        {
            if (note.ID != 0)
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }

        public Task<int> DeleteNoteAsync(NoteSqlite note)
        {
            return _database.DeleteAsync(note);
        }
    }
}
