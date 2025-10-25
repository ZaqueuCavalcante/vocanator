using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public static class DbSeeder
{
    public static void Seed(VocanatorDbContext dbContext)
    {
        var questions = new List<Question>
        {
            new(01, "Tenho facilidade em correlacionar minhas emoções a imagens, cenários e cores."),
            new(02, "Valorizo tarefas que possam desenvolver o meu lado pessoal e minhas relações."),
            new(03, "Normalmente consigo ler nas entrelinhas o que uma pessoa quer dizer, mas tenho dificuldades em expressar com palavras."),
            new(04, "Gosto de escrever e sensibilizar o outro através de palavras."),
            new(05, "Tenho facilidade em executar várias tarefas ao mesmo tempo sem necessariamente ter uma rotina."),
            new(06, "Gosto de tocar instrumentos musicais."),
            new(07, "Utilizo meu tempo livre para ouvir diferentes músicas e associá-las a outras composições."),
            new(08, "Gosto de expressar sentimentos verbalmente para as pessoas."),
            new(09, "Procuro ajudar os outros a resolverem os seus problemas."),
            new(10, "Costumo viajar para lugares que tenham cachoeiras, praias e montanhas."),
            new(11, "Possuo habilidades para reconhecer notas musicais provenientes de qualquer tipo de objeto."),
            new(12, "Costumo interpretar palavras, gestos e objetivos subentendidos em uma conversa."),
            new(13, "Mantenho o foco em atividades que possuo maior domínio, mas não deixo de trabalhar pontos de melhoria em mim."),
            new(14, "Me conheço, compreendo minhas emoções e limites."),
            new(15, "Me considero muito empático."),
            new(16, "Prefiro atividades ao ar livre."),
            new(17, "Me sinto confortável em conviver com grandes grupos."),
            new(18, "As pessoas do meu convívio me consideram aventureiro."),
            new(19, "Prefiro expressar o meu ponto de vista de maneira mais ponderada e comedida."),
            new(20, "Recebo elogios de amigos por criar melodias que lhes agradam."),
            new(21, "Prefiro atuar na criação ao invés da execução de uma tarefa."),
            new(22, "Me interesso por planilhas e gráficos e gosto de explicar aos outros."),
            new(23, "Gosto de identificar detalhes, pois valorizo a estética."),
            new(24, "Frequentemente utilizo o meu corpo para expressar emoções."),
            new(25, "Ao construir pensamentos e ideias, costumo fundamentá-los em argumentos concretos."),
            new(26, "Possuo habilidade e capacidade de entender, criar e sistematizar padrões."),
            new(27, "Aprendo com facilidade e procuro utilizar novas ferramentas para qualquer atividade."),
            new(28, "Consigo visualizar projetos claramente, antes mesmo de fazer os primeiros rascunhos."),
            new(29, "Valorizo áreas do conhecimento que envolvam a natureza."),
            new(30, "Prefiro resolver problemas complexos que envolvam números."),
            new(31, "Tenho facilidade em negociar, motivar e convencer as pessoas e não me preocupo com o que vão pensar."),
            new(32, "Me adapto às situações, mesmo que elas sejam complicadas ou delicadas."),
        };

        var areas = new List<KnowledgeArea>
        {
            new(01, "Exatas"),
            new(02, "Biológicas"),
            new(03, "Saúde"),
            new(04, "Humanas"),
            new(05, "Ciências Sociais"),
            new(06, "Agrociências"),
            new(07, "Engenharias"),
            new(08, "Linguagem e Artes"),
        };

        var weights = new List<QuestionKnowledgeAreaWeight>
        {
            // 1. Emoções e cores → Humanas, Artes
            new(1,1,1), new(1,2,2), new(1,3,3), new(1,4,4), new(1,5,2), new(1,6,2), new(1,7,1), new(1,8,5),

            // 2. Desenvolvimento pessoal e relações → Humanas, Sociais
            new(2,1,1), new(2,2,2), new(2,3,3), new(2,4,5), new(2,5,4), new(2,6,2), new(2,7,1), new(2,8,2),

            // 3. Ler nas entrelinhas → Humanas, Artes
            new(3,1,1), new(3,2,2), new(3,3,2), new(3,4,5), new(3,5,3), new(3,6,1), new(3,7,1), new(3,8,4),

            // 4. Escrever e sensibilizar → Artes, Humanas
            new(4,1,1), new(4,2,1), new(4,3,2), new(4,4,4), new(4,5,3), new(4,6,1), new(4,7,1), new(4,8,5),

            // 5. Multitarefa sem rotina → Sociais, Engenharias
            new(5,1,3), new(5,2,2), new(5,3,2), new(5,4,3), new(5,5,5), new(5,6,2), new(5,7,4), new(5,8,2),

            // 6. Tocar instrumentos → Artes
            new(6,1,1), new(6,2,1), new(6,3,2), new(6,4,3), new(6,5,2), new(6,6,1), new(6,7,1), new(6,8,5),

            // 7. Ouvir músicas e associar → Artes
            new(7,1,1), new(7,2,1), new(7,3,2), new(7,4,3), new(7,5,2), new(7,6,1), new(7,7,1), new(7,8,5),

            // 8. Expressar sentimentos verbalmente → Humanas, Artes
            new(8,1,1), new(8,2,1), new(8,3,3), new(8,4,5), new(8,5,3), new(8,6,1), new(8,7,1), new(8,8,4),

            // 9. Ajudar os outros → Saúde, Humanas
            new(9,1,1), new(9,2,2), new(9,3,5), new(9,4,4), new(9,5,3), new(9,6,2), new(9,7,1), new(9,8,2),

            // 10. Natureza e viagens → Biológicas, Agrárias
            new(10,1,1), new(10,2,5), new(10,3,3), new(10,4,3), new(10,5,2), new(10,6,4), new(10,7,2), new(10,8,2),

            // 11. Reconhecer notas musicais → Artes
            new(11,1,1), new(11,2,1), new(11,3,2), new(11,4,2), new(11,5,1), new(11,6,1), new(11,7,1), new(11,8,5),

            // 12. Interpretar palavras e gestos → Humanas, Artes
            new(12,1,1), new(12,2,1), new(12,3,2), new(12,4,5), new(12,5,3), new(12,6,1), new(12,7,1), new(12,8,4),

            // 13. Foco e autodesenvolvimento → Humanas
            new(13,1,1), new(13,2,1), new(13,3,3), new(13,4,5), new(13,5,3), new(13,6,2), new(13,7,2), new(13,8,2),

            // 14. Autoconhecimento → Humanas
            new(14,1,1), new(14,2,2), new(14,3,3), new(14,4,5), new(14,5,3), new(14,6,2), new(14,7,1), new(14,8,2),

            // 15. Empatia → Saúde, Humanas
            new(15,1,1), new(15,2,2), new(15,3,5), new(15,4,4), new(15,5,3), new(15,6,2), new(15,7,1), new(15,8,2),

            // 16. Atividades ao ar livre → Biológicas, Agrárias
            new(16,1,1), new(16,2,4), new(16,3,2), new(16,4,2), new(16,5,2), new(16,6,5), new(16,7,1), new(16,8,2),

            // 17. Convívio social → Humanas, Sociais
            new(17,1,1), new(17,2,1), new(17,3,3), new(17,4,4), new(17,5,5), new(17,6,2), new(17,7,2), new(17,8,2),

            // 18. Aventureiro → Agrárias, Humanas
            new(18,1,1), new(18,2,3), new(18,3,2), new(18,4,4), new(18,5,2), new(18,6,5), new(18,7,2), new(18,8,2),

            // 19. Ponderado → Humanas
            new(19,1,1), new(19,2,1), new(19,3,2), new(19,4,5), new(19,5,3), new(19,6,1), new(19,7,1), new(19,8,3),

            // 20. Criar melodias → Artes
            new(20,1,1), new(20,2,1), new(20,3,2), new(20,4,2), new(20,5,1), new(20,6,1), new(20,7,1), new(20,8,5),

            // 21. Criar vs executar → Artes, Engenharias
            new(21,1,2), new(21,2,1), new(21,3,2), new(21,4,3), new(21,5,3), new(21,6,2), new(21,7,5), new(21,8,4),

            // 22. Planilhas e gráficos → Exatas, Engenharias
            new(22,1,5), new(22,2,2), new(22,3,2), new(22,4,2), new(22,5,4), new(22,6,1), new(22,7,5), new(22,8,1),

            // 23. Detalhes e estética → Artes, Engenharias
            new(23,1,2), new(23,2,1), new(23,3,2), new(23,4,2), new(23,5,3), new(23,6,1), new(23,7,4), new(23,8,5),

            // 24. Corpo e emoções → Artes
            new(24,1,1), new(24,2,1), new(24,3,2), new(24,4,3), new(24,5,2), new(24,6,1), new(24,7,1), new(24,8,5),

            // 25. Argumentos concretos → Exatas, Engenharias
            new(25,1,5), new(25,2,2), new(25,3,2), new(25,4,3), new(25,5,3), new(25,6,1), new(25,7,5), new(25,8,1),

            // 26. Padrões → Exatas, Engenharias
            new(26,1,5), new(26,2,1), new(26,3,2), new(26,4,3), new(26,5,3), new(26,6,1), new(26,7,5), new(26,8,1),

            // 27. Novas ferramentas → Engenharias, Exatas
            new(27,1,4), new(27,2,2), new(27,3,2), new(27,4,2), new(27,5,3), new(27,6,1), new(27,7,5), new(27,8,1),

            // 28. Visualizar projetos → Engenharias, Artes
            new(28,1,2), new(28,2,1), new(28,3,1), new(28,4,3), new(28,5,3), new(28,6,1), new(28,7,5), new(28,8,4),

            // 29. Natureza → Biológicas, Agrárias
            new(29,1,1), new(29,2,5), new(29,3,3), new(29,4,2), new(29,5,2), new(29,6,5), new(29,7,2), new(29,8,2),

            // 30. Números → Exatas, Engenharias
            new(30,1,5), new(30,2,1), new(30,3,1), new(30,4,2), new(30,5,3), new(30,6,1), new(30,7,5), new(30,8,1),

            // 31. Negociar, motivar → Sociais, Humanas
            new(31,1,1), new(31,2,1), new(31,3,3), new(31,4,4), new(31,5,5), new(31,6,2), new(31,7,2), new(31,8,2),

            // 32. Adaptação → Humanas, Saúde
            new(32,1,1), new(32,2,2), new(32,3,4), new(32,4,5), new(32,5,3), new(32,6,2), new(32,7,1), new(32,8,2),
        };

        dbContext.AddRange(questions);
        dbContext.AddRange(areas);
        dbContext.AddRange(weights);
        dbContext.SaveChanges();
    }
}
