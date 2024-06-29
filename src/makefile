startup: up

up: 
	docker compose up -d --build 

up debug:
	docker compose up --build 
	
down:
	docker compose down

restart: down startup

tests: 
	cd ./FinancialChat.Tests && dotnet test

deps:
	docker compose up rabbitmq db --build 

migrate:
	cd ./FinancialChat.Infra && dotnet ef database update --context MessagesDbContext
	cd ./FinancialChat.Infra && dotnet ef database update --context FinancialChatContext	

undeps:
	cd ./FinancialChat.Tests && docker compose down