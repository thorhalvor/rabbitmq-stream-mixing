# RabbitMQ stream Clients Mixing

The scope of this repo is to mix different clients to test the stream parser AMQP 1.0 and AMQP 091.

Clients should work by producing and consuming with differnt protocols (AMQP 091 and AMQP 1.0)


Commands:

Init the stream
```
make init
```

Init python
```
make init-python
```

Run producers

```
make producer
```

Run consumers
```
make consumer
```

How to add a new client
---

1- Go or create the specif directory for the language: <br />
   ex: golang 

2- Create the direcory for the client AMQP or stream <br />
   
   ex: golang/amqp_client ora golang/stream_client

3- Add two programns: <br />
   1- Producer <br />
   2- Consumer

4- Producer DOES NOT create the stream queue. The producer has to send tree diffent kind of messages:<br />
   1- Only with the body <br />
   2- Body and Properties <br />
   3- Body, Properties and application properites <br />
   
5- Consumer has to consume and print the message. The consumer has to be clear in case of error.

6- Create a Makefile with: <br />
   make producer <br />
   make consumer <br />
   
7- See for example: https://github.com/Gsantomaggio/rabbitmq-stream-mixing/tree/main/python
   


