Using Elasticsearch.Net to create an index and copy data from the local database.

The database contains sample data of teacher subjects. Each teacher has at least one subject.
Subject names are concantenated when creating Elastic documents.
 
# Installation

1. Create the target database, i.e. _Elnes_.

2. Build the solution.

```
dotnet build .\Elnes.sln --configuration Release
```

3. Build the pod.

```
cd Elnes/Docker
make docker-build
```

4. Run __Elnes__  to install migrations and then required users and roles.

Please use one of the next commands:
```
Elnes.exe create-index # creates the sample_teachers index in the Elasticsearch node
Elnes.exe create-db # runs migration scripts in the database
Elnes.exe seed-db # creates test data in the database
Elnes.exe copy-rows --all # copies all data from the database to the Elasticsearch
Elnes.exe copy-rows [id] # copies data from the database to the Elasticsearch. The Teacher id must be provided.
```
