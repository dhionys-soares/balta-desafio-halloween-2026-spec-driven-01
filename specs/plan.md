# Plano técnico

## Contexto

O projeto consiste em uma solução para gerenciamento de registros de ponto e períodos de ausência de funcionários que trabalham em regime de home office.

A aplicação deve permitir o registro de entrada, início do intervalo, fim do intervalo e saída, além do registro e da consulta de períodos de ausência.

O sistema terá dois perfis de acesso: funcionário e administrador. Ambos poderão registrar e consultar seus próprios registros, enquanto o administrador também poderá consultar os registros de ponto e os períodos de ausência dos funcionários.

A implementação deve respeitar os requisitos funcionais, as regras de negócio e os critérios de aceite definidos na spec.md, seguindo os princípios arquiteturais e de qualidade estabelecidos na constitution.md.

## Arquitetura

O projeto seguirá uma arquitetura Clean Architecture, separando responsabilidades para facilitar a manutenção, os testes e a evolução da aplicação.

Camadas

1. API

Responsável por receber as requisições HTTP, validar os contratos de entrada, aplicar as políticas de autorização e retornar respostas adequadas.

Deve conter os controllers, os DTOs de entrada e saída e a configuração dos endpoints.

2. Application

Responsável por coordenar os casos de uso da aplicação, incluindo o registro de ponto, o registro de ausências e a consulta dos históricos.

Deve centralizar a execução dos casos de uso e garantir que as operações respeitem as regras de negócio do domínio.

3. Domain

Responsável por representar as entidades, os conceitos e as regras de negócio fundamentais do sistema.

Deve conter as entidades relacionadas aos usuários, às marcações de ponto e aos períodos de ausência, evitando dependências de infraestrutura.

4. Infrastructure

Responsável pela persistência dos dados, pela integração com os mecanismos de autenticação e por outras implementações técnicas necessárias.

Deve conter o contexto do Entity Framework Core, os mapeamentos das entidades e as implementações de acesso aos dados.

5. Web

Responsável pelo front end, pela interação do usuário com o sistema e encarregado de enviar as requisições HTTP para o projeto Application.

## Decisões

D01 — Linguagem e plataforma

Decisão: utilizar C# e ASP.NET Core na versão LTS do .NET definida para o projeto. Utilizar Angular para o projeto Web.

Justificativa: a plataforma oferece suporte à construção de APIs REST, autenticação, autorização, injeção de dependência e testes, além de estar alinhada à stack definida na Constitution.

D02 — Banco de dados

Decisão: utilizar SQL Server como banco de dados relacional.

Justificativa: o sistema precisa persistir usuários, perfis, marcações de ponto e períodos de ausência, mantendo relacionamentos e integridade dos dados. O SQL Server atende a essas necessidades e faz parte da stack estabelecida na Constitution.

D03 — Acesso a dados

Decisão: utilizar Entity Framework Core para mapeamento objeto-relacional e persistência.

Justificativa: o ORM permite mapear as entidades para tabelas relacionais, gerenciar migrações e executar operações de persistência com integração ao .NET.

D04 — Autenticação e autorização

Decisão: utilizar ASP.NET Core Identity para gerenciar usuários, senhas e perfis, com autenticação baseada em tokens JWT para proteger os endpoints da API.

Justificativa: o sistema exige login individual e diferenciação entre funcionários e administradores. O Identity fornece recursos para gerenciamento seguro de credenciais, enquanto o JWT permite autenticar requisições à API.

As permissões devem ser verificadas no servidor. O perfil do usuário não deve ser aceito como informação confiável fornecida livremente pelo cliente.

D05 — Separação de responsabilidades

Decisão: organizar a solução em Web, API, Application, Domain e Infrastructure.

Justificativa: a separação permite modificar detalhes de persistência ou apresentação sem espalhar alterações pelas regras de negócio, além de facilitar testes isolados.

D06 — Registro das marcações

Decisão: representar cada marcação de ponto como um registro individual associado ao usuário e ao tipo de evento.

Os tipos de evento serão entrada, início do intervalo, fim do intervalo e saída.

Justificativa: essa estrutura mantém o histórico das marcações e permite consultar a sequência de eventos e calcular a duração das jornadas concluídas.

D07 — Data e hora

Decisão: gerar a data e a hora das marcações no servidor, utilizando um padrão consistente de armazenamento e conversão para o horário local definido para a aplicação.

Justificativa: utilizar o relógio do cliente permitiria que o usuário alterasse os horários enviados. A geração no servidor reduz essa possibilidade e mantém um critério uniforme para validar a sequência das marcações.

D08 — Registro de ausências

Decisão: representar cada período de ausência como uma entidade independente das marcações de ponto.

O registro deve conter o funcionário associado, a data e a hora de início, a data e a hora de término, o motivo e os dados de auditoria necessários.

Justificativa: uma ausência não equivale a uma marcação de ponto e não deve criar, alterar ou excluir automaticamente registros de entrada, intervalo ou saída.

D09 — Controle de acesso aos registros

Decisão: identificar o usuário autenticado no servidor e utilizar sua identidade para determinar a titularidade das operações.

Funcionários poderão acessar somente seus próprios registros. Administradores poderão consultar registros dos funcionários, além de gerenciar seus próprios registros de ponto e ausência.

Justificativa: essa abordagem impede que a alteração de um identificador enviado pelo cliente seja suficiente para acessar dados de outro usuário.

D10 — Validação das regras de negócio

Decisão: centralizar as regras de sequência das marcações, integridade das jornadas e validação dos períodos de ausência na camada de aplicação e no domínio, conforme a responsabilidade de cada regra.

Justificativa: as regras devem ser aplicadas independentemente do endpoint utilizado, evitando duplicação e inconsistências.

D11 — Testes automatizados

Decisão: utilizar xUnit para testes unitários e testes de integração.

Justificativa: os testes unitários permitem validar as regras de negócio isoladamente, enquanto os testes de integração verificam a interação entre a API, a autenticação e a persistência.

D12 — Documentação da API

Decisão: utilizar Swagger/OpenAPI para documentar os endpoints e permitir a execução de requisições durante o desenvolvimento.

Justificativa: a documentação facilita a validação dos contratos, a execução de testes manuais e a compreensão da API.

## Modelo de dados
--
O modelo inicial será composto pelas seguintes entidades.

ApplicationUser

Representa um usuário do sistema.

A autenticação será gerenciada pelo ASP.NET Core Identity.

Campos e informações relevantes:

Id: identificador único do usuário.
UserName: nome de usuário utilizado na autenticação.
Email: endereço de e-mail.
PasswordHash: hash da senha, gerenciado pelo Identity.
Role: perfil de acesso, funcionário ou administrador.

O gerenciamento de senhas e os campos de autenticação devem utilizar os mecanismos fornecidos pelo Identity, sem implementar armazenamento de senhas manualmente.

TimeRecord

Representa uma marcação individual de ponto.

Campos:

Id: identificador único da marcação.
UserId: identificador do funcionário responsável pela jornada.
RecordType: tipo da marcação.
RecordedAt: data e hora registradas pelo servidor.
CreatedAt: data e hora de criação do registro, caso seja necessário manter esse dado separadamente.

O campo RecordType deverá aceitar somente os tipos definidos pelo domínio: entrada, início do intervalo, fim do intervalo e saída.

AbsencePeriod

Representa um período de ausência.

Campos:

Id: identificador único do período.
UserId: identificador do funcionário ao qual a ausência está associada.
StartAt: data e hora de início.
EndAt: data e hora de término.
Reason: motivo da ausência.
CreatedAt: data e hora de criação do registro.

O motivo poderá representar ausência médica ou outro motivo permitido pela aplicação.

Relacionamentos
Um usuário poderá possuir várias marcações de ponto.
Um usuário poderá possuir vários períodos de ausência.
Cada marcação de ponto deverá pertencer a um único usuário.
Cada período de ausência deverá estar associado a um único usuário.
Os registros deverão possuir chaves estrangeiras para garantir a integridade referencial.
Integridade e persistência
Os identificadores devem ser únicos.
Os registros devem ser persistidos de forma consistente.
As validações de sequência devem impedir marcações incompatíveis com o estado atual da jornada.
Períodos de ausência devem possuir início anterior ao término.
As consultas de funcionários devem ser restringidas pela identidade autenticada.
As consultas administrativas devem respeitar os filtros e as permissões definidos na especificação.
O modelo não deve permitir a alteração ou exclusão de marcações pelos funcionários.

## Contratos

O sistema seguirá o padrão REST, utilizando JSON para entrada e saída de dados.

Os endpoints abaixo representam a proposta inicial dos contratos.

Autenticação
POST /api/auth/login: autenticar um usuário e retornar o token de acesso.
POST /api/auth/logout: encerrar a sessão lógica do usuário conforme a estratégia de autenticação adotada.
Registro de ponto
POST /api/time-records: registrar a próxima marcação da jornada do usuário autenticado.
GET /api/time-records?date={date}: consultar as marcações do próprio usuário em uma data.
GET /api/time-records/history?startDate={startDate}&endDate={endDate}: consultar o histórico do próprio usuário em um período.

O tipo de marcação deverá ser determinado de acordo com o estado atual da jornada e as regras de negócio, evitando que o cliente escolha livremente um tipo incompatível com a sequência permitida.

Períodos de ausência
POST /api/absences: registrar um período de ausência para o usuário autenticado.
GET /api/absences: consultar os próprios períodos de ausência.
GET /api/absences?startDate={startDate}&endDate={endDate}: consultar os próprios períodos de ausência em um intervalo de datas.

O contrato de criação deverá receber as datas de início e término e o motivo da ausência. O usuário associado será determinado pela identidade autenticada.

Consultas administrativas
GET /api/admin/time-records?userId={userId}&startDate={startDate}&endDate={endDate}: consultar as marcações de um funcionário em um período.
GET /api/admin/absences?userId={userId}&startDate={startDate}&endDate={endDate}: consultar os períodos de ausência de um funcionário em um período.

Esses endpoints deverão exigir autenticação e autorização do perfil de administrador.

Contratos de entrada e saída

Os contratos deverão utilizar DTOs para evitar a exposição direta das entidades de persistência.

As respostas devem apresentar os dados necessários para a funcionalidade solicitada, sem expor hashes de senha, segredos ou informações internas desnecessárias.

Respostas HTTP

O sistema deverá utilizar códigos HTTP adequados, incluindo:

200 OK: consulta ou operação concluída com sucesso.
201 Created: registro criado com sucesso.
400 Bad Request: dados inválidos ou violação de uma regra de negócio representada como erro de requisição.
401 Unauthorized: usuário não autenticado ou token inválido.
403 Forbidden: usuário autenticado sem permissão para a operação.
404 Not Found: recurso não encontrado, quando aplicável.
409 Conflict: conflito com o estado atual da jornada ou dos dados, quando apropriado.
500 Internal Server Error: erro inesperado, sem exposição de detalhes internos.

## Riscos

R01 — Inconsistência nas marcações simultâneas

Duas requisições simultâneas podem tentar registrar marcações incompatíveis para a mesma jornada.

Mitigação: validar a sequência das marcações no servidor e adotar mecanismos de consistência transacional e concorrência adequados.

R02 — Acesso indevido aos registros

Um funcionário pode tentar consultar os dados de outro funcionário alterando parâmetros da requisição.

Mitigação: aplicar autorização no servidor, utilizar a identidade autenticada para restringir consultas e testar explicitamente os acessos entre usuários.

R03 — Divergência de data e hora

Diferenças de fuso horário ou alterações no relógio do cliente podem causar registros inconsistentes.

Mitigação: utilizar o horário gerado pelo servidor, adotar uma política consistente de fuso horário e documentar as conversões utilizadas nas consultas e nos cálculos.

R04 — Jornadas incompletas

O funcionário pode esquecer uma marcação obrigatória ou tentar concluir uma jornada em uma data posterior.

Mitigação: validar a sequência dos eventos, identificar jornadas incompletas e rejeitar marcações que violem a regra de não atravessar a meia-noite.

R05 — Conflitos entre ausências e jornadas

Um período de ausência pode coincidir com uma jornada ou se sobrepor a outro período de ausência.

Mitigação: validar os intervalos conforme as regras aprovadas. A política para sobreposição entre ausências e marcações deverá ser definida antes da implementação, sem presumir que o registro de uma ausência altera a jornada.

R06 — Exposição de credenciais

Tokens, senhas ou dados sensíveis podem ser expostos em respostas, logs ou configurações.

Mitigação: utilizar os mecanismos de segurança do ASP.NET Core Identity, armazenar somente hashes de senha, proteger segredos e evitar registrar credenciais ou tokens nos logs.

R07 — Divergência entre especificação e implementação

A implementação pode introduzir funcionalidades não previstas ou deixar de atender a critérios de aceite.

Mitigação: relacionar as tarefas aos requisitos da spec.md e validar os critérios de aceite por meio de testes automatizados.

R08 — Alterações indevidas no histórico

A ausência de correção de marcações pode deixar jornadas incompletas sem um procedimento de regularização.

Mitigação: preservar os registros existentes, informar claramente as operações rejeitadas e manter a funcionalidade de correção fora do escopo definido.