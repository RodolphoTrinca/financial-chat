# Financial Chat
## Technology used
This project was made using:
- UI: React JS
- API: ASP.NET .NET Core 8
- Database: Postgres
- NGINX
- makefile

## Documentation
The docs are placed in this [location](./docs/docs.md). 

## How to run the project 
### Startup
To startup project run: make startup

### Migrate database
To update the database run the command: make migrate 

### UI
- The ui responses on: [http://localhost:10000/](http://localhost:10000/)

### API Swagger
- The swagger api response on [http://localhost:10000/api/index.html](http://localhost:10000/api/index.html)

## Tests
This project has integration tests in order to run tests properly please run
`make deps` before running the tests then run `make tests`

### Unload dependencies
To unload test dependencies run `make undeps`

### Stooq API failure
- when I was developing this challenge I couldn't fetch the api of stooq from my development computer so I've downloaded the csv file from another device and I make a mocked response to work on my computer, I don't know what is happening but if the api request don't work properly there is a mocked gateway that loads a file with the price of AAPL stock it will work as de same flow as if the request worked with a request to stooq api.

so to work on my computer I've changed the stooq gateway to a mocked one just change the line 50 of [Program.cs](./src/FinancialChat.ChatCommandsWorker/Program.cs) from ChatCommandsWorker to `builder.Services.AddScoped<IStockService, StockPriceMockedGateway>();` the following code flow is the same as if the api responds correctly.

