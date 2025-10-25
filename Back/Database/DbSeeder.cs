using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public static class DbSeeder
{
    public static void Seed(VocanatorDbContext dbContext)
    {
        var areas = new List<KnowledgeArea>
        {
            new("Ciências Exatas e da Terra"),
            new("Ciências Biológicas"),
            new("Ciências da Saúde"),
            new("Ciências Humanas"),
            new("Ciências Sociais Aplicadas"),
            new("Ciências Agrárias"),
            new("Engenharias"),
            new("Linguística, Letras e Artes"),
        };

        dbContext.AddRange(areas);
        dbContext.SaveChanges();

        var questions = new List<Question>
        {
            new("Tenho facilidade em correlacionar minhas emoções a imagens, cenários e cores."),
            new("Valorizo tarefas que possam desenvolver o meu lado pessoal e minhas relações."),
            new("Normalmente consigo ler nas entrelinhas o que uma pessoa quer dizer, mas tenho dificuldades em expressar com palavras."),
            new("Eu gosto de escrever e sensibilizar o outro através de palavras."),
            new("Tenho facilidade em executar várias tarefas ao mesmo tempo sem necessariamente ter uma rotina."),
            new("Gosto de tocar instrumentos musicais."),
            new("Utilizo meu tempo livre para ouvir diferentes músicas e associá-las a outras composições."),
            new("Gosto de expressar sentimentos verbalmente para as pessoas."),
            new("Procuro ajudar os outros a resolverem os seus problemas."),
            new("Eu costumo viajar para lugares que tenham cachoeiras, praias e montanhas."),
            new("Possuo habilidades para reconhecer notas musicais provenientes de qualquer tipo de objeto."),
            new("Costumo interpretar palavras, gestos e objetivos subentendidos em uma conversa."),
            new("Mantenho o foco em atividades que possuo maior domínio, mas não deixo de trabalhar pontos de melhoria em mim."),
            new("Eu me conheço, compreendo minhas emoções e limites."),
            new("Eu me considero muito empático."),
            new("Prefiro atividades ao ar livre."),
            new("Eu me sinto confortável em conviver com grandes grupos."),
            new("As pessoas do meu convívio me consideram aventureiro."),
            new("Prefiro expressar o meu ponto de vista de maneira mais ponderada e comedida."),
            new("Recebo elogios de amigos por criar melodias que lhes agradam."),
            new("Prefiro atuar na criação ao invés da execução de uma tarefa."),
            new("Eu me interesso por planilhas e gráficos e gosto de explicar aos outros."),
            new("Gosto de identificar detalhes, pois valorizo a estética."),
            new("Frequentemente utilizo o meu corpo para expressar emoções."),
            new("Ao construir pensamentos e ideias, costumo fundamentá-los em argumentos concretos."),
            new("Possuo habilidade e capacidade de entender, criar e sistematizar padrões."),
            new("Eu aprendo com facilidade e procuro utilizar novas ferramentas para qualquer atividade."),
            new("Consigo visualizar projetos claramente, antes mesmo de fazer os primeiros rascunhos."),
            new("Valorizo áreas do conhecimento que envolvam a natureza."),
            new("Prefiro resolver problemas complexos que envolvam números."),
            new("Eu tenho facilidade em negociar, motivar e convencer as pessoas e não me preocupo com o que vão pensar."),
            new("Eu me adapto às situações, mesmo que elas sejam complicadas ou delicadas."),
        };
        dbContext.AddRange(questions);
        dbContext.SaveChanges();
    }
}
