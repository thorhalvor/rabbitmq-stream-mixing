.PHONY: producer consumer all



producer:
	@echo "running AMQP pika producer"
	venv/bin/python3 amqp_client/producer.py
	venv/bin/python3 python_rstream/producer.py

consumer:
	@echo "running AMQP pika consumer"
	venv/bin/python3 amqp_client/consumer.py
	venv/bin/python3 python_rstream/consumer.py


init-python:
	@echo "init python venv"
	rm -rf venv
	python3 -m venv venv
	venv/bin/pip install -r requirements.txt

	
	
all: producer consumer