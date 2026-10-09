# Constituição do projeto

## Stack

Utilizar:

C#, .NET 10 para o projeto de back end atraves de uma Minimal API;
Angular para o projeto de front end;
entityframework como ORM;
sqlserver como banco de dados relacional;
xUnit para testes;

## Arquitetura

Utilizar a arquitetura Clean Archtecture, com projetos de API, Application, Infrastructure, Domain e Web. Todos os projetos utilizarão a stack informada na categoria Stack e apenas o projeto Web utilizará Angular.
A camada de Domain será responsável pelas entidades, regras de negócio e interfaces da solução.
A camada API ficará responsável pelos métodos de acesso a dados do banco de dados.
A camada de Infrastructure é responsável por implementar o acesso a dados e a integrações externas se houverem.
A camada de application é responsável por orquestrar as solicitações. Receber as solicitações do projeto Web via Requests, executar as regras de negócio contidas no projeto Domain, encaminhar essas solicitações ao projeto API que por sua vez, acessa o banco de dados através de métodos próprios, utilizando projeto infrastructure e as configurações dele para realizar suas tarefas. e Por fim, o projeto API retorna as respostas para o projeto Application, que por sua vez, retorna as respostas ao projeto Web.
Criar um projeto Tests.

## Qualidade

Respeitar os princípios SOLID;
Implentar o uso de Clean Code;
Respeitar o princípio de Idempotência;
Respeitar o paradigma de POO;
Deve haver um ou mais testes unitários para cada entidade e método do projeto Domain;
Deve haver um ou mais testes unitários para cada método no projeto Application;
Deve haver um ou mais testes unitários para cada método no projeto API.
Utilizar o framework ProblemDetails para tratar e retornar os erros.

## Convenções

Classes, interfaces, métodos e propriedades devem seguir as convenções de nomenclatura do C#.
Classes, métodos e propriedades públicas devem utilizar PascalCase.
Variáveis locais e parâmetros devem utilizar camelCase.
Interfaces devem possuir o prefixo I.
O código deve utilizar recursos assíncronos, como async e await, nas operações de entrada e saída quando apropriado.
Os endpoints da API devem seguir convenções REST e utilizar os métodos HTTP e códigos de status adequados.
Os contratos de entrada e saída da API devem ser definidos por DTOs, evitando a exposição direta das entidades de persistência.
Os nomes de entidades, propriedades e métodos devem utilizar termos consistentes com o domínio de registro de ponto.

## Governança

A Constitution deve ser respeitada durante todas as etapas de especificação, planejamento e implementação.
A implementação deve seguir os requisitos definidos na especificação funcional e as decisões técnicas aprovadas no plano.
Nenhuma funcionalidade deve ser adicionada sem estar prevista na especificação ou ser formalmente aprovada.
Em caso de conflito entre requisitos, decisões técnicas e esta Constituição, o conflito deve ser identificado e resolvido antes da implementação.
Uma tarefa somente deve ser considerada concluída quando seus critérios de aceitação forem atendidos e os testes pertinentes forem aprovados.