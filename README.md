# LibraryAPI
This is an API that functions for simple library management. consists of 2 services:
1. book service, for book management including publisher, category, storage location, and book stock. Consists of 3 tables: Bukus, Categories, StorageLocation

2. Transaction Service, for management of all library transactions and library members, namely students. consists of 3 tables: Mahasiswa, Transactions, TransactionDetail

for communication from the transaction service to the book service using HTTP or Remote Procedure Invocation

# You Need To Install First From Nugget Package
```bash
1.	microsorft.entityFrameworkCore.SqlServer
2.	microsorft.entityFrameworkCore.Tools
3.	microsorft.entityFrameworkCore.Design
4.	Install AutoMapper
5.	Install NewtonSoft.Json
```

# How To Run
### Migration Database
In Package Manager Console make sure to migration all service database
```bash
Update-Database
```
### Setting Multiple StartUp Project

```bash
Solution -> Configuration StartUp Project ->  Startup Project -> and choose Multiple Startup Project
```


