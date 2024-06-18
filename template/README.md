# Desafio Backend Mottu

## Visão Geral
Este projeto é um sistema de gestão de aluguel de motocicletas e entregas desenvolvido em C# .NET 8. 
O sistema permite que administradores cadastrem pedidos na plataforma, notifiquem entregadores aptos a efetuar as entregas, e gerenciem todo o ciclo de vida das entregas. 
A comunicação assíncrona é realizada usando RabbitMQ para notificação de novos pedidos. 
Além disso, o sistema permite que entregadores aceitem pedidos e registrem a finalização das entregas.

## Estrutura do Projeto

### Entidades Pricipais
- **DeliveryRider**: Representa um entregador cadastrado no sistema.
- **Bike**: Representa uma moto cadastrada no sistema.
- **Rental**: Representa um aluguel de motocicleta cadastrado no sistema.
- **DeliveryRequest**: Representa um pedido cadastrado no sistema.
- **DeliveryRequestNotification**: Representa uma notificação de pedido criado cadastrada no sistema.

### Serviços
- **BikeRental.API**: API RESTful que permite a gestão de entregadores, motos, alugueis e pedidos.

### Casos de Uso Solicitados vs Implementados
- [x] Eu como usuário admin quero cadastrar uma nova moto.
  - [x] Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - [x] A placa é um dado único e não pode se repetir.
    
- [x] Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- [x] Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente.
- [x] Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- [x] Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
  - [x] Os dados do entregador são( identificador, nome, cnpj, data de nascimento, numero da cnh, tipo da cnh, imagemCnh)
  - [x] Os tipos de cnh válidos são A, B ou ambas A+B.
  - [x] O cnpj é único e não pode se repetir.
  - [x] O número da CNH é único e não pode se repetir.
- [x] Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
  - [x] O formato do arquivo deve ser png ou bmp.
  - [x] A foto não poderá ser armazenada no banco de dados, você pode utilizar um storage( disco local, amazon s3, minIO ou outros).
- [x] Eu como entregador quero alugar uma moto por um período.
  - [x] Os planos disponíveis para locação são:
    - [x] 7 dias com um custo de R$30,00 por dia
    - [x] 15 dias com um custo de R$28,00 por dia
    - [x] 30 dias com um custo de R$22,00 por dia
  - [x] A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
  - [x] O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
  - [x] O entregador só conseguirá concluir na locação caso exista motos disponíveis.
  - [x] Somente entregadores habilitados na categoria A podem efetuar uma locação
- [x] Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
  - [x] Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
    - [x] Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
    - [x] Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - [x] Para plano de 30 dias o valor da multa é de 60% sobre o valor das diárias não efetivadas.
  - [x] Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.
- [x] Eu como admin quero cadastrar um pedido na plataforma e disponibilizar para os entregadores aptos efetuarem a entrega.
  - [x] Os dados obrigatórios do pedido são: identificador, data de criacao, valor da corrida, situacao.
  - [x] As situações válidas são disponivel, aceito e entregue.
  - [x] Quando o pedido entrar na plataforma a aplicação deverá notificar os entregadores sobre a existencia desse pedido.
    - [x] A notificação deverá ser publicada por mensageria.
    - [x] Somente entregadores com locação ativa e que não estejam com um pedido já aceito deverão ser notificados.
  - [x] Criar um consumidor para notificação de pedido disponível.
    - [x] Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- [x] Eu como admin quero consultar todos entregadores que foram notificados de um pedido.
- [x] Eu como entregador quero aceitar um pedido.
  - [x] Somente entregadores que tenham sido notificados podem aceitar o pedido.
- [x] Eu como entregador quero efetuar a entrega do pedido.

## Padrão de Design Utilizado: CQRS
O projeto utiliza o padrão de design Command Query Responsibility Segregation (CQRS), que separa as operações de leitura (queries) das operações de escrita (commands). Isso permite uma melhor organização do código, além de otimizar e escalar separadamente as partes de leitura e escrita da aplicação.

- **Commands**: Utilizados para ações que alteram o estado do sistema, como cadastrar um pedido, aceitar um pedido e finalizar uma entrega.
- **Queries**: Utilizados para buscar dados do sistema, como consultar pedidos e entregadores notificados.

## Add migration

dotnet ef migrations add Initial_Create --context BikeRentalContext --project ./src/Services/BikeRental/BikeRental.Data/BikeRental.Data.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj -o Migrations
dotnet ef migrations add Initial_Create --context IntegrationEventLogContext --project ./src/CrossCutting/EventBus/BikeRental.CrossCutting.IntegrationEventLog/BikeRental.CrossCutting.IntegrationEventLog.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj -o Migrations

## Remove Migration
dotnet ef migrations remove --context BikeRentalContext --project ./src/Services/BikeRental/BikeRental.Data/BikeRental.Data.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj

## Publish added migrations

dotnet ef database update --context BikeRentalContext --project ./src/BikeRental.API/BikeRental.API.csproj
dotnet ef database update --context IntegrationEventLogContext --project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj
