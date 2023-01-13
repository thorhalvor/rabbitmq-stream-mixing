.PHONY: producer consumer all


producer:
	@echo "running dot dotnet producer"
	dotnet run --project producer/producer.csproj

consumer:
	@echo "running dot dotnet consumer"
	dotnet run --project consumer/consumer.csproj

all: producer consumer