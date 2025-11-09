
CSaver

Fast ETL in CLI, written in .NET for operation of CSV file.



## Build

To build this project run

```bash
  dotnet build
```
Then run a migration. Make sure you have created appsettings.json, which will contain the "DefaultConnection" field.
```bash
  dotnet ef database update
```

## Run Locally

Run the project with command
```bash
  dotnet run {path_to_csv_file}
```
## Opportunities for further improvement
In the future, EST reformatting to UTC needs to be added.

For better processing of larger amounts of data, you can split the import file into chunks and process them with different threads.

Also, this version does not have advanced table settings. 
Future versions will focus more on table indexes, which will allow you to get the data you need faster in the future.