PHONY: producer consumer all init


producer:
	cd dotnet/stream_client/ && Make producer
	cd python && Make producer

consumer:
	cd dotnet/stream_client/ && Make consumer
	cd python && Make consumer

init:
	cd dotnet/stream_client/ && Make init

init-python:
	@echo "init python venv"
	cd python && make init-python