# .netcore-webapi
c# .netcore sqlserver

# 修改Startup.cs内容（数据库信息）：

var conn = @"Server=localhost;Database=test;User ID=sa;Password=123456;"; //Server:本地ip; Database：数据库名称; User ID：数据库账号; Password: 数据库密码
services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(conn));
services.AddControllers();
