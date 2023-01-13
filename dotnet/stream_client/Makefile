.PHONY: producer consumer all init


producer:
	@echo "running dot dotnet producer"
	dotnet run --project producer/producer.csproj

consumer:
	@echo "running dot dotnet consumer"
	dotnet run --project consumer/consumer.csproj

init:
	@echo "running dot dotnet init"
	dotnet run --project init/init.csproj

	
	
all: producer consumer