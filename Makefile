PHONY: producer consumer all init


producer:
	cd dotnet/stream_client/ && Make producer

consumer:
	cd dotnet/stream_client/ && Make consumer

init:
	cd dotnet/stream_client/ && Make init

init-python:
	@echo "init python venv"
	python3 -m venv .venv
	@echo "install python dependencies"
	.venv/bin/pip install -r requirements.txt
