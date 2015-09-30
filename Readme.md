This is the repository for the C# MDF file parser that I originally demoed at Miracle Open World 2011.

Please note that this code is highly experimental. Lots of stuff and special cases are either not supported or outright ignored at this point. While the code is under development, it's current state is purely prototypical.

SQL Server 2008 R2 is the current target but as the storage format is almost identical to SQL Server 2005, few changes should be necessary to accommodate 2005.

If you have any comments, suggestions or fixes, please let me know.

OrcaMDF
=======
OrcaMDF is a parser for Microsoft SQL Server data files. It allows you to read SQL Server data files without the use of SQL Server.

There are two ways in which you can use OrcaMDF to parse data files - either through the ```Database``` or the ```RawDatabase``` class.

Database
--------
```Database``` is intended as a complete replacement for SQL Server when it comes to reading data files. It will automatically parse multiple data files, ensure they belong to the same database, read the boot page, read the metadata, expose DMVs are you're used to and allow you to scan both heaps and indexes (clustered & nonclustered).

While this results in a very powerful interface on top of a raw database file, it's also somewhat brittle as the database *must* be a SQL Server 2008 R2 database, and it must be absolutely corruption-free.

RawDatabase
-----------

Recommended reading
-------------------
Until I get the readme up to date, here's a series of blog posts detailing what OrcaMDF can do as well as how to use it:

* 2011-05-03 - http://improve.dk/introducing-orcamdf/
* 2011-05-05 - http://improve.dk/implementing-data-types-in-orcamdf/
* 2011-05-10 - http://improve.dk/parsing-dates-in-orcamdf/
* 2011-05-12 - http://improve.dk/reading-bits-in-orcamdf/
* 2011-06-14 - http://improve.dk/avoiding-regressions-in-orcamdf-by-system-testing/
* 2011-09-10 - http://improve.dk/orcamdf-feature-recap/
* 2011-10-24 - http://improve.dk/orcamdf-now-supports-databases-with-multiple-data-files/
* 2011-11-10 - http://improve.dk/orcamdf-now-exposes-metadata-through-system-dmvs/
* 2011-11-25 - http://improve.dk/orcamdf-studio-release-feature-recap/
* 2011-11-28 - http://improve.dk/automated-testing-of-orcamdf-against-multiple-sql-server-versions/
* 2012-02-07 - http://improve.dk/orcamdf-row-compression-support/
* 2012-08-27 - http://improve.dk/where-does-sql-server-store-the-source-for-stored-procedures/
* 2013-05-13 - http://improve.dk/orcamdf-is-now-available-on-nuget/
* 2013-11-04 - http://improve.dk/orcamdf-rawdatabase-a-swiss-army-knife-for-mdf-files/
* 2013-11-05 - http://improve.dk/corrupting-databases-purpose-using-orcamdf-corruptor/
