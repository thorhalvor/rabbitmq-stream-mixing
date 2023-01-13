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



