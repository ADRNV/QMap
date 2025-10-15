 ![](https://img.shields.io/badge/-MSSQL-green)
 ![](https://img.shields.io/badge/-SQLite-gray)
 ![](https://img.shields.io/badge/-Postgres-blue)
# QMap
QMap is lightweight, simple, and easy extensible mapper. Represents itself set of extensions-methods, whos works up on IDbConnection.

## Possibilities
QMap can be used for quering and editing tables in database and map them to objects. 

1. Easy to use API
2. You can be use `LINQ` expressions for write easy to understand code.
4. Can write raw `SQL` queries if you want
5. Work with databases MSSQL, Postgres, SQLite
## Not found requered feature or database in list ? 
Fix that. QMap.Core and QMap.SqlBuilder APPis easy to extend or adding work with new database.
1. `QMap.Core` - helps to build from scratch extensions for databse if database works from [System.Data](https://learn.microsoft.com/en-us/dotnet/api/system.data?view=net-8.0)
2. `QMap.SqlBuilder` - helps to build/extend `LINQ` to `SQL` traslators **if** **required** **targeting** on feature of specific `SQL` dialects, `PL/pgSQL` for example.
