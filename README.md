# CS.Campaigns

Microsserviço de campanhas e intenção de doação da **Conexão Solidária**.

A API **não** atualiza `ValorTotalArrecadado` — publica `DoacaoRecebidaEvent` no RabbitMQ.

## Endpoints

- `POST/PUT/GET /api/campanhas` — GestorONG
- `GET /api/campanhas/ativas` — público
- `POST /api/doacoes` — Doador (202 + evento)

Stack completa: [CS.Infra](https://github.com/GabrielNatan2001/CS.Infra).
