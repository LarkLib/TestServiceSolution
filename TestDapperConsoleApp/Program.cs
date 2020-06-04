using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TestDapperConsoleApp
{
    class Program
    {
        private static string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=TestDapper;Integrated Security=true;";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test1();
        }

        static void Test1()
        {
            List<Customer> customers = new List<Customer>();
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                //1 Execute a query and map it to a list of dynamic objects
                var customer = connection.Query<Customer>("select CustomerID=@customerID,FirstName=@firstName,LastName=@lastName,Email=@email", new Customer { CustomerID = 1, FirstName = "f", LastName = "l", Email = "test@test.com" });

                //2 Execute a query and map the results to a strongly typed List
                customers = connection.Query<Customer>("Select * From Customers").ToList();

                //3 Execute a query and map it to a list of dynamic objects
                var rows = connection.Query("select 1 A, 2 B union all select 3, 4");
                foreach (var row in rows)
                {
                    Console.WriteLine("row.A={0}", row.A);
                    Console.WriteLine("row.B={0}", row.B);
                    Console.WriteLine(row);
                }

                //4 Execute a Command that returns no results
                var count = connection.Execute(@"
  set nocount on
  create table #t(i int)
  set nocount off
  insert #t
  select @a a union all select @b
  set nocount on
  drop table #t", new { a = 1, b = 2 });
                Console.WriteLine(count);

                //5 Execute a Command multiple times
                count = connection.Execute(@"insert Customers(FirstName,LastName) values (@a, @b)",
                    new[]
                    {
                        new { a = 1, b = 1 },
                        new { a = 2, b = 2 },
                        new { a = 3, b = 3 }
                    });
                Console.WriteLine(count);

                //6 Parameterized queries
                //6.1 List Support
                //Will be translated to:select * from (select 1 as Id union all select 2 union all select 3) as X where Id in (@Ids1, @Ids2, @Ids3)" // @Ids1 = 1 , @Ids2 = 2 , @Ids2 = 3
                var result = connection.Query<int>("select * from (select 1 as Id union all select 2 union all select 3) as X where Id in @Ids", new { Ids = new int[] { 1, 2, 3 } });
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
                //6.2 Literal replacements
                var result2 = connection.Query("select * from Customers where CustomerID = {=CustomerID}", new { CustomerID = 1 });

                //7 Buffered vs Unbuffered readers
                //Dapper's default behavior is to execute your SQL and buffer the entire reader on return. This is ideal in most cases as it minimizes shared locks in the db and cuts down on db network time.
                //However when executing huge queries you may need to minimize memory footprint and only load objects as needed.To do so pass, buffered: false into the Query method.
                customers = connection.Query<Customer>("Select * From Customers", buffered: false).ToList();

                //8 Multi Mapping
                //Dapper is able to split the returned row by making an assumption that your Id columns are named Id or id. If your primary key is different or you would like to split the row at a point other than Id, use the optional splitOn parameter.
                var sql =
@"select * from Posts p
left join Users u on u.Id = p.OwnerId
Order by p.Id";

                var data = connection.Query<Post, User, Post>(sql, (post, user) => { post.Owner = user; return post; });
                var post = data.FirstOrDefault();

                //9 Multiple Results
                //Dapper allows you to process multiple result grids in a single query.
                sql =
@"
select * from Customers where CustomerID = @id
select * from Posts where OwnerID = @id
select * from Users where Id = @id";

                using (var multi = connection.QueryMultiple(sql, new { id = 1 }))
                {
                    var customerM = multi.Read<Customer>().Single();
                    var orders = multi.Read<User>().ToList();
                    var returns = multi.Read<Post>().ToList();
                }

                //10 Stored Procedures
                var user = connection.Query<User>("spGetUser", new { Id = 1 }, commandType: CommandType.StoredProcedure).SingleOrDefault();

                //If you want something more fancy, you can do:
                var p = new DynamicParameters();
                p.Add("@a", 2);
                p.Add("@b", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var result3 = connection.Query("spMagicProc", p, commandType: CommandType.StoredProcedure);
                var b = p.Get<string>("@b");
                var c = p.Get<int>("@c");

                //11 Ansi Strings and varchar
                var result4 = connection.Query<User>("select * from Users where Name = @Name", new { Name = new DbString { Value = "aaa", IsFixedLength = false, Length = 10, IsAnsi = true } });

                //12 Type Switching Per Row
                //Usually you'll want to treat all rows from a given table as the same data type. However, there are some circumstances where it's useful to be able to parse different rows as different data types. This is where IDataReader.GetRowParser comes in handy.
                //Imagine you have a database table named "Shapes" with the columns: Id, Type, and Data, and you want to parse its rows into Circle, Square, or Triangle objects based on the value of the Type column.
                //var shapes = new List<IShape>();
                //using (var reader = connection.ExecuteReader("select * from Shapes"))
                //{
                //    // Generate a row parser for each type you expect.
                //    // The generic type <IShape> is what the parser will return.
                //    // The argument (typeof(*)) is the concrete type to parse.
                //    var circleParser = reader.GetRowParser<IShape>(typeof(Circle));
                //    var squareParser = reader.GetRowParser<IShape>(typeof(Square));
                //    var triangleParser = reader.GetRowParser<IShape>(typeof(Triangle));

                //    var typeColumnIndex = reader.GetOrdinal("Type");

                //    while (reader.Read())
                //    {
                //        IShape shape;
                //        var type = (ShapeType)reader.GetInt32(typeColumnIndex);
                //        switch (type)
                //        {
                //            case ShapeType.Circle:
                //                shape = circleParser(reader);
                //                break;
                //            case ShapeType.Square:
                //                shape = squareParser(reader);
                //                break;
                //            case ShapeType.Triangle:
                //                shape = triangleParser(reader);
                //                break;
                //            default:
                //                throw new NotImplementedException();
                //        }

                //        shapes.Add(shape);
                //    }
                //}

                //13 User Defined Variables in MySQL
                //In order to use Non - parameter SQL variables with MySql Connector, you have to add the following option to your connection string:
                //Allow User Variables = True
                //Make sure you don't provide Dapper with a property to map.

                //14 Limitations and caveats
                //Dapper caches information about every query it runs, this allows it to materialize objects quickly and process parameters quickly. The current implementation caches this information in a ConcurrentDictionary object.Statements that are only used once are routinely flushed from this cache.Still, if you are generating SQL strings on the fly without using parameters it is possible you may hit memory issues.
                //Dapper's simplicity means that many feature that ORMs ship with are stripped out. It worries about the 95% scenario, and gives you the tools you need most of the time. It doesn't attempt to solve every problem.

                //15 Will Dapper work with my DB provider?
                //Dapper has no DB specific implementation details, it works across all .NET ADO providers including SQLite, SQL CE, Firebird, Oracle, MySQL, PostgreSQL and SQL Server.


                //16 Bulk Inserting Data With Dapper
                sql = "insert into Customers (FirstName, LastName) values (@FirstName, @LastName)";
                var newCategories = new[]
                {
                    new Customer(){FirstName = "New Category 1", LastName = "Description 1"},
                    new Customer(){FirstName = "New Category 2", LastName = "Description 2"},
                    new Customer(){FirstName = "New Category 3", LastName = "Description 3"},
                    new Customer(){FirstName = "New Category 4", LastName = "Description 4"}
                };
                var affectedRows = connection.Execute(sql, newCategories);
                Console.WriteLine($"Affected Rows: {affectedRows}");
            };
        }
    }
}
