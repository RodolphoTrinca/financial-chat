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
