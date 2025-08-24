/** Projeto de Jogo de Cartas -> ProjMarcelo 


/**
Jogo Oitão. Número de jogadores: 2. O objetivo é ser o primeiro a ficar sem cartas. Baralho: tradicional de 52 
carta e sem os coringas tradicionais (usaremos 8 como  coringa). Embaralha; Cada jogador recebe 7 cartas aleatórias; 
Jogador A inicia o jogo apresentando uma carta qualquer; Jogador B precisa corresponder ao número da carta do jogador 
A escolhendo dentro de suas cartas (exemplo, se a carta apresentada pelo jogador A for um 13 de Naipe Espada, o jogador 
B pode jogar qualquer carta de espadas com um valor qualquer ou uma carta 13 naipe diferente do Naipe Espada); Coringas: 
cartas com valor 8 independente do naipe; Utilização do coringa: em qualquer situação e quem utiliza o coringa deve escolher 
o naipe da carta que o oponete deve utilizar; Compra de cartas: quando o jogador não tiver uma carta que atenda as regras para 
ser jogada, ele deve comprar até encontrar uma que atenda; Ganha o jogador que conseguir apresentar primeiro todas as cartas que 
ele adquiriu e caso o jogo se estenda e a lista de cartas do baralho reserva acabe, ganha quem tiver menos carta na mão. 
**/

using System;
using System.Collections.Generic;
using System.Linq;

//criando meu c[ódigo iniciando pela pasta principal
namespace JogoDeCartas
{
    //criar uma lista de 4 naipes 
    public enum Naipe
    {
        Ouros,
        Espadas,
        Copas,
        Paus
    }

    //as cartas tem um naipe e um nº; -> criar uma "forma" de cartas;
    //método escrever/ler; sobreescrever a string dando um retorno novo;
    public class Carta
    {
        public int Valor { get; set; }
        public Naipe Naipe { get; set; }

        public override string ToString()
        {
            return $"{Valor} de {Naipe}";
        }
    }

    //criar jogador "forma de jogador" dando nome e uma lista de carta (chamar de mao);
    //escrever na tela mostrar um  índice  de carta do jogador
    public class Jogador
    {
        public string Nome { get; set; }
        public List<Carta> Mao { get; set; } = new List<Carta>();

        public void MostrarMao()
        {
            Console.WriteLine($"{Nome} tem as cartas:");
            for (int i = 0; i < Mao.Count; i++)
            {
                Console.WriteLine($"[{i}] {Mao[i]}");
            }
        }

        //verificar se o jogador da vez tem carta que seja válida na vez dele;
        //usar método para testar as cartas da mão do jogador e lembrar que 8 é coringa
        public bool TemCartaValida(Carta cartaTopo, Naipe? naipeEscolhido = null)
        {
            return Mao.Any(c =>
                c.Valor == cartaTopo.Valor ||
                c.Naipe == cartaTopo.Naipe ||
                c.Valor == 8 ||
                (naipeEscolhido.HasValue && c.Naipe == naipeEscolhido.Value));
        }
    }

    //monte de carta para sortear
    public class Baralho
    {
        private List<Carta> cartas;
        private Random rnd = new Random();

        //fazer um método baralho e dentro "uma forma" um construtor NEW;
        //ter uma condição de baralho com 52 cartas - esse baralho precisa
        //ser dividido em 4 naipes e valores de 1 á 13;
        public Baralho()
        {
            cartas = new List<Carta>();
            foreach (Naipe naipe in Enum.GetValues(typeof(Naipe)))
            {
                for (int valor = 1; valor <= 13; valor++)
                {
                    cartas.Add(new Carta { Valor = valor, Naipe = naipe });
                }
            }
        }

        //método para embaralhar mudando ordem e tipo sorteio; reordena o baraio
        public void Embaralhar()
        {
            cartas = cartas.OrderBy(c => rnd.Next()).ToList();
        }

        //comprar carta do monte quando as que tem na mao (lista) não for boa pra jogo; 
        public Carta Comprar()
        {
            //não tem mais carta; pega a de cima (1º); pega a carta; entrega para o jogador;
            if (cartas.Count == 0) return null;
            var carta = cartas[0];
            cartas.RemoveAt(0);
            return carta;
        }
         
        //precisará saber quantas cartas tem para saber até onde o jogo poderá ir... um contador
        public int Quantidade => cartas.Count;
    }

    //rodar jogo; criar baralho (construtor); embaralhar (acho que pode ser junto com criar baralho - mesmo bloco);
    //criar jogador (só A e B); dar 7 cartas para cada jogador (acho que usando for - repetir 7 vezes e comprar em
    //cada vez uma carta pra cada jogador); 
    class Program
    {
        static void Main(string[] args)
        {
            Baralho baralho = new Baralho(); // construtor de baralho
            baralho.Embaralhar(); //pega o baralho acima e embaralha

            Jogador jogadorA = new Jogador { Nome = "Jogador A" };
            Jogador jogadorB = new Jogador { Nome = "Jogador B" };

            // distribuir 7 cartas para cada um
            for (int i = 0; i < 7; i++)
            {
                jogadorA.Mao.Add(baralho.Comprar());
                jogadorB.Mao.Add(baralho.Comprar());
            }

            Carta cartaTopo = jogadorA.Mao[0]; // jogador A vai iniciar (indice 0)
            jogadorA.Mao.RemoveAt(0);  // tira essa carta da mão dele (A) - usar removeAt;
                                       // e depois escrever que jogador A começou jogando a carta [0]
            Console.WriteLine($"{jogadorA.Nome} iniciou jogando: {cartaTopo}"); 
            
            //indicar próximo jogador; indicar que vai ser alternada a vez;
            Jogador atual = jogadorB;
            Jogador outro = jogadorA;
            Naipe? naipeEscolhido = null; // usadso para não forçar naipe no começo;

            //preciso fazer um laço de while para rodar até acabar as cartas do monte ou alguém  ficar sem cartas;
            while (true)
            {
                Console.WriteLine($"\n--- Jogada de {atual.Nome} ---"); //escrever quem vai jogar-vez de quem é;
                                                                        //achar uma forma de pular uma linha antes;
                                                                        //puxar método de mostrar a  carta;
                atual.MostrarMao();

                //usar if até encontrar a carta correta para jogar (se não tem no momento); condição - se não achar tem
                //que ir comprando até o baralho acabar; precisa de um retorno no fim do jogo - qdo termina o programa;

                if (!atual.TemCartaValida(cartaTopo, naipeEscolhido))
                {
                    Console.WriteLine($"{atual.Nome} não tem carta válida. Comprando do baralho...");
                    while (!atual.TemCartaValida(cartaTopo, naipeEscolhido)) //se não (enquanto) achar tem que ir comprando; acho que usa while
                        //se encontrar uma carta boa que atenda, precisa falar que o jogador comprou e falar qual é;
                    {
                        // condição para contar as cartas das mãos dos jogadores - fazer separado e comparar; dar uma resposta de vencedor ou empate;
                        var nova = baralho.Comprar();
                        if (nova == null)    //não tem mais carat para comprar
                        {
                            Console.WriteLine("Baralho esgotou!");
                            int qtdA = jogadorA.Mao.Count;
                            int qtdB = jogadorB.Mao.Count;
                            if (qtdA < qtdB) Console.WriteLine("Jogador A venceu!");
                            else if (qtdB < qtdA) Console.WriteLine("Jogador B venceu!");
                            else Console.WriteLine("Empate!");
                            return;
                        }
                        atual.Mao.Add(nova);
                        Console.WriteLine($"{atual.Nome} comprou {nova}");
                    }
                }

                // mandar o jogador usuário da vez jogar a carta chamando pelo indice[] - precisa de um  conversor para string;
                Console.WriteLine("Escolha uma carta para jogar (índice):");
                int indice = int.Parse(Console.ReadLine());

                //escolher a carta da mão atual
                Carta escolhida = atual.Mao[indice];
                //usar if para o coringa - aqui o bicho vai pegar; // ahhhh... se não for curinga, libera a regra de naipe forçado;
                //precisa converter o nº digitado para naipe;
                if (escolhida.Valor == 8)
                {
                    Console.WriteLine($"{atual.Nome} jogou um curinga!");
                    Console.WriteLine("Escolha o Naipe que o próximo deve jogar (0=Ouros,1=Espadas,2=Copas,3=Paus):");
                    int n = int.Parse(Console.ReadLine());
                    naipeEscolhido = (Naipe)n;
                    Console.WriteLine($"O próximo jogador deve jogar {naipeEscolhido}.");
                }
                else
                {
                    naipeEscolhido = null;
                }
                //carta nova e tira da mão e joga na mesa;
                cartaTopo = escolhida;
                atual.Mao.RemoveAt(indice);
                Console.WriteLine($"{atual.Nome} jogou: {cartaTopo}");

                if (atual.Mao.Count == 0) // se chegar a 0 cartas na mao vence e sai;
                {
                    Console.WriteLine($"\n{atual.Nome} venceu o jogo!");
                    //sair do while
                    break;
                }

                // método para mudar/alternar jogador
                var temp = atual;
                atual = outro;
                outro = temp;
            }
        }
    }
}

