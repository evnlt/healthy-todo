1. Run MongoDb in Docker: 

docker run -d -p 27017:27017 --name mongodb mongo:7

2. Run the API:
dotnet run --project HealthyTodo.API

3. Swagger

Once the application is running, open:

https://localhost:7263/swagger
