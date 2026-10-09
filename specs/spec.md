# Sistema de registro de ponto

## Problema

Funcionários que trabalham em regime de home office precisam registrar diariamente seus horários de trabalho. Sem um sistema centralizado, o controle das marcações de entrada, início do intervalo, fim do intervalo e saída pode se tornar desorganizado, dificultando a consulta e a conferência das horas trabalhadas.

## Objetivo

Desenvolver um sistema que permita aos funcionários registrar e consultar seus horários de trabalho, mantendo um histórico organizado das marcações diárias e possibilitando o cálculo do total de horas trabalhadas.

## Usuários

Funcionários em home office: precisam registrar seus horários de trabalho e consultar seu histórico de ponto.

## Histórias

Como funcionário ou administrador, quero registrar minha entrada para informar o início da minha jornada de trabalho.
Como funcionário ou administrador, quero registrar o início do intervalo para informar quando interrompi minhas atividades.
Como funcionário ou administrador, quero registrar o fim do intervalo para informar quando retomei minhas atividades.
Como funcionário ou administrador, quero registrar minha saída para informar o encerramento da minha jornada de trabalho.
Como funcionário ou administrador, quero consultar os registros de um dia específico para conferir meus horários.
Como funcionário ou administrador, quero consultar meu histórico de ponto para acompanhar minhas jornadas anteriores.
Como funcionário ou administrador, quero visualizar o total de horas trabalhadas em um dia para acompanhar minha jornada.
Como funcionário ou administrador, quero receber uma mensagem clara quando tentar realizar uma marcação inválida para entender o que preciso corrigir.
Como funcionário, quero registrar um período de ausência, informando o início, o término e o motivo, para manter meu histórico atualizado.
Como funcionário, quero consultar meus períodos de ausência para acompanhar os registros realizados.
Como administrador, quero registrar meus próprios horários de trabalho para controlar minha jornada.
Como administrador, quero registrar meus próprios períodos de ausência para manter meu histórico atualizado.
Como administrador, quero consultar os registros de ponto dos funcionários para acompanhar suas jornadas de trabalho.
Como administrador, quero consultar os períodos de ausência dos funcionários para acompanhar as ocorrências registradas.
Como administrador, quero filtrar os registros por funcionário e período para localizar as informações necessárias.

## Requisitos funcionais

Autenticação e perfis de usuário

RF01: O sistema deve permitir que funcionários e administradores realizem login individual.
RF02: O sistema deve identificar o perfil do usuário autenticado, distinguindo funcionários de administradores.
RF03: O sistema deve restringir o acesso às funcionalidades de acordo com o perfil do usuário.
RF04: O sistema deve permitir que o usuário encerre sua sessão.
Registro de ponto
RF05: O sistema deve permitir que funcionários registrem sua entrada, início do intervalo, fim do intervalo e saída.
RF06: O sistema deve permitir que administradores registrem sua própria entrada, início do intervalo, fim do intervalo e saída.
RF07: O sistema deve registrar automaticamente a data e a hora de cada marcação.
RF08: O sistema deve associar cada marcação ao usuário autenticado que a realizou.
RF09: O sistema deve validar a sequência das marcações conforme as regras de negócio definidas.
RF10: O sistema deve informar quando uma marcação não puder ser realizada, apresentando o motivo.

Registro de ausências

RF11: O sistema deve permitir que funcionários registrem períodos de ausência, informando a data e a hora de início, a data e a hora de término e o motivo da ausência.
RF12: O sistema deve permitir que administradores registrem seus próprios períodos de ausência.
RF13: O sistema deve permitir o registro de diferentes motivos de ausência, incluindo ausência médica e outros motivos pessoais ou profissionais.
RF14: O sistema deve associar cada período de ausência ao usuário autenticado responsável pelo registro.
RF15: O sistema deve validar os períodos de ausência, impedindo datas inválidas e horários de término anteriores ou iguais aos horários de início.
RF16: O sistema deve permitir que o usuário consulte seus próprios períodos de ausência.

Consulta e acompanhamento

RF17: O sistema deve permitir que funcionários consultem suas marcações de ponto de uma data específica.
RF18: O sistema deve permitir que funcionários consultem seu histórico de ponto.
RF19: O sistema deve permitir que funcionários consultem o total de horas trabalhadas em jornadas concluídas.
RF20: O sistema deve permitir que administradores consultem as marcações de ponto dos funcionários.
RF21: O sistema deve permitir que administradores consultem os períodos de ausência registrados pelos funcionários.
RF22: O sistema deve permitir que administradores filtrem os registros de ponto por funcionário e período.
RF23: O sistema deve identificar jornadas com marcações incompletas, sem apresentá-las como jornadas concluídas.
Segurança e integridade
RF24: O sistema deve impedir o acesso de usuários não autenticados às funcionalidades protegidas.
RF25: O sistema deve impedir que funcionários consultem registros de ponto ou períodos de ausência de outros funcionários.
RF26: O sistema deve permitir que somente administradores acessem as consultas de registros de outros funcionários.
RF27: O sistema deve preservar o histórico de marcações de ponto, sem permitir que funcionários as alterem ou excluam.
RF28: O sistema deve informar quando uma operação de registro não for concluída com sucesso.

## Regras de negócio

RN01: Cada jornada diária deve seguir a sequência: entrada, início do intervalo, fim do intervalo e saída.
RN02: O funcionário deve registrar as marcações na ordem estabelecida.
RN03: Não deve ser permitido registrar duas marcações consecutivas do mesmo tipo na mesma jornada.
RN04: Cada marcação deve possuir uma data e hora válidas, geradas pelo sistema.
RN05: O horário de uma nova marcação deve ser posterior ao horário da marcação anterior da mesma jornada.
RN06: O total de horas trabalhadas deve ser calculado subtraindo o período de intervalo do tempo transcorrido entre a entrada e a saída.
RN07: Jornadas com marcações incompletas não devem ser apresentadas como jornadas concluídas.
RN08: Cada funcionário autenticado deve poder consultar somente os próprios registros.
RN09: As marcações registradas devem ser preservadas no histórico, sem possibilidade de alteração ou exclusão pelo funcionário.
RN10: O cálculo das horas trabalhadas deve considerar os horários efetivamente registrados, sem arredondamentos não especificados.
RN11: Cada jornada deve começar e terminar na mesma data.
RN12: Não deve ser permitido registrar uma nova entrada para uma jornada em uma data que já possua uma jornada iniciada ou concluída.
RN13: O funcionário deve estar autenticado para registrar marcações ou consultar seu histórico.
RN14: O funcionário não pode informar ou alterar o identificador do funcionário associado à marcação; essa associação deve ser determinada pela identidade autenticada.
RN15: A correção de marcações não faz parte do escopo do sistema.
RN16: O sistema deve distinguir os perfis de funcionário e administrador.
RN17: Funcionários podem registrar e consultar seus próprios registros de ponto e períodos de ausência.
RN18: Administradores podem registrar e consultar seus próprios registros de ponto e períodos de ausência, além de consultar os registros dos funcionários.
RN19: O perfil do usuário deve ser verificado no servidor para autorizar cada operação protegida.
RN20: Cada período de ausência deve possuir um início, um término e um motivo.
RN21: O término de um período de ausência deve ser posterior ao seu início.
RN22: O registro de uma ausência não deve criar automaticamente marcações de ponto nem alterar marcações existentes.
RN23: O registro de uma ausência não deve ser interpretado automaticamente como aprovação de afastamento ou justificativa legal da ausência.
RN24: Os períodos de ausência devem ser associados ao funcionário a que se referem e ao usuário responsável pelo registro.
RN25: Funcionários não podem consultar períodos de ausência de outros funcionários.
RN26: O administrador pode consultar os períodos de ausência dos funcionários, mas não pode alterar ou excluir marcações de ponto por meio dessa funcionalidade.
RN27: O registro de uma ausência não deve permitir que uma jornada atravesse a meia-noite, contrariando as regras de registro de ponto.

## Casos de borda
--
CB01: O funcionário tenta registrar o início do intervalo antes de registrar a entrada.
CB02: O funcionário tenta registrar o fim do intervalo sem ter registrado o início do intervalo.
CB03: O funcionário tenta registrar a saída antes de encerrar o intervalo.
CB04: O funcionário tenta realizar duas marcações do mesmo tipo consecutivamente.
CB05: O funcionário tenta realizar uma nova marcação com horário anterior ou igual ao da marcação anterior.
CB06: O funcionário consulta uma data em que não existem registros.
CB07: O funcionário possui uma jornada com uma ou mais marcações ausentes.
CB08: Ocorre uma falha ao salvar uma marcação, e o sistema precisa informar que o registro não foi confirmado.
CB09: O funcionário tenta consultar registros pertencentes a outro funcionário.
CB10: O funcionário tenta registrar uma nova entrada em uma data que já possui uma jornada iniciada ou concluída.
CB11: Um usuário não autenticado tenta registrar uma marcação ou consultar o histórico.
CB12: O funcionário tenta continuar utilizando uma sessão encerrada ou inválida.
CB13: O funcionário tenta registrar uma saída após a mudança da data, quando a jornada foi iniciada no dia anterior.
CB14: O funcionário tenta registrar uma marcação para uma jornada de outra data.
CB15: O funcionário tenta alterar ou excluir uma marcação já registrada.
CB16: O funcionário tenta enviar uma requisição utilizando o identificador de outro funcionário para registrar ou consultar marcações.

## Fora de escopo

Cadastro e gerenciamento administrativo de funcionários.
Criação de contas e recuperação de senhas.
Gestão de cargos, departamentos e hierarquias empresariais.
Aprovação de folhas de ponto por gestores.
Solicitação e aprovação de ajustes em marcações.
Edição ou exclusão de marcações já registradas.
Cálculo de horas extras, banco de horas, adicional noturno e descontos salariais.
Integração com sistemas de folha de pagamento.
Geolocalização, reconhecimento facial e registro por biometria.
Notificações por e-mail, SMS ou aplicativos de mensagens.
Aplicativo móvel nativo.
Relatórios gerenciais avançados e exportação para arquivos PDF ou Excel.
Jornadas que atravessam a meia-noite.

## Critérios de aceite

CA01: Dado um funcionário com credenciais válidas, quando realizar login, o sistema deve autenticar o usuário e permitir o acesso às funcionalidades autorizadas.
CA02: Dado um usuário com credenciais inválidas, quando tentar realizar login, o sistema deve rejeitar a autenticação sem revelar informações sensíveis.
CA03: Dado um usuário não autenticado, quando tentar registrar uma marcação ou consultar o histórico, o sistema deve negar o acesso.
CA04: Dado um funcionário autenticado sem marcações na jornada atual, quando registrar a entrada, o sistema deve salvar a marcação com a data e a hora geradas pelo sistema.
CA05: Dado que a entrada foi registrada, quando o funcionário iniciar o intervalo, o sistema deve salvar a marcação.
CA06: Dado que o início do intervalo foi registrado, quando o funcionário encerrar o intervalo, o sistema deve salvar a marcação.
CA07: Dado que o fim do intervalo foi registrado, quando o funcionário registrar a saída na mesma data, o sistema deve concluir a jornada.
CA08: Dado que uma marcação anterior obrigatória ainda não foi realizada, quando o funcionário tentar registrar uma marcação posterior na sequência, o sistema deve rejeitar a operação e informar o motivo.
CA09: Dado que uma marcação já foi realizada, quando o funcionário tentar registrar outra marcação do mesmo tipo consecutivamente, o sistema deve rejeitar a operação.
CA10: Dado que uma marcação anterior existe, quando o funcionário tentar realizar uma marcação com horário anterior ou igual ao da marcação anterior, o sistema deve rejeitar a operação.
CA11: Dado que uma jornada foi iniciada em uma data anterior, quando o funcionário tentar registrar uma nova marcação após a mudança da data, o sistema deve rejeitar a operação e informar que a jornada não pode atravessar a meia-noite.
CA12: Dado que uma jornada está concluída, quando o funcionário consultar o total de horas trabalhadas, o sistema deve apresentar o tempo entre a entrada e a saída, descontando o intervalo.
CA13: Dado que uma jornada possui marcações incompletas, quando o funcionário consultar seus registros, o sistema deve identificar a jornada como incompleta e não apresentá-la como concluída.
CA14: Dado que o funcionário consulta uma data sem marcações, o sistema deve informar que não existem registros para aquela data.
CA15: Dado que um funcionário autenticado solicita registros de outro funcionário, o sistema deve impedir o acesso aos registros solicitados.
CA16: Dado que uma marcação não foi salva por falha de persistência, o sistema não deve informar que a operação foi concluída com sucesso.
CA17: Dado que uma marcação foi registrada, quando o funcionário tentar alterá-la ou excluí-la, o sistema deve impedir a operação.
CA18: Dado que o funcionário encerrou sua sessão, quando tentar acessar uma funcionalidade protegida sem autenticar-se novamente, o sistema deve negar o acesso.
CA19: Dado que uma jornada já foi iniciada ou concluída na data atual, quando o funcionário tentar registrar uma nova entrada para a mesma data, o sistema deve rejeitar a operação.
CA20: Dado que um funcionário tenta informar o identificador de outro funcionário em uma requisição, o sistema deve utilizar a identidade do usuário autenticado para determinar a titularidade das marcações.
* CA21: Dado um funcionário autenticado, quando registrar um período de ausência com início, término e motivo válidos, o sistema deve salvar o registro associado ao funcionário.
* CA22: Dado um administrador autenticado, quando registrar um período de ausência próprio com dados válidos, o sistema deve salvar o registro associado ao administrador.
* CA23: Dado um período de ausência, quando o término for anterior ou igual ao início, o sistema deve rejeitar o registro e informar o motivo.
* CA24: Dado um funcionário autenticado, quando consultar seus períodos de ausência, o sistema deve apresentar somente os registros associados a ele.
* CA25: Dado um funcionário autenticado, quando tentar consultar os períodos de ausência de outro funcionário, o sistema deve negar o acesso.
* CA26: Dado um administrador autenticado, quando consultar os registros de ponto de um funcionário, o sistema deve apresentar os registros correspondentes ao funcionário selecionado.
* CA27: Dado um administrador autenticado, quando consultar os períodos de ausência de um funcionário, o sistema deve apresentar os registros correspondentes ao funcionário selecionado.
* CA28: Dado um funcionário autenticado, quando tentar acessar uma funcionalidade exclusiva de administrador, o sistema deve negar o acesso.
* CA29: Dado um período de ausência registrado, quando o funcionário consultar suas marcações de ponto, o sistema não deve criar ou alterar marcações automaticamente em razão da ausência.
* CA30: Dado um usuário não autenticado, quando tentar registrar ou consultar períodos de ausência, o sistema deve negar o acesso.
* CA31: Dado um administrador autenticado, quando consultar registros por funcionário e período, o sistema deve retornar somente os registros que correspondam aos filtros informados.