using GestaojogosEFCore.Data;
using GestaojogosEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaojogosEFCore.repositories
{
    public class JogoRepository
    {
        private readonly DbContextOptions<GestaoJogosContext> _options;

        public JogoRepository(DbContextOptions<GestaoJogosContext> options)
        {
            _options = options;
        }

        public List<Jogo> Listar()
        {
            using var context = new GestaoJogosContext(_options);

            return context.Jogo
            //carrega a navegação de desenvolvedora junt com os jogos
            .Include(j => j.IdDesenvolvedoraNavigation)
            .OrderBy(j => j.IdJogo)
            //executa a consulta no banco de dados e materializa os resultados em uma lista
            .ToList();
        }

        public Jogo? BuscarPorId(int id)
        {
            using var context = new GestaoJogosContext(_options);

            return context.Jogo
            .Include(j => j.IdDesenvolvedoraNavigation)
            // retorna o primeiro resultado ou o valor padrão (null)
            .FirstOrDefault(j => j.IdJogo == id);
        }

        public void Adicionar(Jogo jogo)
        {
            using var context = new GestaoJogosContext(_options);

            context.Jogo.Add(jogo);
            context.SaveChanges();
        }

        public bool Atualizar(Jogo jogoAtualizado)
        {
            using var context = new GestaoJogosContext(_options);

            /*
            Create
            Read
            Update
            Delete
            */

            var jogoExistente = context.Jogo.Find(jogoAtualizado.IdJogo);

            if (jogoExistente is null)
                return false;

            //copia para a emtdade rastreada os valores recebidos pelo paramentro da funçãod
            jogoExistente.IdDesenvolvedora = jogoAtualizado.IdDesenvolvedora;
            jogoExistente.Nome = jogoAtualizado.Nome;
            jogoExistente.Genero = jogoAtualizado.Genero;
            jogoExistente.AnoLancamento = jogoAtualizado.AnoLancamento;
            jogoExistente.Preco = jogoAtualizado.Preco;

            context.SaveChanges();
            return true;
        }

        public bool Excluir(int id)
        {
            using var context = new GestaoJogosContext(_options);
            var JogoExistente = context.Jogo.Find(id); //* Identifica se o ID é valido.

            if (JogoExistente is null)
            {
                return false;
            }

            context.Jogo.Remove(JogoExistente);
            context.SaveChanges();
            return true;
        }

        public List<Desenvolvedora> ListarDesenvolvedoras()
        {
            using var context = new GestaoJogosContext(_options);
            return context.Desenvolvedora
            .OrderBy(d => d.Nome)
            .ToList();
        }

        public void AdicionarDesenvolvedora(Desenvolvedora desenvolvedora)
        {
            using var context = new GestaoJogosContext(_options);

            context.Desenvolvedora.Add(desenvolvedora);
            context.SaveChanges();
        }

    }
}